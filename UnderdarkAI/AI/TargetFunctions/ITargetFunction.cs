using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.TargetFunctions
{
    internal interface ITargetFunction
    {
        public double Evaluate(Board board, Turn turn);
    }
}
