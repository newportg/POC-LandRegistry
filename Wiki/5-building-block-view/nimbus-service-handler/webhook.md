
# Webhook implementation

Here is an example of an ASP.NET Core Controller that acts as a webhook receiver. It validates the request, maps the incoming JSON payload to a model, and then downloads the documents using HttpClient. 

# 1. Webhook Payload Models
Map the expected JSON structure from the Nimbus API to C# classes.

```csharp
public class NimbusWebhookPayload
{
    public string OrderId { get; set; }
    public List<NimbusDocument> Documents { get; set; }
}

public class NimbusDocument
{
    public string DocumentType { get; set; }
    public string DownloadUrl { get; set; }
    public string FileName { get; set; }
}
```

# 2. Webhook Controller
Create a controller to handle the POST request sent by the Nimbus service.

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

[ApiController]
[Route("api/webhooks/nimbus")]
public class NimbusWebhookController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NimbusWebhookController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("delivery")]
    public async Task<IActionResult> ReceiveDocument([FromBody] NimbusWebhookPayload payload)
    {
        if (payload?.Documents == null || !payload.Documents.Any())
            return BadRequest("No documents provided in payload.");

        // Immediately return 200 OK to the sender, then process the download
        // In a production app, use a BackgroundTask or Message Queue here
        foreach (var doc in payload.Documents)
        {
            await DownloadDocumentAsync(doc.DownloadUrl, doc.FileName);
        }

        return Ok();
    }

    private async Task DownloadDocumentAsync(string url, string fileName)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsByteArrayAsync();
            var filePath = Path.Combine("Downloads", fileName);
            await System.IO.File.WriteAllBytesAsync(filePath, content);
        }
    }
}
```

# CARE Execution Notes:
* Response Timing: Webhooks expect an immediate 200 OK response. For large downloads, offload the DownloadDocumentAsync task to a Background Service or a queue like RabbitMQ to avoid timing out the Nimbus server's request.
* Security: To protect your endpoint, implement a secret key check in the headers (e.g., X-Nimbus-Secret) to ensure only Nimbus can trigger downloads on your server.
* Storage: The example saves files locally, but for production, consider streaming directly to Azure Blob Storage.

# HMAC signature validation

To implement the HMAC Signature version, you create a hash of the raw request body using your secret key and compare it to the signature sent in the request header. This ensures the data hasn't been tampered with in transit. 

# 1. The HMAC Validation Filter
This filter reads the raw body, computes a SHA256 HMAC, and uses CryptographicOperations.FixedTimeEquals to prevent timing attacks. 

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;

public class NimbusHmacAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _secret;
    private const string HeaderName = "X-Nimbus-Signature";

    public NimbusHmacAttribute(string secret) => _secret = secret;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var request = context.HttpContext.Request;

        // 1. Extract signature from header
        if (!request.Headers.TryGetValue(HeaderName, out var receivedSignature))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // 2. Read the raw request body
        request.EnableBuffering(); // Allows reading the body multiple times
        using var reader = new StreamReader(request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0; // Reset for the controller

        // 3. Compute expected HMAC
        var keyBytes = Encoding.UTF8.GetBytes(_secret);
        var bodyBytes = Encoding.UTF8.GetBytes(body);
        using var hmac = new HMACSHA256(keyBytes);
        var computedHash = hmac.ComputeHash(bodyBytes);
        var computedSignature = Convert.ToHexString(computedHash);

        // 4. Secure comparison
        if (!CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(computedSignature), 
            Encoding.UTF8.GetBytes(receivedSignature.ToString())))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
```

# 2. Applying the Filter
Use the attribute on your webhook endpoint. The secret should match the one configured in your Nimbus dashboard.

```csharp
[ApiController]
[Route("api/webhooks")]
public class WebhookController : ControllerBase
{
    [HttpPost("nimbus")]
    [NimbusHmac("YOUR_SHARED_SECRET")] // Authenticates via HMAC
    public async Task<IActionResult> HandleWebhook([FromBody] NimbusWebhookPayload payload)
    {
        // Your logic to download files using 'payload.Documents'
        return Ok();
    }
}
```

# CARE Framework Implementation Details:
* Request Buffering: You must call request.EnableBuffering() in the filter so that the controller can still read the JSON body after the filter has finished reading it for the hash calculation.
* Hex vs. Base64: Your API spec or documentation will dictate if the signature is Hexadecimal (common for GitHub/Stripe) or Base64. Adjust Convert.ToHexString accordingly.
* Performance: For high-volume webhooks, consider using a Middleware approach instead of an Action Filter to catch unauthorized requests even earlier in the pipeline.










