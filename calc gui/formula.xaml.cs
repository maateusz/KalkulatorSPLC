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
    public partial class formula : PhoneApplicationPage
    {
        private int varCounter = 0;

        public static string currentText = string.Empty;
        public static bool isEdit = false;
        public static TextBox inputExpression = null;

        public formula()
        {
            varCounter = 0;
            InitializeComponent();
            inputExpression = FormulaTextBox;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string msg = string.Empty;

            FormulaTextBox.Text = currentText;
            FormulaTextBox.SelectionStart = FormulaTextBox.Text.Length;
        }

        private void AnulujFormula_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConstButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/constlib.xaml", UriKind.Relative));
        }

        private void Log_Button_Click(object sender, RoutedEventArgs e)
        {
            int pos = FormulaTextBox.SelectionStart;
            string text = ((Button)sender).Content + "( , )";
            if(FormulaTextBox.SelectionStart != FormulaTextBox.Text.Length)
            {
                FormulaTextBox.SelectedText = text;
            }
            else
            {
                FormulaTextBox.Text += text;
            }
            currentText = FormulaTextBox.Text;
            FormulaTextBox.SelectionStart = pos + text.Length;
        }

        private void Trigonometric_Click(object sender, RoutedEventArgs e)
        {

            int pos = FormulaTextBox.SelectionStart;
            string text = (((Button)sender).Content.ToString().Substring(0, ((Button)sender).Content.ToString().IndexOf(" ")) + "( )");
            if (FormulaTextBox.SelectionStart != FormulaTextBox.Text.Length)
            {
                FormulaTextBox.SelectedText = text;
            }
            else
            {
                FormulaTextBox.Text += text;
            }
            currentText = FormulaTextBox.Text;
            FormulaTextBox.SelectionStart = pos + text.Length;
        }

        private void OperationNumberButton_Click(object sender, RoutedEventArgs e)
        {


            int pos = FormulaTextBox.SelectionStart;
            string text = ((Button)sender).Content.ToString();
            if (FormulaTextBox.SelectionStart != FormulaTextBox.Text.Length)
            {
                FormulaTextBox.SelectedText = text;
            }
            else
            {
                FormulaTextBox.Text += text;
            }
            currentText = FormulaTextBox.Text;
            FormulaTextBox.SelectionStart = pos + text.Length;
            
        }

        private void AddVarButton_Click(object sender, RoutedEventArgs e)
        {
            int pos = FormulaTextBox.SelectionStart;
            string text = ((Button)sender).Content.ToString();
            if (FormulaTextBox.SelectionStart != FormulaTextBox.Text.Length)
            {
                FormulaTextBox.SelectedText = text;
            }
            else
            {
                FormulaTextBox.Text += text;
            }
            currentText = FormulaTextBox.Text;
            FormulaTextBox.SelectionStart = pos + text.Length;
        }

        private void calcNumberOfVar(int i = 0)
        {
            if (FormulaTextBox.Text.Contains("var" + i))
            {
                varCounter++;
                calcNumberOfVar(++i);
            }
            else
                return;
        }

        private void ZAPISZ_Formule_Click(object sender, RoutedEventArgs e)
        {
            if (isEdit)//jesli to byla edycja formuly - analogicznie jak dla stalej
            {
                try
                {
                    string message = FormulaTextBox.Text;
                    string name = message.Substring(0, message.IndexOf('='));
                    string expression = message.Substring(message.IndexOf('=') + 1, message.Length - 1 - message.IndexOf('='));
                    
                    UserFunctions.removeFunction(name);
                    varCounter = 0;
                    calcNumberOfVar();
                    UserFunctions.addFunction(name, expression, varCounter, Calculator.parser);
                    Calculator.Instance.NotifyPropertyChanged("functions");
                    Calculator.Instance.setCurrentFunction(name);
                    Calculator.Instance.refreshCalculator();
                    NavigationService.GoBack();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
            else//jesli to bylo dodanie nowej formuly
            {
                
                    string message = FormulaTextBox.Text;
                    string name = message.Substring(0, message.IndexOf('='));
                    string expression = message.Substring(message.IndexOf('=') + 1, message.Length - 1 - message.IndexOf('='));
                    varCounter = 0;
                    calcNumberOfVar();
                    UserFunctions.addFunction(name, expression, varCounter, Calculator.parser);
                    Calculator.Instance.NotifyPropertyChanged("functions");
                    Calculator.Instance.refreshCalculator();
                try
                {
                    Calculator.Instance.setCurrentFunction(name);
                    Calculator.Instance.refreshCalculator();
                    NavigationService.GoBack();
                    //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
                catch (Exception exc)
                {
                    UserFunctions.removeFunction(name);
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void TextChanged_Event(object sender, TextChangedEventArgs e)
        {
            
            
        }

        private void EmptyButton_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
        
    }
}