using NetObjects.Core;

namespace NetObjects.Processors
{
    /// <summary>
    /// На основе базовых классов таких как узел (Node) , задача (CalculationTask) 
    /// и других можно проводить различные численные эксперименты над сетью, 
    /// с использованием данных задачи. Таких типов экспериментов можно быть множество 
    /// и все они связаны с работой с узлами сети и на данной момент используется 2 типа э
    /// кспериментов, логика работы которых находится в классах, которые было принято назвать 
    /// процессорами системы: CinematikProcessor, CollapsingPathProcessor. 
    /// Так, как в будущем могут быть проведены другие типа экспериментов, 
    /// то было принято решение выделить общие шаги для процессоров такие как:   
    /// в интерфейс IProcessor теперь, любой класс, который может проводить 
    /// эксперименты на сети и реализует интерфейс IProcessor без труда и 
    /// нарушения текущей архитектуры может быть встроен в систему и использовать все ее возможности.
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="calculationTask"></param>
        /// <param name="netcreater"></param>
        void InitProcess(CalculationTask calculationTask, INetCreator netcreater);
        /// <summary>
        /// Проведение эксперимента
        /// </summary>
        void Processing();
        /// <summary>
        /// Запись результатов эксперимента
        /// </summary>
        void WriteHistoryLogAll();

        void WriteResult(IResultWriter writer);
    }
}
