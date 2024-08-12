namespace PhoneDirectory.Services.Image
{
    public interface IImageService
    {
        Task<int> Create(byte[] detailsData, 
            byte[] circleData, 
            string fileName,
            string fileType,
            int contactId);
        Task ChangeImage(string fileName,
            string contentType,
            byte[] resizedImage, 
            byte[] circleContent, 
            int contactId);
    }
}
