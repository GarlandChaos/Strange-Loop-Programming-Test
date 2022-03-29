using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public static MazeManager instance = null;  
    Cell[] maze;
    public Cell[] _Maze { get => maze; }
    [SerializeField]
    GameObject mazeContainer = null;
    int mazeWidth = 0;
    public int _MazeWidth { get => mazeWidth; }
    int mazeHeight = 0;
    public int _MazeHeight { get => mazeHeight; }
    [SerializeField]
    PlayerController playerGO = null;
    [SerializeField]
    MinotaurController minotaurGO = null;
    [SerializeField]
    GameObject wallGO = null;
    [SerializeField]
    Cell cellGO = null;
    [SerializeField]
    Cell exitCellGO = null;
    int playerPositionIndex = 0;
    int minotaurPositionIndex = 0;
    public int _PlayerPositionIndex { get => playerPositionIndex; }
    public int _MinotaurPositionIndex { get => minotaurPositionIndex; }
    bool controllersInstantiated = false;
    public bool _ControllersInstantiated { get => controllersInstantiated; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateMaze(TextAsset textAsset)
    {
        Debug.Log(textAsset.text);
        //first value: Width;
        //second value: Height;
        //third value: Player position;
        //fourth value: Minotaur position;
        //next values: instantiate maze cells
        //after maze: instantiate walls
        //-1 - nothing
        //0 - free space
        //1 - exit
        //U - Wall Up
        //D - Wall Down
        //L - Wall Left
        //R - Wall Right
        //N - NOWALL

        Transform[] children = mazeContainer.GetComponentsInChildren<Transform>();
        for (int i = 1; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }

        string[] textValues = textAsset.text.Split(" ");
        mazeWidth = int.Parse(textValues[0]);
        mazeHeight = int.Parse(textValues[1]);
        playerPositionIndex = int.Parse(textValues[2]);
        minotaurPositionIndex = int.Parse(textValues[3]);
        int arraySize = mazeWidth * mazeHeight;
        maze = new Cell[arraySize];
        Quaternion rotation = Quaternion.identity;
        Vector3 cellPos = Vector3.zero;
        float sizeCell = cellGO.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * cellGO.transform.localScale.x;
        int columnCount = 0;
        int maxIterator = arraySize + 4;
        int mazeIterator = 0;
        for (int i = 4; i < maxIterator; i++)
        {
            if(columnCount == mazeWidth)
            {
                columnCount = 0;
                cellPos -= new Vector3(0f, 0f, sizeCell);
                cellPos.x = 0;
            }
            cellPos += new Vector3(sizeCell, 0f, 0f);
            Cell cell = null; //null means that there is no cell to walk into
            if (textValues[i] == "0")
            {
                cell = Instantiate(cellGO, cellPos, rotation);
                cell._Value = 0;
            }
            else if(textValues[i] == "1")
            {
                cell = Instantiate(exitCellGO, cellPos, rotation);
                cell._Value = 1;
            }
            maze[mazeIterator] = cell;
            if(cell != null)
            {
                cell.gameObject.transform.SetParent(mazeContainer.transform);
            }
            columnCount++;
            mazeIterator++;
        }

        mazeIterator = 0;
        for (int i = maxIterator; i < maxIterator + arraySize; i++)
        {
            if(maze[mazeIterator] != null)
            {
                if (textValues[i].Contains("U"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position + Vector3.forward * sizeCell / 2;
                    GameObject wall = Instantiate(wallGO, wallPos, rotation);
                    wall.transform.Rotate(0f, 90f, 0f);
                    maze[mazeIterator]._WallUp = true;
                    wall.gameObject.transform.SetParent(mazeContainer.transform);
                }
                if (textValues[i].Contains("D"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position - Vector3.forward * sizeCell / 2;
                    GameObject wall = Instantiate(wallGO, wallPos, rotation);
                    wall.transform.Rotate(0f, 90f, 0f);
                    maze[mazeIterator]._WallDown = true;
                    wall.gameObject.transform.SetParent(mazeContainer.transform);
                }
                if (textValues[i].Contains("L"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position - Vector3.right * sizeCell / 2;
                    GameObject wall = Instantiate(wallGO, wallPos, rotation);
                    maze[mazeIterator]._WallLeft = true;
                    wall.gameObject.transform.SetParent(mazeContainer.transform);
                }
                if (textValues[i].Contains("R"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position + Vector3.right * sizeCell / 2;
                    GameObject wall = Instantiate(wallGO, wallPos, rotation);
                    maze[mazeIterator]._WallRight = true;
                    wall.gameObject.transform.SetParent(mazeContainer.transform);
                }
            }
            mazeIterator++;
        }

        if (!controllersInstantiated)
        {
            PlayerController player = Instantiate(playerGO, maze[playerPositionIndex].transform.position, rotation);
            player._CurrentPositionIndex = playerPositionIndex;
            MinotaurController minotaur = Instantiate(minotaurGO, maze[minotaurPositionIndex].transform.position, rotation);
            minotaur._CurrentPositionIndex = minotaurPositionIndex;
            minotaur._Player = player;
            controllersInstantiated = true;
        }
    }
}
