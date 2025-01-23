using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Application.Common;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class RAGService : IRAGService
{
    private readonly HttpClient _httpClient;

    public RAGService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        var localIp = GetLocalIPAddress();
        _httpClient.BaseAddress = new Uri($"http://{localIp}:8081");
    }

    private string GetLocalIPAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.OperationalStatus == OperationalStatus.Up && 
                       n.NetworkInterfaceType != NetworkInterfaceType.Loopback);

        foreach (var network in networkInterfaces)
        {
            var properties = network.GetIPProperties();
            var addresses = properties.UnicastAddresses
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork);

            foreach (var address in addresses)
            {
                return address.Address.ToString();
            }
        }

        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public async Task<string> SearchAsync(string query, int limit = 10, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/search?query={Uri.EscapeDataString(query)}&limit={limit}", cancellationToken);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<SearchResponse>(cancellationToken: cancellationToken);
        if (result?.Status != "success" || result.Texts.Count == 0)
            return string.Empty;

        return string.Join("\n", result.Texts.Select(t => t.Text));
    }

    private class SearchResponse
    {
        public string Status { get; set; } = string.Empty;
        public List<TextResult> Texts { get; set; } = new();
    }

    private class TextResult
    {
        public string Text { get; set; } = string.Empty;
        public double Score { get; set; }
    }
}
