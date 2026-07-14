using Microsoft.Extensions.Logging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace KnightFrank.Hub.LandRegistry.Service.Behaviours.SoapHeader
{
    public class HMLRBGMessageInspector : IClientMessageInspector
    {
        #region Member Variables
        private string m_Username = "";
        private string m_Password = "";
        private readonly ILogger<LandRegistrySvc> m_Logger;

        #endregion
        #region Constructor
        public HMLRBGMessageInspector(string username, string password, ILogger<LandRegistrySvc> logger)
        {
            m_Username = username;
            m_Password = password;
            m_Logger = logger;
        }
        #endregion
        #region IClientMessageInspector Methods
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            m_Logger.LogInformation("AfterReceiveReply called :" + reply);
        }
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Create the WsseSecurityHeader with the current user credetials.
            MessageHeader wsseHeader = CreateWsseSecurityHeader();
            request.Headers.Add(wsseHeader);
            // Create the i18nHeader with the locale settings.
            MessageHeader i18nHeader = CreateI18nHeader();
            request.Headers.Add(i18nHeader);
            request.Headers.Action = null;

            m_Logger.LogInformation("BeforeSendRequest called :" + request);

            return null;
        }
        #endregion
        #region Private Methods
        private MessageHeader CreateWsseSecurityHeader()
        {
            XmlDocument doc = new XmlDocument();
            StringBuilder xml = new StringBuilder();
            xml.Append("<UsernameToken xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">");
            xml.Append("<Username>");
            xml.Append(m_Username);
            xml.Append("</Username>");
            xml.Append("<Password>");
            xml.Append(m_Password);
            xml.Append("</Password>");
            xml.Append("</UsernameToken>");
            doc.LoadXml(xml.ToString());
            return MessageHeader.CreateHeader("Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd",
           doc.DocumentElement);
        }
        private MessageHeader CreateI18nHeader()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<locale xmlns=\"http://www.w3.org/2005/09/ws-i18n\">en</locale>");
            return MessageHeader.CreateHeader("international", "http://www.w3.org/2005/09/ws-i18n", doc.DocumentElement);
        }
        #endregion
    }
}
