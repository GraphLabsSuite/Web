using GraphLabs.Site.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class SurveyCreatingModel
    {
        public string Question { get; set; }
        public List<KeyValuePair<String, bool>> QuestionOptions { get; set; }

		public bool IsValid
		{
			get
			{
				// TODO валидация
				return true;
			}
		}

		public SurveyCreatingModel()
		{
			QuestionOptions = new List<KeyValuePair<String, bool>>();
		}

		public SurveyCreatingModel(string question, Dictionary<string, bool> questionOptions)
		{
			Question = question;
			QuestionOptions = questionOptions.ToList();
		}		
    }
}