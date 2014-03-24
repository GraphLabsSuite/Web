using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Logic.LabsLogic
{
    public class LabsLogic
    {
        private readonly GraphLabsContext _ctx = new GraphLabsContext();

        /// <summary> Получение массива всех лабораторных работ </summary>
        public LabWork[] GetLabWorks()
        {
            return (from lw in _ctx.LabWorks
                    select lw).ToArray();
        }

        /// <summary> Получение массива всех заданий </summary>
        public Task[] GetTasks()
        {
            return (from t in _ctx.Tasks
                    select t).ToArray();
        }

        /// <summary> Возвращает количество лабораторных работ с заданным именем. Если задан Id, то в итоговом результате не учитывается работа с таким Id </summary>
        public int ExistedLabWorksCount(string Name, long Id = 0)
        {
            var existlab = (from l in _ctx.LabWorks
                            where l.Name == Name
                            select l).ToList();
            if (Id != 0)
            {
                existlab = existlab.Where(l => l.Id != Id).ToList();
            }
            return existlab.Count();
        }

        /// <summary> Возвращает лабораторную работу с заданным Id </summary>
        public LabWork GetLabWorkById(long Id)
        {
            return _ctx.LabWorks.Find(Id);
        }

        /// <summary> Возвращает лабораторную работу с заданным Id или создает новую, если Id = 0 </summary>
        public LabWork CreateOrGetLabWorkDependingOnId(long Id = 0)
        {
            if (Id == 0)
            {
                return new LabWork();
            }
            return GetLabWorkById(Id);
        }

        /// <summary> Преобразует строку в дату, если строка пустая, то возвращает null </summary>
        public DateTime? ParseDate(string date = "")
        {
            if (date != "")
            {
                return DateTime.Parse(date);
            }
            else
            {
                return null;
            };
        }

        /// <summary> Удаляет содержание лабораторной работы из бд, возвращает экземпляр лабораторной работы с пустым содержанием </summary>
        public LabWork DeleteEntries(LabWork lab)
        {
            var entries = (from e in _ctx.LabEntries
                           where e.LabWork.Id == lab.Id
                           select e).ToList();
            foreach (var e in entries)
            {
                _ctx.LabEntries.Remove(e);
            }
            _ctx.SaveChanges();

            lab.LabEntries.Clear();
            return lab;
        }

        /// <summary> Сохраняет новую лабораторную работу в бд </summary>
        public void SaveNewLabWork(LabWork lab)
        {
            _ctx.LabWorks.Add(lab);
            _ctx.SaveChanges();
        }

        /// <summary> Сохраняет изменения лабораторной работы в бд </summary>
        public void ModifyExistedLabWork(LabWork lab)
        {
            _ctx.Entry(lab).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        /// <summary> Сохраняет лабораторную работу в бд, как сохранять определяется по логическому флагу </summary>
        public void SaveLabWork(LabWork lab, bool IsNewLab)
        {
            if (IsNewLab)
            {
                SaveNewLabWork(lab);
            }
            else
            {
                ModifyExistedLabWork(lab);
            }
        }

        /// <summary> Удаляет варианты заданий из вариантов лабораторной работы, если содержание лабораторной работы изменилось </summary>
        public void DeleteTasksVariantsFromLabVariants(LabWork labWork)
        {
            var tasksInLab = labWork.LabEntries.Select(e => e.Task).ToArray();
            Boolean flag;

            foreach (var labVariant in labWork.LabVariants)
            {
                var tasksInVariant = labVariant.TaskVariants.ToDictionary(v => v.Task);
                flag = false;
                foreach (var taskVariantPair in tasksInVariant)
                {
                    if (!tasksInLab.Contains(taskVariantPair.Key))
                    {
                        labVariant.TaskVariants.Remove(taskVariantPair.Value);
                        flag = true;
                    }
                }

                if (flag)
                {
                    _ctx.Entry(labVariant).State = EntityState.Modified;
                }
            }
            _ctx.SaveChanges();
        }

        /// <summary> Создает в бд содержание лабораторной работы из Id заданий </summary>
        public void SaveLabEntries(LabWork lab, int[] tasksId)
        {
            var i = 0;
            foreach (var task in tasksId.Distinct().Select(id => GetTaskById(id)))
            {
                Contract.Assert(task != null, "Получен некорректный идентификатор задания");

                LabEntry entry = new LabEntry
                {
                    LabWork = lab,
                    Order = ++i,
                    Task = task
                };
                _ctx.LabEntries.Add(entry);
            }
            _ctx.SaveChanges();
        }

        /// <summary> Возвращает задание с заданным Id </summary>
        public Task GetTaskById(long Id)
        {
            return _ctx.Tasks.Find(Id);
        }

        /// <summary> Возвращает вариант лабораторной работы с заданным Id </summary>
        public LabVariant GetLabVariantById(long id)
        {
            return _ctx.LabVariants.Find(id);
        }

        /// <summary> Возвращает количество вариантов заданной лабораторной работы с заданным именем. Если задан VariantId, то в итоговом результате не учитывается вариант с таким именем </summary>
        public int ExistedLabVariantsCount(string VarName, long LabId, long VariantId = 0)
        {
            var nameCollision = (from v in _ctx.LabVariants
                                 where v.Number == VarName
                                 where v.LabWork.Id == LabId
                                 select v).ToList();
            if (VariantId != 0)
            {
                nameCollision = nameCollision.Where(x => x.Id != VariantId).ToList();
            }
            return nameCollision.Count;
        }

        /// <summary> Возвращает вариант лабораторной работы с заданным Id или создает новую, если Id = 0 </summary>
        public LabVariant CreateOrGetLabVariantDependingOnId(long Id = 0)
        {
            if (Id == 0)
            {
                return new LabVariant();
            }
            return GetLabVariantById(Id);
        }

        /// <summary> Возвращает вариант задания с заданным Id </summary>
        public TaskVariant GetTaskVariantById(long Id)
        {
            return _ctx.TaskVariants.Find(Id);
        }

        /// <summary> Возвращает список вариантов заданий с заданными Id </summary>
        public List<TaskVariant> MakeTaskVariantsList(int[] taskVarId)
        {
            var result = new List<TaskVariant>();
            foreach (var taskVariant in taskVarId.Distinct().Select(id => GetTaskVariantById(id)))
            {
                if (taskVariant == null)
                {
                    throw new Exception();
                }
                result.Add(taskVariant);
            }
            return result;
        }

        /// <summary> Сохраняет новый вариант лабораторной работы в бд </summary>
        public void SaveNewLabVariant(LabVariant labVar)
        {
            _ctx.LabVariants.Add(labVar);
            _ctx.SaveChanges();
        }

        /// <summary> Сохраняет изменения варианта лабораторной работы в бд </summary>
        public void ModifyExistedLabVariant(LabVariant labVar)
        {
            _ctx.Entry(labVar).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        /// <summary> Сохраняет вариант лабораторной работы в бд, как сохранять определяется по логическому флагу </summary>
        public void SaveLabVariant(LabVariant labVar, bool IsNewLabVar)
        {
            if (IsNewLabVar)
            {
                SaveNewLabVariant(labVar);
            }
            else
            {
                ModifyExistedLabVariant(labVar);
            }
        }
    }
}
