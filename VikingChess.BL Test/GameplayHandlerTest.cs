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
            board.Turn = 5;
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_TwoPieces()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece2.MovedInTurn = 4;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.attackers);
            var expected = true;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_OnePiece()
        {
            //Arrange
            var position = new Vector2(0);
            Piece piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 5;
            Piece piece2 = null;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.attackers);
            var expected = false;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_WrongTurn()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 3;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece2.MovedInTurn = 4;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.attackers);
            var expected = false;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_WrongTeam()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece2.MovedInTurn = 4;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.defenders);
            var expected = false;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsThereAPieceOnEachSideWithoutTurnCheckTest_WrongTurn()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 8;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece2.MovedInTurn = 1;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSideWithoutTurnCheck(piece1, piece2, Piece.teams.attackers);
            var expected = true;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsThereAPieceOnEachSideWithoutTurnCheckTest_WrongTeam()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 8;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece2.MovedInTurn = 1;

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSideWithoutTurnCheck(piece1, piece2, Piece.teams.defenders);
            var expected = false;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
