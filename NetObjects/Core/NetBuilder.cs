using System;
using System.Collections.Generic;
using System.Linq;
using NetObjects.Enums;

namespace NetObjects.Core
{
   /// <summary>
   /// Class, which can generate net
   /// </summary>
   public class NetBuilder : INetCreator
   {
      #region Поля
      /// <summary>
      /// Add to max link count
      /// </summary>
      protected int AddToMaxLinkCount;
      /// <summary>
      /// Node count in net
      /// </summary>
      protected int NodeCount;
      /// <summary>
      /// Min count of link
      /// </summary>
      protected int Minlink;
      /// <summary>
      /// percent of backup channel
      /// </summary>
      protected double Percentbuckupchanel;
      /// <summary>
      /// Type of net
      /// </summary>
      protected string Nettype;
      #endregion

      #region Constructors

      #region Empty Constructor
      /// <summary>
      /// Empty Constructor
      /// </summary>
      protected NetBuilder()
      {

      }
      #endregion

      #region NetBuilder (CalculationTask)
      /// <summary>
      /// NetBuilder (CalculationTask)
      /// </summary>
      /// <param name="c">Задача на вычисление, по которой выясняется, какую сеть генерировать</param>
      public NetBuilder(CalculationTask c)
      {
          NodeCount = c.NodeCount;
         AddToMaxLinkCount = c.AddToMaxLinkCount;
         Minlink = c.MinLink;
         Percentbuckupchanel = c.PercentBuckupChannel;
         Nettype = c.NetType;
      }
      #endregion

      #endregion

      #region Methods

      #region Public Methods
      #region CreateNet()
      /// <summary>
      /// Есть будет необходимость добавить новый тип сети, будет нужно добавить эту сеть
      /// в файл NetType в Enum и и в класс netBuilder добавить еще одного условия, по которому
      /// будет произведен вызов процедуры построения сети нужного типа. Так же может быть
      /// Вместо NetBuilder выполнена команда загрузки сети из внешнего источника. 
      /// На выходе должен быть массив объектов типа Node. Остальное не так важно.
      /// </summary>
      /// <returns>Net in array</returns>
      public virtual Node[] CreateNet()
      {
         #region 2d regular
         if (NetType.Quadro.ToString() == Nettype)
             return QuadroNetMaker(NodeCount);
         if (NetType.Hexagon.ToString() == Nettype)
             return HexagonNetMaker(NodeCount);
         if (NetType.Net3122.ToString() == Nettype)
             return Net3122Maker(NodeCount);
         if (NetType.KeilyRegular.ToString() == Nettype)
             return KeilyRegularNetMaker(NodeCount);
         if (NetType.TriangleRegular.ToString() == Nettype)
             return TriangleRegularNetMaker(NodeCount, 0);
         #endregion

         #region 2d irregular

         if (NetType.Keily.ToString() == Nettype)
             return KeilyNetMaker(NodeCount);
         if (NetType.MultyLink.ToString() == Nettype)
			 return MultyLinkNetMaker2(NodeCount, AddToMaxLinkCount);
         if (NetType.TriangleIrregular.ToString() == Nettype)
             return TriangleIrregularNetMaker(NodeCount);
         if (NetType.KeilyWithBackupChannel.ToString() == Nettype)
             return KeilyWithBackupChannelNetMaker(NodeCount);
         #endregion

         #region 3d regular
         if (NetType.Cube.ToString() == Nettype)
         {
             return CubeNetMaker(NodeCount);
         }
         if (NetType.EnhancedCube.ToString() == Nettype)
         {
             return EnhancedCubeNetMaker(NodeCount);
         }
         if (NetType.Hexagon3D.ToString() == Nettype)
         {
             return Hexagon3DNetMaker(NodeCount);
         }
         if (NetType.Net31223D.ToString() == Nettype)
         {
             return Net31223DNetMaker(NodeCount);
         }
         if (NetType.TriangleRegular3D.ToString() == Nettype)
         {
             return TriangleRegular3DNetMaker(NodeCount);
         }
         #endregion

         #region 3d irerregular

         if (NetType.Keily3D.ToString() == Nettype)
         {
             return Keily3DNetMaker(NodeCount);
         }
         if (NetType.TriangleIRegular3D.ToString() == Nettype)
         {
             return TriangleIregular3DNetMaker(NodeCount);
         }
         #endregion
         throw new Exception(@"Net type is undefined");
      }
      #endregion
      #endregion

      #region Private Method
      #region TriangleRegular3DNetMaker
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      private Node[] TriangleRegular3DNetMaker(int nodecount)
      {
         //Get list of of triangle net
         var net = new List<Node[]>();
         double s = Math.Pow(nodecount, (1.0 / 3.0));
         int size = (int)Math.Round(s);
         int linearsize = size * size;
         for (int i = 0; i < size; i++)
         {
            net.Add(TriangleRegularNetMaker(linearsize, i * linearsize));
         }

         //Connect part of nets vertical
         var ret = new List<Node>();
         for (int i = 0; i < net.Count - 1; i++)
         {
            for (int j = 0; j < net[i].Length; j++)
            {
               net[i][j].AddNearestNode(net[i][j].Id + linearsize);
               net[i + 1][j].AddNearestNode(net[i][j].Id);
            }
            ret.AddRange(net[i]);
         }
         ret.AddRange(net[net.Count - 1]);
         return ret.ToArray();
      }
      #endregion

