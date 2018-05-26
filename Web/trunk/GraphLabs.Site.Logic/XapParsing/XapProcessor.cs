using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using Mono.Cecil;

namespace GraphLabs.Site.Logic.XapParsing
{
    /// <summary> Класс для получения информации о Xap </summary>
    public class XapProcessor : IXapProcessor
    {
        #region Константы

        private const string APP_MANIFEST_NAME = "AppManifest.xaml";
        private const string ENTRY_POINT_ATTR_NAME = "EntryPointAssembly";
        private const string DEPLOYMENT_SECTION_NAME = "Deployment";
        private const string DEPLOYMENT_PARTS_SECTION_NAME = "Deployment.Parts";
        private const string NAME_ATTR_NAME = "Name";
        private const string SOURCE_ATTR_NAME = "Source";

        #endregion


        #region Вспомагательные методы

        private string GetCustomAttribute<T>(AssemblyDefinition definition)
        {
            var nameToSearch = typeof(T).FullName;
            return definition.CustomAttributes
                .Single(attr => attr.AttributeType.FullName == nameToSearch)
                .ConstructorArguments.Single()
                .Value.ToString();
        }
        
        #endregion


        /// <summary> Информация о файле Xap </summary>
        private class XapInfo : IXapInfo
        {
            public string Name { get; internal set; }
            public string Sections { get; internal set; }
            public string Version { get; internal set; }
        }

        /// <summary> По xap-файлу создаёт сущность Task (в базу не пишет) </summary>
        /// <returns> null, если во время обработки произошла ошибка; иначе - новую сущность </returns>
        public IXapInfo Parse(Stream stream)
        {
            Contract.Assert(stream != null);

            // Парсим примерно следующее (AppManifest.xaml):
            /*
            <Deployment xmlns="..." xmlns:x="..."  EntryPointAssembly="GraphLabs.Tasks.DistanceMatrix" EntryPointType="GraphLabs.Tasks.DistanceMatrix.App" RuntimeVersion="5.0.61118.0">
              <Deployment.OutOfBrowserSettings>
                ...
              </Deployment.OutOfBrowserSettings>
              <Deployment.Parts>
                <AssemblyPart x:Name="GraphLabs.Tasks.DistanceMatrix" Source="GraphLabs.Tasks.DistanceMatrix.dll" />
                ...                                                           ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
              </Deployment.Parts>                                                        то, нужно вытащить
            </Deployment>                                                                      
            */
            try
            {
                using (var xap = ZipFile.Read(stream))
                {
                    if (!xap.ContainsEntry(APP_MANIFEST_NAME))
                        return null;

                    string entryPointAssemblySource;

                    using (var appManifest = new MemoryStream())
                    {
                        xap[APP_MANIFEST_NAME].Extract(appManifest);
                        appManifest.Seek(0, SeekOrigin.Begin);

                        using (var reader = XmlReader.Create(appManifest))
                        {
                            var deploymentSection = XDocument.Load(reader).Root;

                            Contract.Assume(deploymentSection != null);
                            Contract.Assume(deploymentSection.Name.LocalName == DEPLOYMENT_SECTION_NAME);

                            var entryPointAssemblyName = deploymentSection
                                .Attributes()
                                .Single(attr => attr.Name.LocalName == ENTRY_POINT_ATTR_NAME)
                                .Value;

                            var partsSection = deploymentSection
                                .Descendants()
                                .Single(element => element.Name.LocalName == DEPLOYMENT_PARTS_SECTION_NAME);

                            var entryPointAssemblyElement = partsSection.Descendants()
                                .Select(el =>
                                    new
                                    {
                                        NameAttr = el.Attributes()
                                            .SingleOrDefault(attr => attr.Name.LocalName == NAME_ATTR_NAME),
                                        Element = el,
                                    })
                                .Where(el => el.NameAttr != null)
                                .Single(el => el.NameAttr.Value == entryPointAssemblyName)
                                .Element;

                            entryPointAssemblySource = entryPointAssemblyElement
                                .Attributes()
                                .Single(attr => attr.Name.LocalName == SOURCE_ATTR_NAME)
                                .Value;
                        }
                    }

                    // Теперь достаём сам файл и информацию из него
                    if (!xap.ContainsEntry(entryPointAssemblySource))
                        return null;

                    using (var entryPointAssembly = new MemoryStream())
                    {
                        xap[entryPointAssemblySource].Extract(entryPointAssembly);
                        entryPointAssembly.Seek(0, SeekOrigin.Begin);

                        var definition = AssemblyDefinition.ReadAssembly(entryPointAssembly);
                        stream.Seek(0, SeekOrigin.Begin);
                        var info = new XapInfo
                            {
                                Name = GetCustomAttribute<AssemblyTitleAttribute>(definition),
                                Sections = GetCustomAttribute<AssemblyDescriptionAttribute>(definition),
                                Version = definition.Name.Version.ToString()
                            };
                        return info;
                    }
                }
            }
            catch (Exception)
            {
                return null; //TODO: тут когда-нибудь будет писаться лог.
            }
        }
    }
}
