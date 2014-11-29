using GraphLabs.Site.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class SurveyCreatingViewModel : BaseViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать текст вопроса!")]
        [MinLength(3, ErrorMessage = "Текст вопроса слишком короткий!")]
        [MaxLength(3000, ErrorMessage = "Текст вопроса слишком длинный!")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Укажите варианты ответа")]
        public List<KeyValuePair<String, bool>> QuestionOptions { get; set; }

		public bool IsValid
		{
			get
			{
                //длина вопроса от 3 до 3000 символов
                if ((this.Question.Length < 3) || (this.Question.Length > 3000))
                    return false;
                
                //пусть количество ответов от 2 до 20
                if ((this.QuestionOptions.Count < 2) || (this.QuestionOptions.Count > 20))
                    return false;

                //проверка корректности ответов
                var correctCount = 0;
                foreach(KeyValuePair<String, bool> answer in this.QuestionOptions)
                {
                    //сичтаем корректные ответы
                    if (answer.Value)
                        ++correctCount;
                    //длина ответа от 1 до 300 символов
                    if ((answer.Key.Length < 1) || (answer.Key.Length > 3000))
                        return false;
                }

                //не выбрано ни одного верного ответа
                if (correctCount == 0)
                    return false;

                //все проверки пройдены
				return true;
			}
		}

		public SurveyCreatingViewModel()
		{
			QuestionOptions = new List<KeyValuePair<String, bool>>();
		}

		public SurveyCreatingViewModel(string question, Dictionary<string, bool> questionOptions)
		{
			Question = question;
			QuestionOptions = questionOptions.ToList();
		}		

        public void Save()
        {
            //TODO сохраНЯШКА
        }
        //репозиторий если есть такой вопрос, то обновить
        //если нет, то сохранить
        //также может быть левое
    }
}