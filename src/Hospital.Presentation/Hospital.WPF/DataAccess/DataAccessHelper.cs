using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net;

namespace Hospital.WPF.DataAccess;

public static class DataAccessHelper
{
    public static async Task<T> GetCall<T>(string uri, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.Timeout = TimeSpan.FromSeconds(900);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(uri);
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch
                {
                    return null;
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public static async Task PutCall<T>(string uri, T data) where T : class
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.Timeout = TimeSpan.FromSeconds(900);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PutAsJsonAsync(uri, data);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public static async Task<TResult> PostCall<TRequest, TResult>(string uri, TRequest data) where TResult : class
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.Timeout = TimeSpan.FromSeconds(900);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(uri, data);
                var content = await response.Content.ReadAsStringAsync();
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
        catch (Exception ex)
        {
            throw;
        }
    }
}
