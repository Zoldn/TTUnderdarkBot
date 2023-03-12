using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderdarkAI.Context;

namespace UnderdarkAI.IO
{
    public class CardExcelManager : ExcelManager<ModelContext>
    {
        public CardExcelManager(string filePath) : base(filePath) { }
        protected override List<IExcelSheetManager<ModelContext>> GetSheetReports()
        {
            return new List<IExcelSheetManager<ModelContext>>()
            {
                new CardSaveLoader(),
            };
        }

        protected override bool LoadFromPackage(ExcelPackage package, ModelContext t)
        {
            bool isLoaded = true;

            foreach (var manager in GetSheetReports())
            {
                var isManagerLoaded = manager.Load(t, FilePath);

                if (!isManagerLoaded)
                {
                    isLoaded = false;
                }
            }

            return isLoaded;
        }
    }
}
