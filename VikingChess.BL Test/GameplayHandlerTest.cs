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
            Board = board;
            gameplayHandler = new GameplayHandler(Board);
            Board.Turn = 5;
        }

        public PlayBoard Board { get; set; }

        [TestMethod]
        public void KillCheckNormalPieceTest()
        {
            //TODO: Get this to work

            //Arrange
            var position = new Vector2(0);
            Board.Board[2, 2] = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            Board.Board[2, 2].MovedInTurn = 5;
            Board.Board[2, 3] = new Piece(Piece.teams.defenders, Piece.types.normal, position);
            Board.Board[2, 2].MovedInTurn = 4;
            Board.Board[2, 4] = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            Board.Board[2, 2].MovedInTurn = 3;

            //Act
            gameplayHandler.KillCheckNormalPiece(2, 3, Piece.teams.attackers);

            //Assert
            Assert.AreEqual(null, Board.Board[2, 3]);

        }

        [TestMethod]
        public void KillCheckKingPieceTest()
        {
            //TODO: Unit test
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

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_Refuge()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normal, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.refuge, Piece.types.normal, position);

            //Act
            var actual = gameplayHandler.IsThereAPieceOnEachSide(piece1, piece2, Piece.teams.attackers);
            var expected = true;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void WinConditionCheckTest_NoWin()
        {
            //Arrange
            Board.State = PlayBoard.gameState.attackerTurn;

            //Act
            gameplayHandler.WinConditionCheck();

            //Assert
            Assert.AreEqual(PlayBoard.gameState.attackerTurn, Board.State);
        }

        [TestMethod]
        public void WinConditionCheckTest_AttackersWin()
        {
            //Arrange
            Board.State = PlayBoard.gameState.attackerTurn;
            Board.DoesDefendersHaveKing = true;
            Board.Board[5, 5] = null;

            //Act
            gameplayHandler.WinConditionCheck();

            //Assert
            Assert.AreEqual(PlayBoard.gameState.attackerWin, Board.State);
        }

        [TestMethod]
        public void WinConditionCheckTest_DefendersWin()
        {
            //Arrange
            Board.State = PlayBoard.gameState.defenderTurn;
            Board.DoesAttackersHaveKing = true;

            //Act
            gameplayHandler.WinConditionCheck();

            //Assert
            Assert.AreEqual(PlayBoard.gameState.defenderWin, Board.State);
        }
    }
}
