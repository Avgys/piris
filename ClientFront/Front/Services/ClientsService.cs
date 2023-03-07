using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Adapter.Extension;
using Lab1_piris.ClientModels;
using Lab1_piris.Controllers;
using Lab1_piris.Data.Models;
using Newtonsoft.Json;
using RestSharp;
using static Lab1_piris.Controllers.AtmController;

namespace Front.Services
{
    public class ClientsService
    {
        private readonly RequestService _service;
        private string clientsUrl = "Clients";
        private string accountsUrl = "Accounts";
        private string atmsUrl = "Atm";

        public ClientsService(RequestService service)
        {
            _service = service;
        }
        
        public async Task<List<ClientModel>> Get()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource(); 
            var json = await _service.Get(clientsUrl, cancelTokenSource.Token);
            
            return JsonConvert.DeserializeObject<List<ClientModel>>(json.Content);
        }

        internal async Task<RestResponse> CloseDay()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = accountsUrl + "/close-day";
            return await _service.Post(url, cancelTokenSource.Token);
        }

        internal async Task<RestResponse> Delete(ClientModel selectedClient)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = clientsUrl + '/' + selectedClient.Id + "/delete";
            return await _service.Post(url, cancelTokenSource.Token);
        }

        internal async Task<List<AccountModel>> GetAccounts(ClientModel client)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = accountsUrl + '/' + client.Id;
            var json = await _service.Get(url, cancelTokenSource.Token);

            return JsonConvert.DeserializeObject<List<AccountModel>>(json.Content);
        }

        internal async Task<string> GetBalance(int creditId)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = atmsUrl + '/' + creditId;
            var json = await _service.Get(url, cancelTokenSource.Token);

            return json.Content;
        }

        internal async Task<RestResponse> GetWithdraw(int selectedCreditId, decimal amount)
        {
            var model = new WithdrawModel() { Amount= amount };
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = atmsUrl + "/" + selectedCreditId;
            var body = JsonConvert.SerializeObject(model);
            return await _service.Post(url, body, cancelTokenSource.Token);
        }

        internal async Task<RestResponse> LoginATM(AtmController.AuthModel auth)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = atmsUrl + "/pin";
            var body = JsonConvert.SerializeObject(auth);
            return await _service.Post(url, body, cancelTokenSource.Token);
        }

        internal async Task<RestResponse> SaveCredit(ClientModel selectedClient, CreditModel credit)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = clientsUrl + '/' + selectedClient.Id + "/credit";
            var body = JsonConvert.SerializeObject(credit);
            return await _service.Post(url, body, cancelTokenSource.Token);
        }

        internal async Task<RestResponse> SaveDeposit(ClientModel selectedClient, DepositModel deposit)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = clientsUrl + '/' + selectedClient.Id + "/deposit";
            var body = JsonConvert.SerializeObject(deposit);
            return await _service.Post(url, body, cancelTokenSource.Token);
        }

        internal async Task<RestResponse> SaveOrUpdateAsync(ClientModel selectedClient)
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            var url = clientsUrl + '/' + selectedClient.Id;
            var body = JsonConvert.SerializeObject(selectedClient);
            return await _service.Put(url, body, cancelTokenSource.Token);
        }
    }
}
