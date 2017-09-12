using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchAndImages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        bool pageIsActive;

        int _skCanvasWidth = 0;
        int _skCanvasHeight = 0;

        List<SmileyImage> _touchPoints;

        List<SKBitmap> _smileyImagesList;

        public Page2()
        {
            InitializeComponent();
            
            // Load in all the available bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = assembly.GetManifestResourceNames();

            _smileyImagesList = new List<SKBitmap>();

            foreach (string resourceID in resourceIDs)
            {
                if (resourceID.EndsWith(".png") ||
                    resourceID.EndsWith(".jpg"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    using (SKManagedStream skStream = new SKManagedStream(stream))
                    {
                        SKBitmap bitmap = SKBitmap.Decode(skStream);

                        _smileyImagesList.Add(bitmap);
                    }
                }
            }

            _touchPoints = new List<SmileyImage>();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitAnimation();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        private void SKCanvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var skImageInfo = e.Info;
            var skSurface = e.Surface;
            var skCanvas = skSurface.Canvas;

            var skCanvasWidth = skImageInfo.Width;
            var skCanvasHeight = skImageInfo.Height;

            _skCanvasWidth = skCanvasWidth;
            _skCanvasHeight = skCanvasHeight;

            skCanvas.Clear();
            
            foreach (var item in _touchPoints)
            {
                item.Width--;

                item.Alpha--;

                SKRect skRect = new SKRect();
                skRect.Size = new SKSize(item.Width, item.Width);
                skRect.Location = item.Location;

                skCanvas.DrawBitmap(
                    item.Bitmap,
                    skRect);
            }

            // remove ripple once it's disappeared
            _touchPoints.RemoveAll(x => x.Width == 0);
        }
        
        private SKPoint _lastTouchPoint = new SKPoint();
        private void CanvasView_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                _lastTouchPoint = e.Location;
                e.Handled = true;
            }

            _lastTouchPoint = e.Location;

            Random rand = new Random();
            _touchPoints.Add(
                new SmileyImage
                {
                    Location = _lastTouchPoint,
                    Bitmap = _smileyImagesList[rand.Next(0, _smileyImagesList.Count)],
                    Width = 120,
                    Alpha = 255,
                }
            );

            CanvasView.InvalidateSurface();
        }

        private void InitAnimation()
        {
            pageIsActive = true;

            Device.StartTimer(TimeSpan.FromSeconds(1.0 / 30), () => {

                if (_skCanvasWidth != 0 && _skCanvasHeight != 0)
                {
                    Random rand = new Random();
                    _touchPoints.Add(
                        new SmileyImage
                        {
                            Location = new SKPoint(rand.Next(_skCanvasWidth), rand.Next(_skCanvasHeight)),
                            Bitmap = _smileyImagesList[rand.Next(0, _smileyImagesList.Count)],
                            Width = 120,
                            Alpha = 255,
                        }
                    );
                }

                CanvasView.InvalidateSurface();

                return pageIsActive;
            });
        }
    }
}