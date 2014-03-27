using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с пользователями </summary>
    [ContractClass(typeof(UserRepositoryContracts))]
    public interface IUserRepository : IDisposable
    {
        /// <summary> Получить активного пользователя по email </summary>
        /// <remarks> т.е. подтверждённого, не отчисленного и не удалённого. </remarks>
        [CanBeNull]
        User FindActiveUserByEmail(string email);

        /// <summary> Создать нового студента </summary>
        [NotNull]
        Student CreateNotVerifiedStudent(string email, string name, string fatherName, string surname, string passwordHash, Group group);
    }

    /// <summary> Репозиторий с пользователями - контракты </summary>
    [ContractClassFor(typeof(IUserRepository))]
    internal abstract class UserRepositoryContracts : IUserRepository
    {
        // ReSharper disable AssignNullToNotNullAttribute

        /// <summary> Получить активного пользователя по email </summary>
        public User FindActiveUserByEmail(string email)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(email));

            return default(User);
        }

        /// <summary> Создать нового студента </summary>
        public Student CreateNotVerifiedStudent(string email, string name, string fatherName, string surname, string passwordHash, Group group)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(email));
            Contract.Requires(!string.IsNullOrWhiteSpace(name));
            Contract.Requires(!string.IsNullOrWhiteSpace(surname));
            Contract.Requires(!string.IsNullOrWhiteSpace(passwordHash));
            Contract.Requires(group != null && group.IsOpen);

            Contract.Ensures(Contract.Result<Student>() != null);

            return null;
        }

        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
        }
    }
}
