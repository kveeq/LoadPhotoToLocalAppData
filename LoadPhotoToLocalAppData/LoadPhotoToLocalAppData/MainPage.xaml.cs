using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LoadPhotoToLocalAppData
{
    public partial class MainPage : ContentPage
    {
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string path;

        public MainPage()
        {
            InitializeComponent();
            UpdateList();
            imgList.RefreshCommand = new Command(() =>
            {
                UpdateList();
                imgList.IsRefreshing = false;
            });
        }

        async void GetPhotoAsync(object sender, EventArgs e)
        {
            try
            {
                // выбираем фото
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = $"{TitleEntry.Text}.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), photo.FileName);

                // загружаем в ImageView

                path = photo.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateList();
        }

        async void TakePhotoAsync(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                    await DisplayAlert("", "Copy", "ok");
                }

                Debug.WriteLine($"Путь фото {photo.FullPath}");
                // загружаем в ImageView
                path = photo.FullPath;
                await DisplayAlert("", path, "ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
        void UpdateList()
        {
            //imgList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFullPath(f));
            imgList.ItemsSource = null;
            imgList.ItemsSource = App.Db.GetItems();
        }

        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Db.SaveItem(new CarModel(path, TitleEntry.Text));
            }
            catch (Exception ex)
            {
                await DisplayAlert("", $"{ex.Message}", "Ok");
            }
        }

        private async void imgList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //img.Source = ImageSource.FromFile(e.SelectedItem.ToString());
            await Navigation.PushAsync(new PhotoPage((CarModel)e.Item));
        }

        private void SwipeItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var id = ((SwipeItem)sender).CommandParameter.ToString();
                App.Db.DeleteItem(int.Parse(id));
                UpdateList();
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "ok");
            }
        }
    }
}
