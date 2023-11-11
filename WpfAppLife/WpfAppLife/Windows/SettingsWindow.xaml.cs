using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppLife
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window, INotifyPropertyChanged
    {
        public SettingsWindow()
        {
            InitializeComponent();
            settingsWindow.DataContext = this;
        }


        public (int i, int j) gameBordSize;
        
        /// <summary>
        /// Размер доски. Количество строк
        /// </summary>
        public int NumberRows 
        {
            get=> gameBordSize.i;
            set
            {
                gameBordSize.i = value;
                OnPropertyChanged();
            } 
        }

        /// <summary>
        /// Размер доски. Количеству столбцов
        /// </summary>
        public int NumberColumns 
        {
            get => gameBordSize.j;
            set
            {
                gameBordSize.j = value;
                OnPropertyChanged();
            }
        }



        int numberInanimateObjects;

        /// <summary>
        /// Количество неживых объектов
        /// </summary>
        public int NumberInanimateObjects { get => numberInanimateObjects; set { numberInanimateObjects = value; OnPropertyChanged(); } }



        /// <summary>
        /// Количество растений
        /// </summary>
        int numberPlantObjects;
        /// <summary>
        /// Количество растений
        /// </summary>
        public int NumberPlantObjects { get => numberPlantObjects; set { numberPlantObjects = value; OnPropertyChanged(); } }


        /// <summary>
        /// Количество травоядных
        /// </summary>
        int numberHerbivorousObjects;
        /// <summary>
        /// Количество травоядных
        /// </summary>
        public int NumberHerbivorousObjects { get => numberHerbivorousObjects; set { numberHerbivorousObjects = value; OnPropertyChanged(); } }



        int numberPredatorObjects;
        /// <summary>
        /// Количество хищников
        /// </summary>
        public int NumberPredatorObjects { get => numberPredatorObjects; set { numberPredatorObjects = value; OnPropertyChanged(); } }


        //реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

}
