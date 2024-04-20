using ApiAuthDemo.Data.Entities;
using ApiClientBaseLibrary;
using System.Runtime.CompilerServices;

namespace ApiAuthDemo.Client;

public class ApiClient(IHttpClientFactory factory, ILogger<ApiClient> logger) : ApiClientBase(factory.CreateClient(Name), logger)
{
    public const string Name = "API";

    public async Task<IEnumerable<Widget>> GetMyWidgetsAsync() => await GetAsync<IEnumerable<Widget>>("api/Queries/MyWidgets") ?? [];

    protected override async Task<bool> HandleException(HttpResponseMessage? response, Exception exception, [CallerMemberName] string? methodName = null)
    {
        Logger.LogError(exception, "Error in {MethodName}", methodName);

        // assume error is handled
        return await Task.FromResult(true);
    }

    public async Task DeleteWidgetAsync(int id) => await DeleteAsync($"api/Widgets/{id}");
    
    public async Task<Widget> SaveWidgetAsync(Widget data) => await PostWithInputAndResultAsync("api/Widgets", data) ?? throw new Exception("widget not saved");
    
}
