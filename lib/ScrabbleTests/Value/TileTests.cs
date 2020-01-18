
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Value;
using Shouldly;

namespace Scrabble.Tests
{
    [TestClass]
    public class TileTests
    {
        [TestMethod]
        public void TilesAreCreatedWithCorrectValues()
        {
            new Tile(' ').Value.ShouldBe(0, "blank value should be 0");
            new Tile('E').Value.ShouldBe(1, "E value should be 1");
            new Tile('A').Value.ShouldBe(1, "A value should be 1");
            new Tile('I').Value.ShouldBe(1, "I value should be 1");
            new Tile('O').Value.ShouldBe(1, "O value should be 1");
            new Tile('N').Value.ShouldBe(1, "N value should be 1");
            new Tile('R').Value.ShouldBe(1, "R value should be 1");
            new Tile('T').Value.ShouldBe(1, "T value should be 1");
            new Tile('L').Value.ShouldBe(1, "L value should be 1");
            new Tile('S').Value.ShouldBe(1, "S value should be 1");
            new Tile('U').Value.ShouldBe(1, "U value should be 1");
            new Tile('D').Value.ShouldBe(2, "D value should be 2");
            new Tile('G').Value.ShouldBe(2, "G value should be 2");
            new Tile('B').Value.ShouldBe(3, "B value should be 3");
            new Tile('C').Value.ShouldBe(3, "C value should be 3");
            new Tile('M').Value.ShouldBe(3, "M value should be 3");
            new Tile('P').Value.ShouldBe(3, "P value should be 3");
            new Tile('F').Value.ShouldBe(4, "F value should be 4");
            new Tile('H').Value.ShouldBe(4, "H value should be 4");
            new Tile('V').Value.ShouldBe(4, "V value should be 4");
            new Tile('W').Value.ShouldBe(4, "W value should be 4");
            new Tile('Y').Value.ShouldBe(4, "Y value should be 4");
            new Tile('K').Value.ShouldBe(5, "K value should be 5");
            new Tile('J').Value.ShouldBe(8, "J value should be 8");
            new Tile('X').Value.ShouldBe(8, "X value should be 8");
            new Tile('Q').Value.ShouldBe(10, "Q value should be 10");
            new Tile('Z').Value.ShouldBe(10, "Z value should be 10");
        }

        [TestMethod]
        public void TilesShouldStartInTheTileBag()
        {
            new Tile('A').Location.ShouldBe("bag");
        }
    }
}
