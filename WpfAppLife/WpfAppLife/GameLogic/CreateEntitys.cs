using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using WpfAppLife.CommonClasses;
using WpfAppLife.CreatureClassLibrary;
using WpfAppLife.CreatureClassLibrary.Herbivorouses;
using WpfAppLife.CreatureClassLibrary.Plants;
using WpfAppLife.CreatureClassLibrary.Predators;

namespace CreatureClassLibrary
{

    /// <summary>
    /// Создать объеты для доски
    /// </summary>
    public class CreateEntitys
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CreateEntitys()
        {

        }

        /// <summary>
        /// Заполнить доску случайными объектами
        /// </summary>
        /// <param name="maxNumber"></param>
        internal static void CreateRandomEntities(GameBord gameBord, EnumGameState gameState)
        {
            CreateInanimate(gameBord, gameState);
            CreatePlant(gameBord, gameState);
            CreateHerbivorous(gameBord, gameState);
            CreatePredators(gameBord, gameState);
        }


        /// <summary>
        /// Метод отвечает за логику создания неживых объектов
        /// </summary>
        static void CreateInanimate(GameBord gameBord, EnumGameState gameState)
        {
            //если это только начало игры, 
            if (gameState == EnumGameState.StartGame)
            {
                CreateObjects(GameBord.GetInstance().NumberInanimateObjects, GetInanimateList(), gameBord);
            }
        }


        /// <summary>
        /// Метод отвечает за логику создания растений
        /// </summary>
        static void CreatePlant(GameBord gameBord, EnumGameState gameState)
        {
            //если это только начало игры, 
            if (gameState == EnumGameState.StartGame)
            {
                //тогда создаём количество растений по умолчанию
                CreateObjects(GameBord.GetInstance().NumberPlantObjects, GetPlantList(), gameBord);
                Plant.currentCycleForNewPlantAppear = 0;
                return;
            }

            //Если пора появиться новому растению
            if (Plant.currentCycleForNewPlantAppear == Plant.numberCyclesThroughWhichNewPlantAppears - 1)
            {

                CreateObjects(1, GetPlantList(), gameBord);//создаём одно новое растение
                gameBord.isNeedRedrawBord = true;
                Plant.currentCycleForNewPlantAppear = 0;

            }
            else Plant.currentCycleForNewPlantAppear++;

        }



        /// <summary>
        /// Метод отвечает за логику создания травоядных
        /// </summary>
        static void CreateHerbivorous(GameBord gameBord, EnumGameState gameState)
        {
            //если это только начало игры, 
            if (gameState == EnumGameState.StartGame)
            {
                CreateObjects(GameBord.GetInstance().NumberHerbivorousObjects, GetHerbivorousList(), gameBord);
            }
        }



