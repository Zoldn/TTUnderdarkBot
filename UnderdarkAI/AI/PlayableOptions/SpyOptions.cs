using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal static class PlaceSpyHelper
    {
        public static List<PlayableOption> Run(List<PlayableOption> options, Board board, Turn turn,
            int inIteration,
            int returnIteration,
            int placeIteration,
            int outIteration
            )
        {
            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == inIteration)
            {
                if (board.Players[turn.Color].Spies > 0)
                {
                    options.Add(new DoNothingOption(placeIteration));
                }
                else
                {
                    ABCSelectHelper.Run(options, board, turn,
                        inIteration,
                        (b, t) => true, outIteration, /// Не ставить нового шпиона
                        (b, t) => true, returnIteration, /// вернуть одного из шпионов, чтобы поставить нового 
                        outIteration
                        );
                }
            }

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == returnIteration)
            {
                var targets = board
                    .Locations
                    .Where(l => l.Spies[turn.Color])
                    .Select(l => new ReturnOwnExcessSpyOption(l.Id, placeIteration))
                    .ToList();

                Debug.Assert(targets.Count == 5);

                options.AddRange(targets);
            }

            if (turn.State == SelectionState.SELECT_CARD_OPTION
                && turn.CardStateIteration == placeIteration)
            {
                var targets = board
                    .Locations
                    .Where(l => !l.Spies[turn.Color] && l.IsSpyPlacable)
                    .Select(l => new PlaceSpyOption(l.Id, outIteration))
                    .ToList();

                turn.WeightGenerator.FillPlaceSpyOptions(board, turn, targets);

                options.AddRange(targets);
            }

            return options;
        }
    }

    internal class ReturnOwnExcessSpyOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public ReturnOwnExcessSpyOption(LocationId target, int outIteration)
        {
            LocationId = target;
            NextCardIteration = outIteration;
        }
        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.LocationIds[LocationId].Spies[turn.Color] = false;
            board.Players[turn.Color].Spies++;
        }

        public override string GetOptionText()
        {
            return $"\tReturn own spy from {LocationId}";
        }
    }

    internal class PlaceSpyOption : PlayableOption
    {
        public LocationId LocationId { get; }
        public PlaceSpyOption(LocationId target, int outIteration) : base()
        {
            LocationId = target;
            NextCardIteration = outIteration;
        }
        public override int MinVerbosity => 0;

        public override void ApplyOption(Board board, Turn turn)
        {
            board.LocationIds[LocationId].Spies[turn.Color] = true;
            board.Players[turn.Color].Spies--;

            turn.PlacedSpies.Add(LocationId);
        }

        public override string GetOptionText()
        {
            return $"\tPlace spy in {LocationId}";
        }
    }
}
