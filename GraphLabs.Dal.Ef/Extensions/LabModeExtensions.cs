using System;
using GraphLabs.DomainModel;

namespace GraphLabs.Dal.Ef.Extensions
{
    public static class LabModeExtensions
    {
        /// <summary> Значение -> строка </summary>
        public static string ValueToString(this LabExecutionMode mode)
        {
            switch (mode)
            {
                case LabExecutionMode.IntroductoryMode:
                    return "Контрольный";
                case LabExecutionMode.TestMode:
                    return "Ознакомительный";
                default:
                    throw new Exception("something missing here");
            }
        }

        public static LabExecutionMode? GetValueByString(string value)
        {
            if (value == null)
                return null;
            switch (value)
            {
                case "Контрольный":
                    return LabExecutionMode.TestMode;
                case "Ознакомительный":
                    return LabExecutionMode.IntroductoryMode;
                default:
                    return null;
            }
        }
    }
}