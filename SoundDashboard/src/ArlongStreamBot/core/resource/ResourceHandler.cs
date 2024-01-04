using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using SkiaSharp;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace ArlongStreambot.core
{
    public class ResourceHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ResourceHandler));

        
        public static Assembly assembly;
        public static string assemblyLocation;
        public static string assemblyName;
        
        public static string resourceFolder;
        
        public static string imageFolder;
        public static string audioFolder;
        public static string videoFolder;
        public static string fontFolder;
        public static string svgFolder;
        public static string[] resourceFolders;

        public static string[] imageExtensions;
        public static string[] audioExtensions;
        public static string[] videoExtensions;
        public static string[] fontExtensions;
        public static string svgExtension;
        
        public static string imageFolderPath;
        public static string audioFolderPath;
        public static string videoFolderPath;
        public static string fontFolderPath;
        public static string svgFolderPath;
        public static string arlongFolder = @"\arlong";
        public static readonly string soundFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + arlongFolder;

        static ResourceHandler()
        {
            assembly = Assembly.GetExecutingAssembly();
            assemblyName = assembly.GetName().Name;
            
            assemblyLocation = assembly.Location;
            resourceFolder = assemblyLocation.Replace($"\\{assemblyName}.exe","") + @"\resources";
            
            svgExtension = ".svg";
            imageExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
            audioExtensions = new[] { ".mp3", ".wav", ".aiff" };
            videoExtensions = new[] { ".mp4", ".avi", ".mkv", ".webm" };
            fontExtensions = new[] { ".ttf", ".otf" };
            
            imageFolderPath = $"{resourceFolder}\\images";
            audioFolderPath = $"{resourceFolder}\\audios";
            videoFolderPath = $"{resourceFolder}\\videos";
            fontFolderPath = $"{resourceFolder}\\fonts";
            svgFolderPath = $"{resourceFolder}\\svg";
            resourceFolders = new[]
                { imageFolderPath, audioFolderPath, videoFolderPath, fontFolderPath, svgFolderPath };
        }

        public static List<Image> LoadSvg(string name)
        {
            return SvgHandler($@"{svgFolderPath}\\{name}");
        }

        public static List<Image> LoadAllSvg()
        {
            return SvgHandler(null);
        }

        private static List<Image> SvgHandler(string name)
        {
            SKSvg svgDocument = new SKSvg();
            List<Image> imageList = new List<Image>();
            if (name == null)
            {
                foreach (var filePath in Directory.GetFiles(resourceFolder + svgFolder))
                {
                    imageList.Add(LoadSingleSvg(filePath));
                }

                return imageList;
            }
            else
            {
                imageList.Add(LoadSingleSvg(name));
            }


            return imageList;

            Image LoadSingleSvg(string path)
            {
                svgDocument.Load(path);
                var w = svgDocument.Picture.CullRect.Width.ToString(CultureInfo.CurrentCulture);
                var h = svgDocument.Picture.CullRect.Height.ToString(CultureInfo.CurrentCulture);
                SKImage skImage = SKImage.FromPicture(svgDocument.Picture,
                    new SKSizeI(int.Parse(w),
                        int.Parse(h)));
                return Image.FromStream(skImage.Encode(SKEncodedImageFormat.Png, 100).AsStream());
            }
        }

        public static Image LoadImage(string name)
        {
            return ImageHandler(name).First();
        }

        private static List<Image> ImageHandler(string name)
        {
            List<Image> imageList = new List<Image>();
            if (name == null)
            {
                foreach (string filePath in Directory.GetFiles(imageFolderPath))
                {
                    imageList.Add(Image.FromFile(filePath));
                }
            }
            else
            {
                imageList.Add(Image.FromFile($"{imageFolderPath}\\{name}"));
            }

            return imageList;
        }
    }
}