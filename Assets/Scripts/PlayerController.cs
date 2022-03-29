using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    public State _State { get; private set; } = new MoveState();
    public int _CurrentPositionIndex { get; set; } = 0;
    public delegate void endOfPlayerTurnDelegate();
    public endOfPlayerTurnDelegate endOfPlayerTurn;
    public delegate void playerReachedExitDelegate();
    public playerReachedExitDelegate playerReachedExit;
    public delegate void playerChangedStateDelegate(State state);
    public playerChangedStateDelegate playerChangedState;
    public bool _IsReadyToMove { get; private set; } = true;
    Coroutine isMovingCoroutine = null;
    bool endOfMoveCoroutine = false;

    void Start()
    {
        if(GameManager.instance != null)
        {
            playerReachedExit += GameManager.instance.Win;
            GameManager.instance.levelRestart += ResetController;
        }
        if(UIManager.instance != null)
        {
            InfoPanelScreenController infoPanel = UIManager.instance.RequestScreen(ScreenIdentifier.InfoPanel) as InfoPanelScreenController;
            playerChangedState += infoPanel.SetPlayerState;
            playerChangedState.Invoke(new WaitState());
        }
    }

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
        if (_IsReadyToMove)
        {
            if(isMovingCoroutine == null)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                int newPositionIndex = _CurrentPositionIndex;
                if (horizontalInput != 0 || verticalInput != 0)
                {
                    bool right = horizontalInput > 0;
                    bool left = horizontalInput < 0;
                    bool up = verticalInput > 0;
                    bool down = verticalInput < 0;
                    if (right && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallRight) //right
                    {
                        newPositionIndex = _CurrentPositionIndex + 1;
                    }
                    else if (left && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallLeft) //left
                    {
                        newPositionIndex = _CurrentPositionIndex - 1;
                    }
                    else if (up && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallUp) //up
                    {
                        newPositionIndex = _CurrentPositionIndex - MazeManager.instance._MazeWidth;
                    }
                    else if (down && !MazeManager.instance._Maze[_CurrentPositionIndex]._WallDown) //down
                    {
                        newPositionIndex = _CurrentPositionIndex + MazeManager.instance._MazeWidth;
                    }

                    int multWidthHeight = MazeManager.instance._MazeWidth * MazeManager.instance._MazeHeight;
                    if (newPositionIndex != _CurrentPositionIndex && newPositionIndex >= 0 && newPositionIndex < multWidthHeight) //checking if index isn't outside bounds
                    {
                        isMovingCoroutine = StartCoroutine(MoveCoroutine(MazeManager.instance._Maze[newPositionIndex].transform.position, newPositionIndex));
                    }
                }
                else if (Input.GetKeyDown(KeyCode.G)) //G for 'gave up turn'
                {
                    _IsReadyToMove = false;
                    endOfMoveCoroutine = false;
                    isMovingCoroutine = null;
                    endOfPlayerTurn.Invoke();
                    playerChangedState.Invoke(_State);
                    return true;
                }
            }
            if (endOfMoveCoroutine)
            {
                _IsReadyToMove = false;
                endOfMoveCoroutine = false;
                isMovingCoroutine = null;
                if(MazeManager.instance._Maze[_CurrentPositionIndex]._Value == 1)
                {
                    //Player reached exit
                    playerReachedExit.Invoke();
                }
                endOfPlayerTurn.Invoke();
                playerChangedState.Invoke(_State);
                return true;
            }
            
        }
        
        return false;
    }

    public bool Wait()
    {
        if (_IsReadyToMove)
        {
            playerChangedState.Invoke(_State);
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
        _IsReadyToMove = true;
        isMovingCoroutine = null;
        endOfMoveCoroutine = false;
        _CurrentPositionIndex = MazeManager.instance._PlayerPositionIndex;
        transform.position = MazeManager.instance._Maze[_CurrentPositionIndex].transform.position;
        _State = new MoveState();
    }

    IEnumerator MoveCoroutine(Vector3 target, int newPositionIndex) //Smooth movement
    {
        playerChangedState.Invoke(new NullState());
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
        endOfMoveCoroutine = true;
    }
}
