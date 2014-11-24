using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using NUnit.Framework;

namespace GraphLabs.Tests.DomainModel
{
    [TestFixture]
    public class UserExtensionsTests : GraphLabsTestBase
    {
        [Test]
        public void ShortNameTest()
        {
            const string NAME = "Имя";
            const string SURNAME = "Фамилия";
            const string FATHERNAME = "Отчество";
            const string SHORTNAME = "Фамилия И.О.";
            const string SHORT_WO_FATHERNAME = "Фамилия И.";

            var user = new User
                           {
                               Name = NAME,
                               Surname = SURNAME,
                           };
            Assert.AreEqual(SHORT_WO_FATHERNAME, user.GetShortName());

            user.FatherName = FATHERNAME;
            Assert.AreEqual(SHORTNAME, user.GetShortName());
        }
    }
}
