using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using WpfAppLife.CommonClasses;

namespace CreatureClassLibrary
{
    /// <summary>
    /// Животное
    /// </summary>
    public abstract class Animal : Creature
    {

        /// <summary>
        /// период движения. 
        /// </summary>

        // 1) Для травоядных выбирается случайным образом
        // Принимает значение [1;upperBoundForMovingPeriod]
        //В программе выбираться случайным образом, ограничивается upperBoundForMovingPeriod
        // Используется для реализации требования
        // << каждое травоядное двигается на случайную клетку поблизости
        // раз в несколько жизненных циклов(период должен выбираться случайным образом)>>
        // 2) Для хищников устанавливается точное значение
        protected int movingPeriod;

        /// <summary>
        /// текущий шаг, до передвижения травоядного животного
        /// </summary>
        protected int curentStepForMovingPeriod;



        /// <summary>
        /// показатель голода
        /// </summary>
        public decimal HungerIndicator { get; set; }

        /// <summary>
        /// Начальное значение для "показателя голода"
        /// </summary>
        public decimal InitialValueForHungerIndicator => 1.0m;

        /// <summary>
        /// Шаг, на который уменьщается показатель голода на один ход
        /// </summary>
        public decimal StepForHungerIndicator => 0.2m;

        /// <summary>
        /// Шаг, на который уменьщается показатель голода  при рождении  нового  живого организма
        /// </summary>
        public decimal ReproductionForHungerIndicator => 0.4m;

        /// <summary>
        /// Шаг, на который увеличивается показатель голода при Поглощение другого вида
        /// </summary>
        public decimal EateForHungerIndicator => 0.2m;



        /// <summary>
        /// Конструктор
        /// </summary>
        public Animal()
        {
            HungerIndicator = InitialValueForHungerIndicator;
            curentStepForMovingPeriod = 1;
        }


        /// <summary>
        /// Метод возвращает список того (типы), кого я могу съесть
        /// </summary>
        abstract protected List<Type> WhomICanEat();


        /// <summary>
        /// Проверка показателя голода. Если показатель <=0, животное погибает
        /// </summary>
        /// <returns>Живой или умерший объект</returns>
        protected EnumLive CheckHungerIndicator()
        {
            HungerIndicator -= StepForHungerIndicator;
            if (HungerIndicator <= 0)
            {
                this.OnDied();
                return EnumLive.iDied;
            }
            else
                return EnumLive.iAmLiving;
        }


        /// <summary>
        /// Получить список ячеек, куда я могу двигаться
        /// </summary>
        /// <returns>список ячеек, куда я могу двигаться</returns>
        protected abstract List<(int i, int j)> GetCellsListWhereICanMove();

        /// <summary>
        /// Получить список ячеек в радиусе n вокруг меня, куда я могу двигаться
        /// </summary>
        /// <param name="n">радиус просматриваемых ячеек </param>
        /// <returns>список ячеек куда я могу двигаться</returns>
        protected virtual List<(int i, int j)> GetCellsListWhereICanMoveAroundMe(int n)
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cells = new List<(int i, int j)>();


            // обезьяны могут перемещаться на  любую случайную  клетку в  радиусе  3  клеток от  них.
            int startI = this.Coordinates.i - n;
            int startJ = this.Coordinates.j - n;

            for (int i = 0; i < 2 * n + 1; i++)
            {
                for (int j = 0; j < 2 * n + 1; j++)
                {
                    var newCell = (startI + i, startJ + j);
                    if (newCell == this.Coordinates) continue; //свою ячейку сразу пропускаем.
                    if (IsICanGoToCell(newCell, EnumGoToCell.forMove)) cells.Add(newCell);
                }
            }
            return cells;
        }



