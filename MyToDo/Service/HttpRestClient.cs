using MyToDo.Service;
using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;
using System;

public class HttpRestClient
{
    private readonly string webUrl;
    protected readonly RestClient client;

    public HttpRestClient(string webUrl)
    {
        this.webUrl = webUrl;
        this.client = new RestClient(new Uri(webUrl));
    }

    public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
    {
        var request = new RestRequest(baseRequest.Route, baseRequest.Method);
        request.AddHeader("Content-Type", baseRequest.ContentType);
        if (baseRequest.Parameter != null)
        {
            request.AddParameter("param", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
        }

        var response = await client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
    }

    public async Task<ApiResponse<T>> ExecuteAsync<T>(BaseRequest baseRequest)
    {
        var request = new RestRequest(baseRequest.Route, baseRequest.Method);
        request.AddHeader("Content-Type", baseRequest.ContentType);
        
        
        if (baseRequest.Parameter != null)
        {
            request.AddJsonBody(baseRequest.Parameter);
        }

        var response = await client.ExecuteAsync(request);
        return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
    }
}


