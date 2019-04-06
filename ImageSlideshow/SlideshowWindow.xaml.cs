using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ImageSlideshow
{
    public partial class SlideshowWindow : Window
    {

        private DispatcherTimer timerImageChange;
        private Image[] ImageRef;
        private int IntervalTimer = 3;
        private int SourceIndex = -1, ControlIndex;

        //private string TransitionName;
        public Slideshow.ISlideshowEffect TransitionEffect;

        public ObservableCollection<MainWindow.ImageContainer> Images;


        public SlideshowWindow(Object effect, Object imagelist)
        {
            InitializeComponent();
            TransitionEffect = effect as Slideshow.ISlideshowEffect;
            Images = imagelist as ObservableCollection<MainWindow.ImageContainer>;

            ImageRef = new[] { Image1, Image2 };

            timerImageChange = new DispatcherTimer();
            timerImageChange.Interval = new TimeSpan(0, 0, IntervalTimer);
            timerImageChange.Tick += new EventHandler(timerImageChange_Tick);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlaySlideshow();
            timerImageChange.IsEnabled = true;
        }

        private void PlaySlideshow()
        {
            try
            {
                var PrevControlIndex = ControlIndex;
                ControlIndex = (ControlIndex + 1) % 2;
                SourceIndex = (SourceIndex + 1) % Images.Count;

                Image ImageOut = ImageRef[PrevControlIndex];
                Image ImageIn = ImageRef[ControlIndex];
                ImageSource newSource = Images[SourceIndex].Image;
                ImageIn.Source = newSource;

                TransitionEffect.PlaySlideshow(ImageIn, ImageOut, 1024, 768);
            }
            catch (Exception) { }
        }

        private void timerImageChange_Tick(object sender, EventArgs e)
        {
            PlaySlideshow();
        }

        private void PlayPauseOption(object sender, RoutedEventArgs e)
        {
            timerImageChange.IsEnabled = timerImageChange.IsEnabled ? false : true;
        }

        private void StopSlideshow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Escape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }

    }
}
