namespace PhoneDirectory.Data
{
    public static class ErrorMessages
    {
        public static class Contact
        {
            public const string NameIsRequired = "Name is required!";

            public const string ExceededNameLength = $"Name cannot be longer than 50 characters.";

            public const string InvalidEmailAddress = "Invalid Email Address.";

            public const string EmailTooLong = "Email cannot be longer than 100 characters.";

            public const string RequiredPhoneNumber = "Phone number is required.";

            public const string InvalidPhoneNumber = "Invalid phone number.";

            public const string PhoneNumberRequired = "Phone number is required.";
            
            public const string NotesTooLong = "Notes cannot be longer than 500 characters.";
        }
    }
}
