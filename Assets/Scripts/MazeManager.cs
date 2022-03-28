using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    [SerializeField]
    TextAsset test = null;    
    Cell[] maze;
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

    // Start is called before the first frame update
    void Start()
    {
        CreateMaze(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateMaze(TextAsset textAsset)
    {
        Debug.Log(test.text);
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

        string[] textValues = test.text.Split(" ");
        int width = int.Parse(textValues[0]);
        int heigth = int.Parse(textValues[1]);
        int playerPosition = int.Parse(textValues[2]);
        int minotaurPosition = int.Parse(textValues[3]);
        int arraySize = width * heigth;
        maze = new Cell[arraySize];
        Quaternion rotation = Quaternion.identity;
        Vector3 cellPos = Vector3.zero;
        float sizeCell = cellGO.GetComponent<MeshFilter>().sharedMesh.bounds.size.x * cellGO.transform.localScale.x;
        //foreach(string s in textValues)
        //{
        //    Debug.Log(s);
        //}

        int columnCount = 0;
        int maxIterator = arraySize + 4;
        int mazeIterator = 0;
        for (int i = 4; i < maxIterator; i++)
        {
            if(columnCount == width)
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
                }
                if (textValues[i].Contains("D"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position - Vector3.forward * sizeCell / 2;
                    GameObject wall = Instantiate(wallGO, wallPos, rotation);
                    wall.transform.Rotate(0f, 90f, 0f);
                    maze[mazeIterator]._WallDown = true;
                }
                if (textValues[i].Contains("L"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position - Vector3.right * sizeCell / 2;
                    Instantiate(wallGO, wallPos, rotation);
                    maze[mazeIterator]._WallLeft = true;
                }
                if (textValues[i].Contains("R"))
                {
                    Vector3 wallPos = maze[mazeIterator].transform.position + Vector3.right * sizeCell / 2;
                    Instantiate(wallGO, wallPos, rotation);
                    maze[mazeIterator]._WallRight = true;
                }
            }

            Instantiate(playerGO, maze[playerPosition].transform.position , rotation);
            Instantiate(minotaurGO, maze[minotaurPosition].transform.position, rotation);
            
            mazeIterator++;
        }
    }
}
