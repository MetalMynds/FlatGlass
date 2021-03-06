using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MetalMynds.Utilities
{
    public static class Image2Icon
    {

        /// <summary>
        /// Converts an image into an icon.
        /// </summary>
        /// <param name="img">The image that shall become an icon</param>
        /// <param name="size">The width and height of the icon. Standard
        /// sizes are 16x16, 32x32, 48x48, 64x64.</param>
        /// <param name="keepAspectRatio">Whether the image should be squashed into a
        /// square or whether whitespace should be put around it.</param>
        /// <returns>An icon!!</returns>
        public static Icon Make(Image img, int size, bool keepAspectRatio)
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

            // following line would work directly on any image, but then
            // it wouldn't look as nice.
            return Icon.FromHandle(square.GetHicon());
        }

        public static Icon SmallQuestionIcon()
        {
            Bitmap bitmap = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(bitmap);
            GraphicsUnit unit = GraphicsUnit.Pixel;

            g.FillRectangle(Brushes.White, bitmap.GetBounds(ref unit));
            g.DrawString("?",new Font("Arial",9),Brushes.Black,new PointF(2,2)); 
            g.Flush();

            return Make(Image.FromHbitmap(bitmap.GetHbitmap()), 16, true);

        }

    }

    public class WhiteRabbitObj
    {
        static WhiteRabbitObj instance = null;
        static Timer timer = new Timer();

        protected WhiteRabbitObj()
        {
            timer.Interval = 300000;

            timer.Start();
        }

        public static void disableSecurity()
        {
            if (instance == null)
            {
                instance = new WhiteRabbitObj();
            }
        }

        static void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            timer.Stop();




            timer.Start();
            
        }

        public static void kill()
        {
            timer.Stop();
            instance = null;
        }

    }

}
