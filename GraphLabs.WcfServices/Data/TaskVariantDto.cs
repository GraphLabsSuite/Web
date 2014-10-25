using System.Runtime.Serialization;

namespace GraphLabs.WcfServices.Data
{
    /// <summary> Вариант задания </summary>
    [DataContract]
    public class TaskVariantDto
    {
        /// <summary> Id. </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary> Id задания, для которого вариант </summary>
        /// <remarks> Слегка избыточен, но путь будет. </remarks>
        [DataMember]
        public long TaskId { get; set; }

        /// <summary> Данные. Как правило, граф. </summary>
        [DataMember]
        public byte[] Data { get; set; }

        /// <summary> Номер варианта. </summary>
        [DataMember]
        public string Number { get; set; }

        /// <summary> Версия программы-генератора. </summary>
        [DataMember]
        public string GeneratorVersion { get; set; }

        /// <summary> Версия варианта. </summary>
        [DataMember]
        public long? Version { get; set; }
    }
}
