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
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using calc_DB;
using System.ComponentModel;


using Mathos;
using Mathos.Parser;


namespace calc_logic
{
    public sealed class Calculator : INotifyPropertyChanged
    {
        private static Calculator calculator = null;

        public static MathParser parser = new MathParser();
        
        private ObservableCollection<Variable> _variables;//lista zmiennych jakie wystapia w formule
        public ObservableCollection<Variable> variables 
        { 
            get
            {
                return _variables;
            }
            set
            {
                if(_variables == value)
                {
                    return;
                }
                else
                {
                    _variables = value;
                    NotifyPropertyChanged("variables");
                }
            }
        }//właściwosc

        
        
        private ObservableCollection<double> _results;//lista wynikow
        public ObservableCollection<double> results 
        {
            get
            {
                return _results;
            }
            set
            {
                if (_results == value)
                    return;
                else
                {
                    _results = value;
                    NotifyPropertyChanged("results");
                }
            }
        }
        
        private Function _currentFormul;//        
        public Function currentFormul
        {
            get
            {
                return _currentFormul;
            }
            set
            {
                if (currentFormul == value)
                    return;
                else
                {
                    _currentFormul = value;
                    NotifyPropertyChanged("currentFormul");
                }
            }

        }//to jest formula ktora bedziemy obliczac
 
        public string expression { get; set; }//przechwouje wyrazenie zawarte w formule       
        public ObservableCollection<Function> functions { get; set; } //lista wszystkich formuł
        public ObservableCollection<Const> userConsts { get; set; } //lista stałych uzytkownika
        
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
            //parser = new Mathparser;
            functions = UserFunctions.getAllFunctionsFromDataBase();
            userConsts = UserConsts.getAllConstsFromDataBase();
            if (functions != null)
            {
                currentFormul = functions.GetEnumerator().Current;
            }
            else
            {
                functions = null;
            }
            expression = string.Empty;
            results = new ObservableCollection<double>();
            variables = new ObservableCollection<Variable>();
            //for (int i = 0; i < 10; i++)
            //{
            //    variables.Add(new Variable("var" + i, 0));
            //}            
        }

        public void setCurrentFunction(string name)//ustawia biezaca formule
        {
            var dc = DBContext.Context;
            var func = dc.Functions.First(f => f.Name.Equals(name));
            currentFormul = func;
            
            variables = new ObservableCollection<Variable>();
            for (int i = 0; i < func.NumberOfParameters; i++)
            {
                variables.Add(new Variable("var" + i, new ObservableCollection<double>()));
            }
            NotifyPropertyChanged("variables");
            results.Clear();
            NotifyPropertyChanged("results");
            CalculateFormule();
            //CalculateFormule();
        }

        public void calculate(int i = 0)//liczy
        {
            
            if (variables.Count == 0)
            {
                results.Add((double)(parser.Parse(expression)));
                return;
            }
            if (i == variables.Count)//tu mozna zamienic na i==currentFunction.numberOfParam
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

        public void CalculateFormule()//ta funckja liczy formułe
        {
            this.expression = currentFormul.Expression;
            results = new ObservableCollection<double>();
            calculate();
            NotifyPropertyChanged("results");
        }

        public MathParser getParser()//zwaraca parser - niepotrzebne bo parser jest static
        {
            return parser;
        }

        //dodaje zmienna, ale chyba nigdzie nie jest wykorzystywana
        public void addVariable(Variable newVariable)
        {
            variables.Add(newVariable);
        }

        //ustawia zmienna na zakres wartosci
        public void addNewRangeToVariable(string varName, double start, double stop, double step)
        {
            var variable = variables.First(v => v.name.Equals(varName));
            variable.setValues(start, stop, step);
            NotifyPropertyChanged("variables");
            //CalculateFormule();
        }

        //dodaje wartosc do zmiennej
        public void addNewValueToVariable(string varName, double value)
        {
            var variable = variables.First(v => v.name.Equals(varName));
            variable.addValue(value);
            NotifyPropertyChanged("variables");
            //CalculateFormule();
        }

        public void InitializeCalculator()
        {
            UserConsts.LoadConstsFromDataBase();
            UserFunctions.LoadFunctionsFromDataBase();
            UserFunctions.addAverageOfStudies(parser);
            UserFunctions.addProduct(parser);
            parser.LocalFunctions.Add("ctg", x => (decimal)(1 / Math.Tan((double)x[0])));//sprawdzić czy ctg = 1/tan;

            //UserFunctions.addSum(
            //UserFunctions.addFunction("FormulaXYZ", "var0+var1", 2, parser);
            //UserConsts.addConst("Stala0", 0.25, parser);
            //currentFormul = functions.FirstOrDefault();
        }

        public void CloseCalculator()
        {
            UserFunctions.removeFunction("FormulaXYZ");
            UserConsts.removeConst("StalaXYZ");
        }

        public ObservableCollection<double> getResults()
        {
            return results;
        }
        
        public void removeVariables()
        {
            
            variables.Clear();
            
            
        }

        public void removeResults()
        {
            
            results.Clear();
            
        }

        public void cleanCalculator()
        {
            //removeVariables();
            removeResults();
            UserConsts.removeAllConst();
            UserFunctions.removeAllFunctions();
            /*
            foreach (var v in variables)//ustawienie zmienych na 0
            {
                v.listOfValues = new ObservableCollection<double> { 0 };
            }
             */
            refreshCalculator();
        }
        //odswieza caluclator 
        public void refreshCalculator()
        {
            functions = UserFunctions.getAllFunctionsFromDataBase();
            userConsts = UserConsts.getAllConstsFromDataBase();            
            NotifyPropertyChanged("userConsts");
            NotifyPropertyChanged("functions");
            NotifyPropertyChanged("currentFormul");
            NotifyPropertyChanged("results");
            //CalculateFormule();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
