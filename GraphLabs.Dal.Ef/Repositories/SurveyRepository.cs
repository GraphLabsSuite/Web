using System.Collections.Generic;
using System.Linq;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Dal.Ef.Repositories
{
    /// <summary> Репозиторий с вопросами </summary>
    internal class SurveyRepository : RepositoryBase, ISurveyRepository
    {
        /// <summary> Репозиторий с вопросами </summary>
        public SurveyRepository(GraphLabsContext context)
            : base(context)
        {
        }

        #region Получение массивов вопросов

        /// <summary> Получить все вопросы </summary>
        public TestQuestion[] GetAllQuestions()
        {
            CheckNotDisposed();

            return Context.TestQuestions.ToArray();
        }

        /// <summary> Получить все вопросы в заданной категории </summary>
        public TestQuestion[] GetQuestionByCategory(long categoryId)
        {
            CheckNotDisposed();

            return Context.TestQuestions.Where(tq => tq.SubCategory.Category.Id == categoryId).ToArray();
        }

        public TestQuestion[] GetQuestionsSimilarToString(string criteria)
        {
            CheckNotDisposed();

            return Context.TestQuestions.Where(tq => tq.Question.StartsWith(criteria)).ToArray();
        }

        #endregion

        ///<summary> Сохранение вопроса </summary>
        public void SaveQuestion(long questionId, string question, Dictionary<string, bool> questionOptions, long subCategoryId, long categoryId)
        {
            CheckNotDisposed();

            var questionToEdit = Context.TestQuestions.Where(q => q.Id == questionId).SingleOrDefault();
            var quest = questionToEdit ?? Context.TestQuestions.Create();

            quest.Question = question;
            quest.SubCategory = Context.SubCategories.Single(c => c.Id == subCategoryId);
            quest.SubCategory.Category = Context.Categories.Single(c => c.Id == categoryId);


            if (questionToEdit == null)
            {
                Context.TestQuestions.Add(quest);
            }
            foreach (var answerVar in questionOptions)
            {
                var answerVariant = Context.AnswerVariants.Create();
                answerVariant.TestQuestion = quest;
                answerVariant.IsCorrect = answerVar.Value;
                answerVariant.Answer = answerVar.Key;
                Context.AnswerVariants.Add(answerVariant);
            }

            Context.SaveChanges();
        }

        ///<summary> Сохранение вопроса </summary>
        public void DeleteQuestion(TestQuestion question)
        {
            CheckNotDisposed();

            var deleteQuestion = Context.TestQuestions.Where(q => q == question).First();
            var deleteAnswers = Context.AnswerVariants.Where(q => q.TestQuestion == deleteQuestion);

            Context.TestQuestions.Remove(deleteQuestion);
            if (deleteAnswers != null)
            {
                foreach (var answer in deleteAnswers)
                {
                    Context.AnswerVariants.Remove(answer);
                }
            }

            //bool questionModified = Context.TestQuestions.Where(q => q.Id == questionId).;

            /*var deleteAnswers = Context.AnswerVariants.Where(q => q.TestQuestion == changeQuestion));
            foreach (var answer in deleteAnswers)
            {
                Context.AnswerVariants.Remove(answer);
            }
            /*Context.TestQuestions.Remove(changeQuestion);*/

            /*var quest = Context.TestQuestions.Create();
            quest.Id = questionId;
            quest.Question = question;
            quest.SubCategory = Context.SubCategories.Single(c => c.Id == subCategoryId);
            quest.SubCategory.Category = Context.Categories.Single(c => c.Id == categoryId);

            Context.TestQuestions.Add(quest);

            foreach (var answerVar in questionOptions)
            {
                var answerVariant = Context.AnswerVariants.Create();
                answerVariant.TestQuestion = quest;
                answerVariant.IsCorrect = answerVar.Value;
                answerVariant.Answer = answerVar.Key;
                Context.AnswerVariants.Add(answerVariant);
            }

            Context.SaveChanges();*/
        }

        /// <summary> Получить количество вопросов в категории с id == CategoryId </summary>
        public int GetCategorizesTestQuestionCount(long CategoryId)
        {
            CheckNotDisposed();

            return Context.TestQuestions.Count(tq => tq.SubCategory.Id == CategoryId);
        }
    }
}
