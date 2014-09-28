using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace GraphLabs.DomainModel.Repositories
{
    /// <summary> Репозиторий с лабораторными работами </summary>
    [ContractClass(typeof(LabRepositoryContracts))]
    public interface ILabRepository : IDisposable
    {
        // ReSharper disable ReturnTypeCanBeEnumerable.Global

        #region Получение массивов лабораторных работ

        /// <summary> Получить лабораторные работы </summary>
        [NotNull]
        LabWork[] GetLabWorks();

        /// <summary> Получить лабораторные работы, доступные в данный момент в ознакомительном режиме </summary>
        [NotNull]
        LabWork[] GetDemoLabs(DateTime currentDate);

        #endregion

        #region Получение массива вариантов лабораторной работы

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetLabVariantsByLabWorkId(long id);

        /// <summary> Получить ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetDemoLabVariantsByLabWorkId(long id);

        /// <summary> Получить готовые ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        [NotNull]
        LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId);

        #endregion

        #region Проверки

        /// <summary> Проверить существование лабораторной работы по id</summary>
        [NotNull]
        bool CheckLabWorkExist(long id);

        /// <summary> Проверить существование лабораторной работы по имени</summary>
        [NotNull]
        bool CheckLabWorkExist(string name);

        /// <summary> Проверить существование варианта лабораторной работы по Id </summary>
        [NotNull]
        bool CheckLabVariantExist(long id);

		/// <summary> Проверить существование варианта лабораторной работы по имени </summary>
		[NotNull]
		bool CheckLabVariantExist(long labId, string name);

        /// <summary> Проверить принадлежность варианта л.р. лабораторной работе </summary>
        [NotNull]
        bool CheckLabVariantBelongLabWork(long labId, long labVarId);

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        [NotNull]
        bool VerifyCompleteVariant(long variantId);

        #endregion

        #region Получение разнородной информации по id

        /// <summary> Получить лабораторную работу по id </summary>
        [NotNull]
        LabWork GetLabWorkById(long id);

		/// <summary> Получить вариант л.р. по id </summary>
		[NotNull]
		LabVariant GetLabVariantById(long id);

        /// <summary> Найти вариант лабораторной работы по id </summary>
        [CanBeNull]
        LabVariant FindLabVariantById(long id);

        /// <summary> Получить варианты заданий с заданиями варианта лабораторной работы </summary>
        [NotNull]
        TaskVariant[] GetTaskVariantsByLabVarId(long labVarId);

        #endregion

		#region Изменение в БД

		/// <summary> Удаление содержания лабораторной работы </summary>
		[NotNull]
		void DeleteEntries(long labWorkId);

		/// <summary> Сохранение лабораторной работы </summary>
		[NotNull]
		void SaveLabWork(LabWork lab);

		/// <summary> Изменение лабораторной работы </summary>
		[NotNull]
		void ModifyLabWork(LabWork lab);

		/// <summary> Сохранение содержания лабораторной работы </summary>
		[NotNull]
		void SaveLabEntries(long labWorkId, long[] tasksId);

		/// <summary> Удаляет лишние варианты заданий из вариантов лабораторной работы для соответствия содержанию </summary>
		[NotNull]
		void DeleteExcessTaskVariantsFromLabVariants(long labWorkId);

		/// <summary> Сохранение варианта л.р. </summary>
		[NotNull]
		void SaveLabVariant(LabVariant labVar);

		/// <summary> Изменение варианта л.р. </summary>
		[NotNull]
		void ModifyLabVariant(LabVariant labVar);

		#endregion

		#region Получение Id экземпляров

		/// <summary> Получить id лабораторной работы по ее имени </summary>
		[NotNull]
		long GetLabWorkIdByName(string name);

		/// <summary> Получить id варианта л.р. по его имени </summary>
		[NotNull]
		long GetLabVariantIdByNumber(long labId, string number);

		#endregion
	}

    /// <summary> Репозиторий с лаораторными работами - контракты </summary>
    [ContractClassFor(typeof(ILabRepository))]
    internal abstract class LabRepositoryContracts : ILabRepository
    {
        /// <summary> Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
        }

        #region Получение массива лабораторных работ

        public LabWork[] GetLabWorks()
        {
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        public LabWork[] GetDemoLabs(DateTime currentDate)
        {
            Contract.Requires(currentDate != null);
            Contract.Ensures(Contract.Result<LabWork[]>() != null);

            return new LabWork[0];
        }

        #endregion

        #region Получение массива вариантов лабораторной работы

        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        public LabVariant[] GetDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        public LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId)
        {
            Contract.Requires(labId > 0);
            Contract.Ensures(Contract.Result<LabVariant[]>() != null);

            return new LabVariant[0];
        }

        #endregion

        #region Проверки

        public bool CheckLabWorkExist(long id)
        {
            Contract.Requires(id > 0);

            return false;
        }

        public bool CheckLabWorkExist(string name)
        {
            Contract.Requires(name != "");

            return false;
        }

        public bool CheckLabVariantExist(long id)
        {
            Contract.Requires(id > 0);

            return false;
        }

		public bool CheckLabVariantExist(long labId, string name)
		{
			Contract.Requires(labId > 0);
			Contract.Requires(name != "");

			return false;
		}

        public bool CheckLabVariantBelongLabWork(long labId, long labVarId)
        {
            Contract.Requires(labId > 0);
            Contract.Requires(labVarId > 0);

            return false;
        }

        public bool VerifyCompleteVariant(long variantId)
        {
            Contract.Requires(variantId > 0);

            return false;
        }

        #endregion

        #region Получение разнородной информации по id

        public LabWork GetLabWorkById(long id)
        {
            Contract.Requires(id > 0);
            Contract.Ensures(Contract.Result<LabWork>() != null);

            return default(LabWork);
        }

		public LabVariant GetLabVariantById(long id)
		{
			Contract.Requires(id > 0);
			Contract.Ensures(Contract.Result<LabVariant>() != null);

			return default(LabVariant);
		}

        public LabVariant FindLabVariantById(long id)
        {
            Contract.Requires(id > 0);

            return default(LabVariant);
        }

        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            Contract.Requires(labVarId > 0);
            Contract.Ensures(Contract.Result<TaskVariant[]>() != null);

            return new TaskVariant[0];
        }

        #endregion

		#region Изменение в БД

		public void DeleteEntries(long labWorkId)
		{
			Contract.Requires(labWorkId > 0);
		}

		public void SaveLabWork(LabWork lab)
		{
			Contract.Requires(lab != null);
		}

		public void ModifyLabWork(LabWork lab)
		{
			Contract.Requires(lab != null);
			Contract.Requires(lab.Id > 0);
		}

		public void SaveLabEntries(long labWorkId, long[] tasksId)
		{
			Contract.Requires(labWorkId > 0);
			Contract.Requires(tasksId != null);
		}

		public void DeleteExcessTaskVariantsFromLabVariants(long labWorkId)
		{
			Contract.Requires(labWorkId > 0);
		}

		public void SaveLabVariant(LabVariant labVar)
		{
			Contract.Requires(labVar != null);
		}

		public void ModifyLabVariant(LabVariant labVar)
		{
			Contract.Requires(labVar != null);
		}

		#endregion

		#region Получение Id экземпляров

		public long GetLabWorkIdByName(string name)
		{
			Contract.Requires(name != "");
			Contract.Ensures(Contract.Result<long>() != 0);

			return 0;
		}

		public long GetLabVariantIdByNumber(long labId, string number)
		{
			Contract.Requires(labId > 0);
			Contract.Requires(number != "");
			Contract.Ensures(Contract.Result<long>() != 0);

			return 0;
		}

		#endregion
	}
}