      #region TriangleRegular3DNetMaker
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      private Node[] TriangleIregular3DNetMaker(int nodecount)
      {
         //Get list of of triangle net
         var net = new List<Node[]>();
         double s = Math.Pow(nodecount, (1.0 / 3.0));
         int size = (int)Math.Round(s);
         int linearsize = size * size;
         nodecount = linearsize;
         for (int i = 0; i < size; i++)
         {
            net.Add(TriangleIrregularNetMaker(nodecount, i * linearsize));
         }

         //Connect part of the net vertical
         var ret = new List<Node>();
         for (int i = 0; i < net.Count - 1; i++)
         {
            for (int j = 0; j < net[i].Length; j++)
            {
               net[i][j].AddNearestNode(net[i][j].Id + linearsize);
               net[i + 1][j].AddNearestNode(net[i][j].Id);
            }
            ret.AddRange(net[i]);
         }
         ret.AddRange(net[net.Count - 1]);
         return ret.ToArray();
      }
      #endregion
      #region 3d шестиугольник 3122
      #region Hexagon3DNetMaker()
      /// <summary>
      /// Создаем призму шестиугольную из шестеугольников
      /// </summary>
      /// <returns></returns>
      private Node[] Hexagon3DNetMaker(int nodecount)
      {
         int linecount = 8;//Магическая константа. НЕХОРОШО
         Node[][] temp = new Node[linecount][];
        nodecount = nodecount / linecount;
          var startIndex = 0;
         for (int i = 0; i < linecount; i++)
         {
             temp[i] = HexagonNetMaker(nodecount, startIndex);
             startIndex += temp[i].Length;
         }
         for (int i = 0; i < linecount - 1; i++)
         {
            for (int j = 0; j < temp[i].Length; j++)
            {
               temp[i][j].AddNearestNode(temp[i + 1][j].Id);
               temp[i + 1][j].AddNearestNode(temp[i][j].Id);
            }
         }
         Node[] ret = new Node[linecount * temp[0].Length];
         int currentnumber = 0;
         for (int i = 0; i < linecount; i++)
         {
            for (int j = 0; j < temp[i].Length; j++, currentnumber++)
            {
               ret[currentnumber] = temp[i][j];
            }
         }
         return ret;
      }
      #endregion

      #region Net31223DNetMaker()
      /// <summary>
      /// Создаем призму шестиугольную из шестеугольников
      /// </summary>
      /// <returns></returns>
      private Node[] Net31223DNetMaker(int nodecount)
      {
         Node[][] temp = new Node[10][];
         int linecount = 10;//Магическая константа. НЕХОРОШО
         nodecount = nodecount / linecount;
         for (int i = 0; i < linecount; i++)
         {
             temp[i] = Net3122Maker(nodecount, nodecount * i);
         }
         for (int i = 0; i < linecount - 1; i++)
         {
            for (int j = 0; j < temp[i].Length; j++)
            {
               temp[i][j].AddNearestNode(temp[i + 1][j].Id);
               temp[i + 1][j].AddNearestNode(temp[i][j].Id);
            }
         }
         Node[] ret = new Node[linecount * temp[0].Length];
         int currentnumber = 0;
         for (int i = 0; i < linecount; i++)
         {
            for (int j = 0; j < temp[i].Length; j++, currentnumber++)
            {
               ret[currentnumber] = temp[i][j];
            }
         }
         return ret;
      }
      #endregion
      #endregion

      #region CubeNetMaker()
      /// <summary>
      /// Генерирование кубической сети
      /// </summary>
      /// <returns>Сеть</returns>
      private Node[] CubeNetMaker(int nodecount)
      {
         List<Node[]> net = new List<Node[]>();
         double s = Math.Pow(nodecount, (1.0 / 3.0));
         int size = (int)Math.Round(s);

         int quadrosize = size * size;
         for (int i = 0; i < size; i++)
         {
            net.Add(GetLineFromQuadr(CreateQuadro(size, i * quadrosize)));
         }

         List<Node> ret = new List<Node>();
         for (int i = 0; i < net.Count - 1; i++)
         {
            for (int j = 0; j < net[i].Length; j++)
            {
               net[i][j].AddNearestNode(net[i][j].Id + quadrosize);
               net[i + 1][j].AddNearestNode(net[i][j].Id);
            }
            ret.AddRange(net[i]);
         }
         ret.AddRange(net[net.Count - 1]);
         return ret.ToArray();
      }
      #endregion

       #region EnhancedCubeNetMaker()

       /// <summary>
       ///     Генерирование кубической сети с заданным числом связей
       /// </summary>
       /// <returns>Сеть</returns>
      private Node[] EnhancedCubeNetMaker(int nodecount)
       {
           Node[] net = CubeNetMaker(nodecount);
           var r = new Random();

           var linkcount = net.Sum(node => node.NearestNodes.Length);
           var summlinkAdd = 0;
           while (linkcount/(double) net.Length < AddToMaxLinkCount)
           {
               int addLinkCount = (AddToMaxLinkCount*net.Length - linkcount)/2 + 1;
               summlinkAdd += addLinkCount;
               for (int i = 0; i < addLinkCount; i++)
               {
                   int a;
                   int b;
                   do
                   {
                       a = r.Next(net.Length);
                       b = r.Next(net.Length);
                   } while (a == b);

                   net[a].AddNearestNode(b);
                   net[b].AddNearestNode(a);
               }

               linkcount = net.Sum(node => node.NearestNodes.Length);
           }
           return net;
       }

       #endregion


       #region CentricCubeNetMaker()

