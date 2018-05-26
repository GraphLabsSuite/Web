using System;
using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Diagnostics.Contracts;

namespace GraphLabs.Site.Models.Question
{
    internal sealed class QuestionModelLoader : AbstractModelLoader<QuestionModel, TestQuestion>
    {
        /// <summary> Загрузчик моделей вопросов </summary>
        public QuestionModelLoader(IEntityQuery query) : base(query) { }

        /// <summary> Загрузить по сущности-прототипу </summary>
        public override QuestionModel Load(TestQuestion question)
        {
            Contract.Requires<ArgumentNullException>(question != null);

            var model = new QuestionModel
            {
                Id = question.Id,
                Question = question.Question,
                Category = question.SubCategory
            };

            return model;
        }
    }
}
