using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    public override State Execute(IController controller)
    {        
        //PLAYER
        //user input in MoveState then player Moves -> player wait for minotaur to move
        
        //MINOTAUR
        //minotaur is waiting for player to move -> moves after player moves

        bool canMove = controller.Wait();

        return canMove ? new MoveState() : null;
    }
}
