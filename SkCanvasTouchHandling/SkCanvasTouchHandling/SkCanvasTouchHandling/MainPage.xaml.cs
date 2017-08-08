using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SkCanvasTouchHandling
{
    public partial class MainPage : ContentPage
    {
        string _touchEventString = "";

        string _touchMoveDirectionString = "";

        public MainPage()
        {
            InitializeComponent();
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

            using (SKPaint paintTouchPoint = new SKPaint())
            {
                paintTouchPoint.Style = SKPaintStyle.Fill;
                paintTouchPoint.Color = SKColors.Black;
                paintTouchPoint.IsDither = true;
                skCanvas.DrawCircle(
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y,
                    skCanvasWidth / 17f, paintTouchPoint); // 45
            }

            using (SKPaint paintTouchEvent = new SKPaint())
            {
                paintTouchEvent.Color = SKColors.Red;
                paintTouchEvent.TextAlign = SKTextAlign.Center;
                paintTouchEvent.TextSize = 35;
                paintTouchEvent.FakeBoldText = true;
                paintTouchEvent.IsAntialias = true;
                skCanvas.DrawText(_touchEventString,
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y - (skCanvasWidth / 17f) - 10,
                    paintTouchEvent);
            }

            using (SKPaint paintTouchMoveDirection = new SKPaint())
            {
                paintTouchMoveDirection.Color = SKColors.CornflowerBlue;
                paintTouchMoveDirection.TextAlign = SKTextAlign.Center;
                paintTouchMoveDirection.TextSize = 35;
                paintTouchMoveDirection.FakeBoldText = true;
                paintTouchMoveDirection.IsAntialias = true;
                skCanvas.DrawText(_touchMoveDirectionString,
                    _lastTouchPoint.X,
                    _lastTouchPoint.Y - (skCanvasWidth / 17f) - 45,
                    paintTouchMoveDirection);
            }
        }


        private SKPoint _lastTouchPoint = new SKPoint();
        private void CanvasView_Touch(object sender, SkiaSharp.Views.Forms.SKTouchEventArgs e)
        {
            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                _lastTouchPoint = e.Location;
                e.Handled = true;
            }
            else if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Moved)
            {
                if (_lastTouchPoint.Y < e.Location.Y)
                {
                    // swipe down
                    _touchMoveDirectionString = "Down";
                }
                else if (_lastTouchPoint.Y > e.Location.Y)
                {
                    // swipe up
                    _touchMoveDirectionString = "Up";
                }

                _lastTouchPoint = e.Location;
            }

            if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Pressed)
            {
                _touchEventString = "Pressed";
                _touchMoveDirectionString = "";
            }
            else if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Moved)
            {
                _touchEventString = "Moved";
            }
            else if (e.ActionType == SkiaSharp.Views.Forms.SKTouchAction.Released)
            {
                _touchEventString = "Released";
                _touchMoveDirectionString = "";
            }

            CanvasView.InvalidateSurface();
        }

    }
}
