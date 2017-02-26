using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace NetObjects.Core
{
	/// <summary>
	/// задача для рассчета
	/// Для проведения вычислений разумно использовать 
	/// не группу отдельных параметров передаваемых классам 
	/// для вычислениям, а объект класса CalculationTask, 
	/// который уже внутри себя содержит всю необходимую 
	/// информацию о вычислениях которые будут проводится, 
	/// такие как- количество узлов в сети, тип сети, если 
	/// она генерируется программно, тип алгоритма заражения,
	/// стратегию распространения вирусов, информацию о месте 
	/// в файловой системе, куда записывать результаты моделирования, 
	/// число экспериментов с этой конфигурацией  и многие другие параметры
	/// </summary>
	public class CalculationTask
	{
		#region Fields
		/// <summary>
		/// количество узлов в сети
		/// </summary>
		public int NodeCount;
		/// <summary>
		/// колиечество итераций каждого расчета
		/// </summary>
		public int CountOfIteration;
		/// <summary>
		/// максимальное число рассылаемых узлов вирусов за шаг итерации
		/// </summary>       
		public int MaxVirus;
		/// <summary>
		///для сетей таких как случайная с множеством связей(multilink) или произвольная кейли(keilyrandom) количество связей
		///которые можно добавить 
		/// </summary>
		public int AddToMaxLinkCount;
		/// <summary>
		/// для сетей таких как случайная с множеством связей(multilink)
		///  или произвольная кейли(keilyrandom),которые будут всегда       
		/// </summary>
		public int MinLink;
		/// <summary>
		/// Добавочное до максимума число связей
		/// </summary>
		// public int MaxLink;
		/// <summary>
		/// номер задачи
		/// </summary>
		public int TaskId;
		/// <summary>
		/// вероятность заражения
		/// </summary>
		public double PofInfective;
		/// <summary>
		/// проводить ли дозапись в файл каждый шаг или писать все в конце расчета. Если поставить в false
		/// то увеличивается вероятность вылета по памяти с outofmemoryexeption
		/// </summary>
		public bool IsAppend;
		/// <summary>
		/// проводить ли разделение адресного пространства
		/// </summary>
		public bool IsAdresDiff;
		/// <summary>
		/// если IsAdresDiff==true то выбрать тип разделения адресного пространства
		/// </summary>
		public string AdressDiffType;
		/// <summary>
		/// если IsAdresDiff==true то выбрать спасоб заражения адресного пространства
		/// </summary>
		public string VirusStrategy;
		/// <summary>
		/// тип топологии сети для расчета
		/// </summary>
		public string NetType;
		/// <summary>
		/// путь для записи файлов с отчетами
		/// </summary>
		public string Path;
		/// <summary>
		/// для разделяемого адресного пространства граница остановки деления адресного пространства
		/// </summary>
		public int Granica;
		/// <summary>
		/// для разделяемого адресного пространства количество сегментов, на которой делить адресное пространство
		/// </summary>
		public int SegmentCount;
		/// <summary>
		/// процент дублирования каналов. Не используется
		/// </summary>
		public double PercentBuckupChannel;

		/// <summary>
		/// Среднее число связей которое нужно достич, удаляя узлы
		/// </summary>
		// public double AverageLinkCount;
		#endregion

		#region Constructors

		#region CalculationTask Empty
		/// <summary>
		/// Empty Constructor
		/// </summary>
		[DebuggerHidden]
		public CalculationTask()
			: this(0, 0, 0, string.Empty, string.Empty, string.Empty, false, false, 0, 0, 0, 0, 0, 0, string.Empty, 0)
		{
		}
		#endregion

		#region CalculationTask for tests
		/// <summary>
		/// конструктор для тестов по созданию сетей и тп.
		/// </summary>
		/// <param name="netType">тип сети</param>
		/// <param name="nodeCount">количество узлов в сети</param>
		/// <param name="addToMaxLinkCount">число узлов, которое можно добавить  до максимума связей на узел</param>
		/// <param name="minLink">минимальное число связей на узел</param>
		/// <param name="pofInfective">вероятность заражения, нормированная на 0-1</param>
		/// <param name="percentBuckupChannel">процент дублирования каналов нормированный на 0-1</param>
		[DebuggerHidden]
		public CalculationTask(string netType, int nodeCount, int addToMaxLinkCount = 0, int minLink = 0, double pofInfective = 0, double percentBuckupChannel = 0.0)
			: this(0, 0, 0, netType, string.Empty, string.Empty, false, false, nodeCount, 0, 0, pofInfective, addToMaxLinkCount, minLink, string.Empty, percentBuckupChannel)
		{
		}
		#endregion

		#region CalculationTask for CollapsingPathKernel
		/// <summary>
		/// CalculationTask for CollapsingPathKernel
		/// </summary>
		/// <param name="netType">тип сети</param>
		/// <param name="path">Путь для записи в файл</param>
		/// <param name="nodeCount">количество узлов в сети</param>
		/// <param name="addToMaxLinkCount">число узлов, которое можно добавить  до максимума связей на узел</param>
		/// <param name="minLink">минимальное число связей на узел</param>
		/// <param name="countOfIteration">количество итераций расчета</param>
		/// <param name="taskId">Номер исполняемой задачи</param>
		[DebuggerHidden]
		public CalculationTask(string netType, string path, int nodeCount, int addToMaxLinkCount, int minLink, int countOfIteration, int taskId = 0)
			: this(taskId, 0, 0, netType, string.Empty, string.Empty, false, false, nodeCount, countOfIteration, 0, 0, addToMaxLinkCount, minLink, path, 0)
		{
		}
		#endregion

		#region CalculationTask for CinematikKernel
		/// <summary>
		/// CalculationTask for CinematikKernel
		/// </summary>
		/// <param name="taskid"></param>
		/// <param name="nodeCount"></param>
		/// <param name="netType"></param>
		/// <param name="minLink"></param>
		/// <param name="addToMaxLinkCount"></param>
		/// <param name="maxVirus"></param>
		/// <param name="pofInfective"></param>
		/// <param name="granica"></param>
		/// <param name="segmentcount"></param>
		/// <param name="adressDiffType"></param>
		/// <param name="virusStrategy"></param>
		/// <param name="append"></param>
		/// <param name="adressDiff"></param>
		/// <param name="path"></param>
		/// <param name="percentbackupchannel"></param>
		/// <param name="countOfIteration"></param>
		public CalculationTask(int taskid, int nodeCount, string netType, int minLink, int addToMaxLinkCount,
			int maxVirus, double pofInfective, int granica, int segmentcount,
			string adressDiffType, string virusStrategy, bool append, bool adressDiff, int countOfIteration, string path, double percentbackupchannel)
			: this(taskid, granica, segmentcount, netType, adressDiffType, virusStrategy, append, adressDiff, nodeCount, countOfIteration, maxVirus, pofInfective, addToMaxLinkCount, minLink, path, percentbackupchannel)
		{

		}
		#endregion

		#region CalculationTask Max
		/// <summary>
		/// конструктор общий
		/// </summary>
		/// <param name="taskid">номер задачи</param>
		/// <param name="granica">для разделяемого адресного пространства граница остановки деления адресного пространства</param>
		/// <param name="segmentcount">для разделяемого адресного пространства количество сегментов, на которой делить адресное пространство</param>
		/// <param name="adressDiffType">если IsAdresDiff==true то выбрать тип разделения адресного пространства</param>
		/// <param name="virusStrategy">стратегия распространения вирусов</param>
		/// <param name="append">проводить ли дозапись в файл каждый шаг или писать все в конце расчета. Если поставить в false то увеличивается вероятность вылета по памяти с outofmemoryexeption</param>
		/// <param name="adressDiff">проводить ли разделение адресного пространства</param>
		/// <param name="countOfIteration">количество итераций эксперимента</param>
		/// <param name="maxVirus">максимальное числа разсылаемых вирусов на каждом шаге итерации 1 вирусом</param>
		/// <param name="path">путь для записи файлов с результатами</param>
		/// <param name="netType">тип сети</param>
		/// <param name="nodeCount">количество узлов в сети</param>
		/// <param name="addToMaxLinkCount">число узлов, которое можно добавить  до максимума связей на узел</param>
		/// <param name="minLink">минимальное число связей на узел</param>
		/// <param name="pofInfective">вероятность заражения, нормированная на 0-1</param>
		/// <param name="percentBuckupChannel">процент дублирования каналов нормированный на 0-1</param>
		[DebuggerHidden]
		private CalculationTask(int taskid, int granica, int segmentcount, string netType,
			string adressDiffType, string virusStrategy, bool append, bool adressDiff, int nodeCount, int countOfIteration,
			int maxVirus, double pofInfective, int addToMaxLinkCount, int minLink, string path, double percentBuckupChannel)
		{
			NodeCount = nodeCount;
			CountOfIteration = countOfIteration;
			MaxVirus = maxVirus;
			PofInfective = pofInfective;
			NetType = netType;
			IsAppend = append;
			IsAdresDiff = adressDiff;
			AddToMaxLinkCount = addToMaxLinkCount;
			MinLink = minLink;
			Path = path;
			TaskId = taskid;
			AdressDiffType = adressDiffType;
			VirusStrategy = virusStrategy;
			Granica = granica;
			SegmentCount = segmentcount;
			PercentBuckupChannel = percentBuckupChannel;
			// AverageLinkCount = averageLinkCount;
		}
		#endregion
		#endregion

		#region Methods
		#region CalculationTask Clone
		/// <summary>
		/// Clone Current Task
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CalculationTask Clone(int id = 0)
		{
			var ret = new CalculationTask
						  {
							  AddToMaxLinkCount = AddToMaxLinkCount,
							  NodeCount = NodeCount,
							  CountOfIteration = CountOfIteration,
							  MinLink = MinLink,
							  TaskId = id,
							  PofInfective = PofInfective,
							  IsAppend = IsAppend,
							  IsAdresDiff = IsAdresDiff,
							  AdressDiffType = AdressDiffType,
							  VirusStrategy = VirusStrategy,
							  NetType = NetType,
							  Path = Path,
							  Granica = Granica,
							  SegmentCount = SegmentCount,
							  PercentBuckupChannel = PercentBuckupChannel
						  };
			return ret;
		}
		#endregion

		#region GetbytesFromCalculationTask
		/// <summary>
		/// GetbytesFromCalculationTask Serialize
		/// </summary>
		/// <param name="calc"></param>
		/// <returns></returns>
		public static byte[] GetbytesFromCalculationTask(CalculationTask calc)
		{
			var serializer = new XmlSerializer(calc.GetType());
			var memorystream = new MemoryStream();
			serializer.Serialize(memorystream, calc);
			memorystream.Position = 0;
			var len = new byte[memorystream.Length];
			memorystream.Read(len, 0, (int)memorystream.Length);
			memorystream.Position = 0;
			return len;
		}
		#endregion

		#region GetCalculationTaskFrombytes
		/// <summary>
		/// GetCalculationTaskFrombytes Deserialize
		/// </summary>
		/// <param name="bytearray"></param>
		/// <returns></returns>
		public static CalculationTask GetCalculationTaskFrombytes(byte[] bytearray)
		{
			var calc = new CalculationTask();
			var serializer = new XmlSerializer(calc.GetType());
			var memorystream = new MemoryStream();
			memorystream.Write(bytearray, 0, bytearray.Length);
			memorystream.Position = 0;
			var calc2 = (CalculationTask)serializer.Deserialize(memorystream);
			return calc2;
		}
		#endregion
		#endregion
	}


	public static class CalculationTaskCreator
	{
		public static CalculationTask CreateForReferencesCalculator(string logPath, int iterationCount, string netType)
		{
			return new CalculationTask()
					   {
						   Path = logPath,
						   CountOfIteration = iterationCount,
                           NetType = netType
					   };
		}
	}
}
