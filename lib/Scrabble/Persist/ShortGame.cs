using System;
using System.Collections.Generic;

namespace Scrabble.Persist
{
    public class ShortPlayer
    {
        public string Name { get; set; }
        public int  Score { get; set; }
    }

    public class ShortGame
    {
        public Guid Id { get; set; }
        public List<ShortPlayer> Player { get; set; } = new List<ShortPlayer>();
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset LastActiveTime { get; set; }
    }
}
