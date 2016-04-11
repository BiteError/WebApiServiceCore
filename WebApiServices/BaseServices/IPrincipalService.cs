using System.Security.Principal;

namespace WebApiServices.BaseServices
{
    public interface IPrincipalService : IService
    {
        IPrincipal GetPrincipal();
    }
}