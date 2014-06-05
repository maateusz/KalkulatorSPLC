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
using System.Linq;
using calc_DB;

using System.Collections.ObjectModel;

using Mathos;
using Mathos.Parser;

namespace calc_logic
{
    public class UserConsts
    {
        

        public static void addConstToParser(string name, double value, MathParser parser)
        {
            parser.ProgrammaticallyParse("let " + name + " be " + value.ToString());
        }
        public static void addConstToDataBase(string name, double value)
                {
                    using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
                    {
                        var nC = new Const { Name = name, Value = value };
                        try
                        {
                            dc.Consts.InsertOnSubmit(nC);
                            dc.SubmitChanges();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
        public static void addConst(string name, double value, MathParser parser)
        {
            addConstToParser(name, value, parser);
            addConstToDataBase(name, value);
        }  
        public static Const getConstFromDataBase(string name)
        {
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nc = dc.Consts.FirstOrDefault(c => c.Name.Equals(name));
                return nc;
            }
        }
        public static ObservableCollection<Const> getAllConstsFromDataBase()
        {
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var listOfConsts = dc.Consts.Select(t => t);//dc.Consts.ToList();
                return new ObservableCollection<Const>(listOfConsts);
            }
        }
        public static void removeConst(string name)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                try
                {
                    Calculator.parser.LocalVariables.Remove(name);
                    Const nc = dc.Consts.FirstOrDefault(c => c.Name.Equals(name));
                    dc.Consts.DeleteOnSubmit(nc);
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        public static void removeConst(Const uConst)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                Calculator.parser.LocalVariables.Remove(uConst.Name);
                dc.Consts.DeleteOnSubmit(uConst);
                dc.SubmitChanges();
            }
        }
        public static void removeAllConst()
        {
            var dc = DBContext.Context;
            foreach (var c in getAllConstsFromDataBase())
            {
                Calculator.parser.LocalVariables.Remove(c.Name);
            }

            try
            {
                dc.Consts.DeleteAllOnSubmit(dc.Consts.Select(t => t));
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void EditConst(string name, double newValue)
        {
            Calculator.parser.ProgrammaticallyParse("let " + name + " be " + newValue.ToString());
            var dc = DBContext.Context;
            var con = dc.Consts.First(c => c.Name.Equals(name));
            try
            {
                con.Value = newValue;
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static void LoadConstsFromDataBase()
        {
            foreach (var con in getAllConstsFromDataBase())
            {
                addConst(con.Name, con.Value, Calculator.parser);
            }
        }
    }
}
