using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal interface IEffectSelector
    {
        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn);
    }

    internal interface ICardEffectSelector : IEffectSelector
    { 
        public Card Card { get; }
    }


    /// <summary>
    /// Атомарный эффект, применяемый к ходу или состоянию доски
    /// </summary>
    internal interface IAtomicEffect
    {
        /// <summary>
        /// Получена ли в результате этого действия новая информация 
        /// (открылась карта на рынке или задрована карта из колоды)
        /// </summary>
        public bool IsNewInfoRecieved { get; }
        /// <summary>
        /// Привязан ли эффект к карте, если null - то свободное действие
        /// </summary>
        public Card? Card { get; }
        /// <summary>
        /// Применить эффект к текущему состоянию
        /// </summary>
        /// <param name="board"></param>
        /// <param name="turn"></param>
        public void ApplyEffect(Board board, Turn turn);
        /// <summary>
        /// Напечатать эффект для вывода результата
        /// </summary>
        public void PrintEffect();
        public double Value { get; set; }
    }
}
