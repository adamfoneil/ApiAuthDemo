using ApiAuthDemo.Data.Entities;
using ApiClientBaseLibrary;
using System.Runtime.CompilerServices;

namespace ApiAuthDemo.Client;

public class ApiClient(HttpClient httpClient, ILogger<ApiClient> logger) : ApiClientBase(httpClient, logger)
{
    public async Task<IEnumerable<Widget>> GetMyWidgetsAsync() => await GetAsync<IEnumerable<Widget>>("MyWidgets") ?? [];

    protected override async Task<bool> HandleException(HttpResponseMessage? response, Exception exception, [CallerMemberName] string? methodName = null)
    {
        Logger.LogError(exception, "Error in {MethodName}", methodName);

        // assume error is handled
        return await Task.FromResult(true);
    }
}
