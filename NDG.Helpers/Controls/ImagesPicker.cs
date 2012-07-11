using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace NDG.Helpers.Controls
{
    public enum ImagesPickerMode
    {
        Single,
        Multiply
    }

    public class ImagePickerItem
    {
        public BitmapImage Image { get; set; }

        public RelayCommand<ImagePickerItem> RemoveCommand { get; set; }
    }

    public class ImagesPicker : Control
    {
        #region DependencyProperties

        public static readonly DependencyProperty ImagesPickModeProperty = DependencyProperty.Register("ImagesPickMode", typeof(ImagesPickerMode), typeof(ImagesPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty AddButtonStyleProperty = DependencyProperty.Register("AddButtonStyle", typeof(string), typeof(ImagesPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveButtonStyleProperty = DependencyProperty.Register("RemoveButtonStyle", typeof(string), typeof(ImagesPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedImageProperty = DependencyProperty.Register("SelectedImage", typeof(BitmapImage), typeof(ImagesPicker), new PropertyMetadata(OnSelectedImageChenged));

        public static readonly DependencyProperty ImagesCollectionProperty = DependencyProperty.Register("ImagesCollection", typeof(List<BitmapImage>), typeof(ImagesPicker), new PropertyMetadata(null));

        public static readonly DependencyProperty ImageItemStyleProperty = DependencyProperty.Register("ImageItemStyle", typeof(Style), typeof(ImagesPicker), new PropertyMetadata(null));

        #endregion DependencyProperties

        private Button addButton;

        private bool wasAplpyTemplate = false;

        private bool isSelectedImageChanged = false;

        private Collection<ImagePickerItem> items;

        public BitmapImage SelectedImage
        {
            get { return (BitmapImage)this.GetValue(SelectedImageProperty); }
            set { this.SetValue(SelectedImageProperty, value); }
        }

        public ImagesPickerMode ImagesPickMode
        {
            get { return (ImagesPickerMode)this.GetValue(ImagesPickModeProperty); }
            set { this.SetValue(ImagesPickModeProperty, value); }
        }

        public List<BitmapImage> ImagesCollection
        {
            get
            {
                return (List<BitmapImage>)this.GetValue(ImagesCollectionProperty);
            }

            set
            {
                this.SetValue(ImagesCollectionProperty, value);
            }
        }

        public Style ImageItemStyle
        {
            get
            {
                return (Style)this.GetValue(ImageItemStyleProperty);
            }

            set
            {
                this.SetValue(ImageItemStyleProperty, value);
            }
        }

        public string AddButtonStyle
        {
            get { return (string)this.GetValue(AddButtonStyleProperty); }
            set { this.SetValue(AddButtonStyleProperty, value); }
        }

        public string RemoveButtonStyle
        {
            get { return (string)this.GetValue(RemoveButtonStyleProperty); }
            set { this.SetValue(RemoveButtonStyleProperty, value); }
        }

        public ImagesPicker()
        {
            this.DefaultStyleKey = typeof(ImagesPicker);
        }

        ItemsControl itemsControl;

        public event EventHandler SelectedImageChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.addButton = this.GetTemplateChild("addButton") as Button;
            this.addButton.Click += this.OnAddButtonClick;
            this.itemsControl = this.GetTemplateChild("itemsControl") as ItemsControl;
            this.itemsControl.ItemsSource = this.items;
            this.wasAplpyTemplate = true;
            if (this.isSelectedImageChanged)
            {
                AddSelectedImage(this);
            }
        }

        private static void OnSelectedImageChenged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ImagesPicker picker = (ImagesPicker)sender;
            if (picker.wasAplpyTemplate)
            {
                picker.AddSelectedImage(picker);
            }
            else
            {
                picker.isSelectedImageChanged = true;
            }

            if (picker.SelectedImageChanged != null)
            {
                picker.SelectedImageChanged.Invoke(picker, new EventArgs());
            }
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.ImagesPickMode == ImagesPickerMode.Single)
            {
                this.addButton.Visibility = System.Windows.Visibility.Collapsed;
                this.addButton.Click -= this.OnAddButtonClick;
            }

            PhotoChooserTask photoTask = new PhotoChooserTask();
            photoTask.ShowCamera = true;
            photoTask.Completed += this.OnPhotoChoosed;
            photoTask.Show();
        }

        private void AddSelectedImage(ImagesPicker picker)
        {
            if (picker.SelectedImage != null && picker.ImagesPickMode == ImagesPickerMode.Single
                && (picker.ImagesCollection == null || !picker.ImagesCollection.Contains(picker.SelectedImage)))
            {
                picker.addButton.Click -= picker.OnAddButtonClick;
                picker.addButton.Visibility = System.Windows.Visibility.Collapsed;
                picker.ImagesCollection = new List<BitmapImage>();
                picker.items = new Collection<ImagePickerItem>();
                picker.ImagesCollection.Add(picker.SelectedImage);
                picker.items.Add(new ImagePickerItem() { Image = picker.SelectedImage, RemoveCommand = new RelayCommand<ImagePickerItem>(picker.OnRemoveButtonClick) });
                picker.itemsControl.ItemsSource = null;
                picker.itemsControl.ItemsSource = picker.items;
            }
        }

        private void OnPhotoChoosed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK && e.ChosenPhoto != null)
            {
                BitmapImage image = new BitmapImage();
                image.SetSource(e.ChosenPhoto);
                e.ChosenPhoto.Close();
                if (this.ImagesCollection == null)
                {
                    this.ImagesCollection = new List<BitmapImage>();
                    this.items = new Collection<ImagePickerItem>();
                }

                this.ImagesCollection.Add(image);
                this.items.Add(new ImagePickerItem() { Image = image, RemoveCommand = new RelayCommand<ImagePickerItem>(this.OnRemoveButtonClick) });
                this.itemsControl.ItemsSource = null;
                this.itemsControl.ItemsSource = this.items;
            }

            if (this.ImagesPickMode == ImagesPickerMode.Single)
            {
                this.SelectedImage = null;
                if (this.ImagesCollection != null && this.ImagesCollection.Count != 0)
                    this.SelectedImage = this.ImagesCollection.FirstOrDefault();
                if (this.SelectedImage == null)
                {
                    this.addButton.Visibility = System.Windows.Visibility.Visible;
                    this.addButton.Click += this.OnAddButtonClick;
                }
            }
        }

        public void OnRemoveButtonClick(ImagePickerItem item)
        {
            this.ImagesCollection.Remove(item.Image);
            this.items.Remove(item);
            this.itemsControl.ItemsSource = null;
            this.itemsControl.ItemsSource = this.items;
            if (this.ImagesPickMode == ImagesPickerMode.Single)
            {
                this.SelectedImage = null;
                this.addButton.Click += this.OnAddButtonClick;
                this.addButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
