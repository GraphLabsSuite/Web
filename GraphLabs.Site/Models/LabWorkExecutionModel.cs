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

        /// <summary> Задания </summary>
        public TaskExecutionModel[] Tasks { get; set; }

        /// <summary> Текущее задание </summary>
        public int CurrentTask { get; set; }

        /// <summary> Конструктор модели </summary>
        public LabWorkExecutionModel(Guid sessionGuid, LabWork lab, IEnumerable<TaskExecutionModel> variants)
        {
            LabName = lab.Name;
            LabId = lab.Id;
            Tasks = variants.ToArray();
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