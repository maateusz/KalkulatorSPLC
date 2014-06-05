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
    public partial class constant : PhoneApplicationPage
    {
        
        public constant()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (constlib.isEdit)//jesli to ma byc edycja stalej, to wyswietlamy jej aktualna wersje
                StalaTextBox.Text = UserConsts.getConstFromDataBase(constlib.currentConst).Name + "=" + UserConsts.getConstFromDataBase(constlib.currentConst).Value;
            else
                StalaTextBox.Text = "stala" + (Calculator.Instance.userConsts.Count + 1) + "=";
        }

        private void ZAPISZ_Stala_Click(object sender, RoutedEventArgs e)
        {
            if (!StalaTextBox.Text.Contains("="))
            {
                MessageBox.Show("Pamietaj o znaku =");
                return;
            }
            
            string name;
            double value;
            string message = StalaTextBox.Text;
            string msg = string.Empty;
            if (constlib.isEdit)//edycja istniejacej
            {
                try
                {
                    name = message.Substring(0, message.IndexOf('='));//wycinamy nazwe stalej
                    value = double.Parse(message.Substring(message.IndexOf('=') + 1, message.Length - 1 - message.IndexOf('=')));//wycinamy wartosc stalej
                    UserConsts.removeConst(constlib.currentConst);//usuwamy stara wersje
                    UserConsts.addConst(name, value, Calculator.parser);//dodajemy nowa wersje stalej
                    Calculator.Instance.refreshCalculator();//odswiezamy kalkulator
                    NavigationService.GoBack();//wracamy do poprzedniego okienka
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Błedna definicja stałej!");
                }
            }
            else//dodanie nowej stalej
            {
                try
                {
                    name = message.Substring(0, message.IndexOf('='));
                    value = double.Parse(message.Substring(message.IndexOf('=') + 1, message.Length - 1 - message.IndexOf('=')));

                    UserConsts.addConst(name, value, Calculator.parser);
                    Calculator.Instance.refreshCalculator();//sprawdzic ustawienie pierwotnej funkcji
                    NavigationService.GoBack();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Błedna definicja stałej!");
                }
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            StalaTextBox.Text += ((Button)sender).Content.ToString();
        }


    }
}