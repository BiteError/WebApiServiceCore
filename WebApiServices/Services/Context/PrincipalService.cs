using System.Security.Principal;
using System.Web;
using WebApiServices.BaseServices;

namespace WebApiServices.Services
{
    public class PrincipalService : IPrincipalService
    {
        private readonly IPrincipal principal;

        public PrincipalService()
        {
            principal = GetPrincipalFromHttpContext();
        }

        private IPrincipal GetPrincipalFromHttpContext()
        {
            return HttpContext.Current.User;
        }

        public IPrincipal GetPrincipal()
        {
            return GetPrincipalFromHttpContext() ?? principal;
        }
    }
}
