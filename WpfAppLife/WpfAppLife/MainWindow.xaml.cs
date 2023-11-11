using GameLogicClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAppLife.CommonClasses;

namespace WpfAppLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //размер ячейки
        const int cellSize = 40;

        /// <summary>
        /// Экземпляр игрового поля (Объект типа GameBord)
        /// </summary>
        GameBord gameBord;

        public MainWindow()
        {
            InitializeComponent();
            SetResurs();
            IntGame();
        }

        /// <summary>
        /// Загрузка ресурсов приложения
        /// </summary>
        void SetResurs()
        {
            // определяем путь к файлу ресурсов
            var uri = new Uri("LightDictionary.xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }


        /// <summary>
        /// Настройка программы
        /// </summary>
        void IntGame()
        {
            gameBord = GameBord.GetInstance();
            gameBord.RedrawBord += RedrawBord;
            gameBord.NewBordCreated += NewBord;
            gameWindow.DataContext = gameBord;
            gameBord.CreateNewBord((10, 14));
        }

        /// <summary>
        /// Создать новую доску
        /// </summary>
        void NewBord(Object obj, EventArgs args)
        {
            DrawNewCleanBord(gameBord, UniformGridGameBord);
        }
                
        /// <summary>
        /// Нарисовать новую чистую доску
        /// </summary>
        /// <param name="gameBord"></param>
        /// <param name="UniformGridGameBord"></param>
       void DrawNewCleanBord(GameBord gameBord, UniformGrid UniformGridGameBord)
        {
            DrawCleanUniformGrid(gameBord.GameBordSize, UniformGridGameBord);
            DrawButtons(UniformGridGameBord);
        }




        /// <summary>
        /// Перерисовать картинки
        /// </summary>
        /// <param name="image">массив картинок</param>
        void RedrawBord(object source, BitmapImage[,] image)
        {
            int n = image.GetLength(0);
            int m = image.GetLength(1);

            for (int i = 0; i < n; i++) //цикл по строкам UniformGrid
            {
                for (int j = 0; j < m; j++) //цикл по столбцам UniformGrid
                {
                    string name = firstCharInButtonName + i.ToString() + firstCharInButtonName + j.ToString();
                    var obj = UniformGridGameBord.FindName(name);

                    Button button = obj as Button; //пробуем преобразовать объект в Button

                    if (button != null) //если преобразование прошло успешно
                    {
                        //если надо спрятать картинку на текущей кнопке [i,j] и есть рисунок на кнопке
                        if (image[i,j] == null && button.Content!=null)
                        {
                            //SimpleButton(button);
                            button.Content = null; //очистить содержимое кнопки
                            continue;  //сразу переходим к следующей итерации цикла
                        }

                        //todo для ускорения перерисовки сделать проверку что эта картинку уже нарисована,
                        //тогда рисовать её не надо

                        //если надо открыть кнопку=картинку
                        if (image[i, j] != null)
                        {

                            //создание контейнера для хранения изображения
                            Image img = new Image();

                            //запись картинки в контейнер
                            img.Source = image[i, j];

                            /*
                            // создание компонента для отображения изображения
                            StackPanel stackPnl = new StackPanel();
                            //установка толщины границ компонента
                            stackPnl.Margin = new Thickness(1);
                            //добавление контейнера с картинкой в компонент
                            stackPnl.Children.Add(img);
                            //запись компонента в кнопку
                            
                            button.Content = stackPnl;
                            */
                            button.Content = img;

                            continue;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Рисуем чистую доску - UniformGrid
        /// </summary>
        void DrawCleanUniformGrid((int i, int j) size, UniformGrid gameBord)
        {
            gameBord.Children.Clear(); //удаляем все дочерние элементы Grid

            gameBord.Rows = size.i;
            gameBord.Columns = size.j;
            
            gameBord.Height = size.i * cellSize;
            gameBord.Width = size.j * cellSize;
        }

        /// <summary>
        /// Символ, с которого начинается Имя кнопки
        /// </summary>
        readonly char firstCharInButtonName = '_';

        /// <summary>
        /// Рисуем кнопки
        /// </summary>
        /// <param name="grid">UniformGrid на котором рисуем кнопки</param>
        void DrawButtons(UniformGrid grid)
        {
            // Создаём область имен для grid. Для работы с именами создаваемых кнопок
            NameScope.SetNameScope(grid, new NameScope());

            int n = grid.Rows;
            int m = grid.Columns;

            for (int i = 0; i < n; i++) //цикл по строкам UniformGrid
            {
                for (int j = 0; j < m; j++) //цикл по столбцам UniformGrid
                {
                    //создание кнопки
                    Button btn = new Button();
                    
                    //задаём имя Кнопки, чтобы потом можно было находить её по имени
                    btn.Name = firstCharInButtonName+i.ToString() + firstCharInButtonName + j.ToString();
                    grid.RegisterName(btn.Name, btn);

                    //запись координат кнопки в свойство Tag
                    //btn.Tag = (i, j); // (i, j) - используем кортеж

                    //толщина границ кнопки
                    btn.Margin = new Thickness(2);
                    btn.BorderBrush = Brushes.Blue;

                    //при нажатии кнопки, будет вызываться метод Btn_Click    
                    //btn.Click += Btn_Click;

                    //добавление кнопки в сетку
                    grid.Children.Add(btn);
                }
            }
        }

        // меню "О программе"
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WindowAbout about = new WindowAbout();
            about.ShowDialog();
        }


        // меню "Помощь"
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            WindowHelp help = new WindowHelp();
            help.ShowDialog();
        }


        //Меню "Выход"
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //меню "Настройки"
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();

            settings.gameBordSize = GameBord.GetInstance().GameBordSize;
            settings.NumberInanimateObjects = GameBord.GetInstance().NumberInanimateObjects;
            settings.NumberPlantObjects = GameBord.GetInstance().NumberPlantObjects;
            settings.NumberHerbivorousObjects = GameBord.GetInstance().NumberHerbivorousObjects;
            settings.NumberPredatorObjects = GameBord.GetInstance().NumberPredatorObjects;

            if (settings.ShowDialog()==true)
            {
                gameBord.CreateNewBord(settings.gameBordSize);
                gameBord.NumberInanimateObjects = settings.NumberInanimateObjects;
                gameBord.NumberPlantObjects = settings.NumberPlantObjects;
                gameBord.NumberHerbivorousObjects = settings.NumberHerbivorousObjects;
                gameBord.NumberPredatorObjects = settings.NumberPredatorObjects;
            }
        }


        //Старт
        private void Button_Click_StartGame(object sender, RoutedEventArgs e)
        {
            myMenu.IsEnabled = false;
            gameBord.StartGame();
        }

        
        //Стоп
        private void Button_Click_StopGame(object sender, RoutedEventArgs e)
        {
            myMenu.IsEnabled = true;
            gameBord.Stop();
        }
    }
}
