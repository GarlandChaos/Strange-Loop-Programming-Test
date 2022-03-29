using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenController : MonoBehaviour, IScreenController
{
    [SerializeField]
    ScreenIdentifier screenIdentifier = ScreenIdentifier.WinScreen;
    public ScreenIdentifier _ScreenIdentifier => screenIdentifier;

    public void EnableScreen()
    {
        gameObject.SetActive(true);
    }

    public void DisableScreen()
    {
        gameObject.SetActive(false);
    }
}
