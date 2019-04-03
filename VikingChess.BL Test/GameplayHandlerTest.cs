using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using VikingChessBL;

namespace VikingChess.BL_Test
{
    [TestClass]
    public class GameplayHandlerTest
    {
        GameSetup gameSetup = new GameSetup();
        CollisionHandler collisionHandler = new CollisionHandler();
        LegalMove legalMove = new LegalMove();
        PlayBoard board = new PlayBoard(11, 11, new Vector2(0));
        GameplayHandler gameplayHandler;

        public GameplayHandlerTest()
        {
            board.SetRules(doesAttackersHaveKing: false,
                           doesDefendersHaveKing: true,
                           doesAttackerKingWantsToFlee: false,
                           doesDefenderKingWantsToFlee: true);
            Board = board;
            gameplayHandler = new GameplayHandler(Board);
            Board.Turn = 5;
        }

        public PlayBoard Board { get; set; }

        [TestMethod]
        public void KillCheckNormalPieceTest_KillPiece()
        {
            //Arrange
            var position = new Vector2(0);
            Board.Board[2, 2] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[2, 2].MovedInTurn = 5;
            Board.Board[2, 3] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, position);
            Board.Board[2, 3].MovedInTurn = 4;
            Board.Board[2, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[2, 4].MovedInTurn = 3;

            //Act
            gameplayHandler.KillCheckNormalPiece(2, 3, Piece.teams.attackers);

            //Assert
            Assert.AreEqual(null, gameplayHandler.Board.Board[2, 3]);
        }

        [TestMethod]
        public void KillCheckNormalPieceTest_DontKillPiece()
        {
            //Arrange
            var position = new Vector2(0);
            Board.Board[2, 2] = null;
            Board.Board[2, 3] = new Piece(Piece.teams.defenders, Piece.types.normalPiece, position);
            Board.Board[2, 3].MovedInTurn = 4;
            Board.Board[2, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[2, 4].MovedInTurn = 3;

            //Act
            var expected = Board.Board[2, 3];
            gameplayHandler.KillCheckNormalPiece(2, 3, Piece.teams.attackers);

            //Assert
            Assert.AreEqual(expected, gameplayHandler.Board.Board[2, 3]);
        }

        [TestMethod]
        public void KillCheckKingPieceTest_KillKing()
        {
            //Arrange
            var position = new Vector2(0);
            Board.Board[2, 3] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[3, 2] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[3, 3] = new Piece(Piece.teams.defenders, Piece.types.kingPiece, position); //King
            Board.Board[3, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[4, 3] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);

            //Act
            gameplayHandler.KillCheckKingPiece(3, 3, Piece.teams.attackers);

            //Assert
            Assert.AreEqual(null, gameplayHandler.Board.Board[3, 3]);
        }

        [TestMethod]
        public void KillCheckKingPieceTest_DontKillKing()
        {
            //Arrange
            var position = new Vector2(0);
            Board.Board[2, 3] = null;
            Board.Board[3, 2] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[3, 3] = new Piece(Piece.teams.defenders, Piece.types.kingPiece, position); //King
            Board.Board[3, 4] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            Board.Board[4, 3] = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);

            //Act
            var expected = Board.Board[3, 3];
            gameplayHandler.KillCheckKingPiece(3, 3, Piece.teams.attackers);

            //Assert
            Assert.AreEqual(expected, gameplayHandler.Board.Board[3, 3]);
        }

        [TestMethod]
        public void IsThereAPieceOnEachSideTest_TwoPieces()
        {
            //Arrange
            var position = new Vector2(0);
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            Piece piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 3;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 5;
            var piece2 = new Piece(Piece.teams.refuge, Piece.types.normalPiece, position);

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
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 8;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            var piece1 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
            piece1.MovedInTurn = 8;
            var piece2 = new Piece(Piece.teams.attackers, Piece.types.normalPiece, position);
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
            gameplayHandler.WinConditionsCheck();

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
            gameplayHandler.WinConditionsCheck();

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
            gameplayHandler.WinConditionsCheck();

            //Assert
            Assert.AreEqual(PlayBoard.gameState.defenderWin, Board.State);
        }
    }
}
