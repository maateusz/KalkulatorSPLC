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

using Mathos;
using Mathos.Parser;


namespace calc_logic
{
    public sealed class Calculator//singleton
    {

        private static Calculator calculator = null;
        private MathParser parser;
        private string expression;
        private List<double> results;
        private List<Variable> variables;

        //public static void Main() { ;}//to trzeba wyciepnac, bo to tylko do kompilacji, bo cos mi tam nie dzialalo

        public static Calculator Instance
        {
            get
            {
                if (calculator == null)
                {
                    calculator = new Calculator();
                }
                return calculator;
            }
        }

        private Calculator()
        {
            parser = new MathParser();
            expression = string.Empty;
            results = new List<double>();
            variables = new List<Variable>();
        }

        public void setExpression(string expression)
        {
            this.expression = expression;
        }

        public string getExpression()
        {
            return expression;
        }

        public void calculate(int i = 0)
        {
            if (variables.Count == 0)
            {
                results.Add((double)(parser.Parse(expression)));
                return;
            }
            if (i == variables.Count)
                return;
            else
            {
                foreach (double value in variables[i].getValues())
                {
                    parser.ProgrammaticallyParse("let " + variables[i].getName() + " = " + value);//albo lepiej raz przypisac get NAme do imienia
                    calculate(i + 1);
                    if (i == variables.Count - 1)//if (i != 0 && i!=1)                        
                        results.Add((double)(parser.Parse(expression)));
                }
            }
        }

        public MathParser getParser()
        {
            return parser;
        }

        public void addVariable(Variable newVariable)
        {
            variables.Add(newVariable);
        }

        public List<double> getResults()
        {
            return results;
        }
        
        public void removeVariables()
        {
            //sprawdzic czy poprawnie ???
            variables.RemoveAll(v => true);
            
        }

        public void removeResults()
        {
            //sprawdzic czy poprawnie ???
            results.RemoveAll(v => true);
        }

        public void cleanCalculator()
        {
            removeVariables();
            removeResults();
            foreach(string func in UserFunctions.getUsersFunctions())
                parser.LocalFunctions.Remove(func);//jakies inteligentne usuwanie tylko tych dodanych przez uzytkownika funkcji, zmiennych i stalych
            foreach (string con in UserConsts.getUsersConsts())
                parser.LocalVariables.Remove(con);
        
        }


    }
}
