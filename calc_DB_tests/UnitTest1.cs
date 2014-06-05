using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq;
using System.Collections.ObjectModel;
using calc_DB;
using calc_logic;

namespace calc_DB_tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Start()
        {

            var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
            dc.CreateIfNotExists();
            dc.LogDebug = true;
        }
        
        [TestMethod]
        public void AddingAndGettingFunctionFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunctionToDataBase("formulaA", "var0+var1", 2);
            Function func = UserFunctions.getFunctionFromDataBase("formulaA");
            Assert.AreEqual("formulaA", func.Name);
            Assert.AreEqual("var0+var1", func.Expression);
            Assert.AreEqual(2, func.NumberOfParameters);
        }

        [TestMethod]
        public void AddingAndGettingConstFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserConsts.addConstToDataBase("stalaA", 2.2);
            Const con = UserConsts.getConstFromDataBase("stalaA");
            Assert.AreEqual("stalaA", con.Name);
            Assert.AreEqual(2.2, con.Value);
        }

        [TestMethod]
        public void AddingAndGettingFunctionsFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunctionToDataBase("formulaA", "var0+var1", 2);
            UserFunctions.addFunctionToDataBase("formulaB", "var0-var1", 2);
            var functions = UserFunctions.getAllFunctionsFromDataBase();
            Assert.AreEqual(2, functions.ToArray().Length);
            Assert.AreEqual("formulaA", functions.ElementAt(0).Name);
            Assert.AreEqual("var0+var1", functions.ElementAt(0).Expression);
            Assert.AreEqual(2, functions.ElementAt(0).NumberOfParameters);
            Assert.AreEqual("formulaB", functions.ElementAt(1).Name);
            Assert.AreEqual("var0-var1", functions.ElementAt(1).Expression);
            Assert.AreEqual(2, functions.ElementAt(1).NumberOfParameters);
        }

        [TestMethod]
        public void AddingAndGettingConstsFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserConsts.addConstToDataBase("stalaA", 2.2);
            UserConsts.addConstToDataBase("stalaB", 3.3);
            var consts = UserConsts.getAllConstsFromDataBase();
            Assert.AreEqual(2, consts.ToArray().Length);
            Assert.AreEqual("stalaA", consts.ElementAt(0).Name);
            Assert.AreEqual(2.2, consts.ElementAt(0).Value);
            Assert.AreEqual("stalaB", consts.ElementAt(1).Name);
            Assert.AreEqual(3.3, consts.ElementAt(1).Value);
        }

        [TestMethod]
        public void RemovingFunctionFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunctionToDataBase("formulaA", "var0+var1", 2);
            UserFunctions.addFunctionToDataBase("formulaB", "var0-var1", 2);
            UserFunctions.removeFunction("formulaA");
            var functions = UserFunctions.getAllFunctionsFromDataBase();
            Assert.AreEqual(1, functions.ToArray().Length);
            Assert.AreEqual("formulaB", functions.ElementAt(0).Name);
            Assert.AreEqual("var0-var1", functions.ElementAt(0).Expression);
            Assert.AreEqual(2, functions.ElementAt(0).NumberOfParameters);
        }

        [TestMethod]
        public void RemovingConstFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserConsts.addConstToDataBase("stalaA", 2.2);
            UserConsts.addConstToDataBase("stalaB", 3.3);
            UserConsts.removeConst("stalaA");
            var consts = UserConsts.getAllConstsFromDataBase();
            Assert.AreEqual(1, consts.ToArray().Length);
            Assert.AreEqual("stalaB", consts.ElementAt(0).Name);
            Assert.AreEqual(3.3, consts.ElementAt(0).Value);
        }

        [TestMethod]
        public void RemovingAllFunctionsFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunctionToDataBase("formulaA", "var0+var1", 2);
            UserFunctions.addFunctionToDataBase("formulaB", "var0-var1", 2);
            UserFunctions.removeAllFunctions();
            var functions = UserFunctions.getAllFunctionsFromDataBase();
            Assert.AreEqual(0, functions.ToArray().Length);
        }

        [TestMethod]
        public void RemovingAllConstsFromDataBase()
        {
            Calculator.Instance.cleanCalculator();
            UserConsts.addConstToDataBase("stalaA", 2.2);
            UserConsts.addConstToDataBase("stalaB", 3.3);
            UserConsts.removeAllConst();
            var consts = UserConsts.getAllConstsFromDataBase();
            Assert.AreEqual(0, consts.ToArray().Length);
        }

        [TestMethod]
        public void BasicMathematicalOperation()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunction("formulaA", "(2*2+2/2)-1", 0, Calculator.parser);
            Calculator.Instance.setCurrentFunction("formulaA");
            Calculator.Instance.CalculateFormule();
            double result = Calculator.Instance.getResults().ToArray().ElementAt(0);
            Assert.AreEqual(4.0, result);
        }

        [TestMethod]
        public void BasicMathematicalOperation2()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunction("formulaA", "cos(0)+log(4,2)", 0, Calculator.parser);
            Calculator.Instance.setCurrentFunction("formulaA");
            Calculator.Instance.CalculateFormule();
            double result = Calculator.Instance.getResults().ToArray().ElementAt(0);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void DefineFormula()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunction("formulaA", "var0+var1", 2, Calculator.parser);
            Calculator.Instance.setCurrentFunction("formulaA");
            Calculator.Instance.addNewValueToVariable("var0", 1);
            Calculator.Instance.addNewValueToVariable("var0", 2);
            Calculator.Instance.addNewValueToVariable("var1", 0);
            Calculator.Instance.addNewValueToVariable("var1", 1);
            Calculator.Instance.CalculateFormule();
            Assert.AreEqual(4, Calculator.Instance.getResults().ToArray().Length);
            Assert.AreEqual(1.0, Calculator.Instance.getResults().ToArray().ElementAt(0));
            Assert.AreEqual(2.0, Calculator.Instance.getResults().ToArray().ElementAt(1));
            Assert.AreEqual(2.0, Calculator.Instance.getResults().ToArray().ElementAt(2));
            Assert.AreEqual(3.0, Calculator.Instance.getResults().ToArray().ElementAt(3));

        }
        
        [TestMethod]
        public void DefineAdvenced2()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunction("formulaA", "var0+var1", 2, Calculator.parser);
            UserFunctions.addFunction("formulaB", "var0-var1", 2, Calculator.parser);
            UserFunctions.addFunction("formulaC", "formulaA(var0,var1)+formulaB(var0,var1)", 2, Calculator.parser);
            Calculator.Instance.setCurrentFunction("formulaC");
            Calculator.Instance.addNewValueToVariable("var0", 0);
            Calculator.Instance.addNewValueToVariable("var0", 1);
            Calculator.Instance.addNewValueToVariable("var1", 4);
            Calculator.Instance.addNewValueToVariable("var1", 5);
            Calculator.Instance.CalculateFormule();
            Assert.AreEqual(4, Calculator.Instance.getResults().ToArray().Length);
            Assert.AreEqual(0, Calculator.Instance.getResults().ToArray().ElementAt(0));
            Assert.AreEqual(0, Calculator.Instance.getResults().ToArray().ElementAt(1));
            Assert.AreEqual(1, Calculator.Instance.getResults().ToArray().ElementAt(2));
            Assert.AreEqual(1, Calculator.Instance.getResults().ToArray().ElementAt(3));
        }
        
        [TestMethod]
        public void DefineFormulaWithOneParameter()
        {
            Calculator.Instance.cleanCalculator();
            UserFunctions.addFunction("formula", "var0+7", 1, Calculator.parser);
            Calculator.Instance.refreshCalculator();
            Calculator.Instance.setCurrentFunction("formula");
        }
    }        
}
