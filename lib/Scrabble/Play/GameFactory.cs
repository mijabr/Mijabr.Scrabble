using Scrabble.Value;
using System;
using System.Collections.Generic;

namespace Scrabble.Play
{
    public class GameFactory : IGameFactory
    {
        IDateTimeOffset dateTimeOffset;
        public GameFactory(IDateTimeOffset dateTimeOffset)
        {
            this.dateTimeOffset = dateTimeOffset;
        }

        public Game NewGame()
        {
            Game game = new Game();

            game.Id = Guid.NewGuid();
            game.StartTime = dateTimeOffset.Now();
            game.LastActiveTime = dateTimeOffset.Now();

            game.BagTiles = new List<Tile>()
            {
                new Tile(' '), new Tile(' '),
                new Tile('E'), new Tile('E'), new Tile('E'), new Tile('E'), new Tile('E'),
                new Tile('E'), new Tile('E'), new Tile('E'), new Tile('E'), new Tile('E'),
                new Tile('E'), new Tile('E'),
                new Tile('A'), new Tile('A'), new Tile('A'), new Tile('A'), new Tile('A'),
                new Tile('A'), new Tile('A'), new Tile('A'), new Tile('A'),
                new Tile('I'), new Tile('I'), new Tile('I'), new Tile('I'), new Tile('I'),
                new Tile('I'), new Tile('I'), new Tile('I'), new Tile('I'),
                new Tile('O'), new Tile('O'), new Tile('O'), new Tile('O'), new Tile('O'),
                new Tile('O'), new Tile('O'), new Tile('O'),
                new Tile('N'), new Tile('N'), new Tile('N'), new Tile('N'), new Tile('N'),
                new Tile('N'),
                new Tile('R'), new Tile('R'), new Tile('R'), new Tile('R'), new Tile('R'),
                new Tile('R'),
                new Tile('T'), new Tile('T'), new Tile('T'), new Tile('T'), new Tile('T'),
                new Tile('T'),
                new Tile('L'), new Tile('L'), new Tile('L'), new Tile('L'),
                new Tile('S'), new Tile('S'), new Tile('S'), new Tile('S'),
                new Tile('U'), new Tile('U'), new Tile('U'), new Tile('U'),
                new Tile('D'), new Tile('D'), new Tile('D'), new Tile('D'),
                new Tile('G'), new Tile('G'), new Tile('G'),
                new Tile('B'), new Tile('B'),
                new Tile('C'), new Tile('C'),
                new Tile('M'), new Tile('M'),
                new Tile('P'), new Tile('P'),
                new Tile('F'), new Tile('F'),
                new Tile('H'), new Tile('H'),
                new Tile('V'), new Tile('V'),
                new Tile('W'), new Tile('W'),
                new Tile('Y'), new Tile('Y'),
                new Tile('K'),
                new Tile('J'),
                new Tile('X'),
                new Tile('Q'),
                new Tile('Z')
            };

            game.Players = new List<Player>()
            {
                new Player() { Name = "Player" }, new Player() { Name = "Scrabble Bot" }
            };

            return game;
        }
    }
}
