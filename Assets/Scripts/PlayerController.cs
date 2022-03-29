using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    public State _State { get; private set; } = new MoveState();
    public int _CurrentPositionIndex { get; set; } = 0;
    public delegate void endOfPlayerTurnDelegate();
    public endOfPlayerTurnDelegate endOfPlayerTurn;
    public bool _IsReadyToMove { get; private set; } = false;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        State s = _State.Execute(this);
        if(s != null)
        {
            _State = s;
        }
    }

    public bool Move()
    {
        //get input direction
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        int newPositionIndex = _CurrentPositionIndex;

        if(horizontalInput != 0 || verticalInput != 0)
        {
            bool right = horizontalInput > 0;
            bool left = horizontalInput < 0;
            bool up = verticalInput > 0;
            bool down = verticalInput < 0;
            if (right) //right
            {
                newPositionIndex = _CurrentPositionIndex + 1;
            }
            else if (left) //left
            {
                newPositionIndex = _CurrentPositionIndex - 1;
            }
            else if (up) //up
            {
                newPositionIndex = _CurrentPositionIndex - MazeManager.instance._MazeWidth;
            }
            else if (down) //down
            {
                newPositionIndex = _CurrentPositionIndex + MazeManager.instance._MazeWidth;
            }

            int multWidthHeight = MazeManager.instance._MazeWidth * MazeManager.instance._MazeHeight;
            if (newPositionIndex >= 0 && newPositionIndex < multWidthHeight) //checking if index isn't outside bounds
            {
                bool canMoveRight = right && !MazeManager.instance._Maze[newPositionIndex]._WallLeft;
                bool canMoveLeft = left && MazeManager.instance._Maze[newPositionIndex]._WallRight == false;
                bool canMoveUp = up && !MazeManager.instance._Maze[newPositionIndex]._WallDown;
                bool canMoveDown = down && !MazeManager.instance._Maze[newPositionIndex]._WallUp;
                if (canMoveRight || canMoveLeft || canMoveUp || canMoveDown)
                {
                    _CurrentPositionIndex = newPositionIndex;
                    transform.position = MazeManager.instance._Maze[newPositionIndex].transform.position;
                    endOfPlayerTurn.Invoke();
                    return true;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.G)) //G for 'gave up turn'
        {
            endOfPlayerTurn.Invoke();
            return true;
        }

        return false;
    }

    public bool Wait() //check if needed
    {
        return false;
    }

    public void ReadyToMove()
    {
        _IsReadyToMove = true;
    }
}
