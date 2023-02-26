using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal abstract class PlayableOption
    {
        /// <summary>
        /// Порядковый номер опции для быстрой группировки
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Следующее состояние конечного автомата выбора после выполнения этой опции
        /// </summary>
        public abstract SelectionState GetNextState();
        /// <summary>
        /// Вероятность выбора этой опции
        /// </summary>
        public double Weight { get; set; }
        public abstract void ApplyOption(Board board, Turn turn);
        public abstract string GetOptionText();
        public abstract int MinVerbosity { get; }
        public override string ToString()
        {
            return GetOptionText();
        }
        public void Print(int verbosity)
        {
            if (verbosity >= MinVerbosity)
            {
                Console.WriteLine(GetOptionText());
            }
        }
    }
}
