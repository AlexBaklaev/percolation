using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetObjects.Core
{
    public class HoleNetCreator : INetCreator
    {
        private INetCreator _netCreator;
        private double _targetAverageLinkCount;
        public HoleNetCreator(INetCreator netCreator, double targetAverageLinkCount)
        {
            _netCreator = netCreator;
            _targetAverageLinkCount = targetAverageLinkCount;
        }
        public Node[] CreateNet()
        {
            var net = _netCreator.CreateNet();
            var linkcount = net.Sum(node => node.NearestNodes.Length);

            var linkChangeStep = (_targetAverageLinkCount * net.Length - linkcount) / 10;
            if (linkChangeStep < 0)
            {
                while (GrafCalculator.GetAvarageLinkCount(net)>_targetAverageLinkCount)
                {
                    RemoveLinks(net, Math.Abs((int)linkChangeStep));
                }

            }

            //if (linkChangeStep > 0)
            //{
            //    while (GrafCalculator.GetAvarageLinkCount(net) < _targetAverageLinkCount)
            //    {
            //        AddLinks(net, (int)linkChangeStep);
            //    }
            //}
            return net;
        }
        void RemoveLinks(Node[] net, int removeLinkCount)
        {
            var r = new Random();

            while (removeLinkCount>0)
            {
                var a = r.Next(net.Length);
                if (net[a].NearestNodes.Length >= 2)
                {
                    var index = r.Next(net[a].NearestNodes.Length);
                    var b = net[a].NearestNodes[index];
                    if (net[b].NearestNodes.Length >= 2)
                    {
                        net[a].RemoveNearestNode(b);
                        net[b].RemoveNearestNode(a);
                        removeLinkCount--;
                    }
                }
            }
       
        }

        void AddLinks(Node[] net, int addLinkCount)
        {
            throw new NotImplementedException();
        }
    }
}
