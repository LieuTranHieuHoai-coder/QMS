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
    public partial class Setting : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ScanData scan = new ScanData();
        public List<Site> siteList { get; set; }
        public List<Site> factoryList { get; set; }
        public List<SewingLine> sewingsList { get; set; }
        public string site = "";
        public string sitefactory = "";
        public string sewingline = "";
        string apiUrl = "http://qvnqms.qve.com.vn/chooseplace.php";
        public Setting()
        {
            InitializeComponent();
            ShowLocations();
            ShowFactories();
            ShowSewingline();
        }
        // API 
        public async Task<List<Factory>> GetLocationsAsync()
        {
            Factory temp = new Factory();
            List<Factory> f = new List<Factory>();

            var client = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("key","FactoryName"),
            });
            var response = await client.PostAsync(apiUrl, content);
            var res = response.Content.ReadAsStringAsync();
            string splitResult = res.Result.ToString();
            string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
            for (int i = 0; i < arrFactory.Length; i++)
            {
                if (!arrFactory[i].Equals(""))
                {
                    temp.FactoryName = arrFactory[i];
                    temp.ProductionWorkShop = "";
                    f.Add(new Factory()
                    {
                        FactoryName = temp.FactoryName,
                        ProductionWorkShop = temp.ProductionWorkShop
                    });
                }
            }
            f.RemoveAt(0);
            return f;
        }
        public async Task<List<Factory>> GetFactoriesAsync(string site)
        {

            Factory temp = new Factory();
            List<Factory> f = new List<Factory>();

            var client = new HttpClient(new HttpClientHandler());
            if (site != null && !site.Equals(""))
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","ProductionWorkShop"),
                    new KeyValuePair<string,string>("site",site)
                });
                var response = await client.PostAsync(apiUrl, content);
                var res = response.Content.ReadAsStringAsync();
                string splitResult = res.Result.ToString();
                string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                for (int i = 0; i < arrFactory.Length; i++)
                {
                    if (!arrFactory[i].Equals(""))
                    {
                        temp.FactoryName = arrFactory[i];
                        temp.ProductionWorkShop = "";
                        f.Add(new Factory()
                        {
                            FactoryName = temp.FactoryName,
                            ProductionWorkShop = temp.ProductionWorkShop
                        });
                    }
                }
                f.RemoveAt(0);
            }
            else
            {
                PkFactory.IsEnabled = false;
            }

            return f;
        }
        public async Task<List<SewingLine>> GetSewingLineAsync(string site, string factory)
        {

            SewingLine temp = new SewingLine();
            List<SewingLine> f = new List<SewingLine>();

            var client = new HttpClient(new HttpClientHandler());
            if (site != null && !site.Equals("") && factory != null && !factory.Equals(""))
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","SewingLine"),
                    new KeyValuePair<string,string>("site",site),
                    new KeyValuePair<string,string>("factory",factory)
                });
                var response = await client.PostAsync(apiUrl, content);
                var res = response.Content.ReadAsStringAsync();
                string splitResult = res.Result.ToString();
                string[] arrSewing = splitResult.Split(',', '"', '\n', '[', ']');
                for (int i = 0; i < arrSewing.Length; i++)
                {
                    if (!arrSewing[i].Equals(""))
                    {
                        temp.Line = arrSewing[i];

                        f.Add(new SewingLine()
                        {
                            Line = temp.Line
                        });
                    }
                }
                f.RemoveAt(0);
            }
            else
            {
                PkSewingline.IsEnabled = false;
            }

            return f;
        }

        //Show content
        public async void ShowLocations()
        {
            PkSite.ItemsSource = await GetLocationsAsync();
        }
        public async void ShowFactories()
        {
            PkFactory.ItemsSource = await GetFactoriesAsync(sitefactory);
        }
        public async void ShowSewingline()
        {
            PkSewingline.ItemsSource = await GetSewingLineAsync(sitefactory, sewingline);
        }

        // Event
        private async void Btnlogout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnconfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (PkSite.SelectedIndex > -1 && PkFactory.SelectedIndex > -1 && PkSewingline.SelectedIndex > -1)
                {
                    Commons.GlobalDefines.WorkingSite = PkSite.Items[PkSite.SelectedIndex].ToString();
                    Commons.GlobalDefines.WorkingFactory = PkFactory.Items[PkFactory.SelectedIndex].ToString();
                    Commons.GlobalDefines.WorkingLine = PkSewingline.Items[PkSewingline.SelectedIndex].ToString();
                    await Navigation.PushModalAsync(new ScanQR());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "Close");
            }
        }

        [Obsolete]
        private async void imgclose_Clicked(object sender, EventArgs e)
        {
            //await Rg.Plugins.Popup.Services.PopupNavigation.PopAsync();
            await Navigation.PushModalAsync(new MainPage());
        }

        private void pkSite_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sitefff = PkSite.Items[PkSite.SelectedIndex].ToString();

            Task.Delay(200)
            .ContinueWith(async t =>
            {
                Factory temp = new Factory();
                List<Factory> f = new List<Factory>();

                var client = new HttpClient(new HttpClientHandler());
                if (sitefff != null)
                {
                    var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","ProductionWorkShop"),
                    new KeyValuePair<string,string>("site",sitefff)
                });
                    var response = await client.PostAsync(apiUrl, content);
                    var res = response.Content.ReadAsStringAsync();
                    string splitResult = res.Result.ToString();
                    string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                    for (int i = 0; i < arrFactory.Length; i++)
                    {
                        if (!arrFactory[i].Equals(""))
                        {
                            temp.FactoryName = arrFactory[i];
                            temp.ProductionWorkShop = "";
                            f.Add(new Factory()
                            {
                                FactoryName = temp.FactoryName,
                                ProductionWorkShop = temp.ProductionWorkShop
                            });
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    f.RemoveAt(0);

                    PkFactory.ItemsSource = f;

                    PkFactory.IsEnabled = true;
                });
            });
            site = sitefff;
        }

        private void pkFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sitefff = PkSite.Items[PkSite.SelectedIndex].ToString();
            if (PkFactory.SelectedIndex == -1)
            {
                PkSewingline.SelectedItem = null;
                PkSewingline.IsEnabled = false;
                return;
            }
            string factory = PkFactory.Items[PkFactory.SelectedIndex].ToString();

            Task.Delay(200)
            .ContinueWith(async t =>
            {
                SewingLine temp = new SewingLine();
                List<SewingLine> f = new List<SewingLine>();

                var client = new HttpClient(new HttpClientHandler());
                if (sitefff != null && !sitefff.Equals("") && factory != null && !factory.Equals(""))
                {
                    var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","SewingLine"),
                    new KeyValuePair<string,string>("site",sitefff),
                    new KeyValuePair<string,string>("factory",factory)
                });
                    var response = await client.PostAsync(apiUrl, content);
                    var res = response.Content.ReadAsStringAsync();
                    string splitResult = res.Result.ToString();
                    string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                    for (int i = 0; i < arrFactory.Length; i++)
                    {
                        if (!arrFactory[i].Equals(""))
                        {
                            temp.Line = arrFactory[i];

                            f.Add(new SewingLine()
                            {
                                Line = temp.Line
                            });
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    f.RemoveAt(0);

                    PkSewingline.ItemsSource = f;

                    PkSewingline.IsEnabled = true;
                });
            });
        }
    }
}