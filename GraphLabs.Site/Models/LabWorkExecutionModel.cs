using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;
using GraphLabs.Dal.Ef;

namespace GraphLabs.Site.Models
{
    /// <summary> Модель лабы </summary>
    public class LabWorkExecutionModel
    {
        /// <summary> Название лабораторной работы </summary>
        public string LabName { get; set; }

        /// <summary> Id лабораторной работы </summary>
        public long LabId { get; set; }

        /// <summary>
        /// Вариант лабораторной работы
        /// </summary>
        public long LabVarId { get; set; }
        
        /// <summary>
        /// Сессия студента
        /// </summary>
        public Guid SessionGuid { get; set; }
        
        /// <summary> Задания </summary>
        public TaskExecutionModel[] Tasks { get; set; }

        /// <summary> Текущее задание </summary>
        public int CurrentTask { get; set; }


        /// <summary> Конструктор модели </summary>
        public LabWorkExecutionModel(Guid sessionGuid, LabWork lab, long labVarId, IEnumerable<TaskExecutionModel> variants)
        {
            SessionGuid = sessionGuid;
            LabName = lab.Name;
            LabId = lab.Id;
            Tasks = variants.ToArray();
            LabVarId = labVarId;
        }

        /// <summary> Проверка завершенности лабораторной работы </summary>
        public bool CheckCompleteLab()
        {
            bool ready = true;

            for (int i = 0; i < Tasks.Length; ++i )
            {
                ready = ready && Tasks[i].IsSolved;
            }

            return ready;
        }

        /// <summary> Автоматическая установка текущего задания </summary>
        public void SetNotSolvedTaskToCurrent()
        {
            for (int i = 0; i < Tasks.Length; ++i)
            {
                if (!Tasks[i].IsSolved)
                {
                    CurrentTask = i;
                    return;
                }
            }
            throw new Exception("Лабораторная работа выполнена");
        }

        public void SetGivenOrFirstTask(long unsolvedTask)
        {
            bool flag = false;
            if (unsolvedTask != 0)
            {
                for (int i = 0; i < Tasks.Length; ++i)
                {
                    if (Tasks[i].TaskId != unsolvedTask)
                    {
                        Tasks[i].IsSolved = true;
                    }
                    else
                    {
                        CurrentTask = i;
                        flag = true;
                        break;
                    }
                }
            }
            else
            {
                CurrentTask = 0;
                return;
            }

            if (!flag)
            {
                CurrentTask = 0;
                foreach (var task in Tasks)
                {
                    task.IsSolved = false;
                }
            }
        }

        /// <summary> Установить текущее задание </summary>
        public void SetCurrentTask(int num)
        {
            if (Tasks[num].IsSolved)
            {
                throw new Exception("Задание выполнено");
            }
            CurrentTask = num;
        }

        /// <summary> Зафиксировать выполнение текущего задания </summary>
        public void SetCurrentTaskToComplete()
        {
            if (Tasks[CurrentTask].IsSolved)
            {
                throw new Exception("Задание уже было выполнено");
            }
            Tasks[CurrentTask].IsSolved = true;
        }
    }
}