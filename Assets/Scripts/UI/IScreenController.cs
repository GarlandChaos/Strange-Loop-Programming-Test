using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenIdentifier
{
    WinScreen,
    LoseScreen,
    InfoPanel
}

public interface IScreenController
{
    public ScreenIdentifier _ScreenIdentifier { get; }
    public void EnableScreen();
    public void DisableScreen();

}
