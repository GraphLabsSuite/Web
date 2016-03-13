using System;

namespace GraphLabs.Dal.Ef.Services
{
    /// <summary> Время системы </summary>
    public interface ISystemDateService
    {
        /// <summary> Возвращает текущее системное время </summary>
        DateTime GetDate();

        /// <summary> Устанавливает время системы </summary>
        void SetDate(DateTime newDate);

        DateTime Now();
    }
}
