﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators.SpecificOptionGenerators.Drow
{
    internal class SpyMasterOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn, 
                inIteration: 0, 
                returnIteration: 1, 
                placeIteration: 2, 
                outIteration: 3);

            EndCardHelper.Run(options, board, turn,
                endIteration: 3);

            return options;
        }
    }

    internal class InfiltratorOptionGenetator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var options = new List<PlayableOption>();

            PlaceSpyHelper.Run(options, board, turn,
                inIteration: 0,
                returnIteration: 1,
                placeIteration: 2,
                outIteration: 3);

            OptionalResourceGainHelper.Run(options, board, turn,
                inIteration: 3,
                outIteration: 4,
                (b, t) => OptionUtils.IsPlacedSpyContainsEnemyPlayerTroop(b, t),
                swords: 1
                );

            EndCardHelper.Run(options, board, turn,
                endIteration: 4);

            return options;
        }
    }
}
