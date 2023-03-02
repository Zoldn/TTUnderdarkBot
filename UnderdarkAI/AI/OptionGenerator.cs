using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal abstract class OptionGenerator
    {
        /// <summary>
        /// К какому состоянию применяется генератор опций
        /// </summary>
        //public abstract SelectionState State { get; }
        /// <summary>
        /// Опции 
        /// </summary>
        /// <returns></returns>
        public abstract List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn);
    }
}
