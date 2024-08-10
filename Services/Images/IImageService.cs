namespace PhoneDirectory.Services.Images
{
    public interface IImageService
    {
        Task<int> Create(byte[] detailsData, 
            byte[] circleData, 
            string fileName,
            string fileType,
            int contactId);
    }
}
