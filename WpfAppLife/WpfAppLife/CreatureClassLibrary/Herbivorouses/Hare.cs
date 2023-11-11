using CreatureClassLibrary;
using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfAppLife.CommonClasses;
using WpfAppLife.CreatureClassLibrary.Plants;

namespace WpfAppLife.CreatureClassLibrary.Herbivorouses
{
    /// <summary>
    /// Заяц
    /// </summary>
    public class Hare: Herbivorous
    {

        /// <summary>
        /// Конструктор
        /// </summary>
        public Hare():base()
        {
            Name = "Заяц";
            ReproductionProbability = 25;

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "zaiyac.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));
        }


        protected override List<Type> WhomICanEat()
        {
            List<Type> iCanEat = new List<Type>();
            iCanEat.Add(typeof(Carrot));
            return iCanEat;
        }


        // пусть у нас
        // зайцы могут перемещаться только на соседние клетки по горизонтали и вертикали
        protected override List<(int i, int j)> GetCellsListWhereICanMove()
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cells = new List<(int i, int j)>();

            //ячейка выше
            var upCell = (this.Coordinates.i - 1, this.Coordinates.j);
            if (IsICanGoToCell(upCell, EnumGoToCell.forMove)) cells.Add(upCell);

            //ячейка ниже
            var downCell = (this.Coordinates.i + 1, this.Coordinates.j);
            if (IsICanGoToCell(downCell, EnumGoToCell.forMove)) cells.Add(downCell);

            //ячейка левее
            var leftCell = (this.Coordinates.i, this.Coordinates.j - 1);
            if (IsICanGoToCell(leftCell, EnumGoToCell.forMove)) cells.Add(leftCell);

            //ячейка правее
            var rightCell = (this.Coordinates.i, this.Coordinates.j + 1);
            if (IsICanGoToCell(rightCell, EnumGoToCell.forMove)) cells.Add(rightCell);

            return cells;
        }

    }
}
