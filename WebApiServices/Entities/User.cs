using System;
using WebApiServices.Entities.Base;

namespace WebApiServices.Entities
{
    public class User :Entity, IDisablable
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsDisabled { get; set; }
        public string Email { get; set; }
    }
}
