using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadPhotoToLocalAppData.db;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadPhotoToLocalAppData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPage : ContentPage
    {
        CarModel car;
        public static string title { get; set; }
        private bool state = false;

        public PhotoPage(CarModel car)
        {
            this.car = car;
            title = car.Title;
            InitializeComponent();
            Update();
        }

        private void Update()
        {
            img.Source = ImageSource.FromFile(car.Path);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            state = !state;
            titleEntry.IsEnabled = state;
        }

        private void titleEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditConfirmIcon.IsVisible = true;
            car.Title = titleEntry.Text;
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                if (await DisplayAlert("", "Вы точно хотите изменить объект", "Да", "Нет"))
                {
                    App.Db.SaveItem(car);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void EditPhotoBtn_Clicked(object sender, EventArgs e)
        {
            try
            {

                // выбираем фото
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = $"xamarin.{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.png"
                });

                var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), photo.FileName);

                // загружаем в ImageView

                car.Path = photo.FullPath;
                EditConfirmIcon.IsVisible = true;
                Update();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
    }
}