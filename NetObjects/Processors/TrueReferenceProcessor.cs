using System;
using System.Collections.Generic;
using System.Linq;

using NetObjects.Core;

namespace NetObjects.Processors
{
	public class TrueReferenceProcessor : IProcessor
	{
		#region IProcessor Members

		private Node[] _netMap;

		private Node[] _connections;

		private int A;

		private int B;

		private CalculationTask _calculationTask;

		private List<int> _percalationResult;

		public void InitProcess(CalculationTask calculationTask, INetCreator netcreater)
		{
			_netMap = netcreater.CreateNet();
			_calculationTask = calculationTask;
			InitConnections();
			SelectAB();
			_percalationResult = new List<int>(_calculationTask.CountOfIteration);
		}

		public void Processing()
		{
			for (int i = 0; i < _calculationTask.CountOfIteration; i++)
			{
				InitConnections();
				Process();
			}
		}

		private void Process()
		{
			var inactivCount = 0;
			bool hasPath = GrafCalculator.HasPath(_connections, A, B);
			var step = 0;
			while ((inactivCount < _connections.Length) && (hasPath))
			{
				inactivCount = (int)(_calculationTask.PofInfective * (_netMap.Sum(node => node.NearestNodes.Length)/2));
				Inactivate(_connections, inactivCount);
				hasPath = GrafCalculator.HasPath(_connections, A, B);
				step++;
			}
			_percalationResult.Add(step);
		}

		private void Inactivate(Node[] net, int inactivCount)
		{
			var random = new Random();
			while ((inactivCount > 0) && (_connections.Length != 0))
			{
				var node = net[random.Next(net.Length)];
				if (node.NearestNodes.Length > 0)
				{
					inactivCount--;
					var removeconnectionIndex = random.Next(node.NearestNodes.Length);

					var nodeToremove = node.NearestNodes[removeconnectionIndex];
					node.RemoveNearestNode(nodeToremove);
					net[nodeToremove].RemoveNearestNode(node.Id);
				}
			}
		}

		private void InitConnections()
		{
			_connections = new Node[_netMap.Length];

			for (var i = 0; i < _netMap.Length; i++)
			{
				var node = new Node(i);
				node.IsActive = true;

				foreach (var nearestNode in _netMap[i].NearestNodes)
				{
					node.AddNearestNode(nearestNode);
				}
				_connections[i] = node;
			}
		}

		public void WriteHistoryLogAll()
		{
			throw new NotImplementedException();
		}

		private void SelectAB()
		{
			var random = new Random();
			var nodesCount = _connections.Length;
			A = random.Next(nodesCount);
			B = random.Next(nodesCount);
			if ((A >= nodesCount) || (A >= nodesCount))
			{
				throw new ApplicationException(string.Format(" a = {0}, b = {1} nodesCount{2}", A, B, _connections.Length));
			}
			while ((_connections[A].Id == B) || (_connections[A].NearestNodes.Contains(B)))
			{
				B = random.Next(nodesCount);
			}
		}

		public void WriteResult(IResultWriter writer)
		{
			int maxStep = _percalationResult.Max();
			var result = new Dictionary<int, double>(maxStep);

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
						result.Add(t, 1);
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

			writer.WriteDoubleValue(GrafCalculator.GetAvarageLinkCount(_netMap));
			writer.WritePercalation(result);
		}

		#endregion
	}
}