using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;

namespace TUnderdark.TTSParser
{
    internal class TTSSaveParser
    {
        public static bool Read(string json, Board board)
        {
            var container = JsonConvert.DeserializeObject<JSONContainer>(json);

            if (container == null)
            {
                return false;
            }

            if (container.GameMode != "Tyrants of the Underdark")
            {
                Console.WriteLine($"This save is not for Underdark but for {container.GameMode}");
                return false;
            }



            return true;
        }
    }
}
