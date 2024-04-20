namespace ApiAuthDemo.Services;

/// <summary>
/// from Dave's project originally
/// https://github.com/ripteqdavid/sample-blazor8-auth-ms/blob/master/BlazorAppMSAuth/BlazorAppMSAuth/Services/BaseUrlProvider.cs
/// </summary>
public class BaseUrlProvider
{
	public string BaseUrl { get; private set; } = default!;

	public void Set(string baseUrl)
	{
		BaseUrl = baseUrl;
	}
}
