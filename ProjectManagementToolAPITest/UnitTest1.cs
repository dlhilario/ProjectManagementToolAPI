using projectManagementToolWebAPI;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace projectManagementToolWebAPI.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        PGMTWebService _PMTWebAPI;
        public UnitTest1()
        {
            _PMTWebAPI = new PGMTWebService();
        }

        [TestMethod()]
        public void LoginTest()
        {
            var result =_PMTWebAPI.Login("dlhilario", "ando1977");
            Assert.IsNotNull(result, "Unable to login");
        }
    }
}

