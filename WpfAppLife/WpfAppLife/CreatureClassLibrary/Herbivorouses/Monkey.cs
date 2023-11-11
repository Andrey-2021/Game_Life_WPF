using CreatureClassLibrary;
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
    /// Обезьяна
    /// </summary>
    public class Monkey : Herbivorous
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Monkey() : base()
        {
            Name = "Обезьяна";
            ReproductionProbability = 25;

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "Обезьяна.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));

        }

        protected override List<Type> WhomICanEat()
        {
            List<Type> iCanEat = new List<Type>();
            iCanEat.Add(typeof(Banana));
            iCanEat.Add(typeof(Carrot));
            return iCanEat;
        }


        protected override List<(int i, int j)> GetCellsListWhereICanMove()
        {
            return GetCellsListWhereICanMoveAroundMe(3);
        }

        //protected override List<(int i, int j)> GetCellsListWhereICanMove()
        //{
        //    //список соседних ячеек, в которые можно переместиться
        //    List<(int i, int j)> cells = new List<(int i, int j)>();


        //    // обезьяны могут перемещаться на  любую случайную  клетку в  радиусе  3  клеток от  них.
        //    int startI = this.Coordinates.i - 3;
        //    int startJ = this.Coordinates.j - 3;

        //    for (int i = 0; i < 7; i++)
        //    {
        //        for (int j = 0; j < 7; j++)
        //        {
        //            var newCell = (startI+i, startJ+j);
        //            if (newCell == this.Coordinates) continue; //свою ячейку сразу пропускаем.
        //            if (IsICanGoToCell(newCell, EnumGoToCell.forMove)) cells.Add(newCell);
        //        }
        //    }
        //    return cells;
        //}

    }
}
