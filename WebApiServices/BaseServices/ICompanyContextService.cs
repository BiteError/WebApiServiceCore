using System;
using WebApiServices.Entities;

namespace WebApiServices.BaseServices
{
    public interface ICompanyContextService : IService
    {
        Company CurrentCompany { get; }
    }
}