using System.Collections.Generic;
using System.Linq;
using DCT.Monitor.Entities;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;

namespace DCT.Monitor.StreamInsight
{
    public static class Extensibility
    {
        [CepUserDefinedAggregate(typeof(CepPingCount))]
        public static int DistinctCount<TPayload>(this CepWindow<TPayload> window)
        {
            throw CepUtility.DoNotCall();
        }

        [CepUserDefinedAggregate(typeof(CepGroupedElements))]
        public static IEnumerable<TPayload> GroupedElements<TPayload>(this CepWindow<TPayload> window)
        {
            throw CepUtility.DoNotCall();
        }
    }

    public class CepPingCount : CepAggregate<PageRequest, int>
    {
        public override int GenerateOutput(IEnumerable<PageRequest> payloads)
        {
            return payloads.GroupBy(p => p.Id).Count();
        }
    }

    public class CepGroupedElements : CepAggregate<PageRequest, IEnumerable<PageRequest>>
    {
        public override IEnumerable<PageRequest> GenerateOutput(IEnumerable<PageRequest> payloads)
        {
            return payloads;
        }
    }
}