       /// <summary>
       ///     Генерирование кубической сети с дополнительными узлами в центрах
       /// </summary>
       /// <returns>Сеть</returns>
       private Node[] CentricCubeNetMaker(int nodecount)
       {
           List<Node[]> net = new List<Node[]>();
           double s = Math.Pow(nodecount, (1.0 / 3.0));
           int size = (int)Math.Round(s);

           int quadrosize = size * size;
           for (int i = 0; i < size; i++)
           {
               net.Add(GetLineFromQuadr(CreateQuadro(size, i * quadrosize)));
           }
         

           List<Node> ret = new List<Node>();
           for (int i = 0; i < net.Count - 1; i++)
           {
               for (int j = 0; j < net[i].Length; j++)
               {
                   net[i][j].AddNearestNode(net[i][j].Id + quadrosize);
                   net[i + 1][j].AddNearestNode(net[i][j].Id);
               }
               ret.AddRange(net[i]);
           }
           ret.AddRange(net[net.Count - 1]);




           return ret.ToArray();
       }

       #endregion


      #region KeilyWithBackupChannelNetMaker()
      /// <summary>
      /// Мутант Сети Кейли. Создаем сначала сеть Кейли, а потом у некоторый узлов дублируем каналы связи. 
      /// В итоге сеть перестает быть древовидной, а становится графом.  
      /// </summary>
      /// <returns>Возвращет сеть в виде массива объектов типа Node</returns>
      private Node[] KeilyWithBackupChannelNetMaker(int nodecount)
      {
         if (Percentbuckupchanel > 1 || Percentbuckupchanel < 0)
            throw new DivideByZeroException("percentbuckupchanel имеет недопустимое значение либо менее нуля либо более единицы");
         //вначале получили сеть кейли.
         Node[] ret = KeilyNetMaker(nodecount);
         Random r = new Random(DateTime.Now.Millisecond);
         double buckuppercent = Percentbuckupchanel;
         //получили число узлов, которые будут дублироваться в связя
         int nodebuckupchanelcount = (int)(ret.Length * buckuppercent);
         for (int i = 0; i < nodebuckupchanelcount / 2; i++)
         {
            //первый узел от которого будет идти связь
            int first = r.Next(ret.Length);
            // второй узел от которого будет идти связь
            int second = r.Next(ret.Length);
            bool isgood = true;
            // проверка не являются ли это узлы уже соединенными
            for (int j = 0; j < ret[first].NearestNodes.Length; j++)
            {
               for (int k = 0; (k < ret[second].NearestNodes.Length) && (isgood); k++)
               {
                  if (ret[first].NearestNodes[j] == ret[second].NearestNodes[k])
                  {
                     isgood = false;
                     break;
                  }
               }
            }
            //если isgood стало в false значит связи уже существуют
            if (!isgood)
            {
               i--;
               continue;
            }
            //иначе добавляем связи между ними
            ret[first].AddNearestNode(second);
            ret[second].AddNearestNode(first);
         }
         return ret;
      }
      #endregion

      #region Keily3DNetMaker()
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      private Node[] Keily3DNetMaker(int nodecount)
      {
         //magic number 
         const int linecount = 20;
         var ret = new Node[nodecount];
         nodecount /= linecount;
         var list = new List<Node[]>();
         for (int i = 0; i < linecount; i++)
         {
             list.Add(KeilyNetMaker(nodecount, i * nodecount));
         }
         for (int i = 0; i < list.Count - 1; i++)
         {
            for (int j = 0; j < list[i].Length; j++)
            {
               list[i][j].AddNearestNode(list[i + 1][j].Id);
               list[i + 1][j].AddNearestNode(list[i][j].Id);
            }
         }
         int courrentposition = 0;
         foreach (var nodese in list)
         {
            foreach (var node in nodese)
            {
               ret[courrentposition] = node;
            }
            courrentposition++;
         }
         return ret;
      }
      #endregion

      #region KeilyNetMaker()
      /// <summary>
      ///Регулярная Кейли. Сети Кейли представляют собой деревья с 1 корнем и отходящими
      ///от него ветвями. Регулярная сеть Кейли отличается от не Регулярной тем, что число
      ///ветвей отходящих от корня фиксировано, в то время как у не регулярной сети Кейли
      ///оно варьируется от заданных входных минимальных значений, до полученного максимального
      ///значения. Максимальное получается путем сложения минумума и возможного добавочного
      ///значения, передаваемого функции. Сеть создается следующим образом: Создается массив
      ///узлов. Далее проходимся циклом по каждому узлу и в зависимости от указанных ему
      ///количества ссылок ставим ссылки на другие узлы. Узел на который ставтится ссылка,
      ///постоянно смещается пи каждом добавлении ссылкии. Такимо бразом создано дерево,
      ///но необходимо разобраться с листьями дерева, тк на них остались узлы, у которых
      ///количество ячеек для ссылок больше чем самих ссылок. Далее вызывается функция NormalizeNet,
      ///которая выравнивает массивы ссылок на другие узлы.
      /// </summary>
      /// <returns></returns>
      private Node[] KeilyNetMaker(int nodecount, int startnodeid = 0)
      {
         var newuzel = new Node[nodecount];
         var maxlinkcounets = new int[nodecount];
         var current = 1;
         var start = 0;
         CreateNodes(nodecount, ref newuzel, ref maxlinkcounets, startnodeid);
         while (current != nodecount)
         {
            while ((maxlinkcounets[start] != 0) && (current != nodecount))
            {
               newuzel[start].NearestNodes[maxlinkcounets[start] - 1] = current;
               maxlinkcounets[start] -= 1;
               newuzel[current].NearestNodes[maxlinkcounets[current] - 1] = newuzel[start].Id;
               maxlinkcounets[current] -= 1;
               current += 1;
            }
            start += 1;
         }
         NormalizeNet(ref newuzel);
         return newuzel;
      }
      #endregion

