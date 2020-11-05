using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DrowsyDoc
{
    public class CameraCapture
    {
        public void CaptureCamera()
        {
            try
            {
                CaptureCameraCallback();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //Capturing Images Using The Device's Camera
        private static void CaptureCameraCallback()
        {
            Bitmap image;
            bool isCameraRunning = true;

            VideoCapture capture = new VideoCapture(0);
            capture.Open(0);

            Mat frame = new Mat();

            if (capture.IsOpened())
            {
                string name = Environment.CurrentDirectory;
                int i = 0;

                while (isCameraRunning)
                {
                    //i %= 100;
                    capture.Read(frame);
                    image = BitmapConverter.ToBitmap(frame);
                    Bitmap snapshot = new Bitmap(image);
                    image.Dispose();
                    
                    snapshot.Save(name + @"\rawImage\raw" + i + @".png", ImageFormat.Jpeg);
                    ++i;
                    
                    Thread t = Thread.CurrentThread;
                    Thread.Sleep(1500);   
                }  
            }
        }  
    }
}
