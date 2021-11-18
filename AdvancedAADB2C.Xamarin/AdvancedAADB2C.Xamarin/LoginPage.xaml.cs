using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvancedAADB2C.Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            try
            {
                IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();

                AuthenticationResult result = await App.AuthenticationClient
                    .AcquireTokenSilent(Constants.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                await Task.Delay(300); // Dirty hack to ensure page is fully appeared before navigating away to prevent IndexOutOfRange exception when navigating back to this page as mentionend here: https://stackoverflow.com/questions/56792068/popasync-causes-argumentoutofrangeexception
                await Navigation.PushAsync(new MainPage(result));
            }
            catch 
            {
                LoginForm.IsVisible = true;
                RegistrationForm.IsVisible = true;
                ProgressIndicator.IsVisible = false;
            }
            
            base.OnAppearing();
        }

        private async void ButtonSignup_Clicked(object sender, EventArgs e)
        {
            var requestBody = new StringContent(JsonConvert.SerializeObject(new InviteRequest()
            {
                Email = EntryEmail.Text,
                Name = EntryName.Text
            }), Encoding.UTF8, "application/json");

            RegistrationForm.IsVisible = false;
            ProgressIndicator.IsVisible = true;
            ProgressLabel.Text = "Registrierung...";
            LoginForm.IsVisible = false;

            var client = new HttpClient();
            var response = await client.PostAsync(Constants.InviteEndpoint, requestBody);

            ProgressIndicator.IsVisible = false;
            RegistrationResult.IsVisible = true;
        }

        private async void ButtonSignin_Clicked(object sender, EventArgs e)
        {
            AuthenticationResult result;
            try
            {
                result = await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .ExecuteAsync();

                await Navigation.PushAsync(new MainPage(result));
            }
            catch (MsalException ex)
            {
                if (ex.Message != null && ex.Message.Contains("AADB2C90118"))
                {
                    result = await ForgotPasswordAsync();
                    if (result != null)
                    {
                        await Navigation.PushAsync(new MainPage(result));
                    }
                }
                else if (ex.ErrorCode != "authentication_canceled")
                {
                    await DisplayAlert("An error has occurred", "Exception message: " + ex.Message, "Dismiss");
                }
            }
        }

        private async Task<AuthenticationResult> ForgotPasswordAsync()
        {
            try
            {
                return await App.AuthenticationClient
                    .AcquireTokenInteractive(Constants.Scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .WithParentActivityOrWindow(App.UIParent)
                    .WithB2CAuthority(Constants.AuthorityPasswordReset)
                    .ExecuteAsync();
            }
            catch (MsalException)
            {
                return null;
            }

        }
    }
}