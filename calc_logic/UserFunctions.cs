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
using System.Collections.ObjectModel;
using System.Linq;

using Mathos;
using Mathos.Parser;

using calc_DB;

namespace calc_logic
{
    public class UserFunctions
    {
        
        private string exp;//pomocnicza

        public static void addFunctionToParser(string name, string exp, int numberOfVariables, MathParser parser)
        {
            //usersFunctions.Add(name);
            //exp = expression;
            parser.LocalFunctions.Add(name, delegate(decimal[] input)
            {
                ///expression = exp;
                string expression = exp; 
                //ograniczenie: w wyrazeniu musi byc symbol operacji pomiedzy liczba z var
                for (int i = 0; i < numberOfVariables; i++)
                {
                    expression = expression.Replace("var" + i, input[i].ToString());
                }
                return parser.Parse(expression);
            });
        }
        
        public static void addFunctionToDataBase(string name, string expression, int numberOfVariables)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nF = new Function { Name = name, Expression = expression, NumberOfParameters = numberOfVariables };
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
      
        public static void addFunction(string name, string expression, int numberOfVariables, MathParser parser)
        {
            addFunctionToDataBase(name, expression, numberOfVariables);
            addFunctionToParser(name, expression, numberOfVariables, parser);            
        }
        
        //dodaje funckje do parsera
        public static void addAverageOfStudies(MathParser parser)
        {
            parser.LocalFunctions.Add("StudyAvg", delegate(decimal[] input)
            {
                decimal weight = 0;//tu trzeba sie zastanowic czy zmienic wszystkie zmienne na double czy ma byc decimal, 
                decimal sum = 0;//dobrze by bylo to ujednolicic
                if (input.Length % 2 == 1)
                    ;                     
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
        //dodaje funckje do parsera
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
        //dodaje funckje do parsera
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
                
        public static Function getFunctionFromDataBase(string name)
        {            
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nf = dc.Functions.FirstOrDefault(f => f.Name.Equals(name));                
                return nf;
            }
        }

        public static ObservableCollection<Function> getAllFunctionsFromDataBase()
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var listOfFunctions = dc.Functions.Select(t => t);
                return new ObservableCollection<Function>(listOfFunctions);
            }
        }

        public static void EditFunction(string name, string newExpression, int newNumberOfVar)
        {
            Calculator.parser.LocalFunctions.Remove(name);

            addFunctionToParser(name, newExpression, newNumberOfVar, Calculator.parser);

            var dc = DBContext.Context;
            var func = dc.Functions.First(t => t.Name.Equals(name));
            
            func.NumberOfParameters = newNumberOfVar;
            func.Expression = newExpression;
            try
            {
                dc.SubmitChanges();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void LoadFunctionsFromDataBase()
        {
            foreach (var fun in getAllFunctionsFromDataBase())
            {
                addFunction(fun.Name, fun.Expression, fun.NumberOfParameters, Calculator.parser);
                //usersFunctions.Add(fun.Name);//nowe
            }
        }
        
        public static void removeFunction(string name)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                try
                {
                    //usersFunctions.Remove(name);//usuniecie z listy nazw
                    Calculator.parser.LocalFunctions.Remove(name);//usuniecie z parsera
                    Function nf = dc.Functions.FirstOrDefault(f => f.Name.Equals(name));//usuniecie z bazy danych
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
                //usersFunctions.Remove(func.Name);//usuniecie z listy nazw
                Calculator.parser.LocalFunctions.Remove(func.Name);//usuniecie z parsera
                dc.Functions.DeleteOnSubmit(func);//usuniecie z bazy danych
                dc.SubmitChanges();
            }
        }

        public static void removeAllFunctions()
        {
            var dc = DBContext.Context;
            try
            {
                foreach (var func in dc.Functions.Select(f => f))
                {
                    Calculator.parser.LocalFunctions.Remove(func.Name);
                    dc.Functions.DeleteOnSubmit(func);
                    dc.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
   
    }
}
