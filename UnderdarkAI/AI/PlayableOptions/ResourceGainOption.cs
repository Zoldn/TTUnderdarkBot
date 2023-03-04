using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class ResourceGainOptionSelector : OptionGenerator
    {
        public int Mana { get; }
        public int Swords { get; }
        public ResourceGainOptionSelector(int mana = 0, int swords = 0)
        {
            Mana = mana;
            Swords = swords;
        }

        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 0,
                outIteration: 1,
                (board, turn) => true,
                mana: Mana, swords: Swords);

            EndCardHelper.Run(options, board, turn, 1);

            return options;
        }
    }

    internal static class OptionalResourceGainHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn, 
            int inIteration, 
            int outIteration, 
            Func<Board, Turn, bool> condition,
            int mana = 0, int swords = 0)
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                if (condition(board, turn))
                {
                    options.Add(new ResourceGainOption(mana: mana, swords: swords)
                    {
                        Weight = 1.0d,
                        NextCardIteration = outIteration,
                        NextState = SelectionState.SELECT_CARD_OPTION,
                    });
                }
                else
                {
                    options.Add(new DoNothingOption(outIteration));
                }
            }

            return options;
        }
    }

    internal class ResourceGainOption : PlayableOption
    {
        public int Mana { get; }
        public int Swords { get; }
        public override int MinVerbosity => 0;
        public bool MakeCurrentCardPlayed { get; set; }
        public ResourceGainOption(int mana = 0, int swords = 0) : base()
        {
            Mana = mana;
            Swords = swords;
            NextState = SelectionState.CARD_OR_FREE_ACTION;
        }
        public override void ApplyOption(Board board, Turn turn)
        {
            turn.Mana += Mana;
            turn.Swords += Swords;
        }


        public override string GetOptionText()
        {
            if (Mana > 0 && Swords == 0)
            {
                return $"\tGain {Mana} mana";
            }
            if (Mana == 0 && Swords > 0)
            {
                return $"\tGain {Swords} sword(s)";
            }

            return $"\tGain {Mana} mana and {Swords} sword(s)";
        }
    }
}
