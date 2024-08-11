namespace PhoneDirectory.Data.Models.Base
{
    public abstract class DeletableEntity : Entity, IDeletableEntity
    {
        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
