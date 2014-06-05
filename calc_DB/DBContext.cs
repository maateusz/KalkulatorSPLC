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

namespace calc_DB
{
    public sealed class DBContext
    {
        private static CalcDataBaseContext dc = null;
        public static CalcDataBaseContext Context
        {
            get
            {
                if (dc == null)
                {
                    dc = new CalcDataBaseContext(CalcDataBaseContext.ConnectionString);
                }
                return dc;
            }
        }
    }
}
