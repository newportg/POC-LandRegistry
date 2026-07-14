using Microsoft.Extensions.Logging;
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace KnightFrank.Hub.LandRegistry.Service.Behaviours.SoapHeader
{
    public class HMLRBGMessageEndpointBehavior : Attribute, IEndpointBehavior, IOperationBehavior, IContractBehavior
    {
        #region Member Variables
        private string m_Username = "";
        private string m_Password = "";
        private ILogger<LandRegistrySvc> m_Logger;

        #endregion
        #region Constructor
        public HMLRBGMessageEndpointBehavior(string username, string password, ILogger<LandRegistrySvc> logger)
        {
            m_Username = username;
            m_Password = password;
            m_Logger = logger;
        }
        #endregion
        #region IEndpointBehavior Methods
        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new HMLRBGMessageInspector(m_Username, m_Password, m_Logger));
        }
        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {
        }
        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }
        #endregion
        #region IOperationBehavior Members
        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }
        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.Parent.ClientMessageInspectors.Add(new HMLRBGMessageInspector(m_Username, m_Password, m_Logger));
        }
        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            //throw new NotImplementedException();
        }
        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
            // throw new NotImplementedException();
        }
        #endregion
        #region IContractBehavior Members
        void IContractBehavior.AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
        void IContractBehavior.ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new HMLRBGMessageInspector(m_Username, m_Password, m_Logger));
        }
        void IContractBehavior.ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            //throw new NotImplementedException();
        }
        void IContractBehavior.Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
