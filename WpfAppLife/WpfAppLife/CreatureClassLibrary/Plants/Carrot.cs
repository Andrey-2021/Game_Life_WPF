using CreatureClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppLife.CreatureClassLibrary.Plants
{

    /// <summary>
    /// Морковь
    /// </summary>
    public class Carrot: Plant
    {
        override protected int LiveCycles => 5;

        /// <summary>
        /// Конструктор. Класс морковь
        /// </summary>
        public Carrot()
        {
            Name = "Морковь";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "Carrot.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));
        }
    }
}
