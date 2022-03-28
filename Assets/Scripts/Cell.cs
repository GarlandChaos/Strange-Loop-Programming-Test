using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int _Value { get; set; } = -1;
    public bool _WallUp { get; set; } = false;
    public bool _WallDown { get; set; } = false;
    public bool _WallLeft { get; set; } = false;
    public bool _WallRight { get; set; } = false;
}
