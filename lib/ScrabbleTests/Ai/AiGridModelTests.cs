using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scrabble.Ai;
using Scrabble.Value;
using Shouldly;
using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Tests
{
    [TestClass]
    public class AiGridModelTests
    {
        AiGridModel aiGridModel;
        List<Tile> boardTiles;
        List<Tile> playerTiles;
        IEnumerable<AiCandidate> candidates;

        [TestInitialize]
        public void SetUp()
        {
            aiGridModel = new AiGridModel();
            boardTiles = new List<Tile>();
            playerTiles = new List<Tile>();
            candidates = null;
        }

        public void WhenBuildAiGridModel()
        {
            aiGridModel.Build(playerTiles, boardTiles);
            candidates = aiGridModel.Candidates.ToList();
        }

        private void AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed(string pattern, int orientation, int x, int y, int tilesUsed)
        {
            candidates.First(c =>
               c.SearchPattern == pattern &&
               c.Orientation == orientation &&
               c.StartX == x &&
               c.StartY == y &&
               c.TilesUsed == tilesUsed
            );
        }

        [TestMethod]
        public void GivenSomeBoardTiles_TilesShouldBeInTheModel()
        {
            boardTiles.Add(new Tile() { BoardPositionX = 7, BoardPositionY = 7, Letter = 'A', Value = 3 });
            WhenBuildAiGridModel();
            aiGridModel.Grid[7, 7].Letter.ShouldBe('A');
            aiGridModel.Grid[7, 7].TileValue.ShouldBe(3);
        }

        [TestMethod]
        public void GivenSomeBoardTiles_ThenSpacesNextToBoardTilesShouldBeMarkedAsNextToATile()
        {
            boardTiles.Add(new Tile() { BoardPositionX = 7, BoardPositionY = 7, Letter = 'A' });
            WhenBuildAiGridModel();
            aiGridModel.Grid[6, 7].IsNextToTile.ShouldBeTrue();
            aiGridModel.Grid[8, 7].IsNextToTile.ShouldBeTrue();
            aiGridModel.Grid[7, 6].IsNextToTile.ShouldBeTrue();
            aiGridModel.Grid[7, 8].IsNextToTile.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenNoBoardTiles_ThenTheCentreSquareShouldBeConsideredAsNextToATile()
        {
            WhenBuildAiGridModel();
            aiGridModel.Grid[7, 7].IsNextToTile.ShouldBeTrue();
        }

        [TestMethod]
        public void GivenNoPlayerTiles_ThenThereAreNoAiGoCandidates()
        {
            WhenBuildAiGridModel();
            aiGridModel.Candidates.Count().ShouldBe(0);
        }

        [TestMethod]
        public void GivenOnePlayerTile_AndNoBoardTiles_ThenTheOnlyCandidateIsCentreSquare()
        {
            playerTiles.Add(new Tile() { Letter = 'A' });
            WhenBuildAiGridModel();
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?", 0, 7, 7, 1);
            candidates.Count().ShouldBe(1);
        }

        [TestMethod]
        public void GivenTwoPlayerTiles_AndNoBoardTiles_ThenThereShouldBeFiveCandidates()
        {
            playerTiles.Add(new Tile() { Letter = 'A' });
            playerTiles.Add(new Tile() { Letter = 'A' });
            WhenBuildAiGridModel();
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?", 0, 7, 7, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("??", 0, 6, 7, 2);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("??", 0, 7, 7, 2);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("??", 1, 7, 6, 2);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("??", 1, 7, 7, 2);
            candidates.Count().ShouldBe(5);
        }

        [TestMethod]
        public void GivenOnePlayerTile_AndOneBoardTileInCentre_ThenThereShouldBeFourCandidates()
        {
            playerTiles.Add(new Tile() { Letter = 'A' });
            boardTiles.Add(new Tile() { Letter = 'A', BoardPositionX = 7, BoardPositionY = 7 });
            WhenBuildAiGridModel();
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?A", 0, 6, 7, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("A?", 0, 7, 7, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?A", 1, 7, 6, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("A?", 1, 7, 7, 1);
            candidates.Count().ShouldBe(4);
        }

        [TestMethod]
        public void GivenOnePlayerTile_AndOneBoardTileInTopLeftCorner_ThenThereShouldBeThreeCandidates()
        {
            playerTiles.Add(new Tile() { Letter = 'A' });
            boardTiles.Add(new Tile() { Letter = 'A', BoardPositionX = 0, BoardPositionY = 0 });
            WhenBuildAiGridModel();
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?", 0, 7, 7, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("A?", 0, 0, 0, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("A?", 1, 0, 0, 1);
            candidates.Count().ShouldBe(3);
        }

        [TestMethod]
        public void GivenOnePlayerTile_AndOneBoardTileInBottomRightCorner_ThenThereShouldBeThreeCandidates()
        {
            playerTiles.Add(new Tile() { Letter = 'A' });
            boardTiles.Add(new Tile() { Letter = 'A', BoardPositionX = 14, BoardPositionY = 14 });
            WhenBuildAiGridModel();
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?", 0, 7, 7, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?A", 0, 13, 14, 1);
            AssertCandidateHasPattern_Orientation_X_Y_And_TilesUsed("?A", 1, 14, 13, 1);
            candidates.Count().ShouldBe(3);
        }
    }
}
