using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using calc_DB;
using calc_logic;
namespace calc_DB_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddingAndGettingFunctionFromDataBase()
        {
            var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
            dc.CreateIfNotExists();
            dc.LogDebug = true;
            UserFunctions.addFunctionToDataBase("formulaA", "var0+var1", 2);
            UserFunctions.addFunctionToDataBase("formulaB", "4/5", 0);
            Function func = UserFunctions.getFunctionFromDataBase("formulaB");
            Assert.AreEqual("formulaB", func.Name);
        }

        [TestMethod]
        public void AddingAndGettingConstFromDataBase()
        {
            var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
            dc.CreateIfNotExists();
            dc.LogDebug = true;
            UserConsts.addConstToDataBase("myConst", 2.3);
            Const const_ = UserConsts.getConstFromDataBase("myConst");
            Assert.AreEqual("myConst", const_.Name);
        }
    }
}
