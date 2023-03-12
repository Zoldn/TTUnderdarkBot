using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderdarkAI.IO
{
    internal interface ISaveLoadManager<TSolution>
    {
        bool Save(TSolution solution);
        bool Load(TSolution solution);
    }

    internal class DummySaveLoadManager<TSolution> : ISaveLoadManager<TSolution>
        where TSolution : new()
    {
        public bool Load(TSolution solution)
        {
            //solution = default;
            return true;
        }

        public bool Save(TSolution solution)
        {
            return true;
        }
    }
}
