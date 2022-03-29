using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour, IController
{
    public State _State { get; set; } = new WaitState();
    public int _CurrentPositionIndex { get; set; } = 0;
    PlayerController player;
    public PlayerController _Player { 
        get => player; 
        set 
        { 
            player = value; 
            player.endOfPlayerTurn += ReadyToMove;
            player.playerReachedExit += delegate () { _IsReadyToMove = false; _State = new NullState(); };
            endOfMinotaurTurn += player.ReadyToMove;
            
        } 
    }
    public bool _IsReadyToMove { get; private set; } = false;
    public delegate void endOfMinotaurTurnDelegate();
    public endOfMinotaurTurnDelegate endOfMinotaurTurn;
    public delegate void minotaurReachedPlayerDelegate();
    public minotaurReachedPlayerDelegate minotaurReachedPlayer;
    Coroutine isMovingCoroutine = null;
    bool endOfMove = false;
    int moveCount = 0;

    void Start()
    {
        if (GameManager.instance != null)
        {
            minotaurReachedPlayer += GameManager.instance.Lose;
            GameManager.instance.levelRestart += ResetController;
        }
    }

    void Update()
    {
        if(_State.GetType() != typeof(NullState))
        {
            State s = _State.Execute(this);
            if (s != null)
            {
                _State = s;
            }
        }
    }

    public bool Move()
    {
        if (_IsReadyToMove)
        {
            //Minotaur can make two turns;
            //His objective is to reach player;
            //Always at same x or y than player;
            //He chooses horizontal over vertical if possible

            if(isMovingCoroutine == null)
            {
                TryToMove();
            }
            if (endOfMove)
            {
                moveCount++;
                endOfMove = false;
                isMovingCoroutine = null;

                if(moveCount >= 2)
                {
                    moveCount = 0;
                    _IsReadyToMove = false;
                    if(MazeManager.instance._Maze[_CurrentPositionIndex] == MazeManager.instance._Maze[player._CurrentPositionIndex])
                    {
                        minotaurReachedPlayer.Invoke();
                    }
                    endOfMinotaurTurn.Invoke();
                    return true;
                }  
            }
        }
        
        return false;
    }

    void TryToMove()
    {
        int xMinotaur = _CurrentPositionIndex % MazeManager.instance._MazeWidth; //which column minotaur is
        int xPlayer = player._CurrentPositionIndex % MazeManager.instance._MazeWidth; //which column player is
        int newPositionIndex = _CurrentPositionIndex;
        if (xMinotaur > xPlayer && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallLeft) //try to go left and check for a wall
        {   
            //move left
            newPositionIndex = _CurrentPositionIndex - 1;   
        }
        else if (xMinotaur < xPlayer && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallRight) //try to go right and check for a wall
        {  
            //move right
            newPositionIndex = _CurrentPositionIndex + 1;   
        }
        else //is at same column than player or has a wall on side
        {
            int yMinotaur = _CurrentPositionIndex / MazeManager.instance._MazeWidth;
            int yPlayer = player._CurrentPositionIndex / MazeManager.instance._MazeWidth;
            if (yMinotaur > yPlayer && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallUp) //try to go up and check for a wall
            {
                //move up
                newPositionIndex = _CurrentPositionIndex - MazeManager.instance._MazeWidth;   
            }
            else if (yMinotaur < yPlayer && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallDown) //try to go down and check for a wall
            {    
                //move down
                newPositionIndex = _CurrentPositionIndex + MazeManager.instance._MazeWidth;   
            }
        }

        int multWidthHeight = MazeManager.instance._MazeWidth * MazeManager.instance._MazeHeight;
        if (newPositionIndex != _CurrentPositionIndex && newPositionIndex >= 0 && newPositionIndex < multWidthHeight) //checking if index isn't outside bounds
        {
            isMovingCoroutine = StartCoroutine(MoveCoroutine(MazeManager.instance._Maze[newPositionIndex].transform.position, newPositionIndex));
        }
        else
        {
            endOfMove = true;
        }
    }

    public bool Wait()
    {
        if (_IsReadyToMove)
        {
            return true;
        }
        return false;
    }

    public void ReadyToMove()
    {
        _IsReadyToMove = true;
    }

    public void ResetController()
    {
        StopAllCoroutines();
        _IsReadyToMove = false;
        isMovingCoroutine = null;
        moveCount = 0;
        endOfMove = false;
        _CurrentPositionIndex = MazeManager.instance._MinotaurPositionIndex;
        transform.position = MazeManager.instance._Maze[_CurrentPositionIndex].transform.position;
        _State = new WaitState();
    }

    IEnumerator MoveCoroutine(Vector3 target, int newPositionIndex) //Smooth movement
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float timer = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = target;
        do
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, curve.Evaluate(timer));
            yield return wait;
        }
        while (timer < 1f);

        _CurrentPositionIndex = newPositionIndex;
        endOfMove = true;
    }
}
