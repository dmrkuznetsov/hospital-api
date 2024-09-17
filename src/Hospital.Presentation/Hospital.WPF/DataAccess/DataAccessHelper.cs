using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Hospital.WPF.DataAccess;

public static class DataAccessHelper
{
    public static async Task<T> GetCall<T>(HttpClient client, string uri, CancellationToken cancellationToken = default) where T : class
    {
        var response = await client.GetAsync(uri, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch
        {
            return null;
        }
    }
    public static async Task PutCall<T>(HttpClient client, string uri, T data, CancellationToken cancellationToken = default) where T : class
    {
        await client.PutAsJsonAsync(uri, data, cancellationToken);
    }
    public static async Task<TResult> PostCall<TRequest, TResult>(HttpClient client, string uri, TRequest data, CancellationToken cancellationToken = default) where TResult : class
    {
        var response = await client.PostAsJsonAsync(uri, data, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        try
        {
            return JsonConvert.DeserializeObject<TResult>(content);
        }
        catch
        {
            return null;
        }
    }
}
