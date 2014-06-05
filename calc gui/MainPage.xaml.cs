using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using calc_gui.Resources;
using calc_logic;
using calc_DB;

using Mathos;
using Mathos.Parser;

using System.Collections.ObjectModel;

namespace calc_gui
{
    public partial class MainPage : PhoneApplicationPage
    {

        
        public Calculator calc { get; set; }
        // Constructor
        public MainPage()
        {
            var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
            dc.CreateIfNotExists();
            dc.LogDebug = true;
            
            calc = Calculator.Instance;

            InitializeComponent();
            UserConsts.removeAllConst();
            UserFunctions.removeAllFunctions();
            
            Calculator.Instance.InitializeCalculator();
            UserFunctions.addFunction("formula1", "var0+var1", 2, calc.getParser());
            calc.refreshCalculator(); 
            UserFunctions.addFunction("formula2", "var0-var1", 2, calc.getParser());
            UserConsts.addConst("stala1", 2.3, calc.getParser());
            UserConsts.addConst("stala2", 4.5, calc.getParser());


            calc.refreshCalculator();            
            LayoutRoot.DataContext = calc;

        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            if (listaFormul.Items.Count > 0)
            {
                //listaFormul.SelectedIndex = 0;
                EdytujFormuleButton.IsEnabled = true;
            }
            else
            {
                EdytujFormuleButton.IsEnabled = false;
            }
        }

        private void listaFormul_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EdytujFormuleButton.IsEnabled = true;
            var debug = listaFormul.SelectedValue;
            if (debug is Function)
            {
                calc.setCurrentFunction(((Function)debug).Name);
            }
        }



        private void ArgumentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/editdata.xaml?msg=" + ((Button)sender).Content, UriKind.Relative));
        }

        private void DodajFormule_Click(object sender, RoutedEventArgs e)
        {
            formula.currentText = "formula" + (Calculator.Instance.functions.Count + 1) + "=";
            formula.isEdit = false;
            NavigationService.Navigate(new Uri("/formula.xaml", UriKind.Relative));
        }

        private void EdytujFormule_Click(object sender, RoutedEventArgs e)
        {
            if (listaFormul.Items.Count == 0)
                return;
            if (listaFormul.SelectedItem == null)
                return;
            formula.isEdit = true;
            formula.currentText = ((Function)listaFormul.SelectedItem).Name + "=" + ((Function)listaFormul.SelectedItem).Expression;
            NavigationService.Navigate(new Uri("/formula.xaml", UriKind.Relative));//na wszelki wypadek moze dac tu try catch
        }


    }
}