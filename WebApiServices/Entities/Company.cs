using WebApiServices.Entities.Base;

namespace WebApiServices.Entities
{
    public class Company : Entity, IDisablable
    {
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }
}