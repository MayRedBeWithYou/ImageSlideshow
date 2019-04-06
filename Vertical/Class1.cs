using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static ImageSlideshow.Slideshow;

namespace Vertical
{
    public class Vertical : ISlideshowEffect
    {
        public string Name => "Vertical Effect";

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
            Storyboard StoryboardOut = new Storyboard();
            Storyboard StoryboardIn = new Storyboard();
            

            DoubleAnimation VerticalIn = new DoubleAnimation
            {
                From = 0,
                To = windowHeight,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            ThicknessAnimation ThicknessIn = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.5),
                From = new System.Windows.Thickness(0, windowHeight, 0, 0),
                To = new System.Windows.Thickness(0, 0, 0, 0)
            };

            Storyboard.SetTarget(VerticalIn, imageIn);
            Storyboard.SetTargetProperty(VerticalIn, new System.Windows.PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTarget(ThicknessIn, imageIn);
            Storyboard.SetTargetProperty(ThicknessIn, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

            StoryboardIn.Children.Add(VerticalIn);
            StoryboardIn.Children.Add(ThicknessIn);

            DoubleAnimation VerticalOut = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                From = windowHeight,
                To = 0
            };
            
            Storyboard.SetTarget(VerticalOut, imageOut);
            Storyboard.SetTargetProperty(VerticalOut, new System.Windows.PropertyPath(FrameworkElement.HeightProperty));

            StoryboardOut.Children.Add(VerticalOut);

            StoryboardOut.Begin(imageOut);
            StoryboardIn.Begin(imageIn);

        }
    }
}
