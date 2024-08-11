namespace PhoneDirectory.Models.Shared
{
    public class PaginationViewModel
    {
        public int TotalItems { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

        public string Action { get; set; } = string.Empty;

        public string Controller { get; set; } = string.Empty;

        public string QueryParams { get; set; } = string.Empty;
    }
}
