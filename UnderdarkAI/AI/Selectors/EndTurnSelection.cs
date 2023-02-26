using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace UnderdarkAI.AI.Selectors
{
    internal class EndTurnSelection : IEffectSelector
    {
        public EndTurnSelection() { }
        public Dictionary<List<IAtomicEffect>, double> GenerateOptions(Board board, Turn turn)
        {
            if (turn.EndTurnEffects.Count > 0)
            {
                return turn.EndTurnEffects.ToDictionary(e => new List<IAtomicEffect>(1) {  }, e => 1.0d);
            }
            
            return new Dictionary<List<IAtomicEffect>, double> 
            {
                { new List<IAtomicEffect>(1){ new SwitchToFinishSearch() } , 1.0d}
            };
        }
    }

    internal class SwitchToFinishSearch : IAtomicEffect
    {
        public bool IsNewInfoRecieved => false;

        public Card? Card => null;

        public double Value { get; set; }

        public void ApplyEffect(Board board, Turn turn)
        {
            turn.State = SelectionState.FINISH_SELECTION;
        }

        public void PrintEffect()
        {
            Console.WriteLine($"Finishing selection");
        }
    }
}
