namespace GraphLabs.DomainModel.Infrastructure
{
    /// <summary> Ошибка валидации сущности </summary>
    public sealed class EntityValidationError
    {
        public string Property { get; private set; }
        public string Error { get; private set; }

        /// <summary> Ошибка валидации сущности </summary>
        public EntityValidationError(string error, string property)
        {
            Error = error;
            Property = property;
        }
    }
}
