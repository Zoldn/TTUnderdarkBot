using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using UnderdarkAI.Context;
using UnderdarkAI.Context.ContextElements;

namespace UnderdarkAI.IO
{
    internal class CardSaveLoader : ExcelSheetManager<ModelContext, CardStats, CardSaveLoader.Columns>
    {
        internal enum Columns
        {
            SpecificType = 1,
            Type,
            Race,
            Name,
            ManaCost,
            VP,
            PromoteVP,
        }
        public override string SheetName => "Cards";
        public override string FileName => "CardData.xlsx";
        public override Dictionary<Columns, string> Headers => new()
        {
            { Columns.SpecificType, "SpecificType" },
            { Columns.Type, "Type" },
            { Columns.Race, "Race" },
            { Columns.Name, "Name" },
            { Columns.ManaCost, "Cost" },
            { Columns.VP, "VP" },
            { Columns.PromoteVP, "Promote" },
        };

        protected override List<Dictionary<Columns, object>> FormatProductionElements(ModelContext data)
        {
            var loaddata = new List<Dictionary<Columns, object>>();

            foreach (var element in data.CardsStats)
            {
                var item = new Dictionary<Columns, object>()
                {
                    { Columns.SpecificType, element.CardSpecificType },
                    { Columns.Type, element.CardType },
                    { Columns.Race, element.Race },
                    { Columns.Name, element.Name },
                    { Columns.ManaCost, element.ManaCost },
                    { Columns.VP, element.VP },
                    { Columns.PromoteVP, element.PromoteVP },
                };

                loaddata.Add(item);

            }

            return loaddata
                .OrderBy(e => e[Columns.SpecificType])
                .ToList();
        }

        protected override CardStats GetRawElement(Dictionary<Columns, object> item)
        {
            return new CardStats()
            {
                CardSpecificType = AsEnum<CardSpecificType>(item[Columns.SpecificType]),
                CardType = AsEnum<CardType>(item[Columns.Type]),
                Race = AsEnum<Race>(item[Columns.Race]),
                Name = (string)item[Columns.Name],
                ManaCost = AsInt(item[Columns.ManaCost]),
                VP = AsInt(item[Columns.VP]),
                PromoteVP = AsInt(item[Columns.PromoteVP]),
            };
        }

        protected override void PushElements(ModelContext rawContext, List<CardStats> rawElements)
        {
            rawContext.CardsStats = rawElements;
            rawContext.CardsStatsDict = rawContext.CardsStats
                .ToDictionary(d => d.CardSpecificType);
        }
    }
}
