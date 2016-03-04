using System.Data.Entity;

namespace GraphLabs.DomainModel.EF.Contexts
{
    /// <summary> Тесты </summary>
    public interface ITestsContext
    {
        /// <summary> Тестовые вопросы </summary>
        DbSet<TestQuestion> TestQuestions { get; }

        /// <summary> Варианты ответов </summary>
        DbSet<AnswerVariant> AnswerVariants { get; }

        /// <summary> Категории вопросов </summary>
        DbSet<Category> Categories { get; }
    }
}