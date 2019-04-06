using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using MenuItem = System.Windows.Controls.MenuItem;

namespace ImageSlideshow
{
    
    public partial class MainWindow : Window
    {

        public class ImageContainer
        {
            public string Name { get; set; }
            public System.Drawing.Image ImageDetails { get; set; }
            public BitmapImage Image { get; set; }
            public string Size { get; set; }
        }

        public ObservableCollection<ImageContainer> images = new ObservableCollection<ImageContainer>();
        public ObservableCollection<Slideshow.ISlideshowEffect> SlideshowTypes = new ObservableCollection<Slideshow.ISlideshowEffect>();

        public MainWindow()
        {
            InitializeComponent();
            PictureView.ItemsSource = images;

            var plugins = Directory.GetFiles(@Directory.GetCurrentDirectory()).Where(f => f.EndsWith(".dll"));

            foreach(var plugin in plugins)
            {
                Assembly DLL = Assembly.LoadFrom(plugin);
                foreach (Type type in DLL.GetExportedTypes())
                {
                    if (typeof(Slideshow.ISlideshowEffect).IsAssignableFrom(type))
                    {
                        if (type.IsInterface) continue;
                        Slideshow.ISlideshowEffect effect = Activator.CreateInstance(type) as Slideshow.ISlideshowEffect;
                        if (effect == null) continue;
                        else SlideshowTypes.Add(effect);
                    }
                }
            }

            SlideshowTypesHeader.ItemsSource = SlideshowTypes;
            SlideshowEffectsList.ItemsSource = SlideshowTypes;
            SlideshowEffectsList.SelectedIndex = 0;

            this.Loaded += new RoutedEventHandler(LoadTreeView);
        }

        private void LoadFolder(object sender, RoutedEventArgs e)
        {
            string path = string.Empty;

            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    path = fb.SelectedPath;
                    images.Clear();
                    var files = Directory.GetFiles(path, "*.*").Where(s => s.EndsWith(".png") || s.EndsWith(".jpg") || s.EndsWith(".jpeg"));
                    foreach (string file in files)
                    {
                        string[] filename = file.Split('\\');
                        ImageContainer image = new ImageContainer
                        {
                            Name = filename[filename.Length - 1],
                            ImageDetails = System.Drawing.Image.FromFile(file),
                            Image = new BitmapImage(new Uri(file))
                        };
                        images.Add(image);
                    }
                }
                catch (Exception) { }
            }
        }


        private object Dummy = null;

        void LoadTreeView(object sender, RoutedEventArgs e)
        {
            foreach (string Drive in Directory.GetLogicalDrives())
            {
                TreeViewItem Directory = new TreeViewItem();
                Directory.Header = Drive;
                Directory.Tag = Drive;
                Directory.Items.Add(Dummy);
                Directory.Expanded += FolderExpanded;

                FolderTreeView.Items.Add(Directory);
            }
        }

        void FolderExpanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem ParentDirectory = (TreeViewItem)sender;
            if (ParentDirectory.Items.Count == 1 && ParentDirectory.Items[0] == Dummy)
            {
                ParentDirectory.Items.Clear();
                try
                {
                    foreach (string dir in Directory.GetDirectories(ParentDirectory.Tag as string))
                    {
                        TreeViewItem ChildDirectory = new TreeViewItem();
                        ChildDirectory.Header = new DirectoryInfo(dir).Name;
                        ChildDirectory.Tag = dir;
                        ChildDirectory.Items.Add(Dummy);
                        ChildDirectory.Expanded += FolderExpanded;
                        ParentDirectory.Items.Add(ChildDirectory);
                    }
                }
                catch (Exception) { }
            }
        }

        private void ExitOption(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutOption(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("This is a test application made for PwŚG course.", "About");
        }

        private void LoadImagesFromTree(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NoFileSelected.Visibility = Visibility.Visible;
            ImageInfo.Visibility = Visibility.Collapsed;
            try
            {
                if (e.NewValue == null) return;
                TreeViewItem directory = (TreeViewItem)e.NewValue;
                string path = directory.Tag as string;
                images.Clear();

                var files = Directory.GetFiles(path, "*.*").Where(s => s.ToLower().EndsWith(".png") || s.ToLower().EndsWith(".jpg") || s.ToLower().EndsWith(".jpeg"));
                foreach (string file in files)
                {
                    string[] filename = file.Split('\\');
                    ImageContainer image = new ImageContainer
                    {
                        Name = filename[filename.Length - 1],
                        ImageDetails = System.Drawing.Image.FromFile(file),
                        Image = new BitmapImage(new Uri(file)),
                        Size = (new FileInfo(file).Length / 1024).ToString() + " KB"
                    };

                    images.Add(image);
                }
            }
            catch (Exception) { }
        }

        private void PictureSelected(object sender, SelectionChangedEventArgs e)
        {
            ImageContainer image = e.AddedItems[0] as ImageContainer;
            NoFileSelected.Visibility = Visibility.Collapsed;
            ImageInfo.Visibility = Visibility.Visible;
            FileInfoExpander.IsExpanded = true;

            FileNameField.Content = image.Name;
            WidthField.Content = image.ImageDetails.Width + " px";
            HeightField.Content = image.ImageDetails.Height + " px";
            SizeField.Content = image.Size;

            ScrollBarLeft.ScrollToBottom();
        }

        private void StartSlideshow(object sender, RoutedEventArgs e)
        {
            if(images.Count==0)
            {
                System.Windows.MessageBox.Show("There are no images to show in this folder.", "No images here!");
            }
            else if (sender is MenuItem)
            {
                var window = new SlideshowWindow(((MenuItem)e.OriginalSource).CommandParameter, images) { Owner = this };
                window.ShowDialog();
            }
            else
            {
                var window = new SlideshowWindow(SlideshowEffectsList.SelectedItem, images) { Owner = this };
                window.ShowDialog();
            }
        }
    }
}

