using System;
using System.Collections.Generic;

namespace NetObjects.Core
{
   /// <summary>
   /// Border of net segment and methods for split the net 
   /// </summary>
   public class SubSegment
   {
      #region Properties
      /// <summary>
      /// start segment id 
      /// </summary>
      public int Startsegment
      {
         get;
         set;
      }
      /// <summary>
      /// final segment id 
      /// </summary>
      public int Endsegment
      {
         get;
         set;
      }
      #endregion

      #region Methods

      #region GetSubSegmentSQRT
      /// <summary>
      ///  Метод деления адресного пространства на подпространства размеров корень квадратный из размера пространства искомого 
      /// /// </summary>
      /// <param name="currentsubsegment">Сегмент для деления</param>
      /// <param name="granica">Минимальное число сегментов, ниже которого не дробить</param>
      /// <returns>
      /// Массив подсегментов
      /// </returns>
      public static SubSegment[] GetSubSegmentSQRT(SubSegment currentsubsegment, int granica)
      {
         var segmentlenth = currentsubsegment.Endsegment - currentsubsegment.Startsegment + 1;
         //если менее 100 узлов(магическое число сказаное научным руководителем)
         //то далее сегмент мы не делим
         if (segmentlenth <= granica)
            return new[] { currentsubsegment };
         var ret = new List<SubSegment>();
         //длинна будущих сегментов, кроме возможно последнего
         var newsegmentslenth = (int)Math.Sqrt(segmentlenth);
         for (var start = currentsubsegment.Startsegment; (start + newsegmentslenth) < currentsubsegment.Endsegment; start += newsegmentslenth)
         {
            //если еще есть целый кусок, то добавляем его и так продолжаем пока такие куски есть
            ret.Add(new SubSegment
            {
               Endsegment = start + newsegmentslenth - 1,
               Startsegment = start
            });
         }
         //как только целый частей не остается то добавляем остаток. 
         //он будет меньше сегмента но с ним ведь точно также можно работать
         if (ret[ret.Count - 1].Endsegment != currentsubsegment.Endsegment)
            ret.Add(new SubSegment
            {
               Endsegment = currentsubsegment.Endsegment,
               Startsegment = ret[ret.Count - 1].Endsegment + 1
            });
         return ret.ToArray();
      }
      #endregion

      #region GetSubSegmentBinary
      /// <summary>
      /// Метод деления адресного пространства попалам
      /// </summary>
      /// <param name="currentsubsegment">Сегмент для деления</param>
      /// <returns>Сегменты полученные в результате деления</returns>
      public static SubSegment[] GetSubSegmentBinary(SubSegment currentsubsegment)
      {
         return GetSubSegmentPart(currentsubsegment, 0.5);
         //int currentSegmentLength = currentsubsegment.Endsegment - currentsubsegment.Startsegment + 1;
         //if (currentSegmentLength <= 2)
         //   return new[] { currentsubsegment };
         //var ret = new SubSegment[2];
         //ret[0] = new SubSegment
         //{
         //   Startsegment = currentsubsegment.Startsegment,
         //   Endsegment = currentsubsegment.Startsegment + currentSegmentLength / 2
         //};
         //ret[1] = new SubSegment
         //{
         //   Startsegment = ret[0].Endsegment + 1,
         //   Endsegment = currentsubsegment.Endsegment
         //};
         //return ret;
      }
      #endregion

      #region GetSubSegmentGoldedSection
      /// <summary>
      /// Метод деления адресного пространства в соотношении золотого сечения
      /// </summary>
      /// <param name="currentsubsegment">Сегмент для деления</param>
      /// <returns>Сегменты полученные в результате деления</returns>
      public static SubSegment[] GetSubSegmentGoldedSection(SubSegment currentsubsegment)
      {
         const double x = 0.381966011;
         return GetSubSegmentPart(currentsubsegment, x);
         //int currentSegmentLength = currentsubsegment.Endsegment - currentsubsegment.Startsegment + 1;
         //if (currentSegmentLength <= 3)
         //   return new[] { currentsubsegment };
         //var ret = new SubSegment[2];
         //ret[0] = new SubSegment
         //{
         //   Startsegment = currentsubsegment.Startsegment,
         //   Endsegment = currentsubsegment.Startsegment + (int)(currentSegmentLength * x)
         //};
         //ret[1] = new SubSegment
         //{
         //   Startsegment = ret[0].Endsegment + 1,
         //   Endsegment = currentsubsegment.Endsegment
         //};
         //return ret;
      }
      #endregion

      #region GetSubSegmentLoadBalance
      /// <summary>
      /// Метод деления адресного пространства LoadBalance
      /// </summary>
      /// <param name="granica">Минимальное число сегментов, ниже которого не дробить</param>
      /// <param name="segmentcount">Число сегментов, на которое дробить</param>
      /// <param name="currentsubsegment">Сегмент для деления</param>
      /// <returns>Сегменты полученные в результате деления</returns>
      public static SubSegment[] GetSubSegmentLoadBalance(SubSegment currentsubsegment, int granica, int segmentcount)
      {
         int currentSegmentLength = currentsubsegment.Endsegment - currentsubsegment.Startsegment + 1;
         if (currentSegmentLength <= granica)
            return new[]
				       	{
				       		currentsubsegment
				       	};
         var ret = new List<SubSegment>();
         currentSegmentLength /= segmentcount;
         for (int i = currentsubsegment.Startsegment; i < currentsubsegment.Endsegment; i += currentSegmentLength)
         {
            if (i + segmentcount <= currentsubsegment.Endsegment)
            {
               ret.Add(new SubSegment
               {
                  Endsegment = i + currentSegmentLength - 1,
                  Startsegment = i
               });
            }
            else
            {
               ret.Add(new SubSegment
               {
                  Endsegment = currentsubsegment.Endsegment,
                  Startsegment = i
               });
            }
         }
         return ret.ToArray();
      }
      #endregion

      #region GetSubSegmentBinary

      /// <summary>
      /// Метод деления адресного пространства попалам
      /// </summary>
      /// <param name="currentsubsegment">Сегмент для деления</param>
      /// <param name="relation"></param>
      /// <returns>Сегменты полученные в результате деления</returns>
      public static SubSegment[] GetSubSegmentPart(SubSegment currentsubsegment, double relation)
      {
         if (relation > 1.0 || relation < 0)
            throw new Exception("Relations between parts of delimitation is incorrect. It should be between 0-1");

         int currentSegmentLength = currentsubsegment.Endsegment - currentsubsegment.Startsegment + 1;
         if (currentSegmentLength <= 3)
            return new[] { currentsubsegment };
         var ret = new List<SubSegment>
                      {
                         new SubSegment
                            {
                               Startsegment = currentsubsegment.Startsegment,
                               Endsegment = currentsubsegment.Startsegment + (int) (currentSegmentLength*relation)
                            }
                      };
         if (ret[0].Endsegment + 1 <= currentsubsegment.Endsegment)
         {
            return ret.ToArray();
         }
         ret[1] = new SubSegment
         {
            Startsegment = ret[0].Endsegment + 1,
            Endsegment = currentsubsegment.Endsegment
         };
         return ret.ToArray();
      }
      #endregion

      #endregion
   }
}
