using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.Question
{
    sealed class QuestionModelLoader : AbstractModelLoader<QuestionModel, TestQuestion>
    {
        /// <summary> Загрузчик моделей вопросов </summary>
        public QuestionModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override QuestionModel Load(TestQuestion question)
        {
            Contract.Requires(question != null);

            var model = new QuestionModel
            {
                Id = question.Id,
                Question = question.Question,
                LabVariant = question.LabVariant,
                Category = question.Category
            };

            return model;
        }
    }
}
