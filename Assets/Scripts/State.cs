using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract State Execute(IController controller);
}
