using System.Configuration;

namespace GraphLabs.Tests.DomainModel
{
    public abstract class TestBaseBase
    {
        protected static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DirectConnectionString"].ConnectionString;
        }
    }
}