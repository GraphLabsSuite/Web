using System.Configuration;

namespace GraphLabs.Tests.DomainModel
{
    public abstract class TestBase
    {
        protected static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DirectConnectionString"].ConnectionString;
        }
    }
}