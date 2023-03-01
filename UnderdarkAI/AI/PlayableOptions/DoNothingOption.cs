using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.PlayableOptions
{
    internal class DoNothingOption : PlayableOption
    {
        public DoNothingOption() : base() { }
        public override int MinVerbosity => 10;

        public override void ApplyOption(Board board, Turn turn)
        {
            
        }

        public override string GetOptionText()
        {
            return $"Do nothing";
        }
    }
}
