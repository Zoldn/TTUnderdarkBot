using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI
{
    internal interface IPlayableOption
    {
        public double Weight { get; set; }
    }

    internal abstract class PlayableOption : IPlayableOption
    {
        /// <summary>
        /// Порядковый номер опции для быстрой группировки
        /// </summary>
        //public int Index { get; set; }
        /// <summary>
        /// Следующее состояние конечного автомата выбора после выполнения этой опции
        /// </summary>
        public void UpdateTurnState(Turn turn) 
        {
            turn.State = NextState;
            turn.CardStateIteration = NextCardIteration;
            //turn.CardOption = NextCardOption;

            //if (WillMakeCurrentCardPlayed)
            //{
            //    turn.MakeCurrentCardPlayed();
            //}
        }
        /// <summary>
        /// Вероятность выбора этой опции
        /// </summary>
        public double Weight { get; set; }
        public abstract void ApplyOption(Board board, Turn turn);
        public abstract string GetOptionText();
        public abstract int MinVerbosity { get; }

        #region Next State update
        public MonteCarloSelectionStatus MonteCarloStatus { get; set; }
        //public CardOption NextCardOption { get; set; }
        public int NextCardIteration { get; set; }
        //public bool WillMakeCurrentCardPlayed { get; /*set;*/ }
        #endregion
        public SelectionState NextState { get; set; }

        public PlayableOption()
        {
            //NextCardOption = CardOption.NONE_OPTION;
            NextCardIteration = 0;
            NextState = SelectionState.SELECT_CARD_OPTION;
            Weight = 1.0d;
            //WillMakeCurrentCardPlayed = false; 
        }

        public override string ToString()
        {
            return GetOptionText();
        }
        public string Print(int verbosity, MonteCarloSelectionStatus monteCarloStatus)
        {
            if (verbosity >= MinVerbosity)
            {
                if (monteCarloStatus == MonteCarloSelectionStatus.NOT_ANALYSED)
                {
                    //Console.WriteLine(GetOptionText());
                    return GetOptionText();
                }
                else
                {
                    // Console.WriteLine($"{GetOptionText()} [{monteCarloStatus}]");
                    return $"{GetOptionText()} [{monteCarloStatus}]";
                }
            }

            return string.Empty;
        }
    }
}
