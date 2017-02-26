namespace NetObjects.Enums
{
   /// <summary>
   /// Типы сетей
   /// </summary>
   public enum NetType
   {	/// <summary>
      /// Случайна Сеть Кейли
      /// </summary>
      Keily,
      /// <summary>
      /// Многосвязная сеть
      /// </summary>
      MultyLink,
      /// <summary>
      /// Квадратная сеть
      /// </summary>
      Quadro,
      /// <summary>
      /// Регулярная Сеть Кейли, с одним и тем же числом связей для узлов 
      /// </summary>
      KeilyRegular,
      /// <summary>
      /// Треугольная случайная сеть
      /// </summary>
      TriangleIrregular,
      /// <summary>
      /// Треугольная Регулярная сети
      /// </summary>
      TriangleRegular,
      /// <summary>
      /// Шестеугольная сеть
      /// </summary>
      Hexagon,
      /// <summary>
      /// Сеть три, двенадца в квадрате
      /// </summary>
      Net3122,
      /// <summary>
      /// Сеть Кейли с резервированием каналов
      /// </summary>
      KeilyWithBackupChannel,
      /// <summary>
      /// Кубическая сеть
      /// </summary>
      Cube,
      /// <summary>
      /// Кубическая сеть с дополнительными узлами
      /// </summary>
      EnhancedCube,
      /// <summary>
      /// Шестеугольная призма
      /// </summary>
      Hexagon3D,
      /// <summary>
      /// Призма из три-двенадца в квартаре
      /// </summary>
      Net31223D,
      /// <summary>
      /// треугольная, регулярная 3D
      /// </summary>
      TriangleRegular3D,
      /// <summary>
      /// Сеть Кейли 3d 
      /// </summary>
      Keily3D,
      /// <summary>
      /// 
      /// </summary>
      TriangleIRegular3D
   };
}