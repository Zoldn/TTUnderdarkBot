using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnderdarkAI.IO
{
    public abstract class ExcelManager<TData> : ISaveLoadManager<TData>
    {
        //protected ModelContext context;
        public ExcelManager(/*ModelContext context*/ string filePath)
        {
            //this.context = context;
            FilePath = filePath;
        }

        public string FilePath { get; protected set; }

        public bool Save(TData t)
        {
            string savePath = FilePath; // context.Configuration.OutputParams.OutputFilePath;

            var file = new FileInfo(savePath);

            bool isSaveDone = false;
            bool isSaveSuccessful = false;

            while (!isSaveDone)
            {
                try
                {
                    using var package = new ExcelPackage(file);

                    var sheetReports = GetSheetReports();

                    foreach (var sheetReport in sheetReports)
                    {
                        sheetReport.SaveData(package, t); //context
                    }

                    package.Save();

                    Console.WriteLine($"Successful save to {savePath}");

                    isSaveSuccessful = true;
                    isSaveDone = true;
                }
                catch (IOException)
                {
                    Console.WriteLine("IOException: maybe file is opened. Enter 'R' to try again");
                    string enter = Console.ReadLine();
                    isSaveDone = enter != "R";
                }
            }

            return isSaveSuccessful;
        }

        protected abstract List<IExcelSheetManager<TData>> GetSheetReports();

        public bool Load(TData loadedData)
        {
            string loadPath = FilePath; // context.Configuration.OutputParams.OutputFilePath;

            bool loadStatus = false;

            try
            {
                using var fileStream = new FileStream(loadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using var package = new ExcelPackage(fileStream);

                loadStatus = LoadFromPackage(package, loadedData);
            }
            catch (IOException)
            {
                //loadedData = default;
                loadStatus = false;
            }

            return loadStatus;
        }

        protected abstract bool LoadFromPackage(ExcelPackage package, TData t);

        //protected int AsInt<U>(Dictionary<U, object> row, U enumValue) where U : Enum
        //{
        //    if (!row.TryGetValue(enumValue, out var value))
        //    {
        //        throw new IOException($"Can't find key {enumValue} in row dictionary");
        //    }

        //    int parsedValue = (int)(double)value;

        //    return parsedValue;
        //}

        //protected double AsDouble<U>(Dictionary<U, object> row, U enumValue) where U : Enum
        //{
        //    if (!row.TryGetValue(enumValue, out var value))
        //    {
        //        throw new IOException($"Can't find key {enumValue} in row dictionary");
        //    }

        //    double parsedValue = (double)value;

        //    return parsedValue;
        //}
        //protected bool AsBool<U>(Dictionary<U, object> row, U enumValue) where U : Enum
        //{
        //    if (!row.TryGetValue(enumValue, out var value))
        //    {
        //        throw new IOException($"Can't find key {enumValue} in row dictionary");
        //    }

        //    bool parsedValue = (bool)value;

        //    return parsedValue;
        //}

        //protected string AsString<U>(Dictionary<U, object> row, U enumValue) where U : Enum
        //{
        //    if (!row.TryGetValue(enumValue, out var value))
        //    {
        //        throw new IOException($"Can't find key {enumValue} in row dictionary");
        //    }

        //    string parsedValue = (string)value;

        //    return parsedValue;
        //}
    }
}
