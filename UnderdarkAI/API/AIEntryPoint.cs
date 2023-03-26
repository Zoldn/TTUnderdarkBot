using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI;
using UnderdarkAI.Context;
using UnderdarkAI.IO;

namespace UnderdarkAI.API
{
    public class AIEntryPoint
    {
        public AIEntryPoint()
        {

        }

        /// <summary>
        /// Example run args is "!run -color=g -turn=1 -iters=400"
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string RunTurn(string args)
        {
            if (!ArgumentParser.Parse(args, out var parseResultInfo, out var parsedArgs))
            {
                return parseResultInfo;
            }

            if (parsedArgs == null || !parsedArgs.Color.HasValue || !parsedArgs.TurnNumber.HasValue)
            {
                throw new ArgumentNullException();
            }

            var context = new ModelContext();

            var excelManager = new CardExcelManager(@"..\..\..\..\TUnderdark\Resources\Cards.xlsx");

            excelManager.Load(context);

            CardMapper.ReadCardsFromContext(context);

            var board = BoardInitializer.Initialize(isWithChecks: false);

            //string json = TTSLoader.GetJson(isLastSave: false, saveName: @"TS_Save_70.json");
            string json = TTSLoader.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            var turnMaker = new TurnMaker(board, parsedArgs.Color.Value, currentRound: parsedArgs.TurnNumber.Value,
                context: context)
            {
                RestartLimit = parsedArgs.Iterations,
                MonteCarloAggregateMode = AggregateMode.MAX,
                SelectBestMode = SelectBestMode.RETURN_ONLY_BEST,
            };

            if (parsedArgs.DiscardFromColor.HasValue)
            {
                turnMaker.AddForcedDiscardForCurrentPlayer(parsedArgs.DiscardFromColor.Value);
            }

            var resultTurn = turnMaker.MakeTurn();

            string ret;

            if (!parsedArgs.DiscardFromColor.HasValue)
            {
                ret = resultTurn.Print();
            }
            else
            {
                ret = resultTurn.PrintDiscard();
            }

            Console.WriteLine(ret);

            return ret;
        }
    }
}
