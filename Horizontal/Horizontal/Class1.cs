using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static ImageSlideshow.Slideshow;

namespace Horizontal
{
    public class Horizontal : ISlideshowEffect
    {
        public string Name => "Horizontal Effect";

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
            Storyboard StoryboardOut = new Storyboard();
            Storyboard StoryboardIn = new Storyboard();

            DoubleAnimation HorizontalIn = new DoubleAnimation
            {
                From = 0,
                To = windowWidth,
                Duration = TimeSpan.FromSeconds(0.5)
            };
            ThicknessAnimation ThicknessIn = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(0),
                To = new System.Windows.Thickness(0, 0, 0, 0)
            };

            Storyboard.SetTarget(HorizontalIn, imageIn);
            Storyboard.SetTargetProperty(HorizontalIn, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTarget(ThicknessIn, imageIn);
            Storyboard.SetTargetProperty(ThicknessIn, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

            StoryboardIn.Children.Add(HorizontalIn);
            StoryboardIn.Children.Add(ThicknessIn);

            DoubleAnimation HorizontalOut = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                To = 0
            };
            ThicknessAnimation ThicknessOut = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                To = new System.Windows.Thickness(windowWidth, 0, 0, 0)
            };

            Storyboard.SetTarget(HorizontalOut, imageOut);
            Storyboard.SetTargetProperty(HorizontalOut, new System.Windows.PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTarget(ThicknessOut, imageOut);
            Storyboard.SetTargetProperty(ThicknessOut, new System.Windows.PropertyPath(FrameworkElement.MarginProperty));

            StoryboardOut.Children.Add(HorizontalOut);
            StoryboardOut.Children.Add(ThicknessOut);

            StoryboardOut.Begin(imageOut);
            StoryboardIn.Begin(imageIn);

        }
    }
}
