using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AdvancedAADB2C.Xamarin
{
    public partial class MainPage : ContentPage
    {
        private readonly Identity _identity;

        public MainPage(AuthenticationResult result)
        {
            InitializeComponent();
            _identity = AuthHelper.GetIdentityFromAuthenticationResult(result);
        }

        protected override void OnAppearing()
        {
            if (_identity != null)
            {
                labelId.Text = _identity.Id;
                labelName.Text = _identity.Name;
                labelEmail.Text = _identity.Email;
            }

            base.OnAppearing();
        }

        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            IEnumerable<IAccount> accounts = await App.AuthenticationClient.GetAccountsAsync();

            while (accounts.Any())
            {
                await App.AuthenticationClient.RemoveAsync(accounts.First());
                accounts = await App.AuthenticationClient.GetAccountsAsync();
            }

            await Navigation.PopAsync();
        }
    }
}
