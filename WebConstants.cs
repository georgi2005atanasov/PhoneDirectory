namespace PhoneDirectory
{
    public static class WebConstants
    {
        public const int ItemsPerPage = 8;
        public const string DangerMessage = "danger";
        public const string SuccessMessage = "success";

        public static class Contact
        {
            public const string CouldNotGetContact = "Could not get the contact, try to refresh.";

            public const string ContactAlreadyExists = "Contact with this number already exists!";

            public const string ContactAddedSuccessfully = "Contact added successfully!";

            public const string ErrorWhileAddingContact = "An error occurred while adding the contact.";

            public const string ImageTooBig = "The uploaded file exceeds the allowed size limit of 1.5MB.";

            public const string ContactDoesNotExists = "Contact does not exists.";

            public const string ContactEditedSuccessfully = "Contact edited successfully!";

            public const string ErrorWhileEditing = "An error occurred while editing the contact.";

            public const string ContactDeletedSuccessfully = "Contact deleted successfully!";

            public const string CouldNotDeleteContact = "Could not delete the contact, please try again.";

            public const string ContactRestoredSuccessfully = "Contact restored successfully!";

            public const string ErrorWhileRestoringContact = "An error occurred while restoring the contact.";
        }
    }
}
