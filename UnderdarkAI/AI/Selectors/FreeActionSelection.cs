using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI.CardEffects;

namespace UnderdarkAI.AI.Selectors
{
    internal class FreeActionSelection : IEffectSelector
    {
        public FreeActionSelection() { }
        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            Dictionary<List<IAtomicEffect>, double> ret = new();

            AddMarketOptions(board, turn, ret);
            AddLolthBuyOption(board, turn, ret);
            AddHouseguardOption(board, turn, ret);

            AddDeployOptions(board, turn, ret);
            AddAssasinateOptions(board, turn, ret);
            AddReturnSpyOptions(board, turn, ret);

            return ret;
        }

        private void AddReturnSpyOptions(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            if (turn.Swords < 3)
            {
                return;
            }

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, isSpy) in location.Spies)
                {
                    if (color == turn.Color || !isSpy)
                    {
                        continue;
                    }

                    ret.Add(new List<IAtomicEffect>(1) { new ReturnSpyBySwords(location, color) }, 1.0d);
                }
            }
        }

        private void AddDeployOptions(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            if (turn.Swords < 1)
            {
                return;
            }

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence || location.FreeSpaces == 0)
                {
                    continue;
                }

                ret.Add(new List<IAtomicEffect>(1) { new DeployUnitBySwordEffect(location) }, 2.0d);
            }
        }

        private void AddAssasinateOptions(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            if (turn.Swords < 3)
            {
                return;
            }

            foreach (var (location, locationState) in turn.LocationStates)
            {
                if (!locationState.HasPresence)
                {
                    continue;
                }

                foreach (var (color, troop) in location.Troops)
                {
                    if (color == turn.Color || troop == 0)
                    {
                        continue;
                    }

                    ret.Add(new List<IAtomicEffect>(1) { new AssassinateBySwords(location, color) }, 2.0d);
                }
            }
        }

        private static void AddHouseguardOption(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            if (board.HouseGuards <= 0 || turn.Mana < 3)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.HOUSEGUARD].Clone();

            ret.Add(new List<IAtomicEffect>(1) { new BuyCardEffect(card) },
                1.0d + card.VP / card.ManaCost);
        }

        private static void AddLolthBuyOption(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            if (board.Lolths <= 0 || turn.Mana < 2)
            {
                return;
            }

            var card = CardMapper.SpecificTypeCardMakers[CardSpecificType.LOLTH].Clone();

            ret.Add(new List<IAtomicEffect>(1) { new BuyCardEffect(card) },
                1.0d + card.VP / card.ManaCost);
        }

        private static void AddMarketOptions(Board board, Turn turn, Dictionary<List<IAtomicEffect>, double> ret)
        {
            var cardsOnMarket = board.Market
                .Where(c => c.ManaCost <= turn.Mana)
                .ToList();

            foreach (var card in cardsOnMarket)
            {
                ret.Add(new List<IAtomicEffect>(1) { new BuyCardEffect(card) },
                    1.0d + card.VP / card.ManaCost);
            }
        }
    }
}
