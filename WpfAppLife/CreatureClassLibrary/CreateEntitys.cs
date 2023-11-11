using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureClassLibrary
{
    public class CreateEntitys
    {
        /// <summary>
        /// Создать морковь
        /// </summary>
        /// <returns></returns>
        public static Plant CteateCarrot()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "Images\\Carrot.png"; //картинки из папки

            Plant morkov = new Plant();
            morkov.Name = "Морковь";
            morkov.LiveCycles = 5;
            morkov.IsAlive = true;
            //morkov.Coordinates
            morkov.Picture = Image.FromFile(file);

            return morkov;
        }

        /// <summary>
        /// Создать волка
        /// </summary>
        /// <returns></returns>
        public static Predator CteateWolf()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory; //каталог программы
            var file = dir + "Images\\volk.png"; //картинки из папки

            Predator wolf = new Predator();
            wolf.Name = "Волк";
            wolf.IsAlive = true;
            wolf.HungerIndicator = 1.0;
            //morkov.Coordinates
            wolf.Picture = Image.FromFile(file);

            return wolf;
        }


    }
}