      #region MultyLinkNetMaker()
      /// <summary>
      /// Случайная сеть с множеством путей между узлами. Создается массив узлов,
      /// а далее уже выбираются случайным образом 2 узла между которыми установлена связь. 
      /// После чего из массива числа ссылок для каждого узла у обоих связанных только,
      ///  что узлов будет вычтено по 1, так как узлы уже были связаны. Так продолжается 
      /// пока узлы не будут связаны. Что бы сеть не была разомкнутой, существует принудительное
      ///  связывание в линию всех узлов предварительное.
      /// </summary>
      /// <returns></returns>
      private Node[] MultyLinkNetMaker(int nodecount)
      {
         Node[] newuzel = new Node[nodecount];
         int[] linkcounets = new int[nodecount];
         int[] maxlinkcounets = new int[nodecount];
         CreateNodes(nodecount, ref newuzel, ref maxlinkcounets);
         //double a = 0;
         //for (int i = 0; i < maxlinkcounets.Length; i++)
         //{
         //    a += maxlinkcounets[i];
         //}
         //a = a/1000000;
         //Console.WriteLine(a);
         //связали всю сеть в линию
         for (var q = 0; q < nodecount - 1; q++)
         {
            newuzel[q].NearestNodes[0] = q + 1;
            newuzel[q + 1].NearestNodes[0] = q;
            linkcounets[q] += 1;
            linkcounets[q + 1] += 1;
         }
         Random r = new Random();
         for (var q = 0; q < nodecount - 1; q++)
         {
            for (var w = 0; w < newuzel[q].NearestNodes.Length; w++)
            {
               var temp = r.Next(nodecount);
               if (!(linkcounets[q] < maxlinkcounets[q]))
                  break;
               if (!(linkcounets[temp] < maxlinkcounets[temp]))
                  continue;
               newuzel[q].NearestNodes[linkcounets[q]] = newuzel[temp].Id;
               newuzel[temp].NearestNodes[linkcounets[temp]] = newuzel[q].Id;
               linkcounets[q] += 1;
               linkcounets[temp] += 1;
            }
         }
         NormalizeNet(ref newuzel);
         return newuzel;
      }


	  private Node[] MultyLinkNetMaker2(int nodecount, int linkCount)
	  {
		  Node[] newuzel = new Node[nodecount];
		  for (int i = 0; i < nodecount; i++)
		  {
			  newuzel[i] = new Node(i) { IsActive = true, };
		  }
		  var r = new Random();
		  //связали всю сеть в линию + добавили узлы
		  for (var q = 0; q < nodecount - 1; q++)
		  {
			  //newuzel[q].AddNearestNode(q + 1);
			  //newuzel[q + 1].AddNearestNode(q);
			  var add = r.NextGaussian(linkCount, linkCount*0.6);
			  add = Math.Round(add);
			  
			  if (Math.Round(add)<1)
			  {
				  add = 1;
			  }
			  
			  for (int i = 0; i < add; i++)
			  {
				  var temp= r.Next(nodecount);
				  newuzel[q].AddNearestNode(temp);
				  newuzel[temp].AddNearestNode(q);
			  }
			  
		  }
		return newuzel;
	  }
      #endregion

      #region QuadroNetMaker()

       /// <summary>
       /// Квадратная сеть строится проще всех. Создается Массив двумерный.
       /// Затем ставятся связи между узлами квадратной решетки.
       /// После чего процедура GetLineFromQuadro возвращает уже готовый одномерный массив объектов типа Node
       /// </summary>
       /// <param name="Nodecount"></param>
       /// <returns>Сеть</returns>
       private Node[] QuadroNetMaker(int nodecount)
      {
         return GetLineFromQuadr(CreateQuadro((int)Math.Sqrt(nodecount)));
      }
      #endregion

      #region KeilyRegularNetMaker()
      /// <summary>
      /// Не Регулярная Келий. Сети Кэйли представляют собой деревья с 1 корнем и отходящими от него ветвями.
      /// Регулярная сеть Кэйли отличается от не регулярной тем, что число ветвей отходящих от корня фиксировано,
      /// в то время как у не регулярной сети Кэйли оно варьируется от заданных входных минимальных значений,
      /// до полученного максимального значения. Максимальное получается путем сложения минимума и возможного 
      /// добавочного значения, передаваемого функции. 
      /// Сеть создается следующим образом: Создается массив узлов. Далее проходимся циклом по каждому узлу 
      /// и в зависимости от указанных ему количества ссылок ставим ссылки на другие узлы. 
      /// Узел на который ставится ссылка, постоянно смещается пи каждом добавлении ссылки. 
      /// Таким образом создано дерево, но необходимо разобраться с листьями дерева, тк на них остались узлы, 
      /// у которых количество ячеек для ссылок больше чем самих ссылок. Далее вызывается функция NormalizeNet,
      ///  которая выравнивает массивы ссылок на другие узлы.
      /// </summary>
      /// <returns></returns>
       private Node[] KeilyRegularNetMaker(int nodecount)
      {
         var newuzel = new Node[nodecount];
         var maxlinkcounets = new int[nodecount];
         var current = 1;
         var start = 0;
         CreateNodesToKeilyRegularNetMaker(nodecount, ref newuzel, ref maxlinkcounets, AddToMaxLinkCount);
         while (current != nodecount)
         {
            while ((maxlinkcounets[start] != 0) && (current != nodecount))
            {
               newuzel[start].NearestNodes[maxlinkcounets[start] - 1] = current;
               maxlinkcounets[start] -= 1;
               newuzel[current].NearestNodes[maxlinkcounets[current] - 1] = newuzel[start].Id;
               maxlinkcounets[current] -= 1;
               current += 1;
            }
            start += 1;
         }
         NormalizeNet(ref newuzel);
         return newuzel;
      }
      #endregion

