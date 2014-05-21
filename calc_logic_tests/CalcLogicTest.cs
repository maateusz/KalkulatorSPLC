using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;

using NUnit.Framework;
using calc_logic;
using calc_DB;


namespace calc_logic_tests
{
    [TestFixture]
    public class CalcLogicTest
    {
        Calculator calculator = Calculator.Instance;

        [Test]
        public void IsNotNull_ReferenceToCalculatorShouldNotBNull()
        {
            Assert.IsNotNull(calculator);
        }

        [TestCase("2+2", 4)]
        [TestCase("2*2", 4)]
        [TestCase("2-2", 0)]
        [TestCase("2.0/2.0", 1.0)]
        public void SimpleExpressionWithoutVariables(string exp, double result)
        {
            calculator.cleanCalculator();
            calculator.setExpression(exp);
            calculator.calculate();
            //Assert.IsNull(calculator.getResults());
            Assert.AreEqual(result, calculator.getResults().ToArray()[0]);
        }


        [Test]
        public void ExpressionWithUsersFunction()
        {
            calculator.cleanCalculator();
            Variable var0 = new Variable("x", 1);
            Variable var1 = new Variable("y", 1, 2, 1);
            Variable var2 = new Variable("z", new List<double> { 1, 2, 3 });
            calculator.addVariable(var0);
            calculator.addVariable(var1);
            calculator.addVariable(var2);
            UserFunctions.addFunction("formula", "var0+var1+var2", 3, calculator.getParser());
            calculator.setExpression("formula(x,y,z)+100");
            calculator.calculate();
            foreach (var res in calculator.getResults())
                System.Console.WriteLine(res);
            Assert.AreEqual(103, calculator.getResults().ToArray()[0]);
        }

        [Test]
        public void ExpressionWithManyUsersFunctions()
        {
            calculator.cleanCalculator();
            Variable var0 = new Variable("x", 1);
            Variable var1 = new Variable("y", 1, 2, 1);
            Variable var2 = new Variable("z", new List<double> { 1, 2, 3 });
            calculator.addVariable(var0);
            calculator.addVariable(var1);
            calculator.addVariable(var2);
            UserFunctions.addFunction("formulaA", "var0+var1+var2", 3, calculator.getParser());
            UserFunctions.addFunction("formulaB", "var0+var1+var2", 3, calculator.getParser());
            calculator.setExpression("formulaA(x,y,z)+formulaB(x,y,z)");
            calculator.calculate();
            foreach (var res in calculator.getResults())
                System.Console.WriteLine(res);
            Assert.AreEqual(8, calculator.getResults().ToArray()[1]);
        }

        [Test]
        public void ExpressionWithFunctionAsAParameterOfOtherFunction()
        {
            calculator.cleanCalculator();
            Variable var0 = new Variable("x", 1);
            Variable var1 = new Variable("y", 1, 2, 1);
            Variable var2 = new Variable("z", new List<double> { 1, 2, 3 });
            calculator.addVariable(var0);
            calculator.addVariable(var1);
            calculator.addVariable(var2);
            UserFunctions.addFunction("formulaA", "var0+var1+var2", 3, calculator.getParser());
            UserFunctions.addFunction("formulaB", "var0+var1+var2", 3, calculator.getParser());
            calculator.setExpression("formulaA(formulaB(x,y,z),y,z)");
            calculator.calculate();
            foreach (var res in calculator.getResults())
                System.Console.WriteLine(res);
            Assert.AreEqual(7, calculator.getResults().ToArray()[1]);
        }

        [Test]
        public void UsersConstsInExpression()
        {
            calculator.cleanCalculator();
            calc_logic.UserConsts.addConstToParser("A", 2, calculator.getParser());
            calc_logic.UserConsts.addConstToParser("B", 2, calculator.getParser());
            calculator.setExpression("A+B");
            calculator.calculate();
            //Assert.IsNull(calculator.getResults());
            Assert.AreEqual(4, calculator.getResults().ToArray()[0]);
        }
/*
        [Test]
        public void TestDB()
        {
            var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
            dc.CreateIfNotExists();
            Assert.IsTrue(true);
        }*/
    }
}
