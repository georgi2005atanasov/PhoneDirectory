namespace PhoneDirectory.Data.Models.Base
{
    public interface IDeletableEntity : IEntity
    {
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
