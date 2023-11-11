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
    /// Дерево
    /// </summary>
    public class Tree : Inanimate
    {
        /// <summary>
        /// Конструктор. Дерево
        /// </summary>
        public Tree()
        {
            Name = "Дерево";

            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "\\Images\\" + "tree.png"; //картинки из папки

            Picture = new BitmapImage(new Uri(file));

        }
    }
}
