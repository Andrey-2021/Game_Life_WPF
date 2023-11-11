using System;
using System.Drawing;

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
        public (int x, int y) Coordinates { get; set; }

        //todo поменять на Image
        /// <summary>
        /// Изображение
        /// </summary>
        public Image Picture { get; set; }

        /// <summary>
        /// Живое существо. Признак
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        /// Шаг иры. 
        /// </summary>
        public abstract void GameStep();

    }

}
