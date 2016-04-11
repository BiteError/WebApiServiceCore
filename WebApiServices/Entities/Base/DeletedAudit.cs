using System;

namespace WebApiServices.Entities.Base
{
    public abstract class DeletedAudit : Audit
    {
        public Guid? DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}