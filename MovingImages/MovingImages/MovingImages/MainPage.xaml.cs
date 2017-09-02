using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovingImages
{
    public partial class MainPage : ContentPage
    {
        List<TouchManipulationBitmap> bitmapCollection =
            new List<TouchManipulationBitmap>();

        Dictionary<long, TouchManipulationBitmap> bitmapDictionary =
            new Dictionary<long, TouchManipulationBitmap>();

        public MainPage()
        {
            InitializeComponent();


            // Load in all the available bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = assembly.GetManifestResourceNames();
            SKPoint position = new SKPoint();

            foreach (string resourceID in resourceIDs)
            {
                if (resourceID.EndsWith(".png") ||
                    resourceID.EndsWith(".jpg"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    using (SKManagedStream skStream = new SKManagedStream(stream))
                    {
                        SKBitmap bitmap = SKBitmap.Decode(skStream);
                        bitmapCollection.Add(new TouchManipulationBitmap(bitmap)
                        {
                            Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
                        });
                        position.X += 100;
                        position.Y += 100;
                    }
                }
            }
        }

        

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
        }

        private void canvasView_Touch(object sender, SKTouchEventArgs args)
        {
        }
    }
}
