using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class StaticGetManaSwordAction : CardAction
    {
        protected int _mana;
        protected int _swords;
        public override int Mana => _mana;
        public override int Swords => _swords;
        public StaticGetManaSwordAction(int mana = 0, int swords = 0)
        {
            _mana = mana;
            _swords = swords;
        }
    }
}
