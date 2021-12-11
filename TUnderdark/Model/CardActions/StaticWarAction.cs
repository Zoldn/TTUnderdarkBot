using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class StaticWarAction : CardAction
    {
        public override bool IsSimpleAction => false;
        public StaticWarAction(int supplant = 0, int returnTroop = 0,
            int assasinateWhite = 0, int supplantWhite = 0, 
            int supplantWhiteAnywhere = 0, int deploy = 0, 
            int assasinate = 0, int move = 0)
        {
            _supplant = supplant;
            _returnTroop = returnTroop;
            _assasinateWhite = assasinateWhite;
            _supplantWhite = supplantWhite;
            _supplantWhiteAnywhere = supplantWhiteAnywhere;
            _deploy = deploy;
            _assasinate = assasinate;
            _move = move;
        }

        protected int _supplant;
        protected int _returnTroop;
        protected int _assasinateWhite;
        protected int _supplantWhite;
        protected int _supplantWhiteAnywhere;
        protected int _deploy;
        protected int _assasinate;
        protected int _move;
        public override int Supplant => _supplant;
        public override int ReturnTroop => _returnTroop;
        public override int AssasinateWhite => _assasinateWhite;
        public override int SupplantWhite => _supplantWhite;
        public override int SupplantWhiteAnywhere => _supplantWhiteAnywhere;
        public override int Deploy => _deploy;
        public override int Assasinate => _assasinate;
        public override int MoveTroop => _move;
    }
}
