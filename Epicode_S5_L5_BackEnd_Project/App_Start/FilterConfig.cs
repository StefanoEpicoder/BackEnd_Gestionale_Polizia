using System.Web;
using System.Web.Mvc;

namespace Epicode_S5_L5_BackEnd_Project
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
