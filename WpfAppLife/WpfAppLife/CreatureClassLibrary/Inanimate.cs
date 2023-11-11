using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureClassLibrary
{

    /// <summary>
    /// Неодушевлённый объект
    /// </summary>
    public abstract class Inanimate: Creature
    {

        public override void GameStep(object sender, EventArgs e)
        {

        }
    }
}
