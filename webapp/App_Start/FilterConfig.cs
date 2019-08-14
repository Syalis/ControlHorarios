using System.Web;
using System.Web.Mvc;

namespace webapp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           //ilters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());  //Filtro [Authorize] para Todos los controllers
        }
    }
}
