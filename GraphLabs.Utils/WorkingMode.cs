using System.Web.Configuration;

namespace GraphLabs.Site.Utils
{
    /// <summary> ����� ������ ���������� </summary>
    public static class WorkingMode
    {
        /// <summary> Debug? </summary>
        public static bool IsDebug()
        {
            bool result;
            return bool.TryParse(WebConfigurationManager.AppSettings["IsDebug"], out result) && result;
        }
    }
}