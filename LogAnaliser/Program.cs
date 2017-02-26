using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogAnaliser
{
    class Program
    {
        static void Main(string[] args)
        {

            var files = Directory.GetFiles(args[0]);
            foreach (var item in files)
            {
				analise(item);
            }
          
        }
        static void analise(string file)
        {
            var lines = File.ReadAllLines(file);
            var linkCounts = new List<double>();
            var Values = new List<List<double>>();
            int k = 0;
            foreach (var item in lines)
            {
                if (k % 2 == 0)
                {
                    linkCounts.Add(double.Parse(item));
                }
                else
                {
                    var val = item.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    var list = new List<double>();
                    foreach (var v in val)
                    {
                        list.Add(double.Parse(v));
                    }

                    Values.Add(list);
                }
                k++;
            }
	        var linkCount = (linkCounts.Sum() / linkCounts.Count()).ToString();
	        File.WriteAllText(Path.GetFileName(file), linkCount+ Environment.NewLine);
			File.AppendAllText("sum", linkCount+" - ");


            var max = Values.Max(x => x.Count);
            var result = new Dictionary<int, double>(max);
            for (int i = 0; i < max; i++)
            {
                result[i] = 0;
            }


            foreach (var item in Values)
            {
                for (int i = 0; i < max; i++)
                {
                    if (i < item.Count)
                        result[i] += item[i];
                    else
                        result[i] += 1;
                }

            }
            var st = new StringBuilder();
            var perc = false;
            for (int i = 0; i < max; i++)
            {
                var p = result[i] / (double)Values.Count;
                st.Append(string.Format("{0:0.00};", p));
                if (!perc && (p >= 0.994))
                {
                    File.AppendAllText(Path.GetFileName(file), (i * 0.005)+Environment.NewLine);
					File.AppendAllText("sum", (i * 0.005)+Environment.NewLine);
                    perc = true;
                }

            }
            File.AppendAllText(Path.GetFileName(file), st.ToString());

        }
    }
}
