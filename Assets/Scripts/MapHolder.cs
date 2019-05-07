using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.PriorityQueue;
using System;
using System.Linq;

namespace Assets.Scripts
{
    class MapHolder
    {
        private static MapHolder _instance;
        public static MapHolder Instance { get { return _instance; } }
        private static Dictionary<CoordinatePosition, Space> _spaces;

        public MapHolder(Dictionary<CoordinatePosition, Space> spaces)
        {
            _spaces = spaces;
            _instance = this;
        }

        public int GetDistance(Space origin, Space destination)
        {
            return GetRoute(origin, destination).Count;
        }

        public bool TryGetDistance(Space origin, Space destination, out int distance)
        {
            bool result = TryGetRoute(origin, destination, out Queue<Space> queue);
            distance = result ? queue.Count : -1;
            return result;
        }

        public bool TryGetRoute(Space origin, Space destination, out Queue<Space> queue)
        {
            queue = new Queue<Space>(GetRoute(origin, destination));
            return queue != null;
        }

        public Queue<Space> GetRoute(Space origin, Space destination)
        {
            Debug.Log("Finding Route from " + origin.position + " to " + destination.position);
            PriorityQueue<Space> openList = new PriorityQueue<Space>();
            HashSet<Space> closedList = new HashSet<Space>();
            Dictionary<Space, Space> path = new Dictionary<Space, Space>();
            Dictionary<Space, int> distanceFromStart = new Dictionary<Space, int>();

            Space current = origin;
            openList.Enqueue(current, 0);
            distanceFromStart[current] = 0;
            while (openList.Count > 0)
            {
                current = openList.Dequeue();
                closedList.Add(current);
                if(current.position.Equals(destination.position))
                {
                    // by god you're done
                    Space[] spaces = GetPathAsQueue(path, destination).ToArray();
                    for(int i = 0; i<spaces.Length; i++)
                    {
                        Debug.Log("Final path: " + spaces[i].position);
                    }
                    Debug.Log("Path length: " + spaces.Length);
                    return GetPathAsQueue(path, destination);
                }
                foreach(Space neighbor in current.GetNeighbors())
                {
                    if(neighbor.impassable || closedList.Contains(neighbor))
                    {
                        continue;
                    }
                    //G[neighbor] = distanceFromStart.ContainsKey(neighbor) ? Math.Min(G[neighbor], distanceFromStart[current] + neighbor.MovementCost) : distanceFromStart[current] + neighbor.MovementCost;
                    if (distanceFromStart.ContainsKey(neighbor))
                    {
                        if(distanceFromStart[neighbor] > distanceFromStart[current] + neighbor.MovementCost)
                        {
                            distanceFromStart.Remove(neighbor);
                            distanceFromStart.Add(neighbor, distanceFromStart[current] + neighbor.MovementCost);
                            if (path.ContainsKey(neighbor))
                            {
                                path.Remove(neighbor);
                                path.Add(neighbor, current);
                            }
                            distanceFromStart[neighbor] = distanceFromStart[current] + neighbor.MovementCost;
                        }
                    } else
                    {
                        distanceFromStart[neighbor] = distanceFromStart[current] + neighbor.MovementCost;
                    }
                    int F = distanceFromStart[neighbor] + heuristic(neighbor, destination);
                    Debug.Log("(value, start, end) = " +"(" + F  + ", " + distanceFromStart[neighbor] + ", " + heuristic(neighbor, destination) + ")");
                    if (openList.EnqueueWithoutDuplicates(neighbor, F))
                    {
                        path.Add(neighbor, current);
                    }
                    else if (openList.GetPriority(neighbor) < F)
                    {
                        continue;
                    }
                    else
                    {
                        path.Remove(neighbor);
                        path.Add(neighbor, current);
                        openList.UpdatePriority(neighbor, F);
                    }
                }
            }
            return null;
        }

        private int heuristic(Space one, Space two)
        {
            return heuristic(one.position, two.position);
        }

        private int heuristic(CoordinatePosition one, CoordinatePosition two)
        {
            int xDistance = Mathf.Abs(one.X - two.X);
            int zDistance = Mathf.Abs(one.Z - two.Z);
            return xDistance + zDistance;
        }

        private Queue<Space> GetPathAsQueue(Dictionary<Space, Space> path, Space destination)
        {
            Queue<Space> result = new Queue<Space>();
            result.Enqueue(destination);
            Space current = destination;
            while (path.ContainsKey(current))
            {
                current = path[current];
                result.Enqueue(current);
            }
            return new Queue<Space>(result.Reverse());
        }
    }
}
