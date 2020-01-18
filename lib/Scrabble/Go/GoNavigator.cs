namespace Scrabble.Go
{
    internal struct GoNavigator
    {
        public GoNavigator(int x, int y, bool isHorizontal)
        {
            this.isHorizontal = isHorizontal;
            X = x;
            Y = y;
        }

        public void Copy(GoNavigator nav)
        {
            this.isHorizontal = nav.isHorizontal;
            X = nav.X;
            Y = nav.Y;
        }

        bool isHorizontal;
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Main
        {
            get
            {
                return isHorizontal ? X : Y;
            }

            set
            {
                if (isHorizontal)
                {
                    X = value;
                }
                else
                {
                    Y = value;
                }
            }
        }

        public int Side
        {
            get
            {
                return isHorizontal ? Y : X;
            }

            set
            {
                if (isHorizontal)
                {
                    Y = value;
                }
                else
                {
                    X = value;
                }
            }
        }
    }
}
