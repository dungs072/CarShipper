using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WFCBuilder : MonoBehaviour
{
    public static event Action OnGenerateMap;
    public static event Action OnFinishedMap;
    public static event Action<List<PathNode>> OnGenerateShipping;
    [Header("Generator")]
    // the size of the world in grid cells
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int Offset;
    [SerializeField] private float timeDelay = 5f;
    [SerializeField] private CameraController cameraController;
    private WFCNode[,] grid;
    private PathNode[,] pathGrid;
    private List<PathNode> possiblePathNodes = new List<PathNode>();


    // a list contains of all possible nodes
    public List<WFCNode> Nodes = new List<WFCNode>();

    //  A list to store tile positions that need collapsing.
    private List<TileNode> toCollapse = new List<TileNode>();

    private TileNode headNode;
    private PathNode parentPathNode;
    public bool CanDoAnything{get;set;}

    private Vector3Int[] offsets = new Vector3Int[]{
        new Vector3Int(0,0,1), // top
        new Vector3Int(0,0,-1), // bottom
        new Vector3Int(1,0,0), // right
        new Vector3Int(-1,0,0), // left
    };
    private void Start()
    {
        // grid = new WFCNode[Width, Height];
        // pathGrid = new PathNode[Width, Height];

        // StartCoroutine(CollapseWorld());
    }
    public void SetHeightAndWidth(int value)
    {
        Width = value;
        Height = value;
    }
    public void AddHeightAndWidth(int value)
    {
        Width += value;
        Height += value;
    }
    #region UI
    public void OnRegenerateBuilder()
    {
        grid = new WFCNode[Width, Height];
        pathGrid = new PathNode[Width, Height];
        possiblePathNodes.Clear();
        OnGenerateMap?.Invoke();
        //lineController.Reset();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        CanDoAnything = false;
        StartCoroutine(CollapseWorld());
    }
    #endregion

    private IEnumerator CollapseWorld()
    {
        cameraController.transform.position = new Vector3Int(Width/2,0,Height/2)*Offset;
        cameraController.SetScreenXLimits(new Vector2Int(0,Width)*Offset);
        cameraController.SetScreenZLimits(new Vector2Int(0,Height)*Offset);
        cameraController.SetScreenYLimits(new Vector2Int(10,Height*Offset));
        toCollapse.Clear();
        TileNode tileNode = new TileNode();
        tileNode.Coordinate = new Vector3Int(Width / 2, 0, Height / 2);
        toCollapse.Add(tileNode);
        int count = 0;
        while (toCollapse.Count > 0)
        {
            TileNode tile = toCollapse[0];
            int x = tile.Coordinate.x;
            int y = tile.Coordinate.z;
            List<WFCNode> potentialNodes = new List<WFCNode>(Nodes); // copy nodes to potential nodes
            if (count == 0)
            {
                potentialNodes = new List<WFCNode>() { Nodes[0] };
                count++;

            }
            for (int i = 0; i < offsets.Length; i++)
            {
                TileNode neighbourTileNode = new TileNode();
                neighbourTileNode.Coordinate = new Vector3Int(x + offsets[i].x, 0, y + offsets[i].z);
                if (IsInsideGrid(neighbourTileNode.Coordinate))
                {
                    WFCNode neighbourNode = grid[neighbourTileNode.Coordinate.x, neighbourTileNode.Coordinate.z];

                    if (neighbourNode != null)
                    {
                        switch (i)
                        {
                            case 0:
                                WhittleNodes(potentialNodes, neighbourNode.Bottom.CompatibleNodes);
                                break;
                            case 1:
                                WhittleNodes(potentialNodes, neighbourNode.Top.CompatibleNodes);
                                break;
                            case 2:
                                WhittleNodes(potentialNodes, neighbourNode.Left.CompatibleNodes);
                                break;
                            case 3:
                                WhittleNodes(potentialNodes, neighbourNode.Right.CompatibleNodes);
                                break;
                        }
                    }
                    else
                    {
                        if (!ContainCoordinates(neighbourTileNode.Coordinate))
                        {
                            toCollapse.Add(neighbourTileNode);
                        }
                    }
                }
            }
            if (potentialNodes.Count < 1)
            {
                grid[x, y] = Nodes[1];
                Debug.LogWarning("Attempted to collapse wave on " + x + ", " + y + " but found no compatible node.");
            }
            else
            {
                grid[x, y] = potentialNodes[UnityEngine.Random.Range(0, potentialNodes.Count)];

            }
            GameObject newNode = Instantiate(grid[x, y].GetRandomPrefabs(), new Vector3(x * Offset, 0f, y * Offset), Quaternion.identity);
            // assign path
            if (newNode.TryGetComponent(out PathNode pathNode))
            {

                pathGrid[x, y] = pathNode;
                pathGrid[x, y].x = x;
                pathGrid[x, y].y = y;
            }
            newNode.transform.SetParent(transform);
            toCollapse.RemoveAt(0);

            yield return new WaitForSeconds(timeDelay);

        }
        //print(parentNode.ForwardNode);
        FindAllPaths();
        //StartCoroutine(FindAllPaths());
    }
    private bool ContainCoordinates(Vector3Int coor)
    {
        foreach (var tile in toCollapse)
        {
            if (tile.Coordinate == coor)
            {
                return true;
            }
        }
        return false;
    }
    private void WhittleNodes(List<WFCNode> potentialNodes, List<WFCNode> validNodes)
    {
        for (int i = potentialNodes.Count - 1; i > -1; i--)
        {
            if (!validNodes.Contains(potentialNodes[i]))
            {
                potentialNodes.RemoveAt(i);
            }
        }
    }
    private bool IsInsideGrid(Vector3Int v3Int)
    {
        if (v3Int.x > -1 && v3Int.x < Width && v3Int.z > -1 && v3Int.z < Height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool IsInsideGrid(int x, int y)
    {
        if (x > -1 && x < Width && y > -1 && y < Height)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void FindAllPaths()
    {
        int x = Width / 2;
        int y = Height / 2;
        var rootNode = pathGrid[x, y];
        parentPathNode = rootNode;
        Queue<PathNode> queue = new Queue<PathNode>();
        queue.Enqueue(rootNode);
        while (queue.Count > 0)
        {
            PathNode pathNode = queue.Dequeue();
            x = pathNode.x;
            y = pathNode.y;
            possiblePathNodes.Add(pathNode);
            if (pathNode.CanGoForward)
            {
                int currentX = x + offsets[0].x;
                int currentY = y + offsets[0].z;

                if (IsInsideGrid(currentX, currentY))
                {
                    if (pathGrid[currentX, currentY] != null)
                    {
                        if (!pathGrid[currentX, currentY].IsReviewed)
                        {

                            pathGrid[currentX, currentY].IsReviewed = true;
                            queue.Enqueue(pathGrid[currentX, currentY]);
                        }
                        pathNode.Forward = pathGrid[currentX, currentY];


                    }
                }

            }
            if (pathNode.CanGoBackward)
            {

                int currentX = x + offsets[1].x;
                int currentY = y + offsets[1].z;
                if (IsInsideGrid(currentX, currentY))
                {
                    if (pathGrid[currentX, currentY] != null)
                    {
                        if (!pathGrid[currentX, currentY].IsReviewed)
                        {
                            pathGrid[currentX, currentY].IsReviewed = true;
                            queue.Enqueue(pathGrid[currentX, currentY]);
                        }
                        pathNode.Backward = pathGrid[currentX, currentY];

                    }
                }

            }
            if (pathNode.CanTurnRight)
            {

                int currentX = x + offsets[2].x;
                int currentY = y + offsets[2].z;
                if (IsInsideGrid(currentX, currentY))
                {
                    if (pathGrid[currentX, currentY] != null)
                    {
                        if (!pathGrid[currentX, currentY].IsReviewed)
                        {
                            pathGrid[currentX, currentY].IsReviewed = true;
                            queue.Enqueue(pathGrid[currentX, currentY]);
                        }
                        pathNode.Right = pathGrid[currentX, currentY];

                    }
                }

            }
            if (pathNode.CanTurnLeft)
            {

                int currentX = x + offsets[3].x;
                int currentY = y + offsets[3].z;
                if (IsInsideGrid(currentX, currentY))
                {
                    if (pathGrid[currentX, currentY] != null)
                    {
                        if (!pathGrid[currentX, currentY].IsReviewed)
                        {
                            pathGrid[currentX, currentY].IsReviewed = true;
                            queue.Enqueue(pathGrid[currentX, currentY]);
                        }
                        pathNode.Left = pathGrid[currentX, currentY];

                    }
                }

            }
        }
        OnFinishedMap?.Invoke();
        OnGenerateShipping?.Invoke(possiblePathNodes);
        CanDoAnything = true;
        //StartCoroutine(DrawPathLine());
    }
  
    // private IEnumerator DrawPathLine()
    // {
    //     Queue<PathNode> queue = new Queue<PathNode>();
    //     queue.Enqueue(parentPathNode);
    //     while (queue.Count > 0)
    //     {
    //         PathNode node = queue.Dequeue();
    //         //node.DrawLines();
    //         if (node.Forward != null)
    //         {
    //             if (node.Forward.IsReviewed)
    //             {
    //                 queue.Enqueue(node.Forward);
    //                 node.Forward.IsReviewed = false;
    //             }

    //         }
    //         if (node.Backward != null)
    //         {
    //             if (node.Backward.IsReviewed)
    //             {
    //                 queue.Enqueue(node.Backward);
    //                 node.Backward.IsReviewed = false;
    //             }

    //         }
    //         if (node.Right != null)
    //         {
    //             if (node.Right.IsReviewed)
    //             {
    //                 queue.Enqueue(node.Right);
    //                 node.Right.IsReviewed = false;
    //             }

    //         }
    //         if (node.Left != null)
    //         {
    //             if (node.Left.IsReviewed)
    //             {
    //                 queue.Enqueue(node.Left);
    //                 node.Left.IsReviewed = false;
    //             }

    //         }
    //         yield return null;
    //     }
    // }

}
