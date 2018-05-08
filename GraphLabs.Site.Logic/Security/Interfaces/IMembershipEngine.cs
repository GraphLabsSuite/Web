using System;
using System.Diagnostics.Contracts;
using GraphLabs.Site.Utils;

namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Работа с пользователями </summary>
    [ContractClass(typeof(MembershipEngineContracts))]
    public interface IMembershipEngine
    {
        /// <summary> Выполняет вход </summary>
        LoginResult TryLogin(string email, string password, string clientIp, out Guid sessionGuid);

        /// <summary> Выполняет вход, убивая все сессии на других браузерах/компьютерах </summary>
        LoginResult TryForceLogin(string email, string password, string clientIp, out Guid sessionGuid);

        /// <summary> Выход </summary>
        void Logout(string email, Guid sessionGuid, string clientIp);

        /// <summary> Зарегистрировать нового студента</summary>
        /// <returns> false, если такой пользователь уже есть (с таким email), иначе true </returns>
        bool RegisterNewStudent(string email, string name, string fatherName, string surname, string password, long groupId);

        /// <summary> Проверяем пользователя и устанавливаем IPrincipal </summary>
        bool TryAuthenticate(string email, Guid sessionGuid, string clientIp);

        /// <summary> Поменять пароль </summary>
        bool ChangePassword(string email, Guid sessionGuid, string clientIp, string currentPassword, string newPassword);
    }

    /// <summary> Контракты для <see cref="MembershipEngine"/> </summary>
    [ContractClassFor(typeof(IMembershipEngine))]
    internal abstract class MembershipEngineContracts : IMembershipEngine
    {
        /// <summary> Выполняет вход </summary>
        public LoginResult TryLogin(string email, string password, string clientIp, out Guid sessionGuid)
        {
            Contract.Requires<ArgumentException>(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp)); 
            Contract.Ensures(
                Contract.Result<LoginResult>() != LoginResult.Success & Contract.ValueAtReturn(out sessionGuid) == Guid.Empty ||
                Contract.Result<LoginResult>() == LoginResult.Success & Contract.ValueAtReturn(out sessionGuid) != Guid.Empty);
            Guard.Guard.IsTrueAssertion(default(LoginResult) != LoginResult.Success & sessionGuid == Guid.Empty ||
                default(LoginResult) == LoginResult.Success & sessionGuid != Guid.Empty);

            return default(LoginResult);
        }

        /// <summary> Выполняет вход </summary>
        public LoginResult TryForceLogin(string email, string password, string clientIp, out Guid sessionGuid)
        {
            Contract.Requires<ArgumentException>(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp));
            Contract.Ensures(
                Contract.Result<LoginResult>() != LoginResult.Success & Contract.ValueAtReturn(out sessionGuid) == Guid.Empty ||
                Contract.Result<LoginResult>() == LoginResult.Success & Contract.ValueAtReturn(out sessionGuid) != Guid.Empty);
            Guard.Guard.IsTrueAssertion(default(LoginResult) != LoginResult.Success & sessionGuid == Guid.Empty ||
                default(LoginResult) == LoginResult.Success & sessionGuid != Guid.Empty);
            return default(LoginResult);
        }

        /// <summary> Выход </summary>
        public void Logout(string email, Guid sessionGuid, string clientIp)
        {
        }

        /// <summary> Зарегистрировать нового студента</summary>
        public bool RegisterNewStudent(string email, string name, string fatherName, string surname, string password, long groupId)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(email));
            Guard.Guard.IsNotNullOrWhiteSpace(email);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            Guard.Guard.IsNotNullOrWhiteSpace(name);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(surname));
            Guard.Guard.IsNotNullOrWhiteSpace(surname);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(password));
            Guard.Guard.IsNotNullOrWhiteSpace(password);
            Contract.Requires<ArgumentException>(groupId > 0);
            Guard.Guard.IsTrueAssertion(groupId > 0);

            return default(bool);
        }

        /// <summary> Проверяем пользователя </summary>
        public bool TryAuthenticate(string email, Guid sessionGuid, string clientIp)
        {
            return default(bool);
        }

        /// <summary> Поменять пароль </summary>
        public bool ChangePassword(string email, Guid sessionGuid, string clientIp, string currentPassword, string newPassword)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(email));
            Guard.Guard.IsNotNullOrWhiteSpace(email);
            Contract.Requires<ArgumentException>(sessionGuid != Guid.Empty);
            Guard.Guard.IsTrueAssertion(sessionGuid != Guid.Empty);
            Contract.Requires<ArgumentException>(IpHelper.CheckIsValidIP(clientIp));
            Guard.Guard.IsTrueAssertion(IpHelper.CheckIsValidIP(clientIp));
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(currentPassword));
            Guard.Guard.IsNotNullOrWhiteSpace(currentPassword);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(newPassword));
            Guard.Guard.IsNotNullOrWhiteSpace(newPassword);

            return default(bool);
        }
    }
}