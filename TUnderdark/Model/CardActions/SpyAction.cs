using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class PlaceSpyAction : CardAction
    {
        public PlaceSpyAction(int spies = 1)
        {
            _spies = spies;
        }
        protected int _spies;
        public override int PlaceSpy => _spies;
        public override bool IsSimpleAction => false;
    }
    internal class ReturnOwnSpyAction : CardAction
    {
        public ReturnOwnSpyAction(int mana = 0, int swords = 0)
        {
            _mana = mana;
            _swords = swords;
        }

        protected int _mana;
        protected int _swords;

        public override int Mana => _mana;
        public override int Swords => _swords;
        public override int ReturnOwnSpy => 1;
        public override bool IsSimpleAction => false;
        public override bool IsLegal => ActivePlayer.IsReturnableSpies;
    }
}
