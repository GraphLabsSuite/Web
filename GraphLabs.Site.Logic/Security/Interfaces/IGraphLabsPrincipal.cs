using System.Security.Principal;
using GraphLabs.DomainModel.EF;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> <see cref="T:System.Security.Principal.IPrincipal"/> для GraphLabs (+email) </summary>
    public interface IGraphLabsPrincipal : IPrincipal
    {
        /// <summary> Gets the identity of the current principal. </summary>
        new IGraphLabsIdentity Identity { get; }

        /// <summary> Determines whether the current principal belongs to the specified role. </summary>
        bool IsInRole(UserRole role);
    }
}