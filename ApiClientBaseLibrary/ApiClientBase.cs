using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;

namespace ApiClientBaseLibrary;

/// <summary>
/// since Refit doesn't seem to work with authentication in Blazor WebAssembly, this is kind of the next best thing
/// </summary>
public abstract class ApiClientBase(HttpClient httpClient, ILogger<ApiClientBase> logger)
{
	protected readonly ILogger<ApiClientBase> Logger = logger;

	protected HttpClient Client { get; } = httpClient;

	protected abstract Task<bool> HandleException(HttpResponseMessage? response, Exception exception, [CallerMemberName] string? methodName = null);

	protected async Task<T?> GetAsync<T>(string uri)
	{
		var response = await Client.GetAsync(uri);

		try
		{
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<T>();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, "Error in {Method}", nameof(GetAsync));
			if (!await HandleException(response, exc))
			{
				throw;
			}
			return default;
		}
	}

	protected async Task<TResult?> PostWithResultAsync<TResult>(string uri)
	{
		var response = await Client.PostAsync(uri, null);

		try
		{
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<TResult>();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, "Error in {Method}", nameof(PostWithResultAsync));
			if (!await HandleException(response, exc))
			{
				throw;
			}
		}

		return default;
	}

	protected async Task PostWithInputAsync<T>(string uri, T value)
	{
		var response = await Client.PostAsJsonAsync(uri, value);

		try
		{
			response.EnsureSuccessStatusCode();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, "Error in {Method}", nameof(PostWithInputAsync));
			if (!await HandleException(response, exc))
			{
				throw;
			}
		}
	}

	protected async Task<TResult?> PostWithInputAndResultAsync<TResult>(string uri, TResult input)
	{
		var response = await Client.PostAsJsonAsync(uri, input);

		try
		{
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<TResult>();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, "Error in {Method}", nameof(PostWithInputAndResultAsync));
			if (!await HandleException(response, exc))
			{
				throw;
			}
		}

		return default;
	}

	protected async Task DeleteAsync(string uri)
	{
		var response = await Client.DeleteAsync(uri);

		try
		{
			response.EnsureSuccessStatusCode();
		}
		catch (Exception exc)
		{
			Logger.LogError(exc, "Error in {Method}", nameof(DeleteAsync));
			if (!await HandleException(response, exc))
			{
				throw;
			}
		}
	}
}
