using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace QMS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // lbltime.Text += DateTime.Now.ToString("T") + " " + DateTime.Now.ToString("dd/MM/yyyy");
            lbltime.Text +=  DateTime.Now.ToString("dd/MM/yyyy");
        }

        private async void Btnlogin_ClickedAsync(object sender, EventArgs e)
        {
            if (CheckInternet())
            {
                var apiUrl = "http://qvnqms.qve.com.vn/aspnetapi.php";
                //await Navigation.PushModalAsync(new Setting());
                if (txtuser.Text == null || txtuser.Text == null)
                {
                    await DisplayAlert("Msg", "Input your information ", "OK");
                    txtuser.Focus();
                }

                else
                {

                    var user = txtuser.Text;
                    var pw = txtpassword.Text;
                    var client = new HttpClient(new HttpClientHandler());
                    //var jsonData = JsonConvert.SerializeObject(postDataLogin);
                    //var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("func", "LoginAuth"),
                    new KeyValuePair<string, string>("uid", user),
                    new KeyValuePair<string, string>("pw", pw)
                });
                    var response = await client.PostAsync(apiUrl, content);

                    //var result = await responseContent.ReadAsStringAsync();
                    var userResp = JsonConvert.DeserializeObject<Models.LoggedUser>(await response.Content.ReadAsStringAsync());
                    if (userResp.rescode == "200")
                    {
                        Commons.GlobalDefines.LoggedUser = userResp;
                        // await DisplayAlert("Success", "Login Success", "OK");
                        await Navigation.PushModalAsync(new Setting());

                    }
                    else
                    {
                        await DisplayAlert("Lỗi", "Lỗi Đăng Nhập. (Error!)", "OK");

                    }

                }
            }
            else
            {
                _ = DisplayAlert("Msg", "No Internet, pls connect!. Chưa kết nối Internet!", "OK");
                return;
            }

        }
        private bool CheckInternet()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }

            return true;
        }

      

        private void ShowPass(object sender, EventArgs e)
        {
            txtpassword.IsPassword = txtpassword.IsPassword ? false : true;
        }
    }
}
