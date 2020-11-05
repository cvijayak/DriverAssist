

using System;
using System.IO;
using System.Threading;

namespace DrowsyDoc
{
    class Program
    {
        public static int counter = 0;

        static void Main(string[] args)
        {
            string currentDir = Environment.CurrentDirectory;
            
            EmptyFolder(currentDir + @"\rawImage");
            EmptyFolder(currentDir + @"\detectedImage");

            CameraCapture cp = new CameraCapture();
            Detect_Landmarks dl = new Detect_Landmarks();

            Thread camera = new Thread(cp.CaptureCamera);
            camera.Name = "Take Picture Thread";
            Thread d_land = new Thread(dl.detect_landmark);
            d_land.Name = "LandMark Detection Thread";

            camera.Start();
            d_land.Start();
        }

        private static void EmptyFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
    }
}
