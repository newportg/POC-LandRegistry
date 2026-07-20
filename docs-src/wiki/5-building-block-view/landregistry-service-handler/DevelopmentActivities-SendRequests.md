# Develop a mechanism to send requests to Business Gateway for each web service that has been exposed

Once the service reference has been generated and the SOAP header mechanism has been implemented, you can proceed to develop the mechanism to send requests to Business Gateway for each web service that has been exposed.

![Request Abstract Factory AI](.attachments/ServiceHandler/RequestAbstractFactoryAI.png)

```c#
// Security Concerns
public interface ICertificateValidationService
{
    bool VerifyCertificate(byte[] certificateBytes);
    X509Certificate2? GetCertificateFromKeyVault(string key);
}

public class CertificateValidationService : ICertificateValidationService
{
    public bool VerifyCertificate(byte[] certificateBytes) { /* ... implementation ... */ return true; }
    public X509Certificate2? GetCertificateFromKeyVault(string key) { /* ... implementation ... */ return null; }
}

// Utility Concerns
public static class DateTimeHelper
{
    public static string CalculateExpirationTime(DateTime expiryDate)
    {
        /* ... implementation ... */
        return expiryDate.ToString("yyyy-MM-dd");
    }
}
```

```c#
// Use specific, clear DTOs instead of a generic LandRegistryDto
public abstract record SearchCriteria;
public record PropertyDescriptionCriteria(string Description) : SearchCriteria;
public record TitleKnownCriteria(string TitleNumber) : SearchCriteria;

public record LandTitleResponse(/* ... relevant properties ... */);

// The core interface for specific, executable service steps
public interface ILandRegistryServiceStep
{
    Task<LandTitleResponse> ExecuteAsync();
}
```
```c#
// Abstract Factory: Manages the creation process via DI Container
public abstract class ServiceFactory
{
    // The factory uses a service provider to resolve the final service with its dependencies
    protected readonly IServiceProvider _serviceProvider;

    protected ServiceFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    
    public abstract ILandRegistryServiceStep CreateService(SearchCriteria criteria);
}

// Concrete Factory for description searches
public class EnquiryByPropertyDescriptionFactory : ServiceFactory
{
    public EnquiryByPropertyDescriptionFactory(IServiceProvider sp) : base(sp) {}

    public override ILandRegistryServiceStep CreateService(SearchCriteria criteria)
    {
        // Pass specific criteria down
        return ActivatorUtilities.CreateInstance<EnquiryByPropertyDescription>(_serviceProvider, criteria);
    }
}

// Concrete Service Implementation
public class EnquiryByPropertyDescription : ILandRegistryServiceStep
{
    private readonly IMapper _mapper;
    private readonly ILogger<EnquiryByPropertyDescription> _logger;
    private readonly PropertyDescriptionCriteria _criteria;

    // Dependencies injected here by the ServiceProvider used in the factory
    public EnquiryByPropertyDescription(IMapper mapper, ILogger<EnquiryByPropertyDescription> logger, PropertyDescriptionCriteria criteria)
    {
        _mapper = mapper;
        _logger = logger;
        _criteria = criteria;
    }

    public Task<LandTitleResponse> ExecuteAsync()
    {
        _logger.LogInformation($"Executing request for {_criteria.Description}");
        // ... business logic, mapping, and external request ...
        return Task.FromResult(new LandTitleResponse());
    }
}

// ... Repeat the pattern for OfficialCopyTitleKnownFactory and OfficialCopyTitleKnown ...
```
```c#
public interface ILandRegistrySvc
{
    Task<LandTitleResponse> FindProperty(SearchCriteria criteria);
}

public class LandRegistrySvc : ILandRegistrySvc
{
    private readonly ICertificateValidationService _certSvc;
    // Assume a mechanism to find the correct factory (e.g., a dictionary of factories)

    public LandRegistrySvc(ICertificateValidationService certSvc /*, FactoryLookupService lookupSvc */)
    {
        _certSvc = certSvc;
    }

    public async Task<LandTitleResponse> FindProperty(SearchCriteria criteria)
    {
        // Example of using the extracted utility
        var certIsValid = _certSvc.VerifyCertificate(/*...*/);
        var expiration = DateTimeHelper.CalculateExpirationTime(DateTime.Now.AddDays(1));
        
        // Logic to select the correct factory based on the type of 'criteria'
        ServiceFactory factory = SelectFactoryBasedOnCriteria(criteria);

        // Use the factory to create the specific operation handler
        ILandRegistryServiceStep serviceStep = factory.CreateService(criteria);

        // Execute the operation
        return await serviceStep.ExecuteAsync();
    }

    private ServiceFactory SelectFactoryBasedOnCriteria(SearchCriteria criteria)
    {
        // This logic would look at the concrete type of 'criteria' 
        // and return the appropriate concrete factory instance (resolved via DI).
        throw new NotImplementedException(); 
    }
}
```
