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
using calc_DB;

namespace calc_gui
{
    public partial class constlib : PhoneApplicationPage
    {
        public static string currentConst = null;
        public static bool isEdit = false;
        public constlib()
        {
            InitializeComponent();
            LayoutRoot.DataContext = Calculator.Instance;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            currentConst = null;
            base.OnNavigatedTo(e);
            if (listaStalych.Items.Count > 0)
            {
                listaStalych.SelectedIndex = 0;
                EdytujButton.IsEnabled = true;
            }
            else
                EdytujButton.IsEnabled = false; 
        }

        private void DODAJ_Stala_Click(object sender, RoutedEventArgs e)
        {
            isEdit = false;
            NavigationService.Navigate(new Uri("/constant.xaml", UriKind.Relative));
        }

        private void EDYTUJ_Stala_Click(object sender, RoutedEventArgs e)//naprawic
        {
            isEdit = true;
            currentConst = ((Const)listaStalych.SelectedItem).Name;//w msg ukryta tresc edytowanej stalej
            NavigationService.Navigate(new Uri("/constant.xaml", UriKind.Relative));
        }

        private void StalaButton_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            int pos = formula.inputExpression.SelectionStart;
            string text = ((Const)listaStalych.SelectedItem).Name;
            if (formula.inputExpression.SelectionStart != formula.inputExpression.Text.Length)
            {
                formula.inputExpression.SelectedText = text;
            }
            else
            {
                formula.inputExpression.Text += text;
            }
            formula.currentText = formula.inputExpression.Text;
            formula.inputExpression.SelectionStart = pos + text.Length;

            NavigationService.GoBack();
        }

    }
}