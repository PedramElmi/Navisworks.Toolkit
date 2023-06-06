using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace PedramElmi.Navisworks.Toolkit.Helper
{
    public static class IconImage
    {
        static IconImage()
        {
            SetIcons();
        }

        public static IReadOnlyDictionary<IconType, BitmapImage> Icons { get; private set; }

        private static BitmapImage GetEmbeddedImage(string resourceName)
        {
            // Get the executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get the resource stream for the embedded image
            using(var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if(stream == null)
                {
                    throw new ArgumentException($"Resource '{resourceName}' not found in the assembly.");
                }

                // Create a new BitmapImage
                var bitmap = new BitmapImage();

                // Initialize the BitmapImage
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze(); // Optional: Freeze the BitmapImage to improve performance

                return bitmap;
            }
        }

        private static void SetIcons()
        {
            var dict = new Dictionary<IconType, BitmapImage>();

            var iconTypes = (IconType[])Enum.GetValues(typeof(IconType));
            foreach(var iconType in iconTypes)
            {
                BitmapImage image = new BitmapImage();
                switch(iconType)
                {
                    case IconType.Unidentified:
                        goto default;
                    case IconType.File:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-2D8532F2-122E-4218-9E22-44C4BC834F7C.png");
                        break;
                    case IconType.Layer:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-4BCD09CF-FF0C-4B88-B473-B1025A17C100.png");
                        break;
                    case IconType.Collection:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-7AD510FA-7C48-415E-9579-D996820D8BC1.png");
                        break;
                    case IconType.CompositeObject:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-197CB0CC-4CBB-4308-A42C-0B7046B05392.png");
                        break;
                    case IconType.InsertGroup:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-A12DD8E6-A4BE-401A-BB86-6C80E4C4C1FB.png");
                        break;
                    case IconType.Geometry:
                        image = GetEmbeddedImage("PedramElmi.Navisworks.Toolkit.Images.Icons.GUID-8C08B821-22E1-45BA-9421-D9C5E577D4B0.png");
                        break;
                    default:
                        break;
                }
                dict.Add(iconType, image);
            }

            Icons = new ReadOnlyDictionary<IconType, BitmapImage>(dict);
        }
    }
}