using System;

namespace Mastermind
{
    class Program
    {

        static void Main(string[] args)
        {
            var game = new Game();
            /*
             * not doing a bunch of error handling on this level of the game
             * but if logging were going to be put in then we can specify the
             * messages accordingly           
            */
            try
            {
                game.PlayGame();
            } catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            } catch(InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            } catch (Exception)
            {
                // wouldn't want to tell the player this message but in case we wanna log...
                Console.WriteLine("The game encountered an unknown problem and must end");
            }
        
        }
    }
}
