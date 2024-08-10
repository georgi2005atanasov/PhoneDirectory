namespace PhoneDirectory.Models.Contact
{
    public class AllContactsViewModel
    {
        public List<ContactViewModel> Contacts { get; set; } = new List<ContactViewModel>();

        public string? Search { get; set; }
    }
}
