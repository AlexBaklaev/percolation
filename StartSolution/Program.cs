using System;
using System.IO;
using NetObjects.Core;
using NetObjects.Enums;
using NetObjects.Processors;
using System.Threading.Tasks;

namespace StartSolution
{
	class Program
	{
		protected static int CalculationNodeCount = Properties.Settings.Default.CalculationNodeCount;
		protected static int Experementcount = Properties.Settings.Default.ExpCount;
		protected static int Nodecount = Properties.Settings.Default.NodeInNetCount;
		protected static int Iterationcount = Properties.Settings.Default.CountOfIteration;
		protected static double Percentbackupchannel = 0.05;
		protected static int MaxVirus = Properties.Settings.Default.viruscopy;
		protected static double PofInfective = Properties.Settings.Default.pinfective;
		protected static int Addtomaxlinkcount = Properties.Settings.Default.addtomaxlinkcount;
		protected static int Minlink = Properties.Settings.Default.minlink;
		protected static int Segmentcont = Properties.Settings.Default.segmentcount;
		protected static int Granica = Properties.Settings.Default.granica;
		protected static string Adressdiffstrategy = Properties.Settings.Default.adressdiffstrategy;
		protected static string Virussendstrategy = Properties.Settings.Default.virussendstrategy;
		protected static bool Isadresdiff = Properties.Settings.Default.isadresdiff;
		protected static bool Isappend = Properties.Settings.Default.isappend;
		protected static double AverageLinkCount = Properties.Settings.Default.AverageLinkCount;
		protected static double percentbackupchannel = 0.05;


		static void Main(string[] args)		//Type  min max Processor(4) NetBuilder
        // ReferenceProcessor FileNetLoad Percolation1.csv Percolation2.csv Percolation3.csv
		{

			if (args[0] == "Parallel")
			{
				var min = int.Parse(args[1]);
				var max = int.Parse(args[2]);

				Parallel.For(min, max + 1, i => Calc(new[] { args[3], args[4] }, i /*/ (double)10*/));
				//Calc(new[] { args[3], args[4] }, 3);
			}
			else
			{
				Calc(args, AverageLinkCount); //new[] { "4", "HoleNetCreator"}
			}

		}


		static void Calc(string[] userinput, double averageLinkCount)
		{
			string netType = Properties.Settings.Default.NetType;
			INetCreator Netcreator = null;
			IProcessor processor;

			var path = Directory.GetCurrentDirectory() + @"\Results\";
			Directory.CreateDirectory(path);
			CalculationTask calculationTask;
			#region Select CalculationTask

			ComaSepareteFileWriter writer = null;
			switch (userinput[0])
			{
				case "4":
					{
						calculationTask = new CalculationTask(0, Nodecount, netType, Minlink, Addtomaxlinkcount, MaxVirus, PofInfective, Granica, Segmentcont, Adressdiffstrategy, Virussendstrategy, Isappend, Isadresdiff,
						  Iterationcount, path, percentbackupchannel);
						processor = new TrueProcessor();
						break;
					}
				case "TrueReferenceProcessor":
					{
						calculationTask = new CalculationTask(0, Nodecount, netType, Minlink, (int)averageLinkCount, MaxVirus, PofInfective, Granica, Segmentcont, Adressdiffstrategy, Virussendstrategy, Isappend, Isadresdiff,
						  Iterationcount, path, percentbackupchannel);
						processor = new TrueReferenceProcessor();
						break;
					}
				case "ReferenceProcessor":
					{
                        calculationTask = CalculationTaskCreator.CreateForReferencesCalculator(path, Iterationcount, netType);
                        var weight = FileNetLoad.ReadWeight(userinput[3], Properties.Settings.Default.DefaultConnectionWeight);

						var st = userinput[3]; 
						for (int i = 4; i < userinput.Length; i++)
					    {
                            weight = FileNetLoad.MultiplyWeight(weight, FileNetLoad.ReadWeight(userinput[i], Properties.Settings.Default.DefaultConnectionWeight));
						    st += "-" + userinput[i];
					    }
                        processor = new ReferencesProcessor(weight);
						writer = new ComaSepareteFileWriter(Path.Combine(path, st + "_" + Properties.Settings.Default.DefaultConnectionWeight));
						break;
					}
				default:
					{
						Console.WriteLine(@"Input is not correct");
						return;
					}
			}
			#endregion
			#region Select NetBuilder
			switch (userinput[1])
			{
				case "NetBuilder":
					{

						Netcreator = new NetBuilder(calculationTask);
						break;

					}
				case "HoleNetCreator":
					{

						Netcreator = new HoleNetCreator(new NetBuilder(calculationTask), averageLinkCount);
						break;

					}
				case "FileNetLoad":
					{
						Netcreator = new FileNetLoad(userinput[2]);
						break;
					}

				default:
					{
						Console.WriteLine(@"Input is not correct");
						return;
					}
			}
			#endregion

			if (writer == null)
			{
				writer =
					new ComaSepareteFileWriter(Path.Combine(path, calculationTask.NetType + "_" + averageLinkCount.ToString("0.00")));
			}
			var d = new SimpleKernel(processor, calculationTask, Netcreator, writer, Experementcount);
		}
		static string Choise()
		{
			Console.WriteLine(@"Input expirement type");
			Console.WriteLine(@"If Cinematik, press 1");
			Console.WriteLine(@"If CollapsingPath, press 2");
			return Console.ReadLine();
		}
	}
}