        /// <summary>
        /// Метод отвечает за логику создания Хищников
        /// </summary>
        static void CreatePredators(GameBord gameBord, EnumGameState gameState)
        {
            //если это только начало игры, 
            if (gameState == EnumGameState.StartGame)
            {
                CreateObjects(GameBord.GetInstance().NumberPredatorObjects, GetPredatorList(), gameBord);
            }

            // шаг игры
            if (gameState == EnumGameState.GameStep)
            {
                var typeList = GetPredatorList();

                foreach (var type in typeList) //перебираем типы хищников
                {
                    //количество хищников типа type сейчас в игре
                    int count = Predator.HowManyObjectInGame(type);

                    //Создаём объект, чтобы узнать сколько их должно быть в игре
                    var temporaryObj = (Predator)Activator.CreateInstance(type);
                    int MinNumberInGame = temporaryObj.MinNumberInGame; //минимальное количество хищников , нужжного типа, в игре
                    int MaxNumberInGame = temporaryObj.MaxNumberInGame; //максимальное количество хищников , нужжного типа, в игре


                    if (count < MinNumberInGame) //если хищников этого типа не хватает в игре
                    {//создать недостающее количество на границе поля
                        // Начинаем по круго с границ,  если свободных ячеек нет, пересматриваем ячейки ближе к центру по кругу

                        for (int i = count; i < MinNumberInGame; i++)
                        {
                            (int i, int j) cell = GetFreeCellAroundBord(gameBord);
                            if (cell.i >= 0 && cell.j >= 0) //если есть свободная ячейка
                                CreateObjects(type, cell, gameBord);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Получить свободную ячейку на границе поля. 
        /// Если свободной ячейки нет, переходим на следующий внутренний кругю
        /// </summary>
        /// <param name="bord"></param>
        /// <returns>Свободная ячейка или (-1,-1) если свободной ячейки нет</returns>
        // Начинаем по круго с границ,  если свободных ячеек нет, пересматриваем ячейки ближе к центру по кругу
        static (int i, int j) GetFreeCellAroundBord(GameBord bord)
        {

            List<(int i, int j)> freeCells = new(); //список свободных ячее по периметру


            int circle = 0; //номер круга обхода по периметру
            var arr = bord.bord;

            int n = arr.GetLength(0); //количество строк
            int m = arr.GetLength(1); //количество столбцов

            //обходим ячейки по внешнему круги.
            //Если все ячейки заняты, переходим на следующий внутренний круг
            while (true)
            {
                //верхняя и нижняя сторона
                for (int i = circle; i < m - circle; i++)
                {
                    if (arr[circle, i] == null) freeCells.Add((circle, i));      //вырхняя сторона
                    if (circle != n - 1 - circle &&
                        arr[n - 1 - circle, i] == null) freeCells.Add((n - 1 - circle, i)); //нижняя сторона
                }

                //левая и правая сторона
                for (int i = circle; i < n - circle; i++)
                {
                    if (arr[i, circle] == null) freeCells.Add((i, circle));      //левая сторона
                    if (circle != m - 1 - circle &&
                        arr[i, m - 1 - circle] == null) freeCells.Add((i, m - 1 - circle)); //правая сторона
                }
                if (freeCells.Count > 0) break;
                if ((circle + 1) * 2 >= n || (circle + 1) * 2 >= m) break;
                circle++;
            }


            if (freeCells.Count > 0) //если ячейки есть
            {
                //выбираем случайную ячейку из них
                Random rnd = new Random();
                int index = rnd.Next(0, freeCells.Count);

                return freeCells[index];
            }
            else return (-1, -1); //нет свободных ячеек
        }




        /// <summary>
        /// Создать группу объектов
        /// </summary>
        /// <param name="count">Количество создаваемых объектов</param>
        /// <param name="types">Список типов из которых случайным образом выбираются создаваемые объекты</param>
        /// <param name="gameBord">доска</param>
        static void CreateObjects(int count, List<Type> types, GameBord gameBord)
        {
            Creature[,] bord = gameBord.bord;
            EventHandler method = gameBord.RemoveObject;

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                int n = rnd.Next(0, types.Count);
                Type type = types[n];

                var coordinates = GetRandomCell(bord);

                if (coordinates.i >= 0 && coordinates.j >= 0)
                {
                    AddObject(type, coordinates, gameBord);
                }
            }
        }


        public static void CreateObjects(Type type, (int i, int j) cell, GameBord gameBord)
        {
            AddObject(type, cell, gameBord);
        }


        static void AddObject(Type type, (int i, int j) coordinates, GameBord gameBord)
        {
            var obj = (Creature)Activator.CreateInstance(type);

            Creature[,] bord = gameBord.bord;
            EventHandler method = gameBord.RemoveObject;

            obj.Coordinates = coordinates;
            bord[coordinates.i, coordinates.j] = obj; //записываем туда число
            obj.Died += method;
            obj.StartLife(GameBord.GetInstance().secondsInOneStep);

            gameBord.isNeedRedrawBord = true;
        }


        /// <summary>
        /// Получить список типов Неодушевлённые объекты
        /// </summary>
        /// <returns></returns>
        static List<Type> GetInanimateList()
        {
            List<Type> list = new List<Type>();

            list.Add(typeof(Hill));
            list.Add(typeof(Lake));
            list.Add(typeof(Tree));

            return list;
        }



        /// <summary>
        /// Получить список типов Растений
        /// </summary>
        /// <returns></returns>
        static List<Type> GetPlantList()
        {
            List<Type> list = new List<Type>();

            list.Add(typeof(Banana));
            list.Add(typeof(Carrot));

            return list;
        }




        /// <summary>
        /// Получить список типов травоядных животных
        /// </summary>
        /// <returns></returns>
        static List<Type> GetHerbivorousList()
        {
            List<Type> list = new List<Type>();

            list.Add(typeof(Monkey));
            list.Add(typeof(Hare));

            return list;
        }



        /// <summary>
        /// Получить список типов хищников
        /// </summary>
        /// <returns></returns>
        static List<Type> GetPredatorList()
        {
            List<Type> list = new List<Type>();

            list.Add(typeof(Jaguar));
            list.Add(typeof(Lupus));

            return list;
        }





        //Запись объекта в случайную ячейку матрицы
        // Если ячейка уже занята, переходим к следующей по порядку ячейке
        //пока не найдём свободную ячейку

        /// <summary>
        /// Получитьс случайную свободную ячейку матрицы
        /// </summary>
        /// <param name="matr"></param>
        /// <returns></returns>
        static (int i, int j) GetRandomCell(Creature[,] matr)
        {
            Random rnd = new Random();

            int i = rnd.Next(0, matr.GetLength(0)); //случайный номер строки
            int j = rnd.Next(0, matr.GetLength(1)); //случайный номер столбца

            //запоминая координаты с которых начали проверку
            int startI = i;
            int startJ = j;

            do
            {
                // если ячейка свободна
                if (matr[i, j] == null)
                {
                    return (i, j);
                }
                else //если ячека занята
                {
                    j++; //идём к следующей по порядку ячейке

                    //если достигли конца строки
                    if (j == matr.GetLength(1))
                    {
                        i++; //переходим на следующую строку
                        j = 0;
                    }
                    //если это была последняя строка в матрице
                    if (i == matr.GetLength(0))
                    {
                        //переходим на первую строку
                        i = 0;
                        j = 0;
                    }
                }

                //если вернулись к исходной ячейке, значит нет свободных ячеек, выходим из цикла
                if (i == startI && j == startJ) break;
            }
            while (true);

            return (-1, -1);
        }
    }
}
