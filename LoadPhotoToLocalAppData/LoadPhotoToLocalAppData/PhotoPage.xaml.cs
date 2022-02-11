using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadPhotoToLocalAppData.db;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoadPhotoToLocalAppData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPage : ContentPage
    {
        CarModel car;
        public static string title { get; set; }

        public PhotoPage(CarModel car)
        {
            this.car = car;
            title = car.Title;
            InitializeComponent();
            img.Source = ImageSource.FromFile(car.Path);
        }
    }
}