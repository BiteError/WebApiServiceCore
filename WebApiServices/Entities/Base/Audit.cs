using System;

namespace WebApiServices.Entities.Base
{
    public abstract class Audit
    {
        protected Audit()
        {
            if (CreatedOn == DateTime.MinValue)
            {
                CreatedOn = DateTime.UtcNow;
            }

            if (!ModifiedOn.HasValue || ModifiedOn.Value == DateTime.MinValue)
            {
                ModifiedOn = DateTime.UtcNow;
            }
        }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
