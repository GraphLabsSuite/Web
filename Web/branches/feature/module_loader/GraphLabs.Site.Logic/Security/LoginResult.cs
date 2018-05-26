namespace GraphLabs.Site.Logic.Security
{
    /// <summary> Результат логина </summary>
    public enum LoginResult
    {
        /// <summary> Успех </summary>
        Success,

        /// <summary> Неверный логин/пароль </summary>
        InvalidLoginPassword,

        /// <summary> Пользователь залогинен на другой машине </summary>
        LoggedInWithAnotherSessionId
    }
}