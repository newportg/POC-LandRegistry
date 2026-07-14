
# OAuth 2.0 Authentication flow

Implementing an OAuth 2.0 flow in C# typically involves two stages: obtaining the token and using it in subsequent API requests. Based on your specification, you need the Authorization Code flow for user-context actions and Refresh Tokens for long-term access. 

# 1. Token Request (Authorization Code Exchange)

Once you have an authorization_code from the user's login, exchange it for an access_token and refresh_token. 
```csharp
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

public async Task<string> GetAccessToken(string code)
{
    using var client = new HttpClient();
    var values = new Dictionary<string, string>
    {
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", "https://your-app.com" },
        { "client_id", "YOUR_CLIENT_ID" },
        { "client_secret", "YOUR_CLIENT_SECRET" }
    };

    var content = new FormUrlEncodedContent(values);
    var response = await client.PostAsync("https://api.nimbusmaps.co.uk", content);
    
    // Parse the JSON response to extract 'access_token' and 'refresh_token'
    var responseString = await response.Content.ReadAsStringAsync();
    return responseString; 
}
```

# 2. Using the Token (Authenticated Request)
Use the HttpClient class with an Authorization header to call the /check-availability endpoint. 

```csharp
using System.Net.Http.Headers;

public async Task CheckAvailability(string accessToken, string titleNumber)
{
    using var client = new HttpClient();
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

    var requestBody = new { title_number = titleNumber };
    var response = await client.PostAsJsonAsync("https://api.nimbusmaps.co.uk", requestBody);

    if (response.IsSuccessStatusCode)
    {
        var result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);
    }
}
```

# 3. Refreshing the Token
Since tokens expire after 1 hour, use the refresh_token to stay authenticated without re-prompting the user. 

```csharp
var refreshValues = new Dictionary<string, string>
{
    { "grant_type", "refresh_token" },
    { "refresh_token", "YOUR_STORED_REFRESH_TOKEN" },
    { "client_id", "YOUR_CLIENT_ID" },
    { "client_secret", "YOUR_CLIENT_SECRET" }
};
```

# Recommended Libraries:
* IdentityModel: Simplifies token requests and endpoint discovery.
* Microsoft.Identity.Client (MSAL): Best for enterprise-grade authentication. 
