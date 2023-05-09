using Microsoft.VisualStudio.TestTools.UnitTesting;
using projectManagementToolWebAPITests;
using System.Linq;

namespace projectManagementToolWebAPI.Tests
{
    [TestClass()]
    public class PGMTWebServiceTests
    {
        TestHelper helper;
          [TestInitialize]
        public void StartTest()
        {
             helper = new TestHelper();
        }

        [TestMethod()]
        public void GetCompaniesTest()
        {
            PGMTWebService pGMTWebService = new PGMTWebService();
            var result = pGMTWebService.GetCompanies(2);
            Assert.IsNotNull(result.Count > 0);
        }

        [TestMethod()]
        public void AddProjectTest()
        {
            PGMTWebService pGMTWebService = new PGMTWebService();
            var result = pGMTWebService.AddProject(helper.userId, helper.TestProjects());
            Assert.IsNotNull(result);
        }
    }
}