using System;
using WebApiServices.Entities;

namespace WebApiServices.BaseServices
{
    public interface IUserContextService : IService
    {
        Guid Id { get; }
        string Name { get; }
        Guid CompanyId { get; }
        User CurrentUser { get; }
        void InvalidateUserModelCache(string email);
    }
}