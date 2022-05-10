using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUnderdark.Config
{
    public class Configuration
    {
        public string PathToSaveFiles { get; set; }
        public static string GetDefaultPath()
        {
            var pathToUserFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            return pathToUserFolder + @"\Documents\My Games\Tabletop Simulator\Saves\";
        }
        public static Configuration MakeDefault()
        {
            return new Configuration()
            {
                PathToSaveFiles = GetDefaultPath(),
            };
        }

        public static Configuration LoadFromFile()
        {
            string json = string.Empty;
            try
            {
                json = File.ReadAllText("config.json");
            }
            catch (Exception)
            {
                json = string.Empty;
            }

            var config = JsonConvert.DeserializeObject<Configuration>(json);

            if (config is null)
            {
                config = MakeDefault();
            }

            if (config.PathToSaveFiles.Length == 0)
            {
                config.PathToSaveFiles = GetDefaultPath();
            }

            return config;
        }
    }
}
