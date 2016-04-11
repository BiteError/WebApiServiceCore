using System;
using System.Linq;
using WebApiServices.BaseServices;
using WebApiServices.Entities;

namespace WebApiServices.Services
{
    public class UserContextService : IUserContextService
    {
        private readonly IDataContext dataContext;
        private readonly ICacheProvider cacheProvider;
        private readonly IPrincipalService principalService;
        private User currentUser;

        public UserContextService(IDataContext dataContext, ICacheProvider cacheProvider, IPrincipalService principalService)
        {
            this.dataContext = dataContext;
            this.cacheProvider = cacheProvider;
            this.principalService = principalService;
        }

        public Guid Id => CurrentUser.Id;

        public string Name => CurrentUser.Name;

        public Guid CompanyId => CurrentUser.CompanyId;

        public User CurrentUser => currentUser ?? (currentUser = GetCurrentUser());

        private User GetCurrentUser()
        {
            var identity = principalService.GetPrincipal().Identity;

            if (identity.IsAuthenticated)
            {
                var user = cacheProvider.Get(identity.Name, GetUserByEmail);

                //this return 404
                //if (user == null)
                //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

                return user;
            }

            //this return 404
            //throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));

            return null;
        }

        private User GetUserByEmail(string email)
        {
            var user = dataContext.GetQueryable<User>().FirstOrDefault(p => p.Email == email);
            //we must detach if cache using
            dataContext.DetachContext(user);
            return user;
        }

        //Use this method on logout
        public void InvalidateUserModelCache(string email)
        {
            //if store in cache in DTO
            //cacheProvider.Invalidate<UserModel>(email);

            //this work only if entity plain and simple
            cacheProvider.Invalidate<User>(email);
        }
    }
}