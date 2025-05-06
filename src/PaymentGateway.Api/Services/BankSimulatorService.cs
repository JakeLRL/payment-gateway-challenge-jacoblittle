using System.Text.Json;

using PaymentGateway.Api.Models.Requests.External;
using PaymentGateway.Api.Models.Responses.External;

namespace PaymentGateway.Api.Services;
public class BankSimulatorService(IHttpClientFactory httpClientFactory) : IBankSimulatorService
{
    public async Task<PostAuthorisePaymentResponse?> PostAuthorisePayment(PostAuthorisePaymentRequest request)
    {
        var httpClient = httpClientFactory.CreateClient("BankSimulatorClient");
        var url = "payments";

        var response = await httpClient.PostAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<PostAuthorisePaymentResponse>(content);
        }

        return null;
    }
}

