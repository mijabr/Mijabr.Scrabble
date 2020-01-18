using System;

namespace Scrabble.Play
{
    public class DateTimeOffsetClass : IDateTimeOffset
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}
