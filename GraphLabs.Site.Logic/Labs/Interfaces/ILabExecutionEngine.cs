using GraphLabs.DomainModel;

namespace GraphLabs.Site.Logic.Labs
{
    public interface ILabExecutionEngine
    {
        string GetLabName(long id);
        bool IsLabVariantCorrect(long labVarId);
    }
}
