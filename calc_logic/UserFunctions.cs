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
using System.Linq;

using Mathos;
using Mathos.Parser;

using calc_DB;




namespace calc_logic
{
    public class UserFunctions
    {
        private static List<string> usersFunctions = new List<string>();
        private static string exp;

        public static void addFunctionToParser(string name, string expression, int numberOfVariables, MathParser parser)
        {
            usersFunctions.Add(name);
            exp = expression;
            parser.LocalFunctions.Add(name, delegate(decimal[] input)
            {
                expression = exp;
                //ograniczenie: w wyrazeniu musi byc symbol operacji pomiedzy liczba z var
                for (int i = 0; i < numberOfVariables; i++)
                {
                    expression = expression.Replace("var" + i, input[i].ToString());
                }
                return parser.Parse(expression);
            });
        }
        
        public static void addFunction(string name, string expression, int numberOfVariables, MathParser parser)
        {
            addFunctionToDataBase(name, expression, numberOfVariables);
            addFunctionToParser(name, expression, numberOfVariables, parser);
            
        }

        public static void addAverageOfStudies(MathParser parser)
        {
            parser.LocalFunctions.Add("StudyAvg", delegate(decimal[] input)
            {
                decimal weight = 0;//tu trzeba sie zastanowic czy zmienic wszystkie zmienne na double czy ma byc decimal, 
                decimal sum = 0;//dobrze by bylo to ujednolicic
                if (input.Length % 2 == 1)
                    ;//ciepnac wyjatkiem                     
                else
                {
                    for (int i = 0; i < input.Length; i += 2)
                    {
                        weight += input[i + 1];
                        sum += input[i] * input[i + 1];
                    }
                }
                return sum / weight;
            });

        }

        public static void addSum(MathParser parser, string expression)
        {
            decimal sum = 0;
            parser.LocalFunctions.Add("Σ", delegate(decimal[] input)
            {
                string tmp = expression;
                for (decimal i = input[0]; i <= input[1]; i++)
                {
                    tmp = tmp.Replace("var", i.ToString());
                    sum += parser.Parse(tmp);
                    tmp = expression;
                }
                return sum;
            });
        }

        public static void addProduct(MathParser parser)
        {
            parser.LocalFunctions.Add("π", delegate(decimal[] input)
            {
                decimal product = 1;
                for (int i = 0; i < input.Length; i++)
                {
                    product *= input[i];
                }
                return product;
            });
        }

        public static void addFunctionToDataBase(string name, string expression, int numberOfVariables)
        {   
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nF = new Function { Name = name, Expression = expression, NumberOfParameters = numberOfVariables}; 
                try
                {
                    dc.Functions.InsertOnSubmit(nF);
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static Function getFunctionFromDataBase(string name)
        {            
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nf = dc.Functions.First(f => f.Name.Equals(name));                
                return nf;
            }
        }

        public static List<Function> getAllFunctionsFromDataBase()
        {
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var listOfFunctions = dc.Functions.ToList();
                return listOfFunctions;
            }
        }

        public static void initializeParser(MathParser parser)
        {
            addAverageOfStudies(parser);
            addProduct(parser);
            foreach (var fun in getAllFunctionsFromDataBase())
            {
                addFunction(fun.Name, fun.Expression, fun.NumberOfParameters, parser);
            }
        }

        public static List<string> getUsersFunctions()
        {
            return usersFunctions;
        }

        public static void removeFunction(string name)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                try
                {
                    usersFunctions.Remove(name);
                    Function nf = dc.Functions.First(f => f.Name.Equals(name));
                    dc.Functions.DeleteOnSubmit(nf);
                    dc.SubmitChanges();
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static void removeFunction(Function func)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                usersFunctions.Remove(func.Name);
                dc.Functions.DeleteOnSubmit(func);
                dc.SubmitChanges();
            }
        }
            
    }
}
