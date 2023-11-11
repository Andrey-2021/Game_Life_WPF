using CreatureClassLibrary;
using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfAppLife.CreatureClassLibrary.Herbivorouses;

namespace WpfAppLife.CreatureClassLibrary.Predators
{

    /// <summary>
    /// Ягуар
    /// </summary>
    public class Jaguar : Predator
    {

        public override int MinNumberInGame => 1;
        public override int MaxNumberInGame => 2;

        // поскольку в задании не сказано как должен размножаться ягуар,
        // тогда ставим следующие значения
        public override int NumberEatenObjectsAfterWhichAppearsChild => 2;
        public override int NumberBabyWhichICanHave => 3;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Jaguar()
        {
            Name = "Ягуар";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "ягуар.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));

            movingPeriod = 2;
        }
        protected override List<Type> WhomICanEat()
        {
            List<Type> iCanEat = new List<Type>();
            iCanEat.Add(typeof(Monkey));
            iCanEat.Add(typeof(Hare));
            return iCanEat;
        }

        protected override List<(int i, int j)> GetCellsListWhereICanMove()
        {
            //Пока показатель  голода ягуара  больше  0,4,  он перемещается хаотично.
            // ! у нас - на одну клетку вокруг себя

            // В случае если показатель голода будет меньше,
            // он сможет передвигаться на 2 клетки вокруг себя
            //( это для выполнения условия <он автоматически нападает на любую обезьяну в радиусе 2 клеток, сразу же поедая её.>   )

            if (HungerIndicator > 0.4m) return GetCellsListWhereICanMoveAroundMe(1);
            else return GetCellsListWhereICanMoveAroundMe(2);
        }



        protected override void MakeMove()
        {
            //Пока показатель  голода ягуара  больше  0,4,  он перемещается хаотично.
            // В случае если показатель голода будет меньше, он автоматически
            // нападает на любую жетрву в радиусе 2 клеток, сразу же поедая её. 

            if (HungerIndicator>0.4m)
            {
                base.MakeMove();
            }
            else
            {
                List<(int i, int j)> cellsForEat = FindCellsWhareICanEat(2);

                if (cellsForEat.Count > 0) //если вокруг меня есть объекты, которые я могу съесть
                {
                    //выбираем случайный объект из них
                    Random rnd = new Random();
                    int index = rnd.Next(0, cellsForEat.Count);

                    MoveAnimalToCell(this, cellsForEat[index]);
                }
                else //иначе просто ходим
                {
                    base.MakeMove();
                }
            }
        }


        /// <summary>
        /// Список клеток вокруг меня, на которых находятся объекты, которые я могу съесть
        /// </summary>
        /// <param name="n">Радиус просматриваемых ячеек вокруг меня</param>
        /// <returns></returns>
        protected virtual List<(int i, int j)> FindCellsWhareICanEat(int n)
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cells = new List<(int i, int j)>();

            int startI = this.Coordinates.i - n;
            int startJ = this.Coordinates.j - n;

            //список тех, кого я могу есть
            List<Type> whomICanEat = WhomICanEat();

            for (int i = 0; i < 2 * n + 1; i++)
            {
                for (int j = 0; j < 2 * n + 1; j++)
                {
                    (int i, int j) newCell = (startI + i, startJ + j);

                    //если ячейка за пределами доски
                    if (newCell.i < 0 || newCell.j < 0
                        || newCell.i >= GameBord.GetInstance().GameBordSize.i
                        || newCell.j >= GameBord.GetInstance().GameBordSize.j)
                         continue;

                    if (newCell == this.Coordinates) continue; //свою ячейку сразу пропускаем.

                    //берём объект в ячейке
                    var objInCell = GameBord.GetInstance().bord[newCell.i, newCell.j];

                    //если в ячейке находится то, что я могу съесть
                    if (objInCell != null && whomICanEat.Contains(objInCell.GetType()))
                        cells.Add(newCell); //добавляем в список
                }
            }
            return cells;
        }
    }
}
