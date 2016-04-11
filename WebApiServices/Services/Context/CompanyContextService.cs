using System;
using System.Linq;
using WebApiServices.BaseServices;
using WebApiServices.Entities;

namespace WebApiServices.Services
{
    public class CompanyContextService : ICompanyContextService
    {
        private readonly IDataContext dataContext;
        private readonly ICacheProvider cacheProvider;
        private readonly IPrincipalService principalService;

        public CompanyContextService(IDataContext dataContext, ICacheProvider cacheProvider, IPrincipalService principalService)
        {
            this.dataContext = dataContext;
            this.cacheProvider = cacheProvider;
            this.principalService = principalService;
        }

        public Company CurrentCompany => GetCurrentCompany();

        private Company GetCurrentCompany()
        {
            var companyId = GetCompanyId();
            //this work on simple entities
            return cacheProvider.Get(companyId, GetCompanyById);
        }

        private Company GetCompanyById(Guid companyId)
        {
            var company = dataContext
                .GetQueryable<Company>()
                .FirstOrDefault(x => x.Id == companyId);

            //we must detach if cache using
            dataContext.DetachContext(company);

            return company;
        }

        private Guid GetCompanyId()
        {
            var identity = principalService.GetPrincipal().Identity;
            if (identity.IsAuthenticated)
            {
                return cacheProvider.Get(identity.Name, GetCompanyId);
            }

            //404
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            throw new Exception();
        }

        private Guid GetCompanyId(string token)
        {
            return 
                dataContext.GetQueryable<User>()
                    .Where(p => p.Email == token)
                    .Select(x => x.CompanyId)
                    .FirstOrDefault();
        }
    }
}