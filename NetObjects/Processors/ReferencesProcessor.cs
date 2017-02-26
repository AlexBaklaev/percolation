using System;
using System.Collections.Generic;
using System.Linq;
using NetObjects.Core;

namespace NetObjects.Processors
{
    public class ReferencesProcessor : IProcessor
    {
        private readonly double[,] _weight;

        #region IProcessor Members

        private Node[] _netMap;
        private Node[] _connections;
        private int _a;
        private int _b;
        private CalculationTask _calculationTask;
        private List<int> _percalationResult;
        private Random _random;

        public ReferencesProcessor(double[,] weight)
        {
            _weight = weight;
        }

        public void InitProcess(CalculationTask calculationTask, INetCreator netcreater)
        {
            _netMap = netcreater.CreateNet();
            UseOnlyActiveNodes();
            _calculationTask = calculationTask;
            SelectAb();
            _percalationResult = new List<int>(_calculationTask.CountOfIteration);
            _random = new Random();
        }

        public void Processing()
        {
            for (var i = 0; i < _calculationTask.CountOfIteration; i++)
            {
                InitConnections();
                Process();
            }
        }

        private void Process()
        {
            var hasPath = GrafCalculator.HasPath(_connections, _a, _b);

           _percalationResult.Add(hasPath ? 1 : 0);
        }

        private void InitConnections()
        {
            _connections = new Node[_netMap.Length];

            for (var i = 0; i < _netMap.Length; i++)
            {
                var node = new Node(i);

                foreach (var nearestNode in _netMap[i].NearestNodes)
                {
                    if (HasConnection(i, nearestNode))
                    {
                        node.AddNearestNode(nearestNode);
                    }
                }
                _connections[i] = node;
            }
        }
        private void UseOnlyActiveNodes()
        {
           // var t = _netMap.Count(x => x.IsActive);
            for (var i = 0; i < _netMap.Length; i++)
            {
               foreach (var nearestNode in _netMap[i].NearestNodes)
                {
                    if (_weight[i, nearestNode] > 0)
                    {
                        _netMap[nearestNode].IsActive = true;
                        _netMap[i].IsActive = true;
                    }
                    
                }
             }
			//var t2 = _netMap.Count(x => x.IsActive);
			//if (t>t2)
			//	throw new Exception();
        }

        private bool HasConnection(int i, int j)
        {
            return (_weight[i, j] > _random.NextDouble());
        }

        public void WriteHistoryLogAll()
        {
            throw new NotImplementedException();
        }

        private void SelectAb()
        {
            var random = new Random();
            var nodesCount = _netMap.Length;
            _a = random.Next(nodesCount);
           
            while (!_netMap[_a].IsActive)
            {
                _a = random.Next(nodesCount);
            }

            _b = random.Next(nodesCount);

            while (!_netMap[_b].IsActive || (_netMap[_a].Id == _b))
            {
                _b = random.Next(nodesCount);
            }
        }

        public void WriteResult(IResultWriter writer)
        {

            var summ = _percalationResult.Aggregate<int, long>(0, (current, percalationStep) => current + percalationStep);

            writer.WriteDoubleValue((double)summ / _calculationTask.CountOfIteration);

        }

        #endregion
    }
}