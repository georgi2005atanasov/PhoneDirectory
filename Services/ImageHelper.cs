namespace PhoneDirectory.Services
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Processing;
    using System.IO;

    public static class ImageHelper
    {
        public static byte[] ResizeImage(IFormFile imageFile, int width, int height)
        {
            using (var image = SixLabors.ImageSharp.Image.Load(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(width, height));
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, JpegFormat.Instance);
                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] CreateCircleImage(IFormFile imageFile, int diameter)
        {
            using (var image = SixLabors.ImageSharp.Image.Load(imageFile.OpenReadStream()))
            {
                image.Mutate(x => x.Resize(diameter, diameter).ApplyRoundedCorners(diameter / 2));
                using (var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, new JpegEncoder { Quality = 100 });
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
