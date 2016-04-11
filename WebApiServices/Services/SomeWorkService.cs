using System;
using WebApiServices.BaseServices;

namespace WebApiServices.Services
{
    public class SomeWorkService : ISomeWorkService
    {
        private readonly IDataContext dataContext;
        private readonly IUserContextService userContextService;

        public SomeWorkService(IDataContext dataContext, IUserContextService userContextService)
        {
            this.dataContext = dataContext;
            this.userContextService = userContextService;
        }

        public void Do()
        {
        }
    }
}
