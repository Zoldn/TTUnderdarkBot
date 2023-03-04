using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.OptionGenerators
{
    internal class BaseActionOptionGenerator : OptionGenerator
    {
        public override List<PlayableOption> GeneratePlayableOptions(Board board, Turn turn)
        {
            var ret = new List<PlayableOption>();

            if (PlayCardOrBaseActionOptionGenerator.IsAvailableBuy(board, turn))
            {
                ret.Add(new SwitchToBuyingOption() { Weight = 1.0d });
            }

            if (PlayCardOrBaseActionOptionGenerator.IsPlaceActionAvailable(board, turn))
            {
                ret.Add(new SwitchToDeployBySwordOption() { Weight = 1.0d });
            }

            if (PlayCardOrBaseActionOptionGenerator.IsAssassinateBySwords(board, turn))
            {
                ret.Add(new SwitchToAssassinateBySwordOption() { Weight = 1.0d });
            }

            if (OptionUtils.IsReturnableSpies(board, turn) && turn.Swords >= 3)
            {
                ret.Add(new SwitchToReturnEnemySpyBySwordOption() { Weight = 1.0d });
            }

            return ret;
        }
    }

    internal class SwitchToBuyingOption : PlayableOption
    {
        public SwitchToBuyingOption() : base()
        {
            NextState = SelectionState.BUY_CARD_BY_MANA;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 10;

        public override string GetOptionText()
        {
            return $"Switching to buying card by mana";
        }
    }

    internal class SwitchToDeployBySwordOption : PlayableOption
    {
        public SwitchToDeployBySwordOption() : base() { }
        public override void ApplyOption(Board board, Turn turn)
        {
            NextState = SelectionState.DEPLOY_BY_SWORD;
        }

        public override int MinVerbosity => 10;

        public override string GetOptionText()
        {
            return $"Switching to deploying by swords";
        }
    }

    internal class SwitchToAssassinateBySwordOption : PlayableOption
    {
        public SwitchToAssassinateBySwordOption() : base()
        {
            NextState = SelectionState.ASSASSINATE_BY_SWORD;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 10;

        public override string GetOptionText()
        {
            return $"Switching to assassinate by swords";
        }
    }

    internal class SwitchToReturnEnemySpyBySwordOption : PlayableOption
    {
        public SwitchToReturnEnemySpyBySwordOption() : base() 
        {
            NextState = SelectionState.RETURN_ENEMY_SPY_BY_SWORD;
        }
        public override void ApplyOption(Board board, Turn turn)
        {

        }

        public override int MinVerbosity => 0;

        public override string GetOptionText()
        {
            return $"Switching to return enemy spy by swords";
        }
    }
}
