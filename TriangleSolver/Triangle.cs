using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TriangleSolver
{
    /// <summary>
    ///     Class that presents text triangle of integers
    /// </summary>
    internal class Triangle
    {
        public Triangle(string filePath) : this(ReadFile(filePath))
        {
        }

        private Triangle(string[] fileLines)
        {
            TriangleMatrix = null;

            var length = fileLines.Length;
            if (fileLines.Length == 0) throw new Exception("File is empty, please try another file.");

            var nodes = new int[length][];
            for (var i = 0; i < length; i++)
            {
                var lineElems = fileLines[i].Trim().Split(' ');
                if (lineElems.Length != i + 1) throw new Exception("Triangle has wrong length, please try again.");

                nodes[i] = new int[lineElems.Length];
                if (lineElems.Where((t, j) => !int.TryParse(t, out nodes[i][j])).Any())
                    throw new Exception("Triangle has symbols that can't be parse in int, please try again.");
            }

            TriangleMatrix = nodes;
        }

        /// <summary>
        ///     Property that presents Triangle as 2-dimensional array of integers
        /// </summary>
        public int[][] TriangleMatrix { get; }

        /// <summary>
        ///     Method that provides reading from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>
        ///     File lines
        /// </returns>
        private static string[] ReadFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException();
            if (!File.Exists(path))
                throw new FileNotFoundException();
            return File.ReadAllLines(path);
        }

        /// <summary>
        ///     Method that finds route from top to bottom of triangle with max weight of it
        /// </summary>
        /// <returns>String view of route includes integer value from top to bottom and route weight in the end</returns>
        public string GetMaxRoute()
        {
            var routes = new List<Route>();
            Route route;
            for (var i = 0; i < TriangleMatrix[TriangleMatrix.Length - 2].Length; i++)
            {
                route = new Route();
                var j = TriangleMatrix.Length - 1;
                var k = TriangleMatrix[j][i] > TriangleMatrix[j][i + 1] ? i : i + 1;
                route.AddNode(j, k, TriangleMatrix[j][k]);
                route.AddNode(j - 1, i, TriangleMatrix[j - 1][i]);
                routes.Add(route);
            }

            for (var i = TriangleMatrix.Length - 3; i >= 0; i--)
            {
                var buf = TriangleMatrix[i];
                var al2 = new Route[routes.Count];
                routes.CopyTo(al2);
                routes.Clear();
                for (var j = 0; j < buf.Length; j++)
                {
                    route =
                        new Route(al2[j].Weight > al2[j + 1].Weight ? al2[j] : al2[j + 1]);
                    route.AddNode(i, j, TriangleMatrix[i][j]);
                    routes.Add(route);
                }
            }

            var sb = new StringBuilder();
            var ids = routes[0].GetRoute(out var weight);
            sb.Append($"{TriangleMatrix[ids[0][0]][ids[0][1]]} ");
            for (var i = 1; i < ids.Length; i++)
            {
                var buf = ids[i];
                sb.Append($"-> {TriangleMatrix[buf[0]][buf[1]]} ");
            }

            sb.Append($"\nWith total weight {weight}");
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
                RouteIndexes = new List<int[]>();
            }

            /// <summary>
            ///     Constructor, that creates new Route on data from given Route
            /// </summary>
            /// <param name="route">Route which will used to create new</param>
            public Route(Route route)
            {
                Weight = route.Weight;
                RouteIndexes = new List<int[]>(route.RouteIndexes);
            }

            /// <summary>
            ///     Property that presents List of node indexes
            /// </summary>
            private List<int[]> RouteIndexes { get; }

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
                RouteIndexes.Add(new[] {i, j});
                Weight += weight;
            }

            /// <summary>
            ///     Method that reverse route and put it into given variable
            /// </summary>
            /// <param name="weight">Route weight</param>
            /// <returns>Route - array of Nodes</returns>
            public int[][] GetRoute(out int weight)
            {
                weight = Weight;
                var res = new List<int[]>(RouteIndexes);
                res.Reverse();
                return res.ToArray();
            }
        }
    }
}