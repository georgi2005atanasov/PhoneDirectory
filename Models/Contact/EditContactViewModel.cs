namespace PhoneDirectory.Models.Contact
{
    public class EditContactViewModel : ContactDetailsViewModel
    {
        public EditContactViewModel(ContactDetailsViewModel model)
        {
            this.Name = model.Name;
            this.Email = model.Email;
            this.Street = model.Street;
            this.CountryPrefix = model.CountryPrefix;
            this.PhoneNumber = model.PhoneNumber;
            this.ImageData = model.ImageData;
            this.PostalCode = model.PostalCode;
            this.UploadedImage = null;
            this.Notes = model.Notes;
        }

        public EditContactViewModel()
        {

        }

        public IFormFile? UploadedImage { get; set; }
    }
}
