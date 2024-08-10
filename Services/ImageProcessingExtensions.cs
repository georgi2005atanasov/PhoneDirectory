namespace PhoneDirectory.Services
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;
    using SixLabors.ImageSharp.Drawing.Processing;
    using SixLabors.ImageSharp.Drawing;

    public static class ImageProcessingExtensions
    {
        public static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext context, int radius)
        {
            int width = context.GetCurrentSize().Width;
            int height = context.GetCurrentSize().Height;

            // Create a rounded rectangle path
            var rect = new RectangularPolygon(0, 0, width, height);
            var roundedRect = rect.Clip(new EllipsePolygon(new PointF(radius, radius), radius))
                                  .Clip(new EllipsePolygon(new PointF(width - radius, radius), radius))
                                  .Clip(new EllipsePolygon(new PointF(radius, height - radius), radius))
                                  .Clip(new EllipsePolygon(new PointF(width - radius, height - radius), radius));

            // Fill the rounded rectangle
            context.SetGraphicsOptions(new GraphicsOptions { Antialias = true });
            return context.Fill(Color.White, roundedRect);
        }
    }
}