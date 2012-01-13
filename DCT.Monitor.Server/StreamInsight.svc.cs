using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DCT.Monitor.StreamInsight;
using Microsoft.ComplexEventProcessing.ManagementService;
using System.ServiceModel.Activation;
using DCT.Unity;

namespace DCT.Monitor.Server
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StreamInsight : IManagementService
    {
        private static IManagementService _service;

        static StreamInsight()
        {
            var core = ServiceLocator.Current.Resolve<QueriesCore>();
            _service = core.GetManagementService();
        }

        public IAsyncResult BeginCheckpoint(CheckpointRequest request, AsyncCallback asyncCallback, object asyncState)
        {
            return _service.BeginCheckpoint(request, asyncCallback, asyncState);
        }

        public void CancelCheckpoint(CancelCheckpointRequest request)
        {
            _service.CancelCheckpoint(request);
        }

        public ChangeQueryStateResponse ChangeQueryState(ChangeQueryStateRequest request)
        {
            return _service.ChangeQueryState(request);
        }

        public void ClearDiagnosticSettings(ClearDiagnosticSettingsRequest request)
        {
            _service.ClearDiagnosticSettings(request);
        }

        public CreateResponse Create(CreateRequest request)
        {
            return _service.Create(request);
        }

        public DeleteResponse Delete(DeleteRequest request)
        {
            return _service.Delete(request);
        }

        public CheckpointResponse EndCheckpoint(IAsyncResult asyncResult)
        {
            return _service.EndCheckpoint(asyncResult);
        }

        public EnumerateResponse Enumerate(EnumerateRequest request)
        {
            return _service.Enumerate(request);
        }

        public GetResponse Get(GetRequest request)
        {
            return _service.Get(request);
        }

        public GetDiagnosticSettingsResponse GetDiagnosticSettings(GetDiagnosticSettingsRequest request)
        {
            return _service.GetDiagnosticSettings(request);
        }

        public GetDiagnosticViewResponse GetDiagnosticView(GetDiagnosticViewRequest request)
        {
            return _service.GetDiagnosticView(request);
        }

        public void SetDiagnosticSettings(SetDiagnosticSettingsRequest request)
        {
            _service.SetDiagnosticSettings(request);
        }
    }
}
