using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public class ImageHelper
    {
        public static Image Grab()
        {
            return Grab(WindowHelper.GetDesktopWindowHandle());
        }

        public static Image Grab(Control Target)
        {
            return Grab(Target.Handle);
        }

        public static Image Grab(Form Target)
        {
            return Grab(Target.Handle);
        }

        public static Image Grab(IntPtr Handle)
        {
            using (Graphics controlGraphics = Graphics.FromHwnd(Handle))
            {
                Size dimensions;
                Point topLeft;

                if (!WindowHelper.TryGetWindowDetails(Handle, out dimensions, out topLeft))
                {
                    throw new InvalidOperationException(String.Format("Get Window Details Failed! Handle: [{0}]", Handle));
                }

                Bitmap capturedImage = new Bitmap(dimensions.Width, dimensions.Height, controlGraphics);

                using (Graphics memoryGraphics = Graphics.FromImage(capturedImage))
                {
                    memoryGraphics.CopyFromScreen(topLeft, new Point(0, 0), dimensions);
                    memoryGraphics.Flush();
                }

                //capturedImage.Save("C:\\temps\\grab.png");

                return capturedImage;
            }
        }

        public static Image ToOpaque(Image Source, float Level)
        {
            Bitmap bitmap = new Bitmap(Source.Width, Source.Height);
            ImageAttributes IA = new ImageAttributes();
            ColorMatrix CM = new ColorMatrix();
            CM.Matrix00 = 1;
            CM.Matrix11 = 1;
            CM.Matrix22 = 1;
            //CM.Matrix33 = 0.5F;
            CM.Matrix33 = Level;
            CM.Matrix44 = 1;
            //CM.Matrix33=0.5  opacity=0.5;CM.Matrix33=0 opacity=0
            IA.SetColorMatrix(CM);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(Source, new Rectangle(0, 0, Source.Width, Source.Height), 0, 0, Source.Width, Source.Height, GraphicsUnit.Pixel, IA);
            }

            IA.Dispose();

            return bitmap;
        }

        public static Image ToJpeg(Image Source, int Ratio)
        {
            const String JPEG_MIME = "image/jpeg";

            // We will store the correct image codec in this object

            ImageCodecInfo jpegCodecInfo = null;

            // This will specify the image quality to the encoder

            EncoderParameter epQuality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Ratio);

            // Get all image codecs that are available

            ImageCodecInfo[] jpegCodecInfos = ImageCodecInfo.GetImageEncoders();

            // Store the quality parameter in the list of encoder parameters

            EncoderParameters epParameters = new EncoderParameters(1);

            epParameters.Param[0] = epQuality;

            // Loop through all the image codecs

            foreach (ImageCodecInfo codecInfo in jpegCodecInfos)
            {
                if (codecInfo.MimeType == JPEG_MIME)
                {
                    jpegCodecInfo = codecInfo;

                    break;
                }
            }

            if (jpegCodecInfo == null)
            {
                throw new InvalidOperationException(String.Format("ImageHelper:\nError: Unable to Find Codec [{0}]", JPEG_MIME));
            }

            Bitmap tempBitmap = new Bitmap(Source);
            Image compressedImage = Image.FromHbitmap(tempBitmap.GetHbitmap());

            MemoryStream memoryFile = new MemoryStream();

            compressedImage.Save(memoryFile, jpegCodecInfo, epParameters);

            memoryFile.Flush();

            memoryFile.Seek(0, 0);

            compressedImage = Image.FromStream(memoryFile);

            return compressedImage;
        }

        public static Image ToGreyScale(Image Source)
        {
            Bitmap grayscale = (Bitmap)Source.Clone();
            Rectangle bounds = new Rectangle(0, 0, grayscale.Width, grayscale.Height);
            ColorMatrix colorMatrix = new ColorMatrix();

            int mX = 0;
            int mY = 0;

            for (mX = 0; mX <= 2; mX++)
            {
                for (mY = 0; mY <= 2; mY++)
                {
                    colorMatrix[mX, mY] = 0.333333F;
                }
            }

            ImageAttributes imgAttr = new System.Drawing.Imaging.ImageAttributes();

            imgAttr.SetColorMatrix(colorMatrix);

            System.Drawing.Graphics graphics = Graphics.FromImage(grayscale);

            try
            {
                graphics.DrawImage(grayscale, bounds, 0, 0, grayscale.Width, grayscale.Height, System.Drawing.GraphicsUnit.Pixel, imgAttr);
            }
            finally
            {
                graphics.Dispose();
            }

            return grayscale;
        }

        /// <summary>
        /// Converts an image into an icon.
        /// </summary>
        /// <param name="img">The image that shall become an icon</param>
        /// <param name="size">The width and height of the icon. Standard
        /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
        /// <param name="keepAspectRatio">Whether the image should be squashed into a
        /// square or whether whitespace should be put around it.</param>
        /// <returns>An icon!!</returns>
        public static Icon ToIcon(Image img, int size, bool keepAspectRatio)
        {
            Bitmap square = new Bitmap(size, size); // create new bitmap
            Graphics g = Graphics.FromImage(square); // allow drawing to it

            int x, y, w, h; // dimensions for new image

            if (!keepAspectRatio || img.Height == img.Width)
            {
                // just fill the square
                x = y = 0; // set x and y to 0
                w = h = size; // set width and height to size
            }
            else
            {
                // work out the aspect ratio
                float r = (float)img.Width / (float)img.Height;

                // set dimensions accordingly to fit inside size^2 square
                if (r > 1)
                { // w is bigger, so divide h by r
                    w = size;
                    h = (int)((float)size / r);
                    x = 0; y = (size - h) / 2; // center the image
                }
                else
                { // h is bigger, so multiply w by r
                    w = (int)((float)size * r);
                    h = size;
                    y = 0; x = (size - w) / 2; // center the image
                }
            }

            // make the image shrink nicely by using HighQualityBicubic mode
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon

            //square.Save("C:\\ApplicationLarge.ico",ImageFormat.Icon);

            return Icon.FromHandle(square.GetHicon());
        }

        public static Icon SmallQuestionIcon()
        {
            Bitmap bitmap = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(bitmap);
            GraphicsUnit unit = GraphicsUnit.Pixel;

            g.FillRectangle(Brushes.White, bitmap.GetBounds(ref unit));
            g.DrawString("?", new Font("Arial", 9), Brushes.Black, new PointF(2, 2));
            g.Flush();

            return ToIcon(Image.FromHbitmap(bitmap.GetHbitmap()), 16, true);
        }
    }
}