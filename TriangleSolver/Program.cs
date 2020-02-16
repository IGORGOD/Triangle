using System;
using System.IO;
using static System.Console;

namespace TriangleSolver
{
    internal class Program
    {
        private const string ExampleTrianglePath = "../../exampleTriangle.txt";
        private const string FirstTrianglePath = "../../firstTriangle.txt";
        private const string SecondTrianglePath = "../../secondTriangle.txt";

        public static void Main(string[] args)
        {
            int mode;
            Triangle triangle = null;
            WriteLine("TriangleSolver Started\n");
            WriteLine("Choose Scenario:");
            do
            {
                WriteLine("0 - Exit");
                WriteLine("1 - Initialize Triangle");
                if (triangle != null)
                    WriteLine("2 - Solve Triangle");
                if (!int.TryParse(ReadLine(), out mode))
                    mode = -1;

                switch (mode)
                {
                    case 0:
                        WriteLine("I'm done here.\nHave a nice day!");
                        ReadKey();
                        break;
                    case 1:
                        do
                        {
                            WriteLine("\nChoose Triangle to Load:\n");
                            WriteLine("0 - Return to Previous Screen");
                            WriteLine("1 - Load Example Triangle");
                            WriteLine("2 - Load First Triangle");
                            WriteLine("3 - Load Second Triangle");
                            WriteLine("4 - Load Non-Default Triangle");
                            if (!int.TryParse(ReadLine(), out mode))
                                mode = -1;
                            string path;
                            switch (mode)
                            {
                                case 0:
                                    continue;
                                case 1:
                                    path = ExampleTrianglePath;
                                    break;
                                case 2:
                                    path = FirstTrianglePath;
                                    break;
                                case 3:
                                    path = SecondTrianglePath;
                                    break;
                                case 4:
                                    path = ReadLine();
                                    break;
                                default:
                                    WriteLine("Something went wrong. Please try again.");
                                    continue;
                            }

                            triangle = ReadTriangleFromFile(path);
                            if (triangle != null)
                                mode = 0;
                        } while (mode != 0);

                        WriteLine();
                        mode = -1;
                        break;
                    case 2:
                        if (triangle == null)
                            goto default;
                        WriteLine($"{triangle.GetMaxRoute()}\n");
                        break;
                    default:
                        WriteLine("Something went wrong. Please try again.");
                        break;
                }
            } while (mode != 0);
        }

        private static Triangle ReadTriangleFromFile(string path)
        {
            try
            {
                var triangle = new Triangle(path);
                WriteLine("Triangle was successfully loaded from a file.");
                return triangle;
            }
            catch (ArgumentNullException)
            {
                WriteLine("Null or Empty file path was provided, please try another filepath.");
            }
            catch (FileNotFoundException)
            {
                WriteLine("File is not exist, please try another filepath.");
            }
            catch (Exception e)
            {
                WriteLine(e.Message);
            }

            return null;
        }
    }
}