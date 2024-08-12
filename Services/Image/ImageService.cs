namespace PhoneDirectory.Services.Image
{
    using Microsoft.EntityFrameworkCore;
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
                OriginalType = fileType,
                CreatedOn = DateTime.Now,
            };

            db.Images.Add(image);

            await db.SaveChangesAsync();

            return image.Id;
        }

        public async Task ChangeImage(string fileName,
            string contentType,
            byte[] resizedImage, 
            byte[] circleContent, 
            int contactId)
        {
            var existingImage = await db.Images
                .FirstOrDefaultAsync(x => x.ContactId == contactId);

            if (existingImage != null)
            {
                existingImage.OriginalFileName = fileName;
                existingImage.OriginalType = contentType;
                existingImage.DetailsContent = resizedImage;
                existingImage.CircleContent = circleContent;

                await db.SaveChangesAsync();
            }
        }
    }
}
