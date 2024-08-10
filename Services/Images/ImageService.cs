namespace PhoneDirectory.Services.Images
{
    using PhoneDirectory.Data;
    using PhoneDirectory.Data.Models;

    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext db;

        public ImageService(ApplicationDbContext db) 
            => this.db = db;

        public async Task<int> Create(byte[] detailsData, 
            byte[] circleData, 
            string fileName,
            string fileType,
            int contactId)
        {
            var image = new Image
            {
                OriginalFileName = fileName,
                DetailsContent = detailsData,
                CircleContent = circleData,
                ContactId = contactId,
                OriginalType = fileType
            };

            db.Images.Add(image);

            await db.SaveChangesAsync();

            return image.Id;
        }
    }
}
