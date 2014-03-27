using System;
using System.Diagnostics.Contracts;
using System.Security.Principal;
using GraphLabs.DomainModel;
using log4net;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> <see cref="T:System.Security.Principal.IIdentity"/> для GraphLabs (+email) </summary>
    public interface IGraphLabsIdentity : IIdentity
    {
        /// <summary> Email </summary>
        string Email { get; }
    }

    /// <summary> <see cref="T:System.Security.Principal.IPrincipal"/> для GraphLabs (+email) </summary>
    public interface IGraphLabsPrincipal : IPrincipal
    {
        /// <summary> Gets the identity of the current principal. </summary>
        new IGraphLabsIdentity Identity { get; }

        /// <summary> Determines whether the current principal belongs to the specified role. </summary>
        bool IsInRole(UserRole role);
    }

    /// <summary> Реализация <see cref="T:System.Security.Principal.IPrincipal"/> для GraphLabs </summary>
    internal sealed class GraphLabsPrincipal : IGraphLabsPrincipal
    {
        private const string AUTHENTICATION_TYPE = "graphLabs_v1";
        private static readonly ILog _log = LogManager.GetLogger(typeof(GraphLabsPrincipal));

        /// <summary> Личность </summary>
        private sealed class GraphLabsIdentity : IGraphLabsIdentity
        {
            public string Name { get; private set; }

            public string Email { get; private set; }

            /// <summary> Gets the type of authentication used. </summary>
            public string AuthenticationType { get { return AUTHENTICATION_TYPE; } }

            /// <summary> Gets a value that indicates whether the user has been authenticated. </summary>
            public bool IsAuthenticated
            {
                get
                {
                    return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Email);
                }
            }

            /// <summary> Закрытый конструктор, чтобы создать анонима </summary>
            private GraphLabsIdentity()
            {
            }

            /// <summary> Личность </summary>
            public GraphLabsIdentity(string name, string email)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(name));
                Contract.Requires(!string.IsNullOrWhiteSpace(email));
                Name = name;
                Email = email;
            }

            /// <summary> Аноним </summary>
            public static readonly IGraphLabsIdentity Anonymous = new GraphLabsIdentity { Name = null, Email = null };
        }

        /// <summary> Determines whether the current principal belongs to the specified role. </summary>
        bool IPrincipal.IsInRole(string role)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(role));

            UserRole actualRole;
            if (!Enum.TryParse(role, false, out actualRole))
            {
                _log.WarnFormat("Проверка на неизвестную пользовательскую роль \"{0}\".", role);
                return false;
            }

            return IsInRole(actualRole);
        }

        /// <summary> Gets the identity of the current principal. </summary>
        IIdentity IPrincipal.Identity { get { return Identity; } }

        /// <summary> Determines whether the current principal belongs to the specified role. </summary>
        public bool IsInRole(UserRole role)
        {
            return _roles.HasFlag(role);
        }

        /// <summary> Gets the identity of the current principal. </summary>
        public IGraphLabsIdentity Identity { get; private set; }

        /// <summary> Закрытый конструктор, чтобы создать анонима </summary>
        private GraphLabsPrincipal(UserRole roles)
        {
            _roles = roles;
        }

        /// <summary> Реализация <see cref="T:System.Security.Principal.IPrincipal"/> для GraphLabs </summary>
        public GraphLabsPrincipal(string name, string email, UserRole roles)
            : this(roles)
        {
            Identity = new GraphLabsIdentity(name, email);
        }

        private readonly UserRole _roles;

        /// <summary> Аноним </summary>
        public static readonly IPrincipal Anonymous = new GraphLabsPrincipal(UserRole.None) { Identity = GraphLabsIdentity.Anonymous };
    }
}