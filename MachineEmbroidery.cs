﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbroideryCreator
{
    class MachineEmbroidery
    {
        /*Definition used here:
         right diagonal: diagonal with upper part on the right side
         left diagonal: diagonal with upper part on the left side*/
        public ImageAndOperationsData imageData;


        void CreatePath(Dictionary<int, List<Tuple<int, int>>> positionsOfEachColor)
        {
            List<Tuple<StitchType, Tuple<int, int>>> listOfStitches = new List<Tuple<StitchType, Tuple<int, int>>>();

            foreach (int colorIndex in positionsOfEachColor.Keys)
            {
                //TODO: Create dictionary/hashset of positions of each color
                HashSet<Tuple<int, int>> allPositionsForCurrentColor = positionsOfEachColor[colorIndex].ToHashSet<Tuple<int, int>>();
                
                //TODO: while the dictionary created on the previous step is not empty, perform loop containing the next two steps
                while(allPositionsForCurrentColor.Count > 0)
                {
                    //TODO: Find next connected region for this color starting with right diagonal (Flood Fill algorithm.
                    //Don't forget to remove each position from the dictionary throughout the flood fill algorithm
                    HashSet<Tuple<int, int>> currentConnectedRegion = FloodFill(allPositionsForCurrentColor, true);

                    //TODO: Create path for the connected region starting with right diagonal found in previous step (Chinese Postman Problem)
                    List<Tuple<int, int>> pathToFollow = CreateShortestPath(currentConnectedRegion, true);
                }


                //TODO: Do the same as above but for left diagonals

            }
        }

        private List<Tuple<int, int>> CreateShortestPath(HashSet<Tuple<int, int>> currentConnectedRegion, bool startsAtRightDiagonal)
        {
            //Prepare for the implementation by first creating both vertices and edges
            //For vertices I will use the coordinates of the top left corner of each square as the way of identifying each vertex / associating coordinates to them
            HashSet<Tuple<int, int>> vertices = new HashSet<Tuple<int, int>>();
            HashSet<Edge> edges = new HashSet<Edge>();

            Tuple<int, int> startingPosition = currentConnectedRegion.First<Tuple<int, int>>();

            foreach(Tuple<int, int> currentPosition in currentConnectedRegion)
            {
                //For each position add two vertices and one edge
                bool isRightDiagonal = CheckIfDiagonalIsRightOrLeft(startsAtRightDiagonal, startingPosition, currentPosition);

                Tuple<int, int> upperVertex, bottomVertex;
                if (isRightDiagonal)
                {
                    //Upper right vertex
                    upperVertex = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2);
                    //Bottom left vertex
                    bottomVertex = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2 + 1);
                }
                else
                {
                    //Upper left vertex
                    upperVertex = new Tuple<int, int>(currentPosition.Item1, currentPosition.Item2);
                    //Bottom right vertex
                    bottomVertex = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 + 1);
                }
                vertices.Add(upperVertex);
                vertices.Add(bottomVertex);
                edges.Add(new Edge(upperVertex, bottomVertex));
            }

            //Compute degree of each vertex
            Dictionary<Tuple<int, int>, int> degreeOfEachVertex = new Dictionary<Tuple<int, int>, int>();
            foreach(Edge edge in edges)
            {
                UpdateDegreeOfVertex(degreeOfEachVertex, edge.upperVertex);
                UpdateDegreeOfVertex(degreeOfEachVertex, edge.bottomVertex);
            }

            HashSet<Tuple<int, int>> oddDegreeVertices = new HashSet<Tuple<int, int>>();
            foreach(KeyValuePair<Tuple<int, int>, int> vertexWithDegree in degreeOfEachVertex)
            {
                if(vertexWithDegree.Value % 2 != 0)
                {
                    oddDegreeVertices.Add(vertexWithDegree.Key);
                }
            }

            //TODO: Duplicate edges relative to odd degree vertices
            //carefull when duplicating edges because of odd degree vertices, the edges variable used so far is a HashSet, which doesn't accept duplication of values
            //I might use a dictionary containing the edge as key and the number of times that edge is present in the graph as the value
            Dictionary<Edge, int> edgesAndNumberOfTimesItAppears = new Dictionary<Edge, int>();
            foreach(Edge edge in edges)
            {
                edgesAndNumberOfTimesItAppears.Add(edge, 1);
            }

            throw new NotImplementedException();
        }

        private static void UpdateDegreeOfVertex(Dictionary<Tuple<int, int>, int> degreeOfEachVertex, Tuple<int, int> vertexToUpdate)
        {
            if (!degreeOfEachVertex.ContainsKey(vertexToUpdate))
            {
                degreeOfEachVertex.Add(vertexToUpdate, 1);
            }
            else
            {
                degreeOfEachVertex[vertexToUpdate]++;
            }
        }

        private bool CheckIfDiagonalIsRightOrLeft(bool startsAtRightDiagonal, Tuple<int, int> startingPosition, Tuple<int, int> currentPosition)
        {
            int startingPositionParity = startingPosition.Item1 + startingPosition.Item2;
            int currentPositionParity = currentPosition.Item1 + currentPosition.Item2;

            int parityDifference = currentPositionParity - startingPositionParity;
            bool currentPositionIsSameTypeOfDiagonalThanStartingPosition = parityDifference % 2 == 0;
            //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ? currentPositionIsSameTypeOfDiagonalThanStartingPosition : !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
            bool currentPositionIsRightDiagonal = startsAtRightDiagonal ^ !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
            return currentPositionIsRightDiagonal;
        }

        private HashSet<Tuple<int, int>> FloodFill(HashSet<Tuple<int, int>> allPositionsForCurrentColor, bool startsAtRightDiagonal)
        {
            //This version of the Flood Fill algorithm works at diagonal because that's how the path the thread takes in cross stitching

            HashSet<Tuple<int, int>> connectedRegion = new HashSet<Tuple<int, int>>();

            Tuple<int, int> startingPosition = allPositionsForCurrentColor.First<Tuple<int, int>>();
            int startingPositionParity = startingPosition.Item1 + startingPosition.Item2;

            Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(startingPosition);

            while(queue.Count > 0)
            {
                Tuple<int, int> currentPosition = queue.Dequeue();

                connectedRegion.Add(currentPosition);
                allPositionsForCurrentColor.Remove(currentPosition);

                //Try to add adjacent positions to the queue
                int currentPositionParity = currentPosition.Item1 + currentPosition.Item2;
                int parityDifference = currentPositionParity - startingPositionParity;
                bool currentPositionIsSameTypeOfDiagonalThanStartingPosition = parityDifference % 2 == 0;
                //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ? currentPositionIsSameTypeOfDiagonalThanStartingPosition : !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
                //bool currentPositionIsRightDiagonal = startsAtRightDiagonal ^ !currentPositionIsSameTypeOfDiagonalThanStartingPosition;
                bool currentPositionIsRightDiagonal = CheckIfDiagonalIsRightOrLeft(startsAtRightDiagonal, startingPosition, currentPosition);


                Tuple<int, int> abovePosition, belowPosition;
                if (currentPositionIsRightDiagonal)
                {
                    abovePosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 - 1);
                    belowPosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 + 1);
                }
                else
                {
                    abovePosition = new Tuple<int, int>(currentPosition.Item1 - 1, currentPosition.Item2 - 1);
                    belowPosition = new Tuple<int, int>(currentPosition.Item1 + 1, currentPosition.Item2 + 1);
                }

                CheckPositionAndTryToEnqueue(allPositionsForCurrentColor, connectedRegion, queue, abovePosition);
                CheckPositionAndTryToEnqueue(allPositionsForCurrentColor, connectedRegion, queue, belowPosition);
            }

            return connectedRegion;
        }

        private static void CheckPositionAndTryToEnqueue(HashSet<Tuple<int, int>> allPositionsForCurrentColor, HashSet<Tuple<int, int>> connectedRegion, Queue<Tuple<int, int>> queue, Tuple<int, int> positionToEnqueue)
        {
            if (allPositionsForCurrentColor.Contains(positionToEnqueue) && !connectedRegion.Contains(positionToEnqueue))
            {
                queue.Enqueue(positionToEnqueue);
            }
        }
    }


    enum StitchType
    {
        NormalStitch, 
        JumpStitch,
        ColorChange,
        SequinMode
    }

    public struct Edge
    {
        public Tuple<int, int> upperVertex;
        public Tuple<int, int> bottomVertex;

        public Edge(Tuple<int, int> upperVertex, Tuple<int, int> bottomVertex)
        {
            this.upperVertex = upperVertex;
            this.bottomVertex = bottomVertex;
        }
    }
}
