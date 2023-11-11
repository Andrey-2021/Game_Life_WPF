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
    public abstract class Plant : Creature
    {
        /// <summary>
        /// количество циклов, через которое появляется новое растение на доске
        /// </summary>
        public static int numberCyclesThroughWhichNewPlantAppears = 3;
        
        /// <summary>
        /// Текущий цикл. Измеряет время(кол.циклов) до появления нового расстения на доске
        /// </summary>
        public static int currentCycleForNewPlantAppear = 0;

        /// <summary>
        /// Время жизни. Количество циклов в течении которых данное растение живёт
        /// </summary>
        abstract protected int LiveCycles { get; }

        /// <summary>
        /// Текцщий цикл жизни. При нуле растение погибает
        /// </summary>
        public int currentLiveCycles;

        /// <summary>
        /// Текцщий цикл жизни. При нуле растение погибает
        /// </summary>
        public int CurrentLiveCycles 
        {
            get => currentLiveCycles;
            set
            {
                currentLiveCycles = value;
                if (currentLiveCycles <= 0)
                {
                    OnDied();
                }
            } 
        }

        public Plant()
        {
            CurrentLiveCycles = LiveCycles;
        }

        public override void GameStep(object sender, EventArgs e)
        {
            CurrentLiveCycles--;
        }
    }
}
