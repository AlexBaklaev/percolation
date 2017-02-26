using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetObjects.Processors;

namespace NetObjects.Core
{
    public class SimpleKernel
    {
        public SimpleKernel(IProcessor processor, CalculationTask calculationTask, INetCreator netcreater, IResultWriter writer, int expirementCount = 100)
        {
            for (var i = 0; i < expirementCount; i++)
            {
                processor.InitProcess(calculationTask, netcreater);
                processor.Processing();
                processor.WriteResult(writer);
                Console.WriteLine("expirement {0} success", i);
            }
        }
    }
}
