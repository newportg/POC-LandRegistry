# For each request sent, the SOAP header must contain the user ID, password and locale information

To create and populate a SOAP header in C#, you will need to define a custom class that inherits from System.Web.Services.Protocols.SoapHeader (for older ASMX services) or use System.ServiceModel.Channels.MessageHeader with WCF. The latter approach, using message inspectors and behaviors, is recommended for WCF applications. 

1. Define the Custom Header Class 
First, define a class to hold your data (UserID, Password, Locale). You must ensure the class and its members have the correct XML serialization attributes, including the correct Namespace, so the SOAP message is formatted as expected by the service. 
csharp
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// Use the namespace the web service expects for the header

```c#
namespace YourServiceName.Namespace 
{
    // The XmlRoot attribute defines the name and namespace of the header element in the XML
    [XmlRoot(Namespace = "yourwebservice.com")]
    public class CustomSoapHeader : SoapHeader
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Locale { get; set; }
    }
}
```

2. Modify the Web Service Proxy Class 
When you add a "Web Reference" in Visual Studio, a proxy class is generated. You need to create a partial class in a separate file within the same namespace and assembly to add a property that exposes the custom header. 
csharp

```c#
namespace YourServiceName.Namespace
{
    // This partial class extends the auto-generated proxy class
    public partial class MyWebService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        public CustomSoapHeader RequestHeader; // This variable will hold your header data
    }
}
```

3. Add the SoapHeader Attribute to the Method 
You must also apply the [SoapHeaderAttribute(...)] to the specific web service methods in your partial class that require this header. Note that this often requires modifying the generated proxy code or using a more advanced approach (like WCF Message Inspectors) to avoid direct modification. 

4. Utilize the Custom Header in Your Client Code 
Finally, in your application code, you can instantiate the header class, populate its properties, and assign it to the proxy instance's property before making the service call.

```c#
using YourServiceName.Namespace;

// ... inside your client code ...

MyWebService client = new MyWebService();

// 1. Instantiate the custom header
CustomSoapHeader header = new CustomSoapHeader();

// 2. Populate the information
header.UserId = "myuser";
header.Password = "mypass";
header.Locale = "en-GB";

// 3. Attach the header instance to the client proxy property
client.RequestHeader = header;

// 4. Make the web service call
string result = client.SomeWebServiceMethod();
```

For more complex WCF scenarios or .NET Core applications, a message inspector approach offers greater flexibility for injecting headers into every outgoing request. 
For further guidance, explore this article on adding custom SOAP headers in C# for a web service call. 
Are you working with an older ASMX service (Web Reference) or a WCF service (Service Reference)? Knowing this helps tailor the implementation steps to your specific project type.