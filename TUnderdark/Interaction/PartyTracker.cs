using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.Output;
using TUnderdark.TTSParser;

namespace TUnderdark.Interaction
{

    internal class PartyTracker
    {
        internal enum Columns {
            TURN = 1,
            TIMESTAMP,
            COLOR,
            NAME,
            STATISTIC,
            VALUE,
        }

        private static readonly Dictionary<Columns, string> headers = new()
        {
            { Columns.TURN, "Turn" },
            { Columns.TIMESTAMP, "Time stamp" },
            { Columns.COLOR, "Color" },
            { Columns.NAME, "Name" },
            { Columns.STATISTIC, "Property" },
            { Columns.VALUE, "Value" },
        };

        public string TargetSaveFile { get; private set; }
        public Dictionary<Color, string> PlayerNames { get; private set; }
        public PartyTracker(string targetSave, Dictionary<Color, string> playerNames)
        {
            TargetSaveFile = targetSave;
            PlayerNames = playerNames;
        }

        public void Start()
        {
            DateTime lastModifiedTime = DateTime.MinValue;

            int turn = 0;

            List<ResultRecord> resultRecords = new();

            while (true)
            {
                Thread.Sleep(5000);

                Console.WriteLine("Checking save file");

                if (File.GetLastWriteTime(TargetSaveFile) > lastModifiedTime)
                {
                    Console.WriteLine($"File has changed! Writing turn {turn}");

                    try
                    {
                        var board = BoardInitializer.Initialize(isWithChecks: true);

                        string json = File.ReadAllText(TargetSaveFile);

                        TTSSaveParser.Read(json, board);

                        resultRecords.AddRange(board.GetResults(turn, PlayerNames));

                        board.PrintResults();

                        WriteResults(resultRecords);

                        lastModifiedTime = File.GetLastWriteTime(TargetSaveFile);

                        ++turn;
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Exception! Trying next time later...");
                        Console.ResetColor();
                        continue;
                    }
                }
            }
        }

        public void WriteResults(List<ResultRecord> resultRecords)
        {
            var file = new FileInfo("Results.xlsx");

            var sheetName = "Лист1";

            using var package = new ExcelPackage(file);


            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName) ??
                                              package.Workbook.Worksheets.Add(sheetName);

            worksheet.Cells.Clear();

            SetHeaders(worksheet);
            SetData(worksheet, resultRecords);
            SetFormat(worksheet);

            package.Save();
        }

        private void SetData(ExcelWorksheet worksheet, List<ResultRecord> resultRecords)
        {
            int row = 2;

            foreach (var line in resultRecords)
            {
                worksheet.Cells[row, (int)(object)Columns.COLOR].Value = line.Color;
                worksheet.Cells[row, (int)(object)Columns.NAME].Value = line.Name;
                worksheet.Cells[row, (int)(object)Columns.STATISTIC].Value = line.Statictic;
                worksheet.Cells[row, (int)(object)Columns.TIMESTAMP].Value = line.TimeStamp;
                worksheet.Cells[row, (int)(object)Columns.TURN].Value = line.Turn;
                worksheet.Cells[row, (int)(object)Columns.VALUE].Value = line.Value;

                ++row;
            }
        }

        private void SetFormat(ExcelWorksheet worksheet) 
        {
            worksheet.View.FreezePanes(2, 1);
            var dim = worksheet.Dimension;
            worksheet.Cells[dim.Start.Row, dim.Start.Column, dim.End.Row, dim.End.Column].AutoFilter = true;
            for (int i = 1; i <= headers.Count; i++)
            {
                worksheet.Column(i).AutoFit();
            }
            worksheet.Column((int)(object)Columns.TIMESTAMP).Style.Numberformat.Format = "dd.mm.yyyy HH:mm";
            worksheet.Column((int)(object)Columns.TIMESTAMP).AutoFit();
        }

        private void SetHeaders(ExcelWorksheet worksheet)
        {
            for (int i = 1; i <= headers.Count; i++)
            {
                worksheet.Cells[1, i].Value = headers[(Columns)(object)i];
            }
        }
    }
}
