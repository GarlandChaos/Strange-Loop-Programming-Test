using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : MonoBehaviour, IController
{
    public State _State { get; set; } = new WaitState();
    public int _CurrentPositionIndex { get; set; } = 0;
    PlayerController player;
    public PlayerController _Player { get => player; set { player = value; player.endOfPlayerTurn += ReadyToMove; endOfMinotaurTurn += player.ReadyToMove; } }
    public bool _IsReadyToMove { get; private set; } = false;
    public delegate void endOfMinotaurTurnDelegate();
    public endOfMinotaurTurnDelegate endOfMinotaurTurn;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Move()
    {
        //minotaur needs to know when player moved to starting to move
        if(_IsReadyToMove)
        {
            //Minotaur can make two turns;
            //His objective is to reach player;
            //Always at same height than player;
            //He chooses horizontal over vertical if possible

            //check move1

            //check move2
            
            //player._CurrentPositionIndex

            //after move
            _IsReadyToMove = false;
            endOfMinotaurTurn.Invoke();
        }
        return false;
    }

    public bool Wait()
    {
        return false;
    }

    public void ReadyToMove()
    {
        _IsReadyToMove = true;
    }
}
