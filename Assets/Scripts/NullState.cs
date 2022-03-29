using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullState : State
{
    public override State Execute(IController controller)
    {
        return null;
    }
}
