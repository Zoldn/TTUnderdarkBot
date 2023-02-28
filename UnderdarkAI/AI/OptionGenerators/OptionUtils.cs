using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.PlayableOptions;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal static class OptionUtils
    {
        internal static List<PlayableOption> GetPromoteAnotherCardPlayedThisTurnInTheEndOptions(Turn turn,
            CardSpecificType promoter)
        {
            var options = turn.CardStates
                    .Where(s => !s.IsPromotedInTheEnd
                        && s.State == CardState.PLAYED
                        && s.EndTurnState != CardState.NOW_PLAYING
                    )
                    .Select(s => s.SpecificType)
                    .Distinct()
                    .Select(s => new PromoteAnotherCardOption(promoter, s) 
                    { Weight = 1.0d +
                        CardMapper.SpecificTypeCardMakers[s].PromoteVP -
                        CardMapper.SpecificTypeCardMakers[s].VP
                    })
                    .ToList();

            return new List<PlayableOption>(options);
        } 
    }
}
