
using GoogleVisionBarCodeScanner;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQR : ContentPage
    {

        public ScanQR()
        {
            InitializeComponent();
        }
        private void CameraView_OnDetectedAsync(object sender, OnDetectedEventArg e)
        {
            List<BarcodeResult> barcodeResults = e.BarcodeResults;
            string result = string.Empty;
            if (barcodeResults.Count > 0)
            {
                result = barcodeResults[0].DisplayValue;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                Methods.SetIsScanning(false); //disable scanning
                await Navigation.PushModalAsync(new ResultQR(result));

            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Methods.SetIsScanning(true); //enable scanning
        }

        private async void btnBack_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnscan_Clicked(object sender, System.EventArgs e)
        {
            if (txtqrcode.Text==null)
            {
                 await DisplayAlert("Msg", " Nhập Mã QR", "OK");
                 txtqrcode.Focus();
            }       
                else if (txtqrcode.Text != null)
            {
                string result = txtqrcode.Text;
                await Navigation.PushModalAsync(new ResultQR(result));
                txtqrcode.Text = "";
            }     
              
        }
    }
}