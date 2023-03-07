using Adapter.Extension;
using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

namespace Front.Services;

public class Lookups
{
    private readonly RequestService _service;
    private string apiUrl = "Lookups";

    public Lookups(RequestService service)
    {
        _service = service;
    }
        
    public async Task<LookupsModel> Get()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource(); 
        var json = await _service.Get(apiUrl, cancelTokenSource.Token);
            
        return JsonConvert.DeserializeObject<LookupsModel>(json.Content);
    }

    internal async Task<DepositLookupsModel> GetDeposit()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        var url = apiUrl + "/deposit";
        var json = await _service.Get(url, cancelTokenSource.Token);

        return JsonConvert.DeserializeObject<DepositLookupsModel>(json.Content);
    }

    internal async Task<CreditLookupsModel> GetCredit()
    {
        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        var url = apiUrl + "/credit";
        var json = await _service.Get(url, cancelTokenSource.Token);

        return JsonConvert.DeserializeObject<CreditLookupsModel>(json.Content);
    }
}