using Scrabble.Value;
using System;
using System.Collections.Generic;

namespace Scrabble.Persist
{
    public class GameRepository : IGameRepository
    {
        public void Set(Game game)
        {
            games[game.Id] = game;
        }

        public Game GetById(Guid id)
        {
            return games.TryGetValue(id, out var game) ? game : null;
        }

        private readonly Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();

        public List<ShortGame> GetShortList()
        {
            var shortList = new List<ShortGame>();
            foreach (var game in games)
            {
                var shortGame = new ShortGame()
                {
                    Id = game.Key,
                    StartTime = game.Value.StartTime,
                    LastActiveTime = game.Value.LastActiveTime
                };

                foreach (var player in game.Value.Players)
                {
                    shortGame.Player.Add(new ShortPlayer()
                    {
                        Name = player.Name,
                        Score = player.Score
                    });
                }

                shortList.Add(shortGame);
            }

            return shortList;
        }
    }
}
