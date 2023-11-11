using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureClassLibrary
{

    /// <summary>
    /// Неодушевлённый
    /// </summary>
    public class Inanimate: Creature
    {
        /// <summary>
        /// Признак, что можно через него пройти
        /// </summary>
        public bool IsCanGoThroughIt { get; set; }

        public override void GameStep()
        {

        }
    }
}
