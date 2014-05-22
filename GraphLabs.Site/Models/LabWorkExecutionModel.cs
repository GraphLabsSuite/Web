using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    /// <summary> Модель лабы </summary>
    public class LabWorkExecutionModel
    {
        private ITaskExecutionModelFactory TaskExecutionModelFactory
        {
            get { return DependencyResolver.Current.GetService<ITaskExecutionModelFactory>(); }
        }

        /// <summary> Название </summary>
        public string LabName { get; set; }

        /// <summary> Id </summary>
        public long LabId { get; set; }

        /// <summary> Задания </summary>
        public TaskExecutionModel[] Tasks { get; set; }

        /// <summary> Текущее задание </summary>
        public int CurrentTask { get; set; }

        /// <summary> Модель лабы </summary>
        public LabWorkExecutionModel(Guid sessionGuid, string labName, long labId, IEnumerable<TaskVariant> variants)
        {
            LabName = labName;
            LabId = labId;
            Tasks = variants
                .Select(v => TaskExecutionModelFactory.CreateForDemoMode(
                    sessionGuid,
                    v.Task.Name,
                    v.Task.Id,
                    v.Id,
                    labId))
                .ToArray();
        }

        /// <summary> Установить текущее задание </summary>
        public void SetCurrent(int num)
        {
            CurrentTask = num;
        }
    }
}