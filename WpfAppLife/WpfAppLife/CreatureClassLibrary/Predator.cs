using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureClassLibrary
{
    /// <summary>
    /// Хищник
    /// </summary>
    public abstract class Predator : Animal
    {


        /// <summary>
        /// Минимальное количество таких хищников на поле
        /// В  случае  если  это  количество  уменьшится  –  необходимо создать нового хищника
        /// </summary>
        public abstract int MinNumberInGame { get; }

        /// <summary>
        /// Максимальное количество таких хищников на поле.
        /// -1, если количество хищников неограничено
        /// </summary>
        public abstract int MaxNumberInGame { get; }

        /// <summary>
        /// Количество скушанных объектов (зайцев,,,)
        /// </summary>
        public int NumberEatenObjects = 0;

        /// <summary>
        /// Количество скушанных объектов, после которых появляется ребёнок
        /// </summary>
        public abstract int NumberEatenObjectsAfterWhichAppearsChild { get; }

        /// <summary>
        /// Сколько раз можно родить потомство
        /// </summary>
        public abstract int NumberBabyWhichICanHave { get; }

        /// <summary>
        /// Количество уже рождённых потомков
        /// </summary>
        protected int numberBaby=0;


        /// <summary>
        /// Ячейка с которой Хищник скашал объект
        /// </summary>
        //Используется для выполнения условия:
        // "Если хищник(волк) съел больше  NumberEatenObjectsAfterWhichAppearsChild объектов ( 2 зайцев),
        // то после перемещения
        // или съедения объекта (зайца) на предыдущей клетке оставит после себя нового волка."
        protected (int i, int j) CellFromIAmEating;

        public override void Eat(Animal animal, (int i, int j) newCell)
        {
            base.Eat(animal, newCell);

            NumberEatenObjects++;
            CellFromIAmEating = animal.Coordinates;
        }


        protected override List<(int i, int j)> GetFreeCellsListAroundMe()
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cells = new List<(int i, int j)>();

            cells.Add(CellFromIAmEating);

            return cells;
        }


        public override bool IsICanBaby()
        {
            if (numberBaby>= NumberBabyWhichICanHave) return false;

            if (MaxNumberInGame!=-1 && HowManyObjectInGame(this.GetType()) >MaxNumberInGame) return false;

            if (NumberEatenObjects == NumberEatenObjectsAfterWhichAppearsChild)
            {
                NumberEatenObjects = 0;
                numberBaby++;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Подсчитываем количество хищников заданного типа на игровом поле
        /// </summary>
        /// <param name="type">Тип хищника</param>
        /// <returns>количество хищников заданного типа на игровом поле</returns>
        public static int HowManyObjectInGame(Type type)
        {
            var gameBord = GameBord.GetInstance();

            int count = 0; //количество хищников типа type сейчас в игре
            var bord = gameBord.bord;

            foreach (var obj in bord) //перебираем объекты доски
            {
                if (obj!=null && obj.GetType()==type) //если очередной объект - это нужный тип
                {
                    count++;
                }
            }
            return count;
        }

    }
}
