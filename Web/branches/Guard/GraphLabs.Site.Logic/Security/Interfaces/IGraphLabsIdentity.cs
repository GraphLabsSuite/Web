using System.Security.Principal;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> <see cref="T:System.Security.Principal.IIdentity"/> для GraphLabs (+email) </summary>
    public interface IGraphLabsIdentity : IIdentity
    {
        /// <summary> Отображаемое имя </summary>
        string DisplayName { get; }
    }
}