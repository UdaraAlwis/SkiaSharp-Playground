using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TouchPointAnimated
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void HeartBeatEffectTouchPointPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TouchPointAnimation1Page());
        }

        private void RippleEffectTouchPointPageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TouchPointAnimation2Page());
        }
    }
}
