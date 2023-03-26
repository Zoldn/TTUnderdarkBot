using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnderdarkAI.Context;

namespace UnderdarkAI.IO
{
    public interface IExcelSheetManager<TData>
    {
        void SaveData(ExcelPackage package, TData data);
        bool Load(ModelContext rawContext, string inputPath);
    }


    /// <summary>
    /// Загрузчик и выгрузчик данных типа TItem со стобцами TColumns в контейнер TData 
    /// </summary>
    /// <typeparam name="TData">Откуда берем данные</typeparam>
    /// <typeparam name="TItem">Строка-объект для записи</typeparam>
    /// <typeparam name="TColumns">Столбцы-поля объекта</typeparam>
    internal abstract class ExcelSheetManager<TData, TItem, TColumns> : IExcelSheetManager<TData>
        where TColumns : Enum
    {
        public abstract string FileName { get; }
        /// <summary>
        /// Имя листа
        /// </summary>
        public abstract string SheetName { get; }
        /// <summary>
        /// Названия столбцов
        /// </summary>
        public abstract Dictionary<TColumns, string> Headers { get; }
        #region Saver part
        /// <summary>
        /// Форматирование сериализуемых данных в стандартизированный вид для записи в файл
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract List<Dictionary<TColumns, object>> FormatProductionElements(TData data);

        //protected ModelContext context;

        /// <summary>
        /// Основной метод сохранение результата
        /// </summary>
        /// <param name="context"></param>
        /// <param name="package"></param>
        /// <param name="data"></param>
        public void SaveData(/*ModelContext context, */ExcelPackage package, TData data)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == SheetName) ??
                package.Workbook.Worksheets.Add(SheetName);

            worksheet.Cells.Clear();

            SetHeaders(worksheet);
            SetData(worksheet, data);
            SetFormat(worksheet);
        }

        protected virtual void SetHeaders(ExcelWorksheet worksheet)
        {
            for (int i = 1; i <= Headers.Count; i++)
            {
                worksheet.Cells[1, i].Value = Headers[(TColumns)(object)i];
            }
        }

        protected void SetData(ExcelWorksheet worksheet, TData data)
        {
            int row = 2;

            var formattedData = FormatProductionElements(data);

            foreach (var line in formattedData)
            {
                foreach (var kvpair in line)
                {
                    worksheet.Cells[row, (int)(object)kvpair.Key].Value = kvpair.Value;
                }

                ++row;
            }
        }
        protected virtual void SetFormat(ExcelWorksheet worksheet)
        {
            var dim = worksheet.Dimension;
            worksheet.Cells[dim.Start.Row, dim.Start.Column, dim.End.Row, dim.End.Column].AutoFilter = true;

            for (int i = 1; i <= Headers.Count; i++)
            {
                worksheet.Column(i).AutoFit();
            }

            worksheet.View.FreezePanes(2, 1);
        }

        protected void HideColumn(ExcelWorksheet worksheet, TColumns column)
        {
            worksheet.Column((int)(object)column).Hidden = true;
        }
        protected void FormatColumnAsDate(ExcelWorksheet worksheet, TColumns column)
        {
            worksheet.Column((int)(object)column).Style.Numberformat.Format = "dd.mm.yyyy HH:mm";
            worksheet.Column((int)(object)column).AutoFit();
        }
        #endregion

        #region Loader part
        protected abstract void PushElements(ModelContext rawContext, List<TItem> rawElements);
        protected abstract TItem GetRawElement(Dictionary<TColumns, object> item);
        public bool Load(ModelContext rawContext, string inputPath)
        {
            string loadPath = inputPath;
            //  + FileName
            bool loadStatus = false;

            var rawElements = new List<TItem>();

            try
            {
                using var fileStream = new FileStream(loadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using var package = new ExcelPackage(fileStream);

                bool status = GetData(package, out List<Dictionary<TColumns, object>> data);

                foreach (var item in data)
                {
                    rawElements.Add(GetRawElement(item));
                }

                PushElements(rawContext, rawElements);
            }
            catch (IOException)
            {
                loadStatus = false;
            }

            return loadStatus;
        }

        protected bool GetData(ExcelPackage package, out List<Dictionary<TColumns, object>> data)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //TODO: revert to free 4 version

            data = new List<Dictionary<TColumns, object>>();

            var sheet = package.Workbook.Worksheets.FirstOrDefault(sh => sh.Name == SheetName);

            if (sheet == null)
            {
                //throw new IOException($"Missing sheet {SheetName}");
                Console.WriteLine($"WARNING: missing sheet {SheetName}");
                return false;
            }

            var invertedHeaders = Headers.ToDictionary(kvpair => kvpair.Value, kvpair => kvpair.Key);

            var sheetHeadersToEnum = new Dictionary<int, TColumns>();

            for (int column = 1; column <= sheet.Dimension.End.Column; column++)
            {
                string columnHeader = (string)sheet.Cells[1, column].Value;
                TColumns columnEnum = invertedHeaders[columnHeader];
                sheetHeadersToEnum.Add(column, columnEnum);
            }

            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            {
                var oneRow = new Dictionary<TColumns, object>();

                for (int column = 1; column <= sheet.Dimension.End.Column; column++)
                {
                    oneRow.Add(sheetHeadersToEnum[column], sheet.Cells[row, column].Value);
                }

                data.Add(oneRow);
            }

            return true;
        }

        protected int AsInt(object obj)
        {
            return (int)(double)obj;
        }

        protected double AsDouble(object obj)
        {
            return (double)obj;
        }

        protected bool AsBool(object obj)
        {
            return (bool)obj;
        }

        protected DateTime AsDateTime(object obj)
        {
            return (DateTime)obj;
        }

        protected TEnum AsEnum<TEnum>(object obj)
            where TEnum : struct, Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), (string) obj);
        }
        #endregion
    }
}
