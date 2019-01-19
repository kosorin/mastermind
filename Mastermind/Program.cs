using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mastermind
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var numbers = ParseNumbers(args);
            if (numbers == null)
            {
                return;
            }

            var options = Options.Default;

            try
            {
                var palette = new Palette(numbers[0]);
                var pattern = new PegPattern(palette, numbers.Skip(1).ToList());

                if (!options.Quiet && !CheckExecutionTime(palette.PegCount, pattern.Size))
                {
                    return;
                }

                var board = new Board(pattern, options);
                var solver = new Solver(board);
                if (!solver.Solve())
                {
                    Console.WriteLine("It's too difficult to solve :(");
                }
                Console.WriteLine(board.ToPrintString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        private static List<int> ParseNumbers(string[] args)
        {
            var numbers = new List<int>();

            try
            {
                numbers.AddRange(args.Select(x => int.Parse(x)));
            }
            catch
            {
                Console.WriteLine("Unable to parse command line arguments.");
                return null;
            }

            if (numbers.Count < 2)
            {
                Console.WriteLine("Usage:   Program.exe <Palette> <Pattern>");
                Console.WriteLine("Example: Program.exe 6         4 1 2 2");
                return null;
            }

            return numbers;
        }

        private static bool CheckExecutionTime(int palette, int size)
        {
            if (Math.Pow(palette, size) > 1_000_000)
            {
                while (true)
                {
                    Console.Write("It can take some time to solve. Continue? [Y/n]: ");
                    var answer = Console.ReadLine()?.Trim().ToUpper();
                    if (string.IsNullOrWhiteSpace(answer) || answer == "Y")
                    {
                        break;
                    }
                    else if (answer == "N")
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
