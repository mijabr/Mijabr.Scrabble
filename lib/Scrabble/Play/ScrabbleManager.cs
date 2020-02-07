using Scrabble.Ai;
using Scrabble.Draw;
using Scrabble.Go;
using Scrabble.Persist;
using Scrabble.Value;
using System;
using System.Collections.Generic;

namespace Scrabble.Play
{
    public class ScrabbleManager : IScrabbleManager
    {
        IGameFactory gameFactory;
        Board board;
        ITileDrawer drawer;
        IGoHandler goHandler;
        IAiGoHandler aiGoHandler;
        IGameRepository gameRepo;
        public ScrabbleManager(
            IGameFactory gameFactory,
            Board board,
            ITileDrawer drawer,
            IGoHandler goHandler,
            IAiGoHandler aiGoHandler,
            IGameRepository gameRepo)
        {
            this.gameFactory = gameFactory;
            this.board = board;
            this.drawer = drawer;
            this.goHandler = goHandler;
            this.aiGoHandler = aiGoHandler;
            this.gameRepo = gameRepo;
        }

        public IEnumerable<BoardSquare> GetSquares()
        {
            return board.Squares;
        }

        public Game NewGame(string playerName)
        {
            var game = gameFactory.NewGame(playerName);
            drawer.DrawTilesForAllPlayers(game);
            gameRepo.Set(game);
            return game;
        }

        public GoResult SubmitGo(Game game)
        {
            var result = goHandler.Go(game);
            if (result.IsValid)
            {
                game.LastActiveTime = DateTimeOffset.Now;
                gameRepo.Set(result.Game);
            }

            return result;
        }

        public GoResult AiGo(Game game)
        {
            return aiGoHandler.Go(game);
        }

        public List<ShortGame> ShortList()
        {
            return gameRepo.GetShortList();
        }
    }
}
