// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathFinding.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that dan do different kinds of pathfinding.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Pathfinding
{
    using System;
    using System.Collections.Generic;

    using AgeOfDragons.Helper_Classes;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;

    /// <summary>
    /// A class that dan do different kinds of pathfinding.
    /// </summary>
    public static class PathFinding
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        #endregion

        #region Method Region
        /// <summary>
        /// Finds the areas the unit can move to.
        /// </summary>
        /// <param name="unit"> The unit.  </param>
        /// <param name="dataMap"> The datamap.  </param>
        /// <returns>
        /// The list of vectors.
        /// </returns>
        public static List<Vector> MarkValidMoves(Unit unit, MapData dataMap)
        {
            var x = unit.Location.X;
            var y = unit.Location.Y;

            var startVector = new Vector(unit.Location.X, unit.Location.Y);

            var validMovesList = new List<Vector>();
            var validMoves = new LinkedList<Vector>();

            validMovesList.Add(startVector);
            validMoves.AddFirst(startVector);

            while (validMoves.Count > 0)
            {
                var moveNode = validMoves.First.Value;
                validMoves.RemoveFirst();

                foreach (var neighborNode in NeighborNodes(moveNode))
                {
                    var tempVector = new Vector(neighborNode.X, neighborNode.Y);

                    if (validMovesList.Contains(tempVector) ||
                        !unit.PointWithinMoveRange(tempVector))
                    {
                        continue;
                    }

                    if (dataMap.WithinMapMoveRangeAndCanTraverse(tempVector.X, tempVector.Y, unit))
                    {
                        validMoves.AddLast(tempVector);
                        validMovesList.Add(tempVector);
                    }
                }
            }

            return validMovesList;
        }

        /// <summary>
        /// Finds the shortest path between the start vector and the goal vector,
        /// taking into account whether the unit can actually get there or not.
        /// </summary>
        /// <param name="start"> The vector to start the search from. </param>
        /// <param name="goal"> The vector to end the search at. </param>
        /// <param name="unit"> The unit that is trying to find the route. </param>
        /// <param name="dataMap"> The data map. </param>
        /// <returns>
        /// A list representing the shortest path.
        /// </returns>
        public static List<Vector> FindShortestPathWithinReach(Vector start, Vector goal, Unit unit, MapData dataMap)
        {
            var closedList = new List<Vector>();
            var openList = new List<Vector>();
            var openListQueue = new PriorityQueue<Vector, int>();

            var newStart = start;
            newStart.CameFrom = null;
            newStart.GScore = 0;
            newStart.FScore = newStart.GScore + CostEstimate(newStart, goal);

            openList.Add(newStart);
            openListQueue.Enqueue(newStart);

            if (newStart.Equals(goal))
            {
                return openList;
            }

            while (!openListQueue.Empty)
            {
                var current = openListQueue.Dequeue();
                openList.Remove(current);

                if (current.Equals(goal))
                {
                    return RetracePath(current);
                }

                closedList.Add(current);

                foreach (var neighbor in NeighborNodes(current))
                {
                    if (closedList.Contains(neighbor))
                    {
                        continue;
                    }

                    if (!openList.Contains(neighbor) &&
                        dataMap.WithinMapAndCanTraverse(neighbor.X, neighbor.Y, unit))
                    {
                            neighbor.CameFrom = current;
                            neighbor.GScore = current.GScore + 1;
                            neighbor.FScore = newStart.GScore + CostEstimate(neighbor, goal);

                            openList.Add(neighbor);
                            openListQueue.Enqueue(neighbor);
                    }
                }
            }

            return openList;
        }

        /// <summary>
        /// Traces the path back to construct the shortest route.
        /// </summary>
        /// <param name="retraceFrom"> The vector to retrace from. </param>
        /// <returns>
        /// A list that represents the shortest path found.
        /// </returns>
        private static List<Vector> RetracePath(Vector retraceFrom)
        {
            var tempList = new List<Vector>();
            var tempNode = retraceFrom.CameFrom;
            var maxRuns = 0;

            while (maxRuns < 500)
            {
                tempList.Add(tempNode);

                if (tempNode.CameFrom == null)
                {
                    break;
                }

                tempNode = tempNode.CameFrom;
                maxRuns++;
            }
        
            return tempList;
        }

        /// <summary>
        /// Calculates the steps it takes to move from start to target.
        /// </summary>
        /// <param name="start"> The start location. </param>
        /// <param name="target"> The target location. </param>
        /// <returns>
        /// The amount of steps it takes.
        /// </returns>
        private static int CostEstimate(Vector start, Vector target)
        {
            var temp = new Vector(start.X, start.Y);
            var cost = 0;

            while (!temp.Equals(target))
            {
                if (temp.X > target.X)
                {
                    temp = new Vector(temp.X - 1, temp.Y);
                    cost++;
                }
                else if (temp.X < target.X)
                {
                    temp = new Vector(temp.X + 1, temp.Y);
                    cost++;
                }

                if (temp.Y > target.Y)
                {
                    temp = new Vector(temp.X, temp.Y - 1);
                    cost++;
                }
                else if (temp.Y < target.Y)
                {
                    temp = new Vector(temp.X, temp.Y + 1);
                    cost++;
                }
            }

            return cost;
        }

        /// <summary>
        /// Finds the nodes that are just around the input node.
        /// </summary>
        /// <param name="node"> The node to find the neighbors for. </param>
        /// <returns>
        /// A list of the neighbor nodes.
        /// </returns>
        public static List<Vector> NeighborNodes(Vector node)
        {
            var neighbors = new List<Vector>
                {
                    new Vector(node.X - 1, node.Y),
                    new Vector(node.X, node.Y - 1),
                    new Vector(node.X + 1, node.Y),
                    new Vector(node.X, node.Y + 1)
                };

            return neighbors;
        } 

        #endregion

        #region Virtual Method region
        #endregion
    }
}
