using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;

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
                return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
