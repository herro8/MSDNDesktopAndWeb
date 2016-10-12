using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahead.Data
{
    public class CustomWindowsPrincipal
    {

        public static string Name
        {
            get
            {
                return (System.Threading.Thread.CurrentPrincipal as System.Security.Principal.WindowsPrincipal).Identity.Name;
            }
        }
    }
}
