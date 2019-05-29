using System;
using Mastermind;
using NUnit.Framework;

namespace Tests
{
    
    public class Tests
    {
        [Test]
        public void GameSetsUpDefaults_Test()
        {
            var game = new Game();
            Assert.AreEqual(game.Difficulty, 4);
            Assert.AreEqual(game.TopRange, 6);

        }

        [Test]
        public void GameDifficultyIsConfigurable_Test()
        {
            var game = new Game(_difficulty: 9);
            Assert.AreEqual(game.Difficulty, 9);
        }
        [Test]
        public void GameTopRangeIsConfigurable_Test()
        {
            var game = new Game(_topRange: 3);
            Assert.AreEqual(game.TopRange, 3);
        }
        [Test]
        public void GameDifficultyConfigThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>
                (() => new Game(_difficulty:-10));
        }
        [Test]
        public void GameTopRangeConfigThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>
                (()=>new Game(_topRange:20));
        }
        [Test]
        public void GameCheckGuessReturnsWin_Test()
        {
            var game = new Game();
            var guess = new int[] { 1, 2, 3, 4 };
            var answer = new int[] { 1, 2, 3, 4 };
            var gameResult = game.CheckGuess(guess, answer, out var results);

            Assert.AreEqual(gameResult, true);
            Assert.That(results, Is.EquivalentTo(new int[] {4,0}));

        }
        [Test]
        public void GameCheckGuessReturnsCorrectly_Test()
        {
            var game = new Game();
            var guess = new int[] { 1,2,3,4};
            var answer = new int[] { 1,2,5,4};
            var gameResult = game.CheckGuess(guess, answer, out var results);

            Assert.AreEqual(gameResult, false);
            Assert.That(results, Is.EquivalentTo(new int[] {3,0}));
        }


    }
}