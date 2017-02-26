using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetObjects.Core
{
    public class GrafCalculator
    {

        public static bool HasPath(Node[] net, int a, int b)
        {
            Dictionary<int, int> visitedNodes = new Dictionary<int, int>();
            return HasPathInternal2(net, a, b);
        }

        private static bool HasPathInternal(Node[] net, int a, int b, Dictionary<int, int> visitedNodes)
        {
            visitedNodes.Add(a, a);
            
                foreach (var node in net[a].NearestNodes)
                {
                    if (node == b)
                        return true;
                    else
                    {
                        if (!visitedNodes.ContainsKey(node) && (net[node].IsActive))
                            if (HasPathInternal(net, node, b, visitedNodes))
                                return true;
                    }
                }
          
            return false;
        }

        private static bool HasPathInternal2(Node[] net, int a, int b)
        {
            foreach (var node in net)
            {
                node.IsVisited = false;
            }
            
            var queue = new Queue<int>();
            queue.Enqueue(a);
            net[a].IsVisited = true;
           

            while (queue.Count > 0)
            {
                var nodeA = net[queue.Dequeue()];
                foreach (var node in nodeA.NearestNodes)
                {
                    if (node == b)
                        return true;
                    var n = net[node];
                    if (!n.IsVisited && (n.IsActive))
                    {
                        queue.Enqueue(node);
                        n.IsVisited = true;
                    }
                }
            }

            return false;
        }

        public static double GetAvarageLinkCount(Node[] net)
        {
            var lonccount = net.Sum(node => node.NearestNodes.Length);
            return lonccount / (double)net.Length;
        }
    }
}
