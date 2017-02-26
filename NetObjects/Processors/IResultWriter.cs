using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetObjects.Processors
{
    public interface IResultWriter
    {
        void WritePercalation(Dictionary<int, double> percalationByStep);
        void WriteDoubleValue(double d);
    }

    public class ComaSepareteFileWriter : IResultWriter
    {
        private readonly string _filePath;

        public ComaSepareteFileWriter(string filePath)
        {
            _filePath = filePath;
        }

        public void WritePercalation(Dictionary<int, double> percalationByStep)
        {
            var sb = new StringBuilder();
            IOrderedEnumerable<int> keys = percalationByStep.Keys.OrderBy(k => k);
            foreach (int item in keys)
            {
                sb.AppendFormat("{0};", percalationByStep[item].ToString());
            }
            sb.Append("\r\n");
            File.AppendAllText(_filePath, sb.ToString());
        }

        public void WriteDoubleValue(double d)
        {
            File.AppendAllText(_filePath, string.Format("{0}\r\n", d));
        }
    }
}