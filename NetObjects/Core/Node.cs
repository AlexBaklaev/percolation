using System;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using NetObjects.Enums;

namespace NetObjects.Core
{
   /// <summary>
   /// Узел в сети. Базавый элемент над которым проводятся все операции
   /// Основой предметной области являются узлы вычислительной сети, 
   /// в программе представленные классом Node и содержащие в себе 
   /// необходимый набор свойств и методом для работы с ними. 
   /// Node содержит 4 байтовый идентификатор, интерпретируемый как ipv4,
   /// состояние выключен ли узел или включен, заражен ли узел или не заражен,
   /// может ли узел заражать другие узлы или он находится в пассивном 
   /// состоянии, список узлов соседей ,с которыми есть непосредственная
   ///  физическая связь и некоторые дополнительные методы. 
   /// </summary>
   [Serializable()]
   public class Node
   {
      #region Fields
      #region Main Fields
      /// <summary>
      /// Уникальный идентификатор узла
      /// </summary>
      [DebuggerHiddenAttribute]
      public int Id
      {
         get;
         set;
      }
      /// <summary>
      /// массив идентифификаторов блжайших узлов
      /// </summary>
      [DebuggerHiddenAttribute]
      public int[] NearestNodes
      {
         get;
         set;
      }
      /// <summary>
      /// состояние узла. выключен узел или включен.
      /// </summary>
      [DebuggerHiddenAttribute]
      public bool IsActive
      {
         get;
         set;
      }
      /// <summary>
      /// состояние узла. заражен узел или нет
      /// </summary>
      [DebuggerHiddenAttribute]
      public bool IsInfected
      {
         get;
         set;
      }
      /// <summary>
      /// был ли узел посещён при обходе.
      /// </summary>
      [DebuggerHiddenAttribute]
      public bool IsVisited
      {
          get;
          set;
      }
      #endregion

      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      public double InfectionProbability
      {
         get;
         set;
      }

      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      public long MaxCountOfVirusSendingForThisNode
      {
         get;
         set;
      }
      /// <summary>
      /// 
      /// </summary>
      private Random _random;
      /// <summary>
      /// 
      /// </summary>
      public int[] NearestNodesTraffic { get; set; }


      #region Second
      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      public bool MayContinueInfect
      {
         get;
         set;
      }
      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      public SubSegment[] InfectRange
      {
         get;
         set;
      }
      /// <summary>
      /// 
      /// </summary>
      public SubSegment MySegment
      {
         get;
         set;
      }
      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      public bool[] MayInfectSubsegment
      {
         get;
         set;
      }
      #endregion
      #endregion

      #region Constructors
      #region private Node()
      /// <summary>
      /// 
      /// </summary>
      [DebuggerHiddenAttribute]
      private Node()
      {
         _random = new Random(DateTime.Now.Millisecond);
      }
      #endregion

      #region public Node(int id, double probabilityOfInfection = 0.005)
      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="id">Node Id. int is 4 byte type, and idv4 is 4 byte.</param>
      /// <param name="probabilityOfInfection">InfectionProbability</param>
      [DebuggerHiddenAttribute]
      public Node(int id, double probabilityOfInfection = 0.005)
      {
         Id = id;
         InfectionProbability = probabilityOfInfection;
         _random = new Random(DateTime.Now.Millisecond);
         NearestNodes = new int[0];
      }
      #endregion
      #endregion

    
      #region AddNearestNode
      /// <summary> 
      /// Add one more connectiong node to this node 
      /// </summary>
      /// <param name="id">Node Id, which will be connecting with this node</param>
      [DebuggerHiddenAttribute]
      public void AddNearestNode(int id)
      {
         var tempmass = NearestNodes;
         var z = NearestNodes.Length + 1;
         NearestNodes = new int[z];
         for (var i = 0; i < tempmass.Length; i++)
            NearestNodes[i] = tempmass[i];
         NearestNodes[z - 1] = id;
      }

      #endregion

      #region AddNearestNode
      /// <summary> 
      /// 
      /// </summary>
      /// <param name="id">Node Id, which will be connecting with this node</param>
      //[DebuggerHiddenAttribute]
      public void RemoveNearestNode(int id)
      {
        
         var newmass = NearestNodes.Where((t, i) => t != id).ToArray();
         NearestNodes = newmass;
      }
      #endregion



   }
}
