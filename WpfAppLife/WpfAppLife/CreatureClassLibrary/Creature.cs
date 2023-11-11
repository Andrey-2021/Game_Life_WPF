using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAppLife.CommonClasses;

namespace CreatureClassLibrary
{

    /// <summary>
    /// Существо
    /// </summary>
    public abstract class Creature
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public (int i, int j) Coordinates { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public BitmapImage Picture { get; set; }


        /// <summary>
        /// Шаг иры. 
        /// </summary>
        public abstract void GameStep(object sender, EventArgs e);

        /// <summary>
        /// Таймер. 
        /// </summary>
        DispatcherTimer dispatcherTimer; //каждая сущность живёт в своём мире - у неё свой таймер и она действует по этому таймеру

        /// <summary>
        /// Начать жить
        /// </summary>
        public void StartLife(int secondsInOneStep)
        {
            dispatcherTimer=CreateTimer.StartTimer(new TimeSpan(0, 0, secondsInOneStep), GameStep);
        }

        /// <summary>
        /// Остановить объект (остановить его деятельность)
        /// </summary>
        public void StopObjectLife()
        {
            dispatcherTimer.Stop();
        }

        /// <summary>
        /// Событие. Объект умер
        /// </summary>
        public event EventHandler Died;


        /// <summary>
        /// Метод вызывается когда объект умер (съеден, уничтожен)
        /// </summary>
        public void OnDied()
        {
            StopObjectLife();

            if (Died == null)
            {
                throw new Exception("Не задан обработчки для удаления объекта.");
            }
            else
                Died.Invoke(this, EventArgs.Empty);
        }

    }

}
