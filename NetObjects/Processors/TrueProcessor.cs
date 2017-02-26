using System;
using System.Collections.Generic;
using System.Linq;
using NetObjects.Core;

namespace NetObjects.Processors
{
    public class TrueProcessor : IProcessor
    {
        #region IProcessor Members
        private Node[] NetMap;
        private int A;
        private int B;
        private CalculationTask _calculationTask;
        private List<int> _percalationResult;

        public void InitProcess(CalculationTask calculationTask, INetCreator netcreater)
        {
            NetMap = netcreater.CreateNet();
            _calculationTask = calculationTask;
            SelectAB();
            _percalationResult = new List<int>(_calculationTask.CountOfIteration);
        }

        public void Processing()
        {
          
            for (int i = 0; i < _calculationTask.CountOfIteration; i++)
            {
                ClaerNet();
                Process();
            }
        }

       private void Process()
        {
            var inactivCount = 0;
            bool hasPath = GrafCalculator.HasPath(NetMap, A, B);
            var step = 0;
            while ((inactivCount < NetMap.Length) && (hasPath))
            {
                ClaerNet();
                inactivCount = (int)(_calculationTask.PofInfective * step * NetMap.Length);
                Inactivate(NetMap, inactivCount);
                hasPath = GrafCalculator.HasPath(NetMap, A, B);
                step++;
            }
            _percalationResult.Add(step);
        }

        private void Inactivate(Node[] net, int inactivCount)
        {
            var random = new Random();
            while (inactivCount > 0)
            {
                var node = net[random.Next(net.Length)];
                if (node.IsActive)
                {
                    inactivCount--;
                    node.IsActive = false;
                }
            }

        }


        private void ClaerNet()
        {
            foreach (var node in NetMap)
            {
                node.IsActive = true;
            }
        }

        public void WriteHistoryLogAll()
        {
            throw new NotImplementedException();
        }

        private void SelectAB()
        {
            var random = new Random();
            var nodesCount = NetMap.Length;
            A = random.Next(nodesCount);
            B = random.Next(nodesCount);
            if ((A >= nodesCount) || (A >= nodesCount))
                throw new ApplicationException(string.Format(" a = {0}, b = {1} nodesCount{3}", A, B, NetMap.Length));
            while ((NetMap[A].Id == B) || (NetMap[A].NearestNodes.Contains(B)))
            {
                B = random.Next(nodesCount);
            }

        }
        public void WriteResult(IResultWriter writer)
        {
             int maxStep = _percalationResult.Max();
             Dictionary<int, double> result = new Dictionary<int, double>(maxStep);

             foreach (var percalationStep in _percalationResult)
             {
                 var t = percalationStep;
                 while (t <= maxStep)
                 {
                     if (result.ContainsKey(t))
                     {
                         result[t]++; 
                     }
                     else
                     {
                         result.Add(t,1);
                     }
                     t++;
                 }
             }
             for (int i = 0; i <= maxStep; i++)
             {
                 if (result.ContainsKey(i))
                 {
                     result[i] = (double)result[i] / (double)_calculationTask.CountOfIteration;
                 }
                 else
                 {
                     result.Add(i, 0);
                 }
             }

             writer.WriteDoubleValue(GrafCalculator.GetAvarageLinkCount(NetMap));
            writer.WritePercalation(result);
        }
        #endregion

    }
}
