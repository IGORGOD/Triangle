using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Console;

namespace TriangleSolver
{
    /// <summary>
    ///     Class that presents text triangle of integers
    /// </summary>
    internal class Triangle
    {
        /// <summary>
        ///     Basic constructor, that provides loading triangle from file, has 3 prepared file
        /// </summary>
        public Triangle()
        {
            bool b;
            int[][] tri;
            do
            {
                WriteLine("Input path to file with triangle:");
                WriteLine(
                    "\t0 - for example triangle\n\t1 - for first triangle\n\t2 - for second triangle" +
                    "\n\tin toher way - full path to file with triangle");
                var s = ReadLine();
                if (s == "0")
                    s = "../../exampleTriangle.txt";
                if (s == "1")
                    s = "../../firstTriangle.txt";
                if (s == "2")
                    s = "../../secondTriangle.txt";
                b = ReadFile(s, out tri);
            } while (!b);
            Tri = tri;
        }

        /// <summary>
        ///     Property that presents Triangle as 2-dimensional array of integers
        /// </summary>
        public int[][] Tri { get; }

        /// <summary>
        ///     Method that provides reading from triangle file
        /// </summary>
        /// <param name="path">Path to triangle file</param>
        /// <param name="file">2-dimensional array, that will presents triangle</param>
        /// <returns>
        ///     result of read: true - if file exist, not empty and has triangle structure;
        ///     false - in other case
        /// </returns>
        private static bool ReadFile(string path, out int[][] file)
        {
            string[] buf;
            file = new int[0][];
            try
            {
                buf = File.ReadAllLines(path);
            }
            catch (FileNotFoundException)
            {
                WriteLine("File is not exist, please try another filepath.");
                return false;
            }
            var length = buf.Length;
            if (length == 0)
            {
                WriteLine("File is empty, please try another file.");
                return false;
            }
            file = new int[length][];
            for (var i = 0; i < length; i++)
            {
                var buf2 = buf[i].Trim().Split(' ');
                if (buf2.Length != i + 1)
                {
                    WriteLine("Triangle has wrong length, please try again.");
                    return false;
                }
                file[i] = new int[buf2.Length];
                for (var j = 0; j < buf2.Length; j++)
                    if (!int.TryParse(buf2[j], out file[i][j]))
                    {
                        WriteLine("Triangle has symbols that can't be parse in int, please try again.");
                        return false;
                    }
            }
            return true;
        }

        /// <summary>
        ///     Method that finds route from top to bottom of triangle with max weight of it
        /// </summary>
        /// <returns>String view of route includes integer value from top to bottom and route weight in the end</returns>
        public string GetMaxRoute()
        {
            var al = new List<Route>();
            Route nBuf;
            for (var i = 0; i < Tri[Tri.Length - 2].Length; i++)
            {
                nBuf = new Route();
                var j = Tri.Length - 1;
                var k = Tri[j][i] > Tri[j][i + 1] ? i : i + 1;
                nBuf.AddNode(j, k, Tri[j][k]);
                nBuf.AddNode(j - 1, i, Tri[j - 1][i]);
                al.Add(nBuf);
            }
            for (var i = Tri.Length - 3; i >= 0; i--)
            {
                var buf = Tri[i];
                var al2 = new Route[al.Count];
                al.CopyTo(al2);
                al.Clear();
                for (var j = 0; j < buf.Length; j++)
                {
                    nBuf =
                        new Route(al2[j].Weight > al2[j + 1].Weight ? al2[j] : al2[j + 1]);
                    nBuf.AddNode(i, j, Tri[i][j]);
                    al.Add(nBuf);
                }
            }
            var sb = new StringBuilder();
            int[][] ids;
            var weight = al[0].GetRoute(out ids);
            for (var i = 0; i < ids.Length; i++)
            {
                var buf = ids[i];
                sb.Append($"{Tri[buf[0]][buf[1]]} ");
                if (i != ids.Length - 1) sb.Append("-> ");
            }
            sb.Append($"\n With total weight {weight}");
            return sb.ToString();
        }

        /// <summary>
        ///     Class that presents Route as structure of indexes of nodes and weight of route
        /// </summary>
        private class Route
        {
            public Route()
            {
                Weight = 0;
                Al = new List<int[]>();
            }

            /// <summary>
            ///     Constructor, that creates new Route on data from givven Route
            /// </summary>
            /// <param name="r">Route which will used to create new</param>
            public Route(Route r)
            {
                Weight = r.Weight;
                Al = new List<int[]>(r.Al);
            }

            /// <summary>
            ///     Property that presents List of node indexes
            /// </summary>
            private List<int[]> Al { get; }

            /// <summary>
            ///     Property that presents weight of route
            /// </summary>
            public int Weight { get; private set; }

            /// <summary>
            ///     Method that provides adding a node to route
            /// </summary>
            /// <param name="i">First index from triangle</param>
            /// <param name="j">Second index from triangle</param>
            /// <param name="weight">Weight of node</param>
            public void AddNode(int i, int j, int weight)
            {
                int[] buf = {i, j};
                Al.Add(buf);
                Weight += weight;
            }

            /// <summary>
            ///     Method that reverse route and put it into givven variable
            /// </summary>
            /// <param name="ids">Array in which route will be putten</param>
            /// <returns>Weight of route</returns>
            public int GetRoute(out int[][] ids)
            {
                var buf = new List<int[]>(Al);
                buf.Reverse();
                ids = new int[buf.Count][];
                buf.CopyTo(ids);
                return Weight;
            }
        }
    }
}