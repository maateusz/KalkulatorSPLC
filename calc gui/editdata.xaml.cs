using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using calc_logic;

namespace calc_gui
{
    public partial class editdata : PhoneApplicationPage
    {
        private bool isRange = false;
        private string msg = "";

        public editdata()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);         
            if (NavigationContext.QueryString.TryGetValue("msg", out msg))//do msg powinien trafic nadeslany tekst z informacja jaki parametr ustawiamy
            {
                LayoutRoot.DataContext = msg;//ustawiamy kontekst na edytowana zmienna
            }
        }

        private void GOTOWE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculator.Instance.refreshCalculator();//
                Calculator.Instance.CalculateFormule();//liczymy dla nowych wartosic
            }
            catch (Exception exc)
            {
                MessageBox.Show("Prawdopodobnie formula zawiera błędy!!!");
            }
            NavigationService.GoBack();
        }

        private void DODAJ_Click(object sender, RoutedEventArgs e)
        {
            string liczba = "   9";
            double start, stop, step;

            if (isRange)//jesli dodawanymi wartosciami jest zakres 
            {
                try
                {
                    string wyrazenie = (ValueInput.Text);
                    int length = wyrazenie.IndexOf(';');
                    start = double.Parse(wyrazenie.Substring(1, length - 1));
                    length = wyrazenie.IndexOf('>') - length;
                    stop = double.Parse(wyrazenie.Substring(wyrazenie.IndexOf(';') + 1, wyrazenie.IndexOf('>') - 1 - (wyrazenie.IndexOf(';'))));
                    step = double.Parse(wyrazenie.Substring(wyrazenie.IndexOf('(') + 1, wyrazenie.IndexOf(')') - 1 - (wyrazenie.IndexOf('('))));
                    Calculator.Instance.addNewRangeToVariable(edytowanaZmienna.Text, start, stop, step);
                    ValueInput.Text = string.Empty;
                    isRange = false;

                }
                catch (Exception exc)
                {
                    MessageBox.Show("Zły format danych");
                    ValueInput.Text = string.Empty;
                    return;
                }
                return;
            }
            else//jesli chcemy dodac tylko pojedyncza wartosc
            {
                double newValue = 0.0;
                try
                {
                    newValue = double.Parse(ValueInput.Text);
                    Calculator.Instance.addNewValueToVariable(edytowanaZmienna.Text, newValue);
                    ValueInput.Text = string.Empty;
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Zły format danych");
                    ValueInput.Text = string.Empty;
                    return;
                }
                return;
            }
        }

        private void COFNIJ_Click(object sender, RoutedEventArgs e)
        {
            if (ValueInput.Text.Length > 0)
                ValueInput.Text = ValueInput.Text.Remove(ValueInput.Text.Length - 1, 1);
        }

        private void ZAKRES_Click(object sender, RoutedEventArgs e)
        {
            if(!isRange)
            {
                ValueInput.Text = "< ; > ( )";//w tekstboxie przygotowujemy dla zakresu
                isRange = true;
            }
            else
            {
                ValueInput.Text = string.Empty;
                isRange = false;
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            ValueInput.Text += ((Button)sender).Content.ToString();
        }


    }
}