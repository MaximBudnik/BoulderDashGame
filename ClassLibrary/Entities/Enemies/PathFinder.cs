using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using ClassLibrary.Matrix;

namespace ClassLibrary.Entities.Enemies {
    public class Pathfinder {
        public List<Point> FindPath(int selfX, int selfY, int targetX, int targetY, Level level,
            Func<Level, Point, bool> conditionToMove) {
            var closed = new Collection<Node>();
            var open = new Collection<Node>();

            var root = new Node {
                PositionX = selfX,
                PositionY = selfY,
                Parent = null,
                LengthFromStart = 0,
                HeuristicLength = GetHeuristicPathLength(selfX, selfY, targetX, targetY)
            };
            open.Add(root);
            while (open.Count > 0) {
                var current = open.OrderBy(node =>
                    node.FullPathLength).First();
                if (current.PositionX == targetX && current.PositionY == targetY) return GetPathForNode(current);
                open.Remove(current);
                closed.Add(current);
                //get all neighbours of current point
                foreach (var neighbour in GetNeighbours(current, level, conditionToMove, targetX, targetY)) {
                    //if neighbour is closed, ignore it
                    if (closed.Count(key =>
                        key.PositionX == neighbour.PositionX && key.PositionY == neighbour.PositionY) > 0)
                        continue;
                    var next = open.FirstOrDefault(key =>
                        key.PositionX == neighbour.PositionX && key.PositionY == neighbour.PositionY);
                    //if it is not in open, add
                    if (next == null) {
                        open.Add(neighbour);
                    }
                    //else check distance (route from neighbours to here can be less) and add 
                    else if (next.LengthFromStart > neighbour.LengthFromStart) {
                        next.Parent = current;
                        next.LengthFromStart = neighbour.LengthFromStart;
                    }
                }
            }
            return null;
        }

        private static int GetDistanceWithWeight(Level level, int posX, int posY) {
            return level[posX, posY].MoveWeight;
        }

        private Collection<Node> GetNeighbours(Node node, Level level, Func<Level, Point, bool> conditionToMove,
            int targetX, int targetY) {
            var result = new Collection<Node>();
            var neighbourArray = new Point[4];
            neighbourArray[0] = new Point(node.PositionX + 1, node.PositionY);
            neighbourArray[1] = new Point(node.PositionX - 1, node.PositionY);
            neighbourArray[2] = new Point(node.PositionX, node.PositionY + 1);
            neighbourArray[3] = new Point(node.PositionX, node.PositionY - 1);

            foreach (var point in neighbourArray) {
                if (!GameEntity.IsLevelCellValid(point.X, point.Y, level.Width, level.Height)) continue;
                if (conditionToMove(level, point)) {
                    var neighbour = new Node {
                        PositionX = point.X,
                        PositionY = point.Y,
                        Parent = node,
                        LengthFromStart = node.LengthFromStart + GetDistanceWithWeight(level, point.X, point.Y),
                        HeuristicLength = GetHeuristicPathLength(point.X, point.Y, targetX, targetY)
                    };
                    result.Add(neighbour);
                }
            }
            return result;
        }
        private static List<Point> GetPathForNode(Node node) {
            var result = new List<Point>();
            var current = node;
            while (current != null) {
                result.Add(new Point(current.PositionX, current.PositionY));
                current = current.Parent;
            }
            result.Reverse();
            return result;
        }

        private int GetHeuristicPathLength(int selfX, int selfY, int targetX, int targetY) {
            return Math.Abs(selfX - targetX) + Math.Abs(selfY - targetY);
        }

        public class Node {
            public int PositionX { get; set; }
            public int PositionY { get; set; }
            public int LengthFromStart { get; set; }
            public Node Parent { get; set; }
            public int HeuristicLength { get; set; }
            public int FullPathLength => LengthFromStart + HeuristicLength;
        }
    }
}