using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchPointAnimated
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TouchPointAnimation1Page : ContentPage
    {
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;

        AnimatedTouchPoint _animatedTouchPoint;
        
        public TouchPointAnimation1Page()
        {
            InitializeComponent();

            _animatedTouchPoint = new AnimatedTouchPoint
            {
                CycleTime = 0.8,
                MaxRadius = 30,
                MinRadius = 10,
            };
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

            skCanvas.Clear();

            //skCanvas.Translate((float)skCanvasWidth / 2, (float)skCanvasHeight / 2);

            _animatedTouchPoint.AnimatingRadius
                    = _animatedTouchPoint.MinRadius * _animatedTouchPoint.CalculatedScaleValue
                                        + _animatedTouchPoint.MaxRadius * (1 - _animatedTouchPoint.CalculatedScaleValue);

            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Fill;
                paintTouchPoint.Color = SKColors.Red;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    _animatedTouchPoint.AnimatingRadius,
                    paintTouchPoint);
            }

            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Stroke;
                paintTouchPoint.Color = SKColors.Red.WithAlpha(150);
                paintTouchPoint.StrokeWidth = 20;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    _animatedTouchPoint.AnimatingRadius + 10,
                    paintTouchPoint);
            }

            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Stroke;
                paintTouchPoint.Color = SKColors.Red.WithAlpha(100);
                paintTouchPoint.StrokeWidth = 20;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    _animatedTouchPoint.AnimatingRadius + 30,
                    paintTouchPoint);
            }

            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Stroke;
                paintTouchPoint.Color = SKColors.Red.WithAlpha(60);
                paintTouchPoint.StrokeWidth = 20;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    _animatedTouchPoint.AnimatingRadius + 50,
                    paintTouchPoint);
            }
        }

        private SKPoint _lastTouchPoint = new SKPoint();
        private void CanvasView_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                _animatedTouchPoint.MaxRadius = 50;
                _animatedTouchPoint.MinRadius = 30;

                _lastTouchPoint = e.Location;
                e.Handled = true;
            }

            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Released)
            {
                _animatedTouchPoint.MaxRadius = 30;
                _animatedTouchPoint.MinRadius = 10;
            }

            _lastTouchPoint = e.Location;

            CanvasView.InvalidateSurface();
        }


        private async void InitAnimation()
        {
            pageIsActive = true;
            stopwatch.Start();

            while (pageIsActive)
            {
                double t = stopwatch.Elapsed.TotalSeconds %
                                    _animatedTouchPoint.CycleTime / _animatedTouchPoint.CycleTime;

                _animatedTouchPoint.CalculatedScaleValue
                            = (1 + (float)Math.Sin(2 * Math.PI * t)) / 2;

                CanvasView.InvalidateSurface();

                await Task.Delay(TimeSpan.FromSeconds(1.0 / 30));
            }

            stopwatch.Stop();
        }

    }


    public class AnimatedTouchPoint
    {
        /// <summary>
        /// 1 cycle's time in Milliseconds
        /// </summary>
        public double CycleTime { get; set; }

        /// <summary>
        /// MaxRadius of the touch point cyircle
        /// </summary>
        public float MaxRadius { get; set; }

        /// <summary>
        /// MinRadius of the touch point cyircle
        /// </summary>
        public float MinRadius { get; set; }

        /// <summary>
        /// CalculatedScaleValue at a given cycle 
        /// </summary>
        public float CalculatedScaleValue { get; set; }

        /// <summary>
        /// Animating Radius
        /// </summary>
        public float AnimatingRadius { get; set; }
    }
}