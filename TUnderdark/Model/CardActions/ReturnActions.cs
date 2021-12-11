using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Model.CardActions
{
    internal class ReturnEnemySpyAction : CardAction
    {
        public override bool IsSimpleAction => false;
        public override int ReturnEnemySpy => 1;
    }
}