      #region TriangleIrregularNetMaker()
      /// <summary>
      ///Треугольная иррегулярная сеть строится следующим образом: Создается Список с узлами
      ///первого треугольника. Ставятся в другом уже массиве круговые ссылки друг на друга.
      ///Затем в этот список добавляется новый узел и случайным образом выбираются 2 узла,
      ///которые уже находятся в списке в качестве соседей. На них в массиве ссылок ставиятся
      ///ссылки. Так продолжается пока в сети не окажется указанное в параметре входном число
      ///узлов.
      /// </summary>
      /// <returns></returns>
      private Node[] TriangleIrregularNetMaker(int nodecount, int startnodeid = 0)
      {
          var newuzel = new Node[nodecount];
         newuzel[0] = new Node(startnodeid + 0);
         newuzel[1] = new Node(startnodeid + 1);
         newuzel[2] = new Node(startnodeid + 2);
         var l = new List<int>[nodecount];
         for (var i = 0; i < nodecount; i++)
            l[i] = new List<int>();
         l[0].Add(startnodeid + 1);
         l[0].Add(startnodeid + 2);
         l[1].Add(startnodeid + 0);
         l[1].Add(startnodeid + 2);
         l[2].Add(startnodeid + 1);
         l[2].Add(startnodeid + 0);
         var r = new Random();
         var currentmakedelement = 3;
         for (var i = 3; i < nodecount; i++)
         {
            newuzel[i] = new Node(startnodeid + i);
            int temp = startnodeid + r.Next(currentmakedelement);
            l[temp].Add(startnodeid + i);
            l[i].Add(temp);
            currentmakedelement += 1;
         }
         for (var i = 0; i < nodecount; i++)
            newuzel[i].NearestNodes = l[i].ToArray();
         return newuzel;
      }
      #endregion

      #region TriangleRegularNetMaker()
      /// <summary>
      /// Треугольная регулярная сеть строится так же как и квадратная, за исключением дополнительного шага. 
      /// Создается Массив двумерный. Затем ставятся связи между узлами квадратной решетки.
      /// Затем ставятся дополнительные ссылки, делающие из квадратной сети, регулярную треугольную.
      /// После чего процедура GetLineFromQuadro возвращает уже готовый одномерный массив объектов типа Node.
      /// </summary>
      /// <returns></returns>
      private Node[] TriangleRegularNetMaker(int nodecount, int startid)
      {
         int size = (int)Math.Sqrt(nodecount);
         Node[,] newnet = CreateQuadro(size, startid);
         //for (var i = 1; i < newnet.GetLength(0) - 1; )
         //{
         //   int t;
         //   if (newnet.GetLength(0) % 2 == 0)
         //   {
         //      t = newnet.GetLength(1);
         //   }
         //   else
         //   {
         //      t = newnet.GetLength(0) - 1;
         //   }
         //   for (var j = 0; j < t - 1; j++)
         //   {
         //      newnet[i, j].AddNearestNode(newnet[i + 1, j + 1].Id);
         //      newnet[i, j].AddNearestNode(newnet[i - 1, j + 1].Id);
         //      newnet[i + 1, j + 1].AddNearestNode(newnet[i, j].Id);
         //      newnet[i - 1, j + 1].AddNearestNode(newnet[i, j].Id);
         //   }
         //   if (t != 0)
         //   {
         //      var j = newnet.GetLength(1) - 2;
         //      newnet[i, j].AddNearestNode(newnet[i - 1, j + 1].Id);
         //      newnet[i - 1, j + 1].AddNearestNode(newnet[i, j].Id);
         //   }
         //   i += 2;
         //}
          for (var i = 0; i < size - 1; i++)
          {
              for (var j = 0; j < size - 1; j++)
              {
                  newnet[i, j].AddNearestNode(newnet[i + 1, j + 1].Id);
                  newnet[i + 1, j + 1].AddNearestNode(newnet[i, j].Id);
              }

          }


          var q = GetLineFromQuadr(newnet);
         return q;
      }
      #endregion

