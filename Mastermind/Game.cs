using System;
using System.Collections.Generic;

namespace Mastermind
{
    public class Game
    {
        #region Fields
        /// <summary>
        /// The game's answer stored as an int array.
        /// </summary>
        private readonly int[] _answer;
        /// <summary>
        /// Hashset used for quicker answer component lookups.
        /// </summary>
        private HashSet<int> _answerValues = new HashSet<int>();
        /// <summary>
        /// The turns the player taken.
        /// </summary>
        private int _turns = 0;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the difficulty.
        /// </summary>
        /// <value>The difficulty.</value>
        public int Difficulty { get; private set; }
        /// <summary>
        /// Gets the top range.
        /// </summary>
        /// <value>The top range.</value>
        public int TopRange { get; private set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mastermind.Game"/> class.
        /// </summary>
        /// <param name="_difficulty">Difficulty.</param>
        /// <param name="_topRange">Top range.</param>
        public Game(int _difficulty = 4, int _topRange = 6)
        {
            /*
             * The game is set up with varying difficulty levels and can be
             * top ranged up to 9 (0-9)
            */
            if (_difficulty <= 0)
            {
                throw new ArgumentOutOfRangeException($"The value {_difficulty} is outside of the allowable range");
            }

            if(_topRange > 9 || _topRange <= 0)
            {
                // cannot have a top range of 0 nor can we have more than single digit
                throw new ArgumentOutOfRangeException($"The value {_topRange} is outside of the specified range");
            }

            // let the player know that we're working here
            WriteToPlayer("Constructing game");

            // using backing properties so the methods can be more easily tested
            Difficulty = _difficulty;
            TopRange = _topRange;

            // seeding with seconds for a pseudorandom experience
            var rand = new Random(Seed: DateTime.Now.Second);
            _answer = new int[this.Difficulty];
            for (var i = 0; i < this.Difficulty; i++)
            {
                var newVal = rand.Next(0, this.TopRange);
                _answer[i] = newVal;
            }

        }

        #endregion

        /// <summary>
        /// Checks the guess.
        /// </summary>
        /// <returns><c>true</c>, if guess was checked, <c>false</c> otherwise.</returns>
        /// <param name="guess">Guess.</param>
        /// <param name="answer">Answer.</param>
        /// <param name="results">Results.</param>
        public bool CheckGuess(int[] guess, int[] answer, out int[] results)
        {
            if (guess.Length != answer.Length)
            {
                // it is unlikely that we should get here but we can't proceed
                // if we are here
                throw new ArgumentException("The guess it not the correct length");
            }

            /*
             * Using a hashset here so we can check for the existence of a 
             * misplaced number (assuming correct) 
             * this is more of optimized for each guess iteration than
             * using a loop and scanning the array repeatedly
            */
            _answerValues = new HashSet<int>();

            for(var i = 0; i < answer.Length; i++)
            {
                _answerValues.Add(answer[i]);
            }

            var correctPieces = 0;
            var incorrectPieces = 0;

            for (var i = 0; i < answer.Length; i++)
            {
                if (guess[i] == answer[i])
                {
                    correctPieces++;
                }
                else if (_answerValues.Contains(guess[i]))
                {
                    incorrectPieces++;
                }

                // else - nothing happens, keep it moving
            }

            // set the out param - could eventually use a real data
            // structure if things expand but keeping it simple for now
            results = new int[] { correctPieces, incorrectPieces };

            /*
             * the main result of this method determines if we've won or not           
            */
            if (correctPieces == answer.Length)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Plays the game.
        /// </summary>
        public void PlayGame()
        {
            WriteToPlayer("Please enter your guess", false);

            // reset the turns in case this is called again
            _turns = 0;
            while (++_turns <= 10)
            {
                var originalGuess = Console.ReadLine();

                if(int.TryParse(originalGuess, out var guess))
                {
                    /*
                     * so the guess var is a real integer thanks to the 
                     * tryparse - the next goal is to break down the
                     * integer into its single digit parts and throw them
                     * into an array so we can more easily check the
                     * individual digits for correctness                    
                    */

                    // break apart into array
                    var nums = new List<int>();
                    for (; guess != 0; guess /= 10)
                    {
                        nums.Add(guess % 10);
                    }
                    // convert to an array and reverse using built in methods
                    var numbers = nums.ToArray();

                    Array.Reverse(numbers);
                    var guessResult = CheckGuess(numbers, _answer, out int[] results);

                    if(results.Length != 2)
                    {
                        // in case the other code messes up
                        throw new InvalidOperationException("The game has encountered an error and cannot continue.");
                    }

                    if (guessResult)
                    {
                        // winner winner chicken dinner
                        WriteToPlayer("Congrats!  You guessed correctly");
                        return;
                    }
                   
                    // tell player what happened - _turns will increment itself
                    WriteToPlayer($"{new String('+', results[0])}{Environment.NewLine}{new String('-', results[1])}", true);
                    WriteToPlayer("Please enter your guess", false);

                }
                else
                {
                    WriteToPlayer($"{originalGuess} is invalid.  Please try again", false);
                }
            }
        }

        #region Helpers
        /// <summary>
        /// Writes message to player.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="includeSeparator">If set to <c>true</c> include separator.</param>
        private void WriteToPlayer(string msg, bool includeSeparator=true)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            if (includeSeparator)
            {
                Console.WriteLine("--------------------");
            }
            Console.WriteLine(msg);
            if (includeSeparator)
            {
                Console.WriteLine("--------------------");
            }

        }
        #endregion


    }
}
