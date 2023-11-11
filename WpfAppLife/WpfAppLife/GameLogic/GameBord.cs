using CreatureClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAppLife.CommonClasses;

namespace GameLogicClassLibrary
{
    /// <summary>
    /// Игровое поле
    /// </summary>
    public class GameBord : INotifyPropertyChanged
    {
        // картинка на доске обновляется раз в одну секунду


        /// <summary>
        /// ссылка на конкретный экземпляр данного объекта - игрового поля
        /// </summary>
        private static GameBord instance;

        /// <summary>
        /// Матрица, содержащая объекты игры  (игровая доска)
        /// </summary>
        internal Creature[,] bord;

        /// <summary>
        /// Размер доски (количество строк,количеству столбцов)
        /// </summary>
        public (int i, int j) GameBordSize
        {
            get
            {
                return (bord.GetLength(0), bord.GetLength(1));
            }
        }



        /// <summary>
        /// Количество неживых объектов
        /// </summary>
        int numberInanimateObjects;
        
        /// <summary>
        /// Количество неживых объектов
        /// </summary>
        public int NumberInanimateObjects { get =>numberInanimateObjects ; set { numberInanimateObjects = value; OnPropertyChanged(); } }

        int currentNumberInanimateObjects;
        /// <summary>
        /// Текущее Количество неживых объектов
        /// </summary>
        public int CurrentNumberInanimateObjects { get => currentNumberInanimateObjects; set { currentNumberInanimateObjects = value; OnPropertyChanged(); } }



        /// <summary>
        /// Количество растений
        /// </summary>
        int numberPlantObjects;
        /// <summary>
        /// Количество растений
        /// </summary>
        public int NumberPlantObjects { get => numberPlantObjects; set { numberPlantObjects = value; OnPropertyChanged(); } }

        int currentNumberPlantObjects;
        /// <summary>
        /// Текущее Количество растений
        /// </summary>
        public int CurrentNumberPlantObjects { get => currentNumberPlantObjects; set { currentNumberPlantObjects= value; OnPropertyChanged(); } }



        /// <summary>
        /// Количество травоядных
        /// </summary>
        int numberHerbivorousObjects;
        /// <summary>
        /// Количество травоядных
        /// </summary>
        public  int NumberHerbivorousObjects { get => numberHerbivorousObjects; set { numberHerbivorousObjects= value; OnPropertyChanged(); } }

        int currentNumberHerbivorousObjects;
        /// <summary>
        /// Текущее Количество травоядных
        /// </summary>
        public int CurrentNumberHerbivorousObjects { get => currentNumberHerbivorousObjects; set { currentNumberHerbivorousObjects= value; OnPropertyChanged(); } }



        int numberPredatorObjects;
        /// <summary>
        /// Количество хищников
        /// </summary>
        public  int NumberPredatorObjects { get => numberPredatorObjects; set { numberPredatorObjects= value; OnPropertyChanged(); } }


        int currentNumberPredatorObjects;
        /// <summary>
        /// Текущее Количество хищников
        /// </summary>
        public int CurrentNumberPredatorObjects { get => currentNumberPredatorObjects; set { currentNumberPredatorObjects= value; OnPropertyChanged(); } }





        /// <summary>
        /// Событие. Создана новая доска
        /// </summary>
        public EventHandler NewBordCreated;




        /// <summary>
        /// Событие. Перерисовать доску
        /// </summary>
        public EventHandler<BitmapImage[,]> RedrawBord;


        //реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }



        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="n">Размер создаваемой доски</param>
        private GameBord()
        {
            NumberInanimateObjects = 5;
            NumberPlantObjects = 20;
            NumberHerbivorousObjects = 10;
            NumberPredatorObjects = 5;

            CreateNewBord((10, 14));
        }


        public void CreateNewBord( (int i, int j) size)
        {
            bord = new Creature[size.i, size.j];
            NewBordCreated?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged("GameBordSize");
        }



        /// <summary>
        /// Метод, обеспечивающий перерисовку доски - сообщающий подписчикам, чтобы они перерисовали доску
        /// </summary>
        void OnRedrawBord()
        {
            RedrawBord?.Invoke(this, GetBitmapImages(bord));
        }


        /// <summary>
        /// Возвращает экземпляр игрового поля - объект GameBord
        /// </summary>
        /// <returns></returns>
        public static GameBord GetInstance()
        {
            if (instance == null)
                instance = new GameBord();
            return instance;
        }



        /// <summary>
        /// Получить матрицу картинок
        /// </summary>
        /// <param name="bord">Матрица объектов Creature</param>
        /// <returns>матрица картинок</returns>
        BitmapImage[,] GetBitmapImages(Creature[,] bord)
        {
            int n = bord.GetLength(0);
            int m = bord.GetLength(1);

            BitmapImage[,] bitmapImages = new BitmapImage[n,m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {

                    if (bord[i, j] != null && bord[i, j].Picture != null)
                    {
                        bitmapImages[i, j] = bord[i, j].Picture;
                    }
                    else
                        bitmapImages[i, j] = null;
                }
            }
            return bitmapImages;
        }