      #region Шестиугольник 3122
      #region HexagonNetMaker()
      /// <summary>
      ///   Шестиугольная TODO сделать рисунок.
      /// </summary>
      /// <returns>Сеть</returns>
      private Node[] HexagonNetMaker(int nodecount, int startID = 0)
      {
         var n = 0;//количество частей по оси Y
         var k = 0;//количество частей по оси X
         for (var ik = 100; ik < nodecount / 100; ik++)
         {
            int t = nodecount / (4 * ik + 2);
            if (t * (4 * ik + 2) <= n * k)
               continue;
            n = t;
            k = ik;
            if (t * (4 * ik + 2) == nodecount)
            {
               ik++;
               break;
            }
         }
          if ((k==0)&&(n==0))
              throw new Exception(string.Format("Nodecount too small startID {0}, nodecount {1}", startID, nodecount));
         var ret = new Node[n, k][];
         var startid = startID;
         var finishid = startid + 6;
         //генерируем части для последующего соединения
         //идем по оси Y
         for (var w = 0; w < k; w++)
         {
            //для первого ряда по оси X
            if (w == 0)
            {
               for (var q = 0; q < n; q++)
               {
                  var temp = new Node[6];
                  for (var i = 0; i < 6; i++)
                  {
                     temp[i] = new Node(startid + i)
                     {
                        NearestNodes = new int[0]
                     };
                  }
                  //ставим ссылки по кругу от начального ID до конечного
                  for (var i = 0; i < 4; i++)
                  {
                     temp[i].AddNearestNode(temp[i + 1].Id);
                     temp[i + 1].AddNearestNode(temp[i].Id);
                  }
                  //верхние замыкания
                  temp[5].AddNearestNode(temp[0].Id);
                  temp[0].AddNearestNode(temp[5].Id);

                  startid = finishid;
                  if (q != n - 1)
                     finishid = startid + 6;
                  else
                     finishid = startid + 4;
                  ret[q, w] = temp;
               }
            }
            //для НЕ первого ряда по оси X
            else
            {
               for (var q = 0; q < n; q++)
               {
                  var temp = new Node[4];

                  for (var i = 0; i < 4; i++)
                  {
                     temp[i] = new Node(startid + i)
                     {
                        NearestNodes = new int[0]
                     };
                  }
                  //ставим ссылки по кругу от начального ID до конечного
                  for (var i = 0; i < 3; i++)
                  {
                     temp[i].AddNearestNode(temp[i + 1].Id);
                     temp[i + 1].AddNearestNode(temp[i].Id);
                  }

                  startid = finishid;
                  finishid = startid + 4;
                  ret[q, w] = temp;
               }
            }
         }
         //соединяем части
         //идем по Y
         for (var i = 0; i < k; i++)
         {
            //взяли столбцы и замкнули их между собой
            //идем по оси X
            for (var index = 0; index < n - 1; index++)
            {
               //если это первый ряд
               if (i == 0)
               {
                  ret[index, i][1].AddNearestNode(ret[index + 1, i][4].Id);
                  ret[index + 1, i][4].AddNearestNode(ret[index, i][1].Id);
               }
               //если это не первый ряд
               else
               {
                  ret[index, i][0].AddNearestNode(ret[index + 1, i][3].Id);
                  ret[index + 1, i][3].AddNearestNode(ret[index, i][0].Id);
               }
            }
         }
         //связываем горизонтальные
         for (var i = 0; i < k - 1; i++)
         {
            for (var index = 0; index < n; index++)
            {
               if (index == 0)
               {
                  ret[index, i][2].AddNearestNode(ret[index, i + 1][0].Id);

                  ret[index, i + 1][0].AddNearestNode(ret[index, i][2].Id);

                  ret[index, i][3].AddNearestNode(ret[index, i + 1][3].Id);

                  ret[index, i + 1][3].AddNearestNode(ret[index, i][3].Id);
               }
               else
               {
                  ret[index, i][1].AddNearestNode(ret[index, i + 1][0].Id);
                  ret[index, i + 1][0].AddNearestNode(ret[index, i][1].Id);

                  ret[index, i][2].AddNearestNode(ret[index, i + 1][3].Id);
                  ret[index, i + 1][3].AddNearestNode(ret[index, i][2].Id);
               }
            }
         }
         List<Node> tq = new List<Node>();
         for (var i = 0; i < k; i++)
            for (var index = 0; index < n; index++)
               for (var a = 0; a < ret[index, i].Length; a++)
                  tq.Add(ret[index, i][a]);

         tq = tq.OrderBy(u => u.Id).ToList();
         return tq.ToArray();
      }
      #endregion

