using System;
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
            DuplicateEdgesRelativeToOddDegreeVertices(oddDegreeVertices, edgesAndNumberOfTimesItAppears);

            //Find Eulerian cycle now that the graph is guaranteed to be Eulerian

            throw new NotImplementedException();
        }

        private void DuplicateEdgesRelativeToOddDegreeVertices(HashSet<Tuple<int, int>> oddDegreeVertices, Dictionary<Edge, int> edgesAndNumberOfTimesItAppears)
        {
            //Breadth first search while both keeping track of already visited vertices and "parent" of each vertex, i.e., the vertex that came before the current vertex in that path
            
            while(oddDegreeVertices.Count > 0)
            {
                HashSet<Tuple<int, int>> alreadyVisitedVertices = new HashSet<Tuple<int, int>>();

                Tuple<int, int> startingPosition = oddDegreeVertices.First<Tuple<int, int>>();
                VertexAndParent startingVertex = new VertexAndParent(startingPosition, null);

                Queue<VertexAndParent> queue = new Queue<VertexAndParent>();
                queue.Enqueue(startingVertex);

                VertexAndParent endingVertex = startingVertex;
                //Breadth First Search from the top position among the odd degree vertices until I find another vertex that also is an odd degree vertex
                while (queue.Count > 0)
                {
                    VertexAndParent currentVertex = queue.Dequeue();
                    alreadyVisitedVertices.Add(currentVertex.vertex);

                    if (oddDegreeVertices.Contains(currentVertex.vertex))
                    {
                        endingVertex = currentVertex;
                        break;
                    }

                    //Add connecting vertices
                    //Upper left
                    Tuple<int, int> upperLeftPosition = new Tuple<int, int>(currentVertex.vertex.Item1 - 1, currentVertex.vertex.Item2 - 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, upperLeftPosition, false);
                    //Upper right
                    Tuple<int, int> upperRightPosition = new Tuple<int, int>(currentVertex.vertex.Item1 + 1, currentVertex.vertex.Item2 - 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, upperRightPosition, false);
                    //Bottom left
                    Tuple<int, int> bottomLeftPosition = new Tuple<int, int>(currentVertex.vertex.Item1 - 1, currentVertex.vertex.Item2 + 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, bottomLeftPosition, false);
                    //Bottom right
                    Tuple<int, int> bottomRightPosition = new Tuple<int, int>(currentVertex.vertex.Item1 + 1, currentVertex.vertex.Item2 + 1);
                    TryToEnqueueNewVertex(edgesAndNumberOfTimesItAppears, alreadyVisitedVertices, queue, currentVertex, bottomRightPosition, false);
                }

                //Double all edges going from the ending vertex to the starting vertex
                //Back tracing from ending vertex until the parent vertex is null
                while(endingVertex.parent != null)
                {
                    Edge edgeToDouble;
                    if (endingVertex.parent.vertex.Item2 < endingVertex.vertex.Item2)
                    {
                        //Parent vertex is above the current vertex, the parent vertex then needs to come first in the edge as per the convention used in this code
                        edgeToDouble = new Edge(endingVertex.parent.vertex, endingVertex.vertex);
                    }
                    else
                    {
                        //Parent vertex is below the current vertex, the parent vertex then needs to come after the current one in the edge as per the convention used in this code
                        edgeToDouble = new Edge(endingVertex.vertex, endingVertex.parent.vertex);
                    }
                    //"Double" this edge
                    edgesAndNumberOfTimesItAppears[edgeToDouble]++;
                }
            }
        }

        private static void TryToEnqueueNewVertex(Dictionary<Edge, int> edgesAndNumberOfTimesItAppears, HashSet<Tuple<int, int>> alreadyVisitedVertices, Queue<VertexAndParent> queue, VertexAndParent currentVertex, Tuple<int, int> potentialNewPosition, bool currentVertexIsUpper)
        {
            if (!alreadyVisitedVertices.Contains(potentialNewPosition))
            {
                Edge potentialEdge = currentVertexIsUpper ? new Edge(currentVertex.vertex, potentialNewPosition) : new Edge(potentialNewPosition, currentVertex.vertex);
                if (edgesAndNumberOfTimesItAppears.ContainsKey(potentialEdge))
                {
                    queue.Enqueue(new VertexAndParent(potentialNewPosition, currentVertex));
                }
            }
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

    public class VertexAndParent
    {
        public Tuple<int, int> vertex;
        public VertexAndParent parent;

        public VertexAndParent(Tuple<int, int> vertex, VertexAndParent parent)
        {
            this.vertex = vertex;
            this.parent = parent;
        }
    }
}
