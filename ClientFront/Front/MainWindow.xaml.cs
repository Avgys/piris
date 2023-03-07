using Adapter;
using Adapter.Extension;
using Front.Services;
using Lab1_piris.ClientModels;
using Lab1_piris.Data.Models;
using Lab1piris.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Lab1_piris.Controllers.AtmController;

namespace Front
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<AccountModel> _accounts;

        private RequestService requestService { get; set; }
        private ClientsService clientsService { get; set; }

        private List<UIElement> Tabs { get; set; }
        private List<UIElement> ATMTabs { get; set; }
        private List<UIElement> ClientSubTabs { get; set; }
        private List<UIElement> DepositMode { get; set; }
        private List<UIElement> CreditMode { get; set; }

        private List<ClientModel> Clients { get; set; }

        private LookupsModel _lookupsModel { get; set; }
        private DepositLookupsModel _depositLookupsModel { get; set; }
        private CreditLookupsModel _creditLookupsModel { get; set; }

        private ClientModel SelectedClient { get; set; }
        public int SelectedCreditId { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            AppSettingsProvider.LoadConfiguration();
            requestService = new();

            clientsService = new(requestService);

            Tabs = new();
            Tabs.Add(ClientsTab);
            Tabs.Add(ClientChangeTab);
            Tabs.Add(ATMTab);

            ClientSubTabs = new();
            ClientSubTabs.Add(InfoTab);
            ClientSubTabs.Add(AccountTab);
            ClientSubTabs.Add(DepositCreditTab);

            DepositMode = new();
            DepositMode.Add(DepositType);
            DepositMode.Add(DepositSave);
            DepositMode.Add(DepositCreditTab);

            CreditMode = new();
            CreditMode.Add(CreditType);
            CreditMode.Add(DepositCreditTab);
            CreditMode.Add(CreditSave);
            CreditMode.Add(CreditPercent);

            ATMTabs = new();
            ATMTabs.Add(Login);
            ATMTabs.Add(Menu);


            SettingsComboboxes();

            IsMale.Checked += (object sender, RoutedEventArgs e) => IsFemale.IsChecked = false;
            IsFemale.Checked += (object sender, RoutedEventArgs e) => IsMale.IsChecked = false;

            var lookups = new Lookups(requestService);

            lookups.Get().ContinueWith(x =>
            {
                _lookupsModel = x.Result;
                GetComboBoxes(_lookupsModel);
            });

            lookups.GetDeposit().ContinueWith(x =>
            {
                _depositLookupsModel = x.Result;
                foreach (var item in _depositLookupsModel.DepositTypes)
                {
                    this.Dispatcher.Invoke(() => DepositType.Items.Add(item));
                }

                foreach (var item in _depositLookupsModel.Currencies)
                {
                    this.Dispatcher.Invoke(() => Currencies.Items.Add(item));
                }
            });

            lookups.GetCredit().ContinueWith(x =>
            {
                _creditLookupsModel = x.Result;
                foreach (var item in _creditLookupsModel.CreditTypes)
                {
                    this.Dispatcher.Invoke(() => CreditType.Items.Add(item));
                }
            });

            ChangeToClientsTab();
        }

        private void SettingsComboboxes()
        {
            LivingCity.SelectedValuePath = "Id";
            LivingCity.DisplayMemberPath = "Name";

            RegistrationCity.SelectedValuePath = "Id";
            RegistrationCity.DisplayMemberPath = "Name";

            Disability.SelectedValuePath = "Id";
            Disability.DisplayMemberPath = "Name";

            FamilyState.SelectedValuePath = "Id";
            FamilyState.DisplayMemberPath = "Name";

            Citizenship.SelectedValuePath = "Id";
            Citizenship.DisplayMemberPath = "Name";

            DepositType.SelectedValuePath = "Id";
            DepositType.DisplayMemberPath = "Name";

            CreditType.SelectedValuePath = "Id";
            CreditType.DisplayMemberPath = "Name";

            Currencies.SelectedValuePath = "Id";
            Currencies.DisplayMemberPath = "Name";

            //Interval.SelectedValuePath = "Id";
            //Interval.DisplayMemberPath = "Name";

            for (int i = 1; i < 13; i++)
            {
                Interval.Items.Add(i.ToString());
            }
        }

        private void GetComboBoxes(LookupsModel _lookupsModel)
        {
            foreach (var item in _lookupsModel.Cities)
            {
                this.Dispatcher.Invoke(() => LivingCity.Items.Add(item));
            }

            foreach (var item in _lookupsModel.Cities)
            {
                this.Dispatcher.Invoke(() => RegistrationCity.Items.Add(item));
            }


            foreach (var item in _lookupsModel.Disabilities)
            {
                this.Dispatcher.Invoke(() => Disability.Items.Add(item));
            }


            foreach (var item in _lookupsModel.FamilyStates)
            {
                this.Dispatcher.Invoke(() => FamilyState.Items.Add(item));
            }


            foreach (var item in _lookupsModel.Citizenships)
            {
                this.Dispatcher.Invoke(() => Citizenship.Items.Add(item));
            }
        }

        private async void ChangeToClientsTab()
        {
            HideAllTabs(Tabs);
            HideAllTabs(ClientSubTabs);
            ClientsList.Items.Clear();
            Clients = await clientsService.Get();

            foreach (var item in Clients)
                ClientsList.Items.Add(item.LastName + ' ' + item.FirstName + ' ' + item.MiddleName);

            ClientsTab.Visibility = Visibility.Visible;
            InfoTab.Visibility = Visibility.Visible;
        }

        private void HideAllTabs(List<UIElement> tabs)
        {
            foreach (var tab in tabs)
            {
                tab.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowAllTabs(List<UIElement> tabs)
        {
            foreach (var tab in tabs)
            {
                tab.Visibility = Visibility.Visible;
            }
        }


        private void ClientsList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var index = ClientsList.SelectedIndex;
            if (index != -1)
            {
                SelectedClient = Clients[index];
                ChangeToClientTab(SelectedClient);
            }
        }

        private void ChangeToClientTab(ClientModel client)
        {
            HideAllTabs(Tabs);
            HideAllTabs(ClientSubTabs);

            InfoRadioButton.IsChecked = true;

            if (client != null)
            {
                FirstName.Text = client.FirstName;
                LastName.Text = client.LastName;
                MiddleName.Text = client.MiddleName;
                BirthDate.SelectedDate = client.BirthDate;
                IsMale.IsChecked = client.IsMale;
                IsFemale.IsChecked = !client.IsMale;
                PassportSeries.Text = client.PassportSeries;
                PassportNumber.Text = client.PassportNumber;
                PassportIssuedBy.Text = client.PassportIssuedBy;
                PassportIssuedAt.SelectedDate = client.PassportIssuedAt;
                PassportId.Text = client.PassportId;
                BirthPlace.Text = client.BirthPlace;
                LivingCity.SelectedValue = client.LivingCity?.Id ?? _lookupsModel.Cities.First().Id;
                LivingAddress.Text = client.LivingAddress;
                HomePhone.Text = client.HomePhone;
                MobilePhone.Text = client.MobilePhone;
                Email.Text = client.Email;
                RegistrationCity.SelectedValue = client.RegistrationCity?.Id ?? _lookupsModel.Cities.First().Id;
                FamilyState.SelectedValue = client.FamilyState?.Id ?? _lookupsModel.FamilyStates.First().Id;
                Citizenship.SelectedValue = client.Citizenship?.Id ?? _lookupsModel.Citizenships.First().Id;
                Disability.SelectedValue = client.Disability?.Id ?? _lookupsModel.Disabilities.First().Id;
                Pensioner.IsChecked = client.Pensioner;
                PlaceOfWork.Text = client.PlaceOfWork;
                WorkingPosition.Text = client.WorkingPosition;
                MonthIncome.Text = client.MonthIncome.HasValue ? client.MonthIncome.Value.ToString("N2") : "0";
            }
            else
            {
                SelectedClient = new();
                client = SelectedClient;

                //FirstName.Text = client.FirstName;
                //LastName.Text = client.LastName;
                //MiddleName.Text = client.MiddleName;
                //BirthDate.SelectedDate = client.BirthDate;
                //IsMale.IsChecked = client.IsMale;
                //IsFemale.IsChecked = !client.IsMale;
                //PassportSeries.Text = client.PassportSeries;
                //PassportNumber.Text = client.PassportNumber;
                //PassportIssuedBy.Text = client.PassportIssuedBy;
                //PassportIssuedAt.SelectedDate = client.PassportIssuedAt;
                //PassportId.Text = client.PassportId;
                //BirthPlace.Text = client.BirthPlace;
                //LivingCity.SelectedValue = client.LivingCity.Id;
                //LivingAddress.Text = client.LivingAddress;
                //HomePhone.Text = client.HomePhone;
                //MobilePhone.Text = client.MobilePhone;
                //Email.Text = client.Email;
                //RegistrationCity.SelectedValue = client.RegistrationCity.Id;
                //FamilyState.SelectedValue = client.FamilyState.Id;
                //Citizenship.SelectedValue = client.Citizenship.Id;
                //Disability.SelectedValue = client.Disability.Id;
                //Pensioner.IsChecked = client.Pensioner;
                //PlaceOfWork.Text = client.PlaceOfWork;
                //WorkingPosition.Text = client.WorkingPosition;
                //MonthIncome.Text = client.MonthIncome.HasValue ? client.MonthIncome.Value.ToString("N2") : "0";
            }

            ClientChangeTab.Visibility = Visibility.Visible;
            InfoTab.Visibility = Visibility.Visible;
        }

        private async void ChangeToAccountSubTab(ClientModel client)
        {
            HideAllTabs(ClientSubTabs);

            _accounts = await clientsService.GetAccounts(client);
            Accounts.ItemsSource = _accounts;

            AccountTab.Visibility = Visibility.Visible;
        }

        private void AddClient_Click(object sender, RoutedEventArgs e)
        {
            SelectedClient = new();
            SelectedClient.BirthDate = DateTime.Now;
            SelectedClient.PassportIssuedAt = DateTime.Now;

            ChangeToClientTab(SelectedClient);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveOrUpdateClient(SelectedClient);
        }

        private async void SaveOrUpdateClient(ClientModel selectedClient)
        {
            try
            {
                selectedClient.FirstName = FirstName.Text;
                selectedClient.LastName = LastName.Text;
                selectedClient.MiddleName = MiddleName.Text;
                selectedClient.BirthDate = BirthDate.SelectedDate ?? DateTime.UtcNow;
                selectedClient.IsMale = IsMale.IsChecked ?? true;
                selectedClient.PassportSeries = PassportSeries.Text;
                selectedClient.PassportNumber = PassportNumber.Text;
                selectedClient.PassportIssuedBy = PassportIssuedBy.Text;
                selectedClient.PassportId = PassportId.Text;
                selectedClient.BirthPlace = BirthPlace.Text;
                selectedClient.LivingCity = (LivingCity.SelectedItem ?? _lookupsModel.Cities.First())! as SelectedItemModel;
                selectedClient.LivingAddress = LivingAddress.Text;
                selectedClient.HomePhone = HomePhone.Text;
                selectedClient.MobilePhone = MobilePhone.Text;
                selectedClient.Email = Email.Text;
                selectedClient.RegistrationCity = (RegistrationCity.SelectedItem ?? _lookupsModel.Cities.First())! as SelectedItemModel;
                selectedClient.RegistrationCity = (RegistrationCity.SelectedItem ?? _lookupsModel.Cities.First())! as SelectedItemModel;
                selectedClient.FamilyState = (FamilyState.SelectedItem ?? _lookupsModel.FamilyStates.First())! as SelectedItemModel;
                selectedClient.Citizenship = (Citizenship.SelectedItem ?? _lookupsModel.Citizenships.First())! as SelectedItemModel;
                selectedClient.Disability = (Disability.SelectedItem ?? _lookupsModel.Disabilities.First())! as SelectedItemModel;
                selectedClient.Pensioner = Pensioner.IsChecked ?? true;
                selectedClient.PlaceOfWork = PlaceOfWork.Text;
                selectedClient.WorkingPosition = WorkingPosition.Text;
                //if(decimal.TryParse(MonthIncome))
                selectedClient.MonthIncome = decimal.Parse(MonthIncome.Text);

                var result = await clientsService.SaveOrUpdateAsync(selectedClient);
                MessageBox.Show(result.StatusCode.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeToClientsTab();
        }

        private void AccountRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeToAccountSubTab(SelectedClient);
        }

        private void InfoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ChangeToClientTab(SelectedClient);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var response = await clientsService.Delete(SelectedClient);
            ChangeToClientsTab();

            MessageBox.Show(response.StatusCode.ToString());
        }

        private async void CloseDay_Click(object sender, RoutedEventArgs e)
        {
            var response = await clientsService.CloseDay();
            MessageBox.Show(response.StatusCode.ToString());
        }

        private void Credit_Checked(object sender, RoutedEventArgs e)
        {
            ChangeToCreditTab(SelectedClient);
        }

        private void Deposit_Checked(object sender, RoutedEventArgs e)
        {
            ChangeToDepositTab(SelectedClient);
        }

        private void ChangeToDepositTab(ClientModel selectedClient)
        {
            HideAllTabs(ClientSubTabs);
            HideAllTabs(CreditMode);
            ShowAllTabs(DepositMode);
        }

        private void ChangeToCreditTab(ClientModel selectedClient)
        {
            HideAllTabs(ClientSubTabs);
            HideAllTabs(DepositMode);
            ShowAllTabs(CreditMode);
        }


        private async void DepositSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var deposit = new DepositModel();
                deposit.Amount = decimal.Parse(Amout.Text);
                deposit.CurrencyType = Currencies.SelectedItem as SelectedItemModel;
                deposit.ContractNumber = ContractNumber.Text;
                deposit.StartDate = DateTime.Now;
                deposit.EndDate = deposit.StartDate.AddMonths(Interval.SelectedIndex + 1);
                deposit.DepositType = DepositType.SelectedItem as SelectedItemModel;

                var response = await clientsService.SaveDeposit(SelectedClient, deposit);
                MessageBox.Show(response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void CreditSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var credit = new CreditModel();
                credit.Amount = decimal.Parse(Amout.Text);
                credit.CurrencyType = Currencies.SelectedItem as SelectedItemModel;
                credit.ContractNumber = ContractNumber.Text;
                credit.StartDate = DateTime.Now;
                credit.EndDate = credit.StartDate.AddMonths(Interval.SelectedIndex + 1);
                credit.CreditType = CreditType.SelectedItem as SelectedItemModel;

                var response = await clientsService.SaveCredit(SelectedClient, credit);
                MessageBox.Show(response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ATMTab.Visibility = ATMTab.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (ATMTab.Visibility == Visibility.Visible) 
            {
                HideAllTabs(ATMTabs);
                Login.Visibility = Visibility.Visible;
            }

            AccountManagmentTab.Visibility = AccountManagmentTab.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            if (AccountManagmentTab.Visibility == Visibility.Visible)
            {
                HideAllTabs(Tabs);
                ClientsTab.Visibility = Visibility.Visible;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var auth = new AuthModel()
            {
                Pin = "1111",
                AccountNumber = AccountNumber.Text,
            };

            var response = await clientsService.LoginATM(auth);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var CreditId = int.Parse(response.Content);
                ChangeToMenuTab(CreditId);
            }
        }

        private void ChangeToLoginTab()
        {
            HideAllTabs(ATMTabs);
            Login.Visibility = Visibility.Visible;
        }

        private async void ChangeToMenuTab(int creditId)
        {
            HideAllTabs(ATMTabs);
            Menu.Visibility = Visibility.Visible;
            SelectedCreditId = creditId;
            var balance = decimal.Parse(await clientsService.GetBalance(creditId));
            Balance.Text = "Баланс: " + balance.ToString();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var amount = decimal.Parse(AmountOfWithdraw.Text);
                var response = await clientsService.GetWithdraw(SelectedCreditId, amount);
                MessageBox.Show(response.StatusCode.ToString());
                ChangeToMenuTab(SelectedCreditId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SelectedCreditId = 0;

            ChangeToLoginTab();
        }

        private void UpdateDates(object sender, TextChangedEventArgs e)
        {           
        }

        private void UpdateDates(object sender, SelectionChangedEventArgs e)
        {
            UpdateDates();
        }

        void UpdateDates()
        {
            StartDate.Content = "Дата начала: " + DateTime.Now.ToString("g"); 
            EndDate.Content = "Дата окончания: " + DateTime.Now.AddMonths(Interval.SelectedIndex + 1).ToString("g"); 
        }

        private void CreditType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var credit = CreditType.SelectedItem as CreditTypeModel;
            CreditPercent.Content = "Процент кредитования:\r\n" + $"{credit.Percent}%," + (credit.IsDifferentiated ? "Дифферинцированный" : "");
        }
    }
}