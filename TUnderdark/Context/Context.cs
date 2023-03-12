using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.Context;
using UnderdarkAI.Context.ContextElements;

namespace UnderdarkAI.Context
{
    public class ModelContext
    {
        public List<CardStats> CardsStats { get; set; }
        public Dictionary<CardSpecificType, CardStats> CardsStatsDict { get; set; }

        public ModelContext()
        {
            CardsStats = new (125);
            CardsStatsDict = new();
        }
    }
}
