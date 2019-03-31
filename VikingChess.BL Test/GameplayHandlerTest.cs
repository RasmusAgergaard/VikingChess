using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using VikingChessBL;

namespace VikingChess.BL_Test
{
    [TestClass]
    public class GameplayHandlerTest
    {
        CollisionHandler collisionHandler = new CollisionHandler();
        LegalMove legalMove = new LegalMove();
        PlayBoard board = new PlayBoard(11, 11, new Vector2(0), doesAttackersHaveKing: false, doesDefendersHaveKing: true);
        GameplayHandler gameplayHandler;

        public GameplayHandlerTest()
        {
            gameplayHandler = new GameplayHandler(board);
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.attackers);
            var expected = true;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
