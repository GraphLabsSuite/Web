﻿using GraphLabs.DomainModel;
using GraphLabs.Site.Models.Infrastructure;
using System.Linq;

namespace GraphLabs.Site.Models.Question
{
    public sealed class QuestionListModel : ListModelBase<QuestionModel>
    {
        private readonly IEntityQuery _query;
        private readonly IEntityBasedModelLoader<QuestionModel, TestQuestion> _modelLoader;

        /// <summary> Модель списка вопросов </summary>
        public QuestionListModel(IEntityQuery query, IEntityBasedModelLoader<QuestionModel, TestQuestion> modelLoader)
        {
            _query = query;
            _modelLoader = modelLoader;
        }

        /// <summary> Загружает вопросы </summary>
        protected override QuestionModel[] LoadItems()
        {
            return _query.OfEntities<TestQuestion>()
                .ToArray()
                .Select(l => _modelLoader.Load(l))
                .OrderBy(i => i.Category.Name)
                .ToArray();
        }
    }
}
