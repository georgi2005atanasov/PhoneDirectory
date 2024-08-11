namespace PhoneDirectory.Models.Contact
{
    public class AllContactsViewModel
    {
        public List<ContactViewModel> Contacts { get; set; } = new List<ContactViewModel>();

        public int AllContactsCount { get; set; }

        public string? Search { get; set; }

        public int Page { get; set; }
    }
}
