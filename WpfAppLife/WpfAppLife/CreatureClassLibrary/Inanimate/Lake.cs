using CreatureClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfAppLife.CreatureClassLibrary
{
    /// <summary>
    /// Озеро
    /// </summary>
    public class Lake : Inanimate
    {
        /// <summary>
        /// Конструктор. Озеро
        /// </summary>
        public Lake()
        {
            Name = "Озеро";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "lake.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));
        }
    }
}
