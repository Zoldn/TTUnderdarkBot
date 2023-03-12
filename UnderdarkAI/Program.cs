using TUnderdark.Model;
using TUnderdark.TTSParser;
using UnderdarkAI.AI;
using UnderdarkAI.Context;
using UnderdarkAI.IO;
using UnderdarkAI.Utils;

namespace UnderdarkAI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var context = new ModelContext();

            // FillContext(context);

            var excelManager = new CardExcelManager(@"..\..\..\..\TUnderdark\Resources\Cards.xlsx");

            excelManager.Load(context);

            CardMapper.ReadCardsFromContext(context);

            //CardMapper.ReadCards();

            //ArgumentParser.Parse("-color=y -turn=1 -iters=50", out var parseResultInfo, out var parsedArgs);

            Console.WriteLine("Reading current save");

            var board = BoardInitializer.Initialize(isWithChecks: false);

            //string json = TTSLoader.GetJson(isLastSave: false, saveName: @"TS_Save_70.json");
            string json = TTSLoader.GetJson(isLastSave: true);

            TTSSaveParser.Read(json, board);

            //board.Players[Color.GREEN].Hand.Remove(
            //    board.Players[Color.GREEN].Hand.First(d => d.SpecificType == CardSpecificType.GRIMLOCK)
            //    );

            //board.Players[Color.GREEN].Hand.Add(
            //    CardMapper.SpecificTypeCardMakers[CardSpecificType.GRIMLOCK].Clone()
            //    );

            var turnMaker = new TurnMaker(board, Color.GREEN, currentRound: 0) //, seed: 8984314
            {
                RestartLimit = 400,
            };

            // turnMaker.AddForcedDiscardForCurrentPlayer(sourcePlayer: Color.RED);

            var resultTurn = turnMaker.MakeTurn();

            Console.WriteLine(resultTurn.Print());
            //Console.WriteLine(resultTurn.PrintDiscard());

            //board.PrintResults();

            Console.ReadLine();
        }

        private static void FillContext(ModelContext context)
        {
            foreach (var (_, card) in CardMapper.SpecificTypeCardMakers)
            {
                context.CardsStats.Add(new Context.ContextElements.CardStats()
                {
                    CardSpecificType = card.SpecificType,
                    CardType = card.CardType,
                    ManaCost = card.ManaCost,
                    Name = card.Name,
                    PromoteVP = card.PromoteVP,
                    Race = card.Race,
                    VP = card.VP,
                });
            }
        }
    }
}