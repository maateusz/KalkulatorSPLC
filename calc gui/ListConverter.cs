using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using calc_logic;

namespace calc_gui
{
    public class ListConverter : IValueConverter//konwersja listy wartosci typu double na string
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            string answer;
            ObservableCollection<double> list = (ObservableCollection<double>)value;
            if (list.Count == 0)
                return null;
            else if (list.Count == 1)
                return list.First().ToString();
            else
            {
                double step = list.ElementAt(1) - list.ElementAt(0);
                bool flag = true;
                for (int i = 1; i < list.Count; i++)
                {
                    if (step - (list.ElementAt(i) - list.ElementAt(i - 1)) == 0.0)
                    {
                        continue;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }

                }
                if (flag == false)
                    return "{" + string.Join("; ", list) + "}";
                else
                {
                    answer = "< " + list.First() + "; " + list.ElementAt(list.Count - 1) + " >" + " (" + step + ")";
                    return answer;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
