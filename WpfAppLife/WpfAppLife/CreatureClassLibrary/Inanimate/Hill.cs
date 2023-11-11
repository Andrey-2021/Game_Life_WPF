using CreatureClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppLife.CreatureClassLibrary
{
    /// <summary>
    /// Гора
    /// </summary>
    public class Hill: Inanimate
    {

        /// <summary>
        /// Конструктор. Гора
        /// </summary>
        public Hill()
        {
            Name = "Гора";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\"+ "hill.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));
        }
    }
}
