

using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using WpfAppLife.CommonClasses;

namespace CreatureClassLibrary
{
    /// <summary>
    /// травоядный
    /// </summary>
    public abstract class Herbivorous : Animal
    {
        /// <summary>
        /// верхняя граница для периода движения. 
        /// </summary>
        int upperBoundForMovingPeriod = 5;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Herbivorous():base()
        {
            //период должен выбираться случайным образом
            Random rnd = new Random();
            movingPeriod = rnd.Next(1, upperBoundForMovingPeriod + 1);
        }


        /// <summary>
        /// Вероятность размножения (проценты)
        /// </summary>
        public int ReproductionProbability { get; set; }

        
        public override bool IsICanBaby()
        {
            Random rnd = new Random();
            int i = rnd.Next(0, 100);
            if (i <= ReproductionProbability)
            {
                //можно создать "ребёнка", т.е. такой же объект
                return true;
            }

            return false;
        }


        /// <summary>
        /// Список свободжных ячеек вокруг меня
        /// </summary>
        /// <returns></returns>
        protected override List<(int i, int j)> GetFreeCellsListAroundMe()
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cells = new List<(int i, int j)>();


            // перебираем клетки вокруг меня
            int startI = this.Coordinates.i - 1;
            int startJ = this.Coordinates.j - 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    var newCell = (startI + i, startJ + j);
                    if (newCell == this.Coordinates) continue; //свою ячейку сразу пропускаем.
                    if (IsICanGoToCell(newCell, EnumGoToCell.forBaby)) cells.Add(newCell);
                }
            }
            return cells;
        }
    }
}