      #region Net3122Maker()
      /// <summary>
      /// 3 12^2 Строится аналогично шестиугольной, только с другими параметрами уравнения
      /// и номеров узлов, на которые ставятся ссылки.
      /// </summary>
      /// <returns>Сеть</returns>
      private Node[] Net3122Maker(int nodecount, int startId = 0)
      {
         var n = 0;//количество частей по оси Y
         var k = 0;//количество частей по оси X
         for (var ik = 100; ik < nodecount / 100; ik++)
         {
            int t = nodecount / (12 * ik + 2);
            if (t * (12 * ik + 2) <= n * k)
               continue;
            n = t;
            k = ik;
            if (t * (12 * ik + 2) == nodecount)
            {
               ik++;
               break;
            }
         }

         var ret = new Node[n, k][];
         var startid = startId;
         var finishid = startid + 14;
         //генерируем части для последующего соединения
         //идем по оси Y
         for (var w = 0; w < k; w++)
         {
            //для первого ряда по оси X
            if (w == 0)
            {
               for (var q = 0; q < n; q++)
               {
                  var temp = new Node[14];
                  for (var i = 0; i < 14; i++)
                  {
                     temp[i] = new Node(startid + i)
                     {
                        NearestNodes = new int[0]
                     };
                  }
                  //ставим ссылки по кругу от начального ID до конечного
                  for (var i = 0; i < 11; i++)
                  {
                     temp[i].AddNearestNode(temp[i + 1].Id);
                     temp[i + 1].AddNearestNode(temp[i].Id);
                  }
                  //верхние замыкания
                  temp[11].AddNearestNode(temp[0].Id);
                  temp[0].AddNearestNode(temp[11].Id);

                  //боковые левые
                  temp[9].AddNearestNode(temp[13].Id);
                  temp[13].AddNearestNode(temp[9].Id);
                  temp[8].AddNearestNode(temp[13].Id);
                  temp[13].AddNearestNode(temp[8].Id);
                  //боковые правые
                  temp[2].AddNearestNode(temp[12].Id);
                  temp[12].AddNearestNode(temp[2].Id);
                  temp[3].AddNearestNode(temp[12].Id);
                  temp[12].AddNearestNode(temp[3].Id);

                  startid = finishid;
                  if (q != n - 1)
                     finishid = startid + 14;
                  else
                     finishid = startid + 12;
                  ret[q, w] = temp;
               }
            }
            //для НЕ первого ряда по оси X
            else
            {
               for (var q = 0; q < n; q++)
               {
                  var temp = new Node[12];

                  for (var i = 0; i < 12; i++)
                  {
                     temp[i] = new Node(startid + i)
                     {
                        NearestNodes = new int[0]
                     };
                  }
                  //ставим ссылки по кругу от начального ID до конечного
                  for (var i = 0; i < 9; i++)
                  {
                     temp[i].AddNearestNode(temp[i + 1].Id);
                     temp[i + 1].AddNearestNode(temp[i].Id);
                  }
                  //боковые левые
                  temp[8].AddNearestNode(temp[11].Id);
                  temp[11].AddNearestNode(temp[8].Id);
                  temp[7].AddNearestNode(temp[11].Id);
                  temp[11].AddNearestNode(temp[7].Id);
                  //боковые правые
                  temp[2].AddNearestNode(temp[10].Id);
                  temp[10].AddNearestNode(temp[2].Id);
                  temp[1].AddNearestNode(temp[10].Id);
                  temp[10].AddNearestNode(temp[1].Id);

                  startid = finishid;
                  finishid = startid + 12;
                  ret[q, w] = temp;
               }
            }
         }
         //соединяем части
         //идем по Y
         for (var i = 0; i < k; i++)
         {
            //взяли столбцы и замкнули их между собой
            //идем по оси X
            for (var index = 0; index < n - 1; index++)
            {
               //если это первый ряд
               if (i == 0)
               {
                  ret[index, i][12].AddNearestNode(ret[index + 1, i][13].Id);
                  ret[index + 1, i][13].AddNearestNode(ret[index, i][12].Id);
               }
               //если это не первый ряд
               else
               {
                  ret[index, i][10].AddNearestNode(ret[index + 1, i][11].Id);
                  ret[index + 1, i][11].AddNearestNode(ret[index, i][10].Id);
               }
            }
         }

         //связываем горизонтальные
         for (var i = 0; i < k - 1; i++)
         {
            for (var index = 0; index < n; index++)
            {
               if (index == 0)
               {
                  ret[index, i][5].AddNearestNode(ret[index, i + 1][0].Id);

                  ret[index, i + 1][0].AddNearestNode(ret[index, i][5].Id);

                  ret[index, i][6].AddNearestNode(ret[index, i + 1][9].Id);

                  ret[index, i + 1][9].AddNearestNode(ret[index, i][6].Id);
               }
               else
               {
                  ret[index, i][4].AddNearestNode(ret[index, i + 1][0].Id);
                  ret[index, i + 1][0].AddNearestNode(ret[index, i][4].Id);

                  ret[index, i][5].AddNearestNode(ret[index, i + 1][9].Id);
                  ret[index, i + 1][9].AddNearestNode(ret[index, i][5].Id);
               }
            }
         }
         var tq = new List<Node>();
         for (var i = 0; i < k; i++)
         {
            for (var index = 0; index < n; index++)
            {
               for (var a = 0; a < ret[index, i].Length; a++)
               {
                  tq.Add(ret[index, i][a]);
               }
            }
         }
         tq = tq.OrderBy(u => u.Id).ToList();
         return tq.ToArray();
      }
      #endregion
      #endregion

      #region Вспомогательные методы

      #region CreateQuadro()
      /// <summary>
      ///  создание квадратной сети
      /// </summary>
      /// <returns>Квадратной сети в массиве</returns>
      private Node[,] CreateQuadro(int Size, int startId = 0)
      {
         int currnum = startId;
         int size = Size;
         Node[,] newnet = new Node[size, size];
         for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++, currnum++)
               newnet[i, j] = new Node(currnum);

