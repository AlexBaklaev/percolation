using System;
using System.IO;

namespace NetObjects.Core
{
    public class FileNetLoad : INetCreator
    {
        private readonly string _path;

        public FileNetLoad(string path)
        {
            _path = path;
        }

        public Node[] CreateNet()
        {
            var lines = File.ReadAllLines(_path);

            var net = new Node[lines.Length];
            for (var i = 0; i < lines.Length; i++)
            {
                net[i] = new Node(i);
                var connections = lines[i].Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);

                if (connections.Length != lines.Length)
                {
                    throw new Exception(
                        string.Format("Line line{0} has {1} elements. Expected {2}", lines[i], connections.Length,
                            lines.Length));
                }

                for (var j = 0; j < connections.Length; j++)
                {
                    if (connections[j] == "1")
                    {
                        net[i].AddNearestNode(j);
                    }
                }
            }
            return net;
        }

        public static double[,] ReadWeight(string file, double defaultConnectionWeight)
        {
            var lines = File.ReadAllLines(file);

            var weight = new double[lines.Length, lines.Length];
            for (var i = 0; i < lines.Length; i++)
            {
                var w = lines[i].Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);

                if (w.Length != lines.Length)
                {
                    throw new Exception(
                        string.Format("Line line{0} has {1} elements. Expected {2}", lines[i], w.Length, lines.Length));
                }

                for (var j = 0; j < w.Length; j++)
                {
                    if (w[j] != "*")
                    {
                        double d;
                        if (double.TryParse(w[j], out d))
                        {
                            weight[i, j] = d;
                        }
                        else
                        {
                            throw new Exception(string.Format("{0} not decimal", w[j]));
                        }
                    }
                    else
                    {
                        weight[i, j] = defaultConnectionWeight;
                    }
                }
            }
            return weight;
        }

        public static double[,] MultiplyWeight(double[,] a, double[,] b)
        {
            if (a.Length != b.Length)
                throw new Exception(string.Format("Weight matrix not equals {0}!={1}", a.Length, b.Length));

            var weight = new double[a.GetLength(0), a.GetLength(0)];
            var demen = Math.Sqrt(a.Length);
            if (demen != a.GetLength(0))
                throw new Exception(string.Format("matrix not square {0}!={1}", demen, a.GetLength(0)));
            for (var i = 0; i < demen; i++)
            {
                for (var j = 0; j < demen; j++)
                {
                    weight[i, j] = a[i, j]*b[i, j];
                }
            }
            return weight;
        }
    }
}