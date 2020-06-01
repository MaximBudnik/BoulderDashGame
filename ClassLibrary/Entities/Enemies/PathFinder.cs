using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace ClassLibrary.Entities.Enemies {
    public partial class EnemyWalker {
        public class Node {
            public int PositionX { get; set; }
            public int PositionY { get; set; }
            public int LengthFromStart { get; set; }
            public Node Parent { get; set; }
            public int Heuristic { get; set; }
        }

        private List<Point> FindPath(int selfX, int selfY, int targetX, int targetY) {
            var closed = new Collection<Node>();
            var open = new Collection<Node>();

            // int maxDepth = 200;
            // int depth = 0;

            var root = new Node {
                PositionX = selfX,
                PositionY = selfY,
                Parent = null,
                LengthFromStart = 0,
                Heuristic = GetHeuristic(selfX, selfY, targetX, targetY)
            };
            open.Add(root);
            while (open.Count > 0) {
                var current = open.First();
                if (current.PositionX == targetX && current.PositionY == targetY) return GetPathForNode(current);
                open.Remove(current);
                closed.Add(current);
                foreach (var neighbour in GetNeighbours(current, targetX, targetY)) {
                    if (closed.Count(key =>
                        key.PositionX == neighbour.PositionX && key.PositionY == neighbour.PositionY) > 0)
                        continue;
                    var next = open.FirstOrDefault(key =>
                        key.PositionX == neighbour.PositionX && key.PositionY == neighbour.PositionY);
                    if (next == null) open.Add(neighbour);
                    else if (next.LengthFromStart > neighbour.LengthFromStart) {
                        next.Parent = current;
                        next.LengthFromStart = neighbour.LengthFromStart;
                    }
                }
            }
            return null;
        }
        private static int GetHeuristic(int selfX, int selfY, int targetX, int targetY) {
            return Math.Abs(selfX - targetX) + Math.Abs(selfY - targetY);
        }
        private Collection<Node> GetNeighbours(Node node,
            int targetX, int targetY) {
            var result = new Collection<Node>();
            var neighbourPoints = new Point[4];
            neighbourPoints[0] = new Point(node.PositionX + 1, node.PositionY);
            neighbourPoints[1] = new Point(node.PositionX - 1, node.PositionY);
            neighbourPoints[2] = new Point(node.PositionX, node.PositionY + 1);
            neighbourPoints[3] = new Point(node.PositionX, node.PositionY - 1);

            try {
                foreach (var point in neighbourPoints) {
                    if (point.X < 0 || point.X >= level.Width || point.Y < 0 || point.Y >= level.Height) continue;
                    if (level[point.X, point.Y].CanMove || level[point.X, point.Y].PathFinderMove) {
                        var neighbour = new Node {
                            PositionX = point.X,
                            PositionY = point.Y,
                            Parent = node,
                            LengthFromStart = node.LengthFromStart + 1,
                            Heuristic = GetHeuristic(point.X, point.Y, targetX, targetY)
                        };
                        result.Add(neighbour);
                    }
                }
            }
            catch (Exception e) {
                return null;
            }
            return result;
        }
        private static List<Point> GetPathForNode(Node node) {
            var result = new List<Point>();
            var currentNode = node;
            while (currentNode != null) {
                result.Add(new Point(currentNode.PositionX, currentNode.PositionY));
                currentNode = currentNode.Parent;
            }
            result.Reverse();
            return result;
        }
    }
}