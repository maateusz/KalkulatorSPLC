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
using System.ComponentModel;
using Mathos;
using Mathos.Parser;

namespace calc_logic
{
    public class Variable : INotifyPropertyChanged
    {
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;
                else
                {
                    _name = value;
                    NotifyPropertyChanged("name");
                }
            }
            
        }//zmienna reprezentujaca nazwe zmiennej

        private ObservableCollection<double> _listOfValues;

        public ObservableCollection<double> listOfValues 
        {
            get
            {
                return _listOfValues;
            }
            set
            {
                if (_listOfValues == value)
                    return;
                else
                {
                    _listOfValues = value;
                    NotifyPropertyChanged("listOfValues");
                }
            }
        }
        //private List<double> listOfValues;//lista wartosci dla jakie przyjmuje zmienna w obliczanym wyrazeniu

        public Variable(string name, double value)
        {
            this.name = name;
            listOfValues = new ObservableCollection<double>();
            listOfValues.Add(value);
            
            //listOfVariables.Add(name);
        }

        public Variable(string name, double start, double stop, double step)
        {
            listOfValues = new ObservableCollection<double>();
            this.name = name;
            double counter = start;
            while (counter <= stop)
            {
                listOfValues.Add(counter);
                counter += step;
            }
            //listOfVariables.Add(name);
        }

        public Variable(string name, ObservableCollection<double> list)
        {
            //listOfValues = new List<double>();
            this.name = name;
            this.listOfValues = list;
            //listOfVariables.Add(name);
        }

        public string getName()
        {
            return _name;
        }

        public void setName(string newName)
        {
            name = newName;
        }

        public ObservableCollection<double> getValues()
        {
            return listOfValues;
        }

        public void setValues(ObservableCollection<double> newList)
        {
            listOfValues = newList;
            NotifyPropertyChanged("listOfValues");
        }

        public void setValues(double value)
        {
            listOfValues = new ObservableCollection<double>();
            listOfValues.Add(value);
            NotifyPropertyChanged("listOfValues");
        }

        public void setValues(double start, double stop, double step)
        {
            listOfValues = new ObservableCollection<double>();
            double counter = start;
            while (counter <= stop)
            {
                listOfValues.Add(counter);
                counter += step;
            }
            NotifyPropertyChanged("listOfValues");
        }

        public void addValue(double value)
        {
            listOfValues.Add(value);
            NotifyPropertyChanged("listOfValues");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
