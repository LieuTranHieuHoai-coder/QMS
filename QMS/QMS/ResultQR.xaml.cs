using Newtonsoft.Json;
using QMS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultQR : ContentPage
    {
        private string QRCodeNumber { get; set; }

        public StickerInformation QrCodeData = new StickerInformation();
        public ResultQR()
        {
            InitializeComponent();
        }
        public ResultQR(string codeNumber)
        {
            InitializeComponent();
            QRCodeNumber = codeNumber;
        }

        private async Task GetStickerInformation()
        {
            try
            {
                if (!string.IsNullOrEmpty(QRCodeNumber))
                {
                    string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";
                    /*
                    string data = @"{func:'SelectOrderQRCodeData',da:{qrcode:'" + QRCodeNumber + "'," +
                        "site:'" + Commons.GlobalDefines.UserInfo.site + "'," +
                        "sewingline:'" + Commons.GlobalDefines.UserInfo.sewingline + "'}}";
                    */
                    //form-data
                    ScanData data = new ScanData
                    {
                        qrcode = QRCodeNumber,
                        site = Commons.GlobalDefines.WorkingSite,
                        sewingline = Commons.GlobalDefines.WorkingLine
                    };
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","SelectBundleOrderQRCodeData"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("da[qrcode]",data.qrcode),
                                new KeyValuePair<string, string>("da[site]",data.site),
                                new KeyValuePair<string, string>("da[sewingline]",data.sewingline),

                                });
                    HttpClient client = new HttpClient();
                    //string jsonData = JsonConvert.SerializeObject(data);
                    //StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        QmsApiResponseData resData = new QmsApiResponseData();
                        resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                        if (resData.rescode == 200)
                        {
                            QrCodeData = resData.data[0];
                            //StickerInformation sticker = JsonConvert.DeserializeObject<StickerInformation>(resData.data);
                            lbWorkNo.Text = resData.data[0].WorkNo;
                            lblCustomer.Text = resData.data[0].Customer;
                            lblstyle.Text = resData.data[0].Style;
                            lblOrder.Text = resData.data[0].BuyMonth;
                            lblSeason.Text = resData.data[0].Season;
                            lblcolor.Text = resData.data[0].ColorName;
                            lblSize.Text = resData.data[0].SizeName;
                            lblPO.Text = resData.data[0].PoNo;
                            lblnumberQR.Text = resData.data[0].CodeId;


                            resData = new QmsApiResponseData();
                        }
                        if (resData.rescode == 412)
                        {
                            btnSave.IsEnabled = false;
                            lblMessage.Text = "Mã QRCode Đã Scan";
                            // await DisplayAlert("Cảnh Báo", "Mã QRCode đã được đóng gói", "Close");
                            btnSave.IsVisible = false;
                        }
                        if (resData.rescode == 414)
                        {
                            btnSave.IsEnabled = false;
                            lblMessage.Text = " Mã QRCode Đã Tồn Tại";
                            btnSave.IsVisible = false;
                            //await DisplayAlert("Cảnh Báo", "Mã QRCode được thông qua", "Close");
                        }
                        if (resData.rescode == 416)
                        {
                            btnSave.IsEnabled = false;
                            lblMessage.Text = " Mã QRCode Không Đúng";
                            //await DisplayAlert("Cảnh Báo", "Mã QRCode không khả dụng", "Close");
                            btnSave.IsVisible = false;
                        }
                        if (resData.rescode == 423)
                        {
                            btnSave.IsEnabled = false;
                            lblMessage.Text = "Lỗi Cài Đặt Chuyền May";
                            btnSave.IsVisible = false;
                            //await DisplayAlert("Cảnh Báo", "Chuyền may của người dùng bị lỗi", "Close");
                        }
                        if (resData.rescode==413)
                        {
                            btnSave.IsEnabled = false;
                            lblMessage.Text = "Mã QR Chưa Được Tích Hợp";
                            btnSave.IsVisible = false;
                        }

                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert(ex.Source, ex.Message, "Close");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblMessage.Text = "Đang truy vấn dữ liệu...";
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetStickerInformation();
               
            });

        }
        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            btnSave.IsEnabled = true;
            await Navigation.PopModalAsync(); //close (remove) result page -> back to scan page, don't use push (open new scan page)
        }

        private async void btnSaved_Clicked(object sender, EventArgs e)
        {

            var client = new HttpClient(new HttpClientHandler());
            if (!string.IsNullOrEmpty(QRCodeNumber))
            {
                string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";

                ScanData data = new ScanData
                {
                    qrcode = QRCodeNumber,
                    site = Commons.GlobalDefines.WorkingSite,
                    sewingline = Commons.GlobalDefines.WorkingLine
                };
                FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdatePrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype","P"),
                                new KeyValuePair<string, string>("worktype",""),
                                new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
                                new KeyValuePair<string, string>("customer",QrCodeData.Customer),
                                new KeyValuePair<string, string>("season",QrCodeData.Season),
                                new KeyValuePair<string, string>("buymonth",QrCodeData.BuyMonth),
                                new KeyValuePair<string, string>("style",QrCodeData.Style),
                                new KeyValuePair<string, string>("color",QrCodeData.ColorName),
                                new KeyValuePair<string, string>("size",QrCodeData.SizeName),
                                new KeyValuePair<string, string>("pono",QrCodeData.PoNo),
                                new KeyValuePair<string, string>("oemno","choose"),
                                new KeyValuePair<string, string>("qrcode",data.qrcode),
                                new KeyValuePair<string, string>("site",QrCodeData.Factory),
                                new KeyValuePair<string, string>("floor",Commons.GlobalDefines.WorkingFactory),
                                new KeyValuePair<string, string>("sewingline",data.sewingline),
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.uid),
                                });

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string result = await response.Content.ReadAsStringAsync();
                QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
               // await DisplayAlert("Success", resData.rescode.ToString(), "Close");
                if (response.IsSuccessStatusCode)
                {

                    if (resData.rescode == 411)
                    {
                        btnSave.IsVisible = false;
                        lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                    }
                }
                else
                {
                    await DisplayAlert("Error", response.ReasonPhrase, "Close");
                }
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                btnSave.IsVisible = false;
            });
        }

        //private async void btnback_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopModalAsync();
        //}
    }
}