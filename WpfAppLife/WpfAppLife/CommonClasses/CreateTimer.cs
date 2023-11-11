using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfAppLife.CommonClasses
{
    /// <summary>
    /// Создание таймера
    /// </summary>
    internal class CreateTimer
    {
        /// <summary>
        /// Запуск таймера
        /// </summary>
        /// <param name="timeSpan">Интервал срабатывания таймера</param>
        /// <param name="method">Метод, вызываемый при срабатывании таймера</param>
        /// <returns></returns>
        internal static DispatcherTimer StartTimer(TimeSpan timeSpan, EventHandler method)
        {
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += method;
            dispatcherTimer.Interval = timeSpan;
            dispatcherTimer.Start();
            return dispatcherTimer;
        }
    }
}
