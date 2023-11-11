using CreatureClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfAppLife.CreatureClassLibrary.Herbivorouses;
using WpfAppLife.CreatureClassLibrary.Plants;

namespace WpfAppLife.CreatureClassLibrary.Predators
{
    /// <summary>
    /// Волк
    /// </summary>
   public class Lupus : Predator
    {
        public override int MinNumberInGame => 5;
        public override int MaxNumberInGame => -1;
        public override int NumberEatenObjectsAfterWhichAppearsChild => 2;
        public override int NumberBabyWhichICanHave => 1;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Lupus()
        {
            Name = "Волк";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "volk.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));

            movingPeriod = 2;
        }

        protected override List<Type> WhomICanEat()
        {
            List<Type> iCanEat = new List<Type>();
            iCanEat.Add(typeof(Hare));
            return iCanEat;
        }

        protected override List<(int i, int j)> GetCellsListWhereICanMove()
        {
            //Если волк  имеет показатель  голода меньше  чем  0,5
            //может перемещаться  на  2 клетки вокруг себя, а не на одну. 

            if (HungerIndicator<0.5m) return GetCellsListWhereICanMoveAroundMe(2);
            else return GetCellsListWhereICanMoveAroundMe(1);
        }
    }
}
