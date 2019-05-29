using System;
using System.Collections.Generic;

namespace Mastermind
{
    public class Game
    {
        private readonly int[] _answer;
        private HashSet<int> _answerValues = new HashSet<int>();
        private int _turns = 0;

        public int Difficulty { get; private set; }
        public int TopRange { get; private set; }

        public Game(int _difficulty = 4, int _topRange = 6)
        {
            if (_difficulty <= 0)
            {
                throw new ArgumentOutOfRangeException($"The value {_difficulty} is outside of the allowable range");
            }

            if(_topRange > 9 || _topRange <= 0)
            {
                throw new ArgumentOutOfRangeException($"The value {_topRange} is outside of the specified range");
            }

            WriteToPlayer("Constructing game");

            Difficulty = _difficulty;
            TopRange = _topRange;

            var rand = new Random(Seed: DateTime.Now.Second);
            _answer = new int[this.Difficulty];
            for (var i = 0; i < this.Difficulty; i++)
            {
                var newVal = rand.Next(0, this.TopRange);
                _answer[i] = newVal;

            }

        }

        public bool CheckGuess(int[] guess, int[] answer, out int[] results)
        {
            if (guess.Length != answer.Length)
            {
                throw new ArgumentException("The guess it not the correct length");
            }

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
            results = new int[] { correctPieces, incorrectPieces };

            if (correctPieces == answer.Length)
            {
                return true;
            }

            return false;

        }

        public void PlayGame()
        {
            WriteToPlayer("Please enter your guess", false);

            while (++_turns <= 10)
            {
                var originalGuess = Console.ReadLine();
                if(int.TryParse(originalGuess, out var guess))
                {
                    // break apart into array
                    var nums = new List<int>();
                    for (; guess != 0; guess /= 10)
                    {
                        nums.Add(guess % 10);
                    }
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
                        WriteToPlayer("Congrats!  You guessed correctly");
                        return;
                    }
                   
                    WriteToPlayer($"{new String('+', results[0])}{Environment.NewLine}{new String('-', results[1])}", true);
                    WriteToPlayer("Please enter your guess", false);

                }
                else
                {
                    WriteToPlayer($"{originalGuess} is invalid.  Please try again", false);
                }
            }
        }


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



    }
}
