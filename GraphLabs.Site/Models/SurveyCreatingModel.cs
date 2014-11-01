using GraphLabs.Site.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
    public class QuestionOptions
    {
        public string Answer;
        public bool IsCorrect;

        public QuestionOptions()
        {

        }

        public QuestionOptions(string ans, bool corr)
        {
            this.Answer = ans;
            this.IsCorrect = corr;
        }
    }

    public class SurveyCreatingModel
    {
        public string Question { get; set; }
        public QuestionOptions[] QuestionOptions { get; set; }
    }
}