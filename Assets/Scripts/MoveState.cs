using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public override State Execute(IController controller)
    {
        bool moved = controller.Move();

        return moved ? new WaitState() : null;
    }
}
