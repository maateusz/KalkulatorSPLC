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
    public class Variable
    {
        private string name;//zmienna reprezentujaca nazwe zmiennej
        private List<double> listOfValues;//lista wartosci dla jakie przyjmuje zmienna w obliczanym wyrazeniu
        //private static List<string> listOfVariables = new List<string>();
        public Variable(string name, double value)
        {
            this.name = name;
            listOfValues = new List<double>();
            listOfValues.Add(value);
            //listOfVariables.Add(name);
        }

        public Variable(string name, double start, double stop, double step)
        {
            listOfValues = new List<double>();
            this.name = name;
            double counter = start;
            while (counter <= stop)
            {
                listOfValues.Add(counter);
                counter += step;
            }
            //listOfVariables.Add(name);
        }

        public Variable(string name, List<double> list)
        {
            //listOfValues = new List<double>();
            this.name = name;
            this.listOfValues = list;
            //listOfVariables.Add(name);
        }

        public string getName()
        {
            return name;
        }

        public void setName(string newName)
        {
            name = newName;
        }

        public List<double> getValues()
        {
            return listOfValues;
        }

        public void setValues(List<double> newList)
        {
            listOfValues = newList;
        }

        public void addValue(double value)
        {
            listOfValues.Add(value);
        }
    }
}
