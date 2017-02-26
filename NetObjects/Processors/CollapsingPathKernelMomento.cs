using System;
using System.Collections.Generic;
using NetObjects.Core;

namespace NetObjects.Processors
{
    /// <summary>
    /// 
    /// </summary>
    public class CollapsingPathKernelMomento
    {
        /// <summary>
        /// 
        /// </summary>
        public const double Step = 0.005;
        /// <summary>
        /// 
        /// </summary>
        public const double Precision = 0.00006;
        /// <summary>
        /// 
        /// </summary>
        public Node[] Netmap;
        /// <summary>
        /// 
        /// </summary>
        public Random Random;
        /// <summary>
        /// 
        /// </summary>
        public int A;
        /// <summary>
        /// 
        /// </summary>
        ///  public int _b;
        public CalculationTask C;
        /// <summary>
        /// 
        /// </summary>
        public List<int> AllRezults;
        /// <summary>
        /// 
        /// </summary>
        public int i;
        /// <summary>
        /// 
        /// </summary>
        public double Start;
        /// <summary>
        /// 
        /// </summary>
        public List<int> AllOneNetExpPathExist;
        /// <summary>
        /// 
        /// </summary>
        public bool Isinwhile;
        /// <summary>
        /// 
        /// </summary>
        public List<bool> Pathexists;
    }
}
