using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client
{
    public class Identity
    {
        public string UniqueMsgId { get; set; } = Guid.NewGuid().ToString();

        [Description("The unique message id of the request")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "ExternalRef: string between 1 and 25 characters")]
        [RegularExpression(@"[A-Za-z0-9\s~!&quot;@#$%'\(\)\*\+,\-\./:;=&gt;\?\[\\\]_\{\}\^&#xa3;]*")]
        [Required]
        public string ExternalRef { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // Guid in Base64 is a 24 char string

        [Description("\"A unique reference given to identify a particular request, order or instruction in the system of the organisation allocating it.")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "CustomerRef: string between 1 and 25 characters")]
        [RegularExpression(@"[A-Za-z0-9\s~!&quot;@#$%'\(\)\*\+,\-\./:;=&gt;\?\[\\\]_\{\}\^&#xa3;]*")]
        [Required]
        public string CustomerRef { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // Guid in Base64 is a 24 char string

        [Description("The name of the organisation that has allocated the Reference")]
        public string AllocatedBy { get; set; } = Environment.GetEnvironmentVariable("LandRegistryContactName");

        [Description("This field can be used to provide reference description")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }
}
