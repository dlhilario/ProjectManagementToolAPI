using System;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.Data.Entity.Core.Objects;

namespace projectManagementToolWebAPI
{
    public class ApplyDataContractResolver : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
           
        }

        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
            DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior =
                description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver =
                new ProxyDataContractResolver();
        }

        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior =
                description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver =
                new ProxyDataContractResolver();
        }

        public void Validate(OperationDescription operationDescription)
        {
           
        }
    }
}