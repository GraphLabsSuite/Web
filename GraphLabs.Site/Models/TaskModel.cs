using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    /// <summary> Данные о задании </summary>
    public class TaskModel
    {
        /// <summary> Id </summary>
        public long Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Темы")]
        public string Sections { get; set; }

        [Display(Name = "Версия")]
        public string Version { get; set; }

        [Display(Name = "Примечание")]
        public string Note { get; set; }

        /// <summary> Ctor. </summary>
        public TaskModel(Task task, bool cutNote = false)
        {
            Name = task.Name;
            Sections = task.Sections;
            Version = task.Version;

            const int NOTE_CUT_LENGTH = 30;
            if (cutNote && task.Note.Length > NOTE_CUT_LENGTH)
            {
                Note = string.Format("{0}...", task.Note.Substring(0, NOTE_CUT_LENGTH).Trim());
            }
            else
            {
                Note = task.Note;
            }
        }
    }
}