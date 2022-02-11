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
        CarModel car;

        public MainPage()
        {
            InitializeComponent();
            GetItemsInDb();
            car = new CarModel("Error", "SmallError");
        }

        private void GetItemsInDb()
        {
            imgList.ItemsSource = App.Db.GetItems();

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

                img.Source = ImageSource.FromFile(photo.FullPath);
                car = new CarModel(TitleEntry.Text, photo.FullPath);
                UpdateList();
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
                    Title = $"{TitleEntry.Text}.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                Debug.WriteLine($"Путь фото {photo.FullPath}");
                // загружаем в ImageView
                img.Source = ImageSource.FromFile(photo.FullPath);
                UpdateList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
        void UpdateList()
        {
            imgList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFullPath(f));
            imgList.SelectedItem = 0;
        }

        private void imgList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //img.Source = ImageSource.FromFile(e.SelectedItem.ToString());
        }

        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.Db.SaveItem(car);
            }
            catch (Exception ex)
            {
                await DisplayAlert("", $"{ex.Message}", "Ok");
            }
        }
    }
}
