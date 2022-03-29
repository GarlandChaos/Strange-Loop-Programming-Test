using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public State _State { get; }
    public int _CurrentPositionIndex { get; set; }
    public bool _IsReadyToMove { get; }
    public bool Move();
    public bool Wait();
    public void ReadyToMove();
    public void ResetController();
}
