using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppLife.CommonClasses
{
    /// <summary>
    /// Цель проверки ячейки
    /// </summary>
    public enum EnumGoToCell
    {
        /// <summary>
        /// Для перемещения в неё (Ячейка может содержать то, что можно съесть)
        /// </summary>
        forMove, 

        /// <summary>
        /// для потомка(ребёнка) (Ячека должна быть обязательна пустая)
        /// </summary>
        forBaby
    }
}
