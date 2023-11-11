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
    /// Банан
    /// </summary>
   public class Banana : Plant
    {
        override protected int LiveCycles => 10;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Banana()
        {
            Name = "Банан";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "банан.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));

        }

    }
}
