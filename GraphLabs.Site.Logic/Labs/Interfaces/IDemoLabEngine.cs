using GraphLabs.DomainModel;

namespace GraphLabs.Site.Logic.Labs
{
    public interface IDemoLabEngine
    {
        LabWork[] GetDemoLabs();

        LabVariant[] GetDemoLabVariantsByLabWorkId(long id);
    }
}
