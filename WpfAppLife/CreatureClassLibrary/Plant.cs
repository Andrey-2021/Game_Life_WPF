using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureClassLibrary
{
    /// <summary>
    /// Растение
    /// </summary>
    public class Plant : Creature
    {
        /// <summary>
        /// Время жизни. Количество цикловЮ в течении которых растение живёт
        /// </summary>
        public int LiveCycles { get; set; }

        public override void GameStep()
        {

        }
    }
}