        /// <summary>
        /// Очистить доску
        /// </summary>
        void ClearBord()
        {
            int n = bord.GetLength(0);
            int m = bord.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {

                    bord[i, j] = null;
                }
            }
        }


        /// <summary>
        /// Начать игру
        /// </summary>
        public void StartGame()
        {
            //List<Creature> list=CreateCreature();
            ClearBord();

            CreateEntitys.CreateRandomEntities(this, EnumGameState.StartGame);
            OnRedrawBord();
            CalculateNumberObjects();

            CurreentGameStep = 0;
            timeSinceStartGame = new TimeSpan();
            oneSecondDispatcherTimer = CreateTimer.StartTimer(new TimeSpan(0, 0, 1), OneSecond);
        }

        DispatcherTimer DispatcherTimerForRedrawBord;

        /// <summary>
        /// Одна секунда
        /// </summary>
        DispatcherTimer oneSecondDispatcherTimer;

        /// <summary>
        /// Время, прошедшее от начала игры
        /// </summary>
        TimeSpan timeSinceStartGame;

        /// <summary>
        /// Время, прошедшее от начала игры
        /// </summary>
        public TimeSpan TimeSinceStartGame
        { get => timeSinceStartGame; set { timeSinceStartGame = value; OnPropertyChanged(); } } //timeSinceStartGame.ToString(@"hh\:mm\:ss")


        
        /// <summary>
        /// Количество секунд на обин цикл/ход
        /// </summary>
        public int secondsInOneStep = 2;


        /// <summary>
        /// Текущий шаг игры
        /// </summary>
        int curreentGameStep;
        
        /// <summary>
        /// Текущий шаг игры
        /// </summary>
        public int CurreentGameStep
        { get => curreentGameStep; set { curreentGameStep = value; partCurrentStep = 0;  OnPropertyChanged(); } }
        
        /// <summary>
        /// Счётчик, считает часть шага игры 
        /// </summary>
        int partCurrentStep;

        /// <summary>
        /// Флаг, что надо перерисовать доску
        /// </summary>
        internal bool isNeedRedrawBord = false;

        /// <summary>
        /// Прошла одна секунда игры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OneSecond(object sender, EventArgs e)
        {
            TimeSinceStartGame = TimeSinceStartGame.Add(new TimeSpan(0, 0, 1));

            partCurrentStep++;
            if (partCurrentStep== secondsInOneStep)
            {
                CurreentGameStep++;
                partCurrentStep = 0;
                OneGameStep();

            }

            CalculateNumberObjects();
            if (isNeedRedrawBord)
            {
                OnRedrawBord();
                isNeedRedrawBord = false;
            }
        }


        /// <summary>
        /// Один цикл игры
        /// </summary>
        private void OneGameStep()
        {
            //если надо создать новые объекты на этом шаге 
            CreateEntitys.CreateRandomEntities(this, EnumGameState.GameStep);
            
        }






        /// <summary>
        /// Пересчитать количество объектов
        /// </summary>
        void CalculateNumberObjects()
        {
            CurrentNumberInanimateObjects = 0;
            CurrentNumberPlantObjects = 0;
            CurrentNumberHerbivorousObjects = 0;
            CurrentNumberPredatorObjects = 0;

            for (int i = 0; i < bord.GetLength(0); i++)
            {
                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    if (bord[i, j] != null)
                    {
                        if (bord[i, j] is Inanimate) CurrentNumberInanimateObjects++;
                        if (bord[i, j] is Plant) CurrentNumberPlantObjects++;
                        if (bord[i, j] is Herbivorous) CurrentNumberHerbivorousObjects++;
                        if (bord[i, j] is Predator) CurrentNumberPredatorObjects++;
                    }
                }
            }
        }

        /// <summary>
        /// Остановить игру
        /// </summary>
        public void Stop()
        {
            oneSecondDispatcherTimer.Stop();

            for (int i = 0; i < bord.GetLength(0); i++)
            {
                for (int j = 0; j < bord.GetLength(1); j++)
                {
                    if (bord[i, j] != null) bord[i, j].StopObjectLife(); 
                }
            }

        }

        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <param name="obj">Удаляемый объект</param>
        /// <param name="data"></param>
        internal void RemoveObject(Object obj, EventArgs data)
        {
            Creature creature = obj as Creature;
            if (creature != null)
            {
                bord[creature.Coordinates.i, creature.Coordinates.j]= null;
                isNeedRedrawBord = true;
            }
        }
        
    }
}