        /// <summary>
        /// Проверка, можно ли переместиться в эту ячейку
        /// </summary>
        /// <param name="cell">координаты ячейки, в которую хотим переместиться</param>
        /// <returns>можно/нельзя переместиться</returns>
        protected bool IsICanGoToCell((int i, int j) cell, EnumGoToCell chekPurpose)
        {
            //если ячейка за пределами доски
            if (cell.i < 0 || cell.j < 0
                || cell.i >= GameBord.GetInstance().GameBordSize.i
                || cell.j >= GameBord.GetInstance().GameBordSize.j)
                return false;

            //берём объект в ячейке
            var objInCell = GameBord.GetInstance().bord[cell.i, cell.j];

            //если в ячейке никого нет, значит в неё можно перейти
            if (objInCell == null) return true;

            //если в этой ячейке Неодушевлённый объект
            if (objInCell != null && objInCell is Inanimate)
                return false;

            //если цель - переместиться в эту ячейку
            if (chekPurpose == EnumGoToCell.forMove)
            {
                //если в ячейке находится то, что я могу съесть
                List<Type> whomICanEat = WhomICanEat();
                if (objInCell != null && whomICanEat.Contains(objInCell.GetType()))
                    return true;
            }

            //если ячейка занята
            if (objInCell != null)
                return false;

            return true;
        }



        public override void GameStep(object sender, EventArgs e)
        {
            if (CheckHungerIndicator() == EnumLive.iAmLiving)
            {
                Moving();
                ReproductYouSelf();
            }
        }

        /// <summary>
        /// Список свободных (пустых) ячеек вокруг меня
        /// </summary>
        /// <returns></returns>
        abstract protected List<(int i, int j)> GetFreeCellsListAroundMe();

        /// <summary>
        /// Размножение. Могу ли я создать ребёнка?
        /// </summary>
        /// <returns></returns>
        abstract public bool IsICanBaby();


        /// <summary>
        /// Размножение
        /// </summary>
        protected void ReproductYouSelf()
        {
            var freeSels = GetFreeCellsListAroundMe();
            if (freeSels.Count > 0 && IsICanBaby()) //если есть свободные ячейки вокруг меня
            {
                CreateEntitys.CreateObjects(this.GetType(), freeSels[0], GameBord.GetInstance());

                // Рождение  нового  живого организма отнимает 0,4.
                HungerIndicator -= ReproductionForHungerIndicator;

                //Если после рождения ребёнка Показатель голода <0, то даём шанс выжить на один ход
                if (HungerIndicator < 0) HungerIndicator = StepForHungerIndicator;
            }
        }


        /// <summary>
        /// Передвижение животного
        /// </summary>
        protected void Moving()
        {
            if (curentStepForMovingPeriod >= movingPeriod)
            {
                MakeMove();
                curentStepForMovingPeriod = 1;
            }
            else curentStepForMovingPeriod++;
        }




        /// <summary>
        /// Переместиться в ячейку. Сделать шаг.
        /// Передвижение на случайную клетку из списка клеток, на которые объекту разрешено передвигаться
        /// </summary>
        protected virtual void MakeMove()
        {
            //список соседних ячеек, в которые можно переместиться
            List<(int i, int j)> cellsForMove = GetCellsListWhereICanMove();

            //если есть соседние клетки, в которые можно переместиться
            if (cellsForMove.Count > 0)
            {
                //выбираем случайную из них
                Random rnd = new Random();
                int index = rnd.Next(0, cellsForMove.Count);

                MoveAnimalToCell(this, cellsForMove[index]);
            }
        }



        /// <summary>
        /// Переместить животное в ячейку
        /// </summary>
        public void MoveAnimalToCell(Animal animal, (int i, int j) newCell)
        {
            var bord = GameBord.GetInstance().bord;


            if (bord[newCell.i, newCell.j] != null) //если в ячеке, в которую перемещаемся, есть объект
            {
                Eat(animal, newCell); //скушать
            }

            //перемещаем животное

            //1.убрали животоное со старой клетки доски
            //var cureentCoordinates = animal.Coordinates;
            bord[animal.Coordinates.i, animal.Coordinates.j] = null;

            //2.поместили на новую клетку
            animal.Coordinates = newCell;
            bord[animal.Coordinates.i, animal.Coordinates.j] = animal;

            GameBord.GetInstance().isNeedRedrawBord = true;
        }

        /// <summary>
        /// Скушать
        /// </summary>
        public virtual void Eat(Animal animal, (int i, int j) newCell)
        {
            var bord = GameBord.GetInstance().bord;

            //объект съеден
            bord[newCell.i, newCell.j].OnDied();

            // Поглощение другого вида прибавляет к голоду 0,2. 
            animal.HungerIndicator += animal.EateForHungerIndicator;
        }

    }
}
