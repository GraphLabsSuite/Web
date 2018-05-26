using System;

namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Тесты </summary>
    [Obsolete("Использовать глобальный контекст IGraphLabsContext")]
    public interface ITestsContext
    {
        /// <summary> Тестовые вопросы </summary>
        IEntitySet<TestQuestion> TestQuestions { get; }

        /// <summary> Варианты ответов </summary>
        IEntitySet<AnswerVariant> AnswerVariants { get; }

        /// <summary> Категории вопросов </summary>
        IEntitySet<Category> Categories { get; }
    }
}