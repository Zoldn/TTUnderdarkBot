using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.Output
{
    internal enum ResultRecordStatictic
    {
        TROPHY_HALL_VP,
        DECK_VP,
        INNER_CIRCLE_VP,
        CONTROL_VP,
        TOTAL_CONTROL_VP,
        TOTAL_VP,
        INSANE_OUTCASTS,
        DECK_COUNT,
        TOTAL_MP_COST,
    }

    internal class ResultRecord
    {
        public int Turn { get; set; }
        public DateTime TimeStamp { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public ResultRecordStatictic Statictic { get; set; }
        public int Value { get; set; }
        public override string ToString()
        {
            return $"{Name}, Turn = {Turn}, {Statictic} = {Value}";
        }
    }
}
