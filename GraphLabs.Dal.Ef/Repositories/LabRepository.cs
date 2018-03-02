using System;
using System.Data.Entity;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Contexts;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.Site.Utils;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с лабами </summary>
    internal class LabRepository : RepositoryBase, ILabRepository
    {
		private readonly ITasksContext _taskRepository;

        /// <summary> Репозиторий с лабораторными работами </summary>
        public LabRepository(GraphLabsContext context, ITasksContext taskRepository)
            : base(context)
        {
			_taskRepository = taskRepository;
        }
		
        #region Получение массива вариантов лабораторной работы

        /// <summary> Получить варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetLabVariantsByLabWorkId(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(lv => lv.LabWork.Id == id)
                .ToArray();
        }

        /// <summary> Получить ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetDemoLabVariantsByLabWorkId(long labId)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(lv => lv.LabWork.Id == labId)
                .Where(lv => lv.IntroducingVariant)
                .ToArray();
        }

        /// <summary> Получить готовые ознакомительные варианты лабораторной работы по id лабораторной работы </summary>
        public LabVariant[] GetCompleteDemoLabVariantsByLabWorkId(long labId)
        {
            CheckNotDisposed();

            return GetDemoLabVariantsByLabWorkId(labId)
                .Where(lv => VerifyCompleteVariant(lv.Id))
                .ToArray();
        }

        #endregion

        #region Проверки

        /// <summary> Проверить существование лабораторной работы </summary>
        public bool CheckLabWorkExist(long id)
        {
            CheckNotDisposed();

            LabWork lab = Context.LabWorks.SingleOrDefault(l => l.Id == id);

            return (lab == null ? false : true);
        }

        /// <summary> Проверить существование лабораторной работы по имени</summary>
        public bool CheckLabWorkExist(string name)
        {
            CheckNotDisposed();

            LabWork lab = Context.LabWorks.SingleOrDefault(l => l.Name == name);

            return (lab == null ? false : true);
        }

        /// <summary> Проверить существование варианта лабораторной работы по Id</summary>
        public bool CheckLabVariantExist(long id)
        {
            CheckNotDisposed();

            LabVariant labVariant = Context.LabVariants.SingleOrDefault(l => l.Id == id);

            return (labVariant == null ? false : true);
        }

		/// <summary> Проверить существование варианта лабораторной работы по имени</summary>
		public bool CheckLabVariantExist(long labId, string name)
		{
			CheckNotDisposed();

			LabVariant labVariant = Context.LabVariants
										.Where(lv => lv.LabWork.Id == labId)
										.SingleOrDefault(l => l.Number == name);

			return (labVariant == null ? false : true);
		}

        /// <summary> Проверить принадлежность варианта л.р. лабораторной работе </summary>
        public bool CheckLabVariantBelongLabWork(long labId, long labVarId)
        {
            CheckNotDisposed();

            LabVariant labVariant = Context.LabVariants.Where(lv => lv.Id == labVarId).SingleOrDefault(lv => lv.LabWork.Id == labId);

            return (labVariant == null ? false : true);
        }

        /// <summary> Проверка соответствия варианта лабораторной работы содержанию работы </summary>
        public bool VerifyCompleteVariant(long variantId)
        {
            CheckNotDisposed();

            long labWorkId = Context.LabVariants
                .Where(v => v.Id == variantId)
                .Select(v => v.LabWork.Id)
                .Single();

            long[] labEntry = Context.LabEntries
                .Where(e => e.LabWork.Id == labWorkId)
                .Select(e => e.Task.Id)
                .ToArray();

            long[] currentVariantEntry = Context.LabVariants
                .Where(l => l.Id == variantId)
                .SelectMany(t => t.TaskVariants)
                .Select(t => t.Task.Id)
                .ToArray();

            return labEntry.ContainsSameSet(currentVariantEntry);
        }

        #endregion

        #region Получение разнородной информации по id

        /// <summary> Получить лабораторную работу по id </summary>
        public LabWork GetLabWorkById(long id)
        {
            CheckNotDisposed();

            return Context.LabWorks.SingleOrDefault(l => l.Id == id);
        }

		/// <summary> Получить вариант л.р. по id </summary>
		public LabVariant GetLabVariantById(long id)
		{
			CheckNotDisposed();

			return Context.LabVariants
						.Include(lv => lv.LabWork)
						.SingleOrDefault(lv => lv.Id == id);
		}

        /// <summary> Найти вариант лабораторной работы по id </summary>
        public LabVariant FindLabVariantById(long id)
        {
            CheckNotDisposed();

            return Context.LabVariants.SingleOrDefault(lv => lv.Id == id);
        }

        /// <summary> Получить варианты заданий с заданиями варианта лабораторной работы </summary>
        public TaskVariant[] GetTaskVariantsByLabVarId(long labVarId)
        {
            CheckNotDisposed();

            return Context.LabVariants
                .Where(v => v.Id == labVarId)
                .SelectMany(v => v.TaskVariants)
                .Include(v => v.Task)
                .ToArray();
        }

        #endregion

		#region Изменение в БД

		/// <summary> Удаление содержания лабораторной работы </summary>
		public void DeleteEntries(long labWorkId)
		{
			CheckNotDisposed();

			var entries = (from e in Context.LabEntries
						   where e.LabWork.Id == labWorkId
						   select e);
			foreach (var e in entries)
			{
				Context.LabEntries.Remove(e);
			}

			Context.SaveChanges();
		}

		/// <summary> Сохранение лабораторной работы </summary>
		public void SaveLabWork(LabWork lab)
		{
			CheckNotDisposed();

			Context.LabWorks.Add(lab);
			Context.SaveChanges();
		}

		/// <summary> Изменение лабораторной работы </summary>
		public void ModifyLabWork(LabWork lab)
		{
			CheckNotDisposed();

			Context.Entry(lab).State = EntityState.Modified;
			Context.SaveChanges();
		}

		/// <summary> Сохранение содержания лабораторной работы </summary>
		public void SaveLabEntries(long labWorkId, long[] tasksId)
		{
			CheckNotDisposed();

			int i = 0;
			LabWork lab = GetLabWorkById(labWorkId);

			foreach (var task in tasksId.Distinct().Select(id => _taskRepository.Tasks.Find(id)))
			{
				LabEntry entry = Context.LabEntries.Create();
			    entry.LabWork = lab;
			    entry.Order = ++i;
			    entry.Task = task;
			    
                Context.LabEntries.Add(entry);
			}

			Context.SaveChanges();
		}

		/// <summary> Удаляет лишние варианты заданий из вариантов лабораторной работы для соответствия содержанию </summary>
		public void DeleteExcessTaskVariantsFromLabVariants(long labWorkId)
		{
			CheckNotDisposed();

			bool flag;
			var labTasks = (from e in Context.LabEntries
							where e.LabWork.Id == labWorkId
							select e.Task.Id);
			var labVariants = (from lv in Context.LabVariants
							   where lv.LabWork.Id == labWorkId
							   select lv);

			foreach (var labVar in labVariants)
			{
				var varTasks = labVar.TaskVariants.ToDictionary(tv => tv.Task);
				flag = false;
				foreach (var tv in varTasks)
				{
					if (!labTasks.Contains(tv.Key.Id))
					{
						labVar.TaskVariants.Remove(tv.Value);
						flag = true;
					}
				}

				if (flag)
				{
					Context.Entry(labVar).State = EntityState.Modified;
				}
			}

			Context.SaveChanges();
		}

		/// <summary> Сохранение варианта л.р. </summary>
		public void SaveLabVariant(LabVariant labVar)
		{
			CheckNotDisposed();

			Context.LabVariants.Add(labVar);
			Context.SaveChanges();
		}

		/// <summary> Изменение варианта л.р. </summary>
		public void ModifyLabVariant(LabVariant labVar)
		{
			CheckNotDisposed();

			Context.Entry(labVar).State = EntityState.Modified;
			Context.SaveChanges();
		}

		#endregion

		#region Получение Id экземпляров

		/// <summary> Получить id лабораторной работы по ее имени </summary>
		public long GetLabWorkIdByName(string name)
		{
			CheckNotDisposed();

			return Context.LabWorks.Single(l => l.Name == name).Id;
		}

		/// <summary> Получить id варианта л.р. по его имени </summary>
		public long GetLabVariantIdByNumber(long labId, string number)
		{
			CheckNotDisposed();

			return Context.LabVariants
						.Where(lv => lv.LabWork.Id == labId)
						.Single(lv => lv.Number == number)
						.Id;
		}

		#endregion
	}
}
