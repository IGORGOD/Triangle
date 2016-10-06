using static System.Console;

namespace TriangleSolver
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var t = new Triangle();
            WriteLine(t.GetMaxRoute());
            ReadKey();
        }
    }
}