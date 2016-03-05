namespace GraphLabs.DomainModel.Contexts
{
    /// <summary> Тесты </summary>
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