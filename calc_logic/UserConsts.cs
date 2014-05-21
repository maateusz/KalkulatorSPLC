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

using Mathos;
using Mathos.Parser;

namespace calc_logic
{
    public class UserConsts
    {
        private static List<string> usersConsts = new List<string>();
        public static void addConstToParser(string name, double value, MathParser parser)
        {
            parser.ProgrammaticallyParse("let " + name + " be " + value.ToString());
            usersConsts.Add(name);
        }


        public static void addConst(string name, double value, MathParser parser)
        {
            addConstToParser(name, value, parser);
            addConstToDataBase(name, value);
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

        public static Const getConstFromDataBase(string name)
        {
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var nc = dc.Consts.First(c => c.Name.Equals(name));
                return nc;
            }
        }

        public static List<Const> getAllConstsFromDataBase()
        {
            //pobranie wszystkich funckji z bazy danych i dodanie ich do parsera
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                var listOfConsts = dc.Consts.ToList();
                return listOfConsts;
            }
        }

        public static void initializeParser(MathParser parser)
        {
            foreach (var con in getAllConstsFromDataBase())
            {
                addConst(con.Name, con.Value, parser);
            }
        }

        public static List<string> getUsersConsts()
        {
            return usersConsts;
        }

        public static void removeConst(string name)
        {
            using (var dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString))
            {
                try
                {
                    usersConsts.Remove(name);
                    Const nc = dc.Consts.First(c => c.Name.Equals(name));
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
                usersConsts.Remove(uConst.Name);
                dc.Consts.DeleteOnSubmit(uConst);
                dc.SubmitChanges();
            }
        }

        
    }
}
