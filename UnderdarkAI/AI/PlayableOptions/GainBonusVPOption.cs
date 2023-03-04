using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class GetOptionalVPHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            Func<Board, Turn, int> vpCounter
            )
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                int vps = vpCounter(board, turn);

                options.Add(new GainBonusVPOption(vps, outIteration));
            }

            return options;
        }

        public static List<PlayableOption> RunEndTurn(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int outIteration,
            Func<Board, Turn, int> vpCounter
            )
        {
            if (turn.State == SelectionState.SELECT_END_TURN_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                int vps = vpCounter(board, turn);

                options.Add(new GainBonusVPEndTurnOption(vps, outIteration));
            }

            return options;
        }
    }

    internal class GainBonusVPOption : PlayableOption
    {
        public int VPs { get; }
        public GainBonusVPOption(int vp, int outIteration)
        {
            NextCardIteration = outIteration;
            VPs = vp;
        }
        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].VPTokens += VPs;
        }

        public override string GetOptionText()
        {
            return $"\tGain {VPs} VP";
        }
    }

    internal class GainBonusVPEndTurnOption : PlayableOption
    { 
        public int VPs { get; }
        public GainBonusVPEndTurnOption(int vp, int outIteration) : base()
        {
            NextCardIteration = outIteration;
            NextState = SelectionState.SELECT_END_TURN_CARD_OPTION;
            VPs = vp;
        }
        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.Players[turn.Color].VPTokens += VPs;
        }

        public override string GetOptionText()
        {
            return $"\tGain {VPs} VP";
        }
    }
}
