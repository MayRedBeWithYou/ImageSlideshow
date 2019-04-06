using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static ImageSlideshow.Slideshow;

namespace Fade
{
    public class Fade : ISlideshowEffect
    {
        public string Name => "Fade Effect";

        public void PlaySlideshow(Image imageIn, Image imageOut, double windowWidth, double windowHeight)
        {
            Storyboard StoryboardOut = new Storyboard();
            Storyboard StoryboardIn = new Storyboard();

            DoubleAnimation FadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            Storyboard.SetTarget(FadeIn, imageIn);
            Storyboard.SetTargetProperty(FadeIn, new System.Windows.PropertyPath(UIElement.OpacityProperty));

            StoryboardIn.Children.Add(FadeIn);

            DoubleAnimation FadeOut = new DoubleAnimation
            {
                Duration = TimeSpan.FromSeconds(0.5),
                To = 0
            };

            Storyboard.SetTarget(FadeOut, imageOut);
            Storyboard.SetTargetProperty(FadeOut, new System.Windows.PropertyPath(UIElement.OpacityProperty));

            StoryboardOut.Children.Add(FadeOut);

            StoryboardOut.Begin(imageOut);
            StoryboardIn.Begin(imageIn);


        }
    }
}