         for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
               //первая строка
               if (i == 0)
               {
                  //первый столбец
                  if (j == 0)
                  {
                     newnet[i, j].NearestNodes = new int[2];
                     newnet[i, j].NearestNodes[0] = newnet[i + 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j + 1].Id;
                  }
                  //последний столбец
                  else if (j == size - 1)
                  {
                     newnet[i, j].NearestNodes = new int[2];
                     newnet[i, j].NearestNodes[0] = newnet[i + 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j - 1].Id;
                  }
                  else
                  {
                     newnet[i, j].NearestNodes = new int[3];
                     newnet[i, j].NearestNodes[0] = newnet[i, j + 1].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i + 1, j].Id;
                     newnet[i, j].NearestNodes[2] = newnet[i, j - 1].Id;
                  }
               }
               //последняя строка
               else if (i == size - 1)
               {
                  //первый столбец
                  if (j == 0)
                  {
                     newnet[i, j].NearestNodes = new int[2];
                     newnet[i, j].NearestNodes[0] = newnet[i - 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j + 1].Id;
                  }
                  //последний столбец
                  else if (j == size - 1)
                  {
                     newnet[i, j].NearestNodes = new int[2];
                     newnet[i, j].NearestNodes[0] = newnet[i - 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j - 1].Id;
                  }
                  else
                  {
                     newnet[i, j].NearestNodes = new int[3];
                     newnet[i, j].NearestNodes[0] = newnet[i, j + 1].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i - 1, j].Id;
                     newnet[i, j].NearestNodes[2] = newnet[i, j - 1].Id;
                  }
               }

               else
               {
                  //первый столбец
                  if (j == 0)
                  {
                     newnet[i, j].NearestNodes = new int[3];
                     newnet[i, j].NearestNodes[0] = newnet[i + 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j + 1].Id;
                     newnet[i, j].NearestNodes[2] = newnet[i - 1, j].Id;
                  }
                  //последний столбец
                  else if (j == size - 1)
                  {
                     newnet[i, j].NearestNodes = new int[3];
                     newnet[i, j].NearestNodes[0] = newnet[i + 1, j].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i, j - 1].Id;
                     newnet[i, j].NearestNodes[2] = newnet[i - 1, j].Id;
                  }
                  else
                  {
                     newnet[i, j].NearestNodes = new int[4];
                     newnet[i, j].NearestNodes[0] = newnet[i, j + 1].Id;
                     newnet[i, j].NearestNodes[1] = newnet[i - 1, j].Id;
                     newnet[i, j].NearestNodes[2] = newnet[i, j - 1].Id;
                     newnet[i, j].NearestNodes[3] = newnet[i + 1, j].Id;
                  }
               }
            }
         return newnet;
      }
      #endregion

      #region CreateNodes(ref Node[] NewNodes, ref int[] addtomaxlinkcounets)

      /// <summary>
      /// вызывается  только не из KeilyRegularNetMaker
      /// </summary>
      /// <param name="newNodes">массив узлов сети</param>
      /// <param name="addtomaxlinkcounets">массив количества ссылок для каждого элемента массива</param>
      /// <param name="startnodeid"></param>
      private void CreateNodes(int nodecount, ref Node[] newNodes, ref int[] addtomaxlinkcounets, int startnodeid = 0)
      {
         var r = new Random(DateTime.Now.Millisecond);
         int currentstartnodeid = startnodeid;
         for (var q = 0; q < nodecount; q++, currentstartnodeid++)
         {
            newNodes[q] = new Node(currentstartnodeid)
            {
               NearestNodes = new int[Minlink + r.Next(AddToMaxLinkCount)]
            };
            addtomaxlinkcounets[q] = newNodes[q].NearestNodes.Length;
            for (var t = 0; t < newNodes[q].NearestNodes.Length; t++)
               newNodes[q].NearestNodes[t] = -1;
         }
      }
      #endregion

      #region CreateNodesToKeilyRegularNetMaker
      /// <summary>
      /// Создать массив узлов. Вызывается только из KeilyRegularNetMaker
      /// </summary>
      /// <param name="newNodes">массив узлов сети</param>
      /// <param name="maxlinkcounets">массив количества ссылок для каждого элемента массива</param>
      /// <param name="nearestnodecount">количество ближайших узлов</param>
      private void CreateNodesToKeilyRegularNetMaker(int nodecount, ref Node[] newNodes, ref int[] maxlinkcounets, int nearestnodecount)
      {
         for (var q = 0; q < nodecount; q++)
         {
            newNodes[q] = new Node(q)
            {
               NearestNodes = new int[nearestnodecount]
            };
            maxlinkcounets[q] = nearestnodecount;
            for (var t = 0; t < newNodes[q].NearestNodes.Length; t++)
               newNodes[q].NearestNodes[t] = -1;
         }
      }
      #endregion

      #region NormalizeNet(ref Node[] newNodes)
      /// <summary>
      /// если у узла после всей обработки остались в массиве соседей узлы со значением id=-1 то мы их убираем.
      /// это и есть нормализация
      /// </summary>
      /// <param name="newNodes">нормализуемая сеть</param>
      private void NormalizeNet(ref Node[] newNodes)
      {
         for (var t = 0; t < newNodes.Length; t++)
         {
            var temp = newNodes[t].NearestNodes.Length;
            temp = newNodes[t].NearestNodes.Where(t1 => t1 == -1).Aggregate(temp, (current, t1) => current - 1);
            if (temp == newNodes[t].NearestNodes.Length)
            {
               continue;
            }
            var tq = new int[temp];
            var i = 0;
            for (var q = 0; q < newNodes[t].NearestNodes.Length; q++)
            {
               if (newNodes[t].NearestNodes[q] != -1)
               {
                  tq[i] = newNodes[t].NearestNodes[q];
                  i += 1;
               }
            }
            newNodes[t].NearestNodes = tq;
         }
      }
      #endregion

      #region GetLineFromQuadr(Node[,] newnet)
      /// <summary>
      /// Получить из квадратной сети массив
      /// </summary>
      /// <param name="newnet">Сеть текущая, из которой будет получина квадратная</param>
      /// <returns>новая сеть физически квадратной топологии</returns>
      private Node[] GetLineFromQuadr(Node[,] newnet)
      {
         int size = newnet.GetLength(0);
         var currentnet = new Node[size * size];
         for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
               currentnet[i * size + j] = newnet[i, j];
         return currentnet;
      }
      #endregion

      #endregion
      #endregion

      #endregion
   }
}
