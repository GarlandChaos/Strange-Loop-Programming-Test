using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WinScreenController : ScreenController
{
    [SerializeField]
    Button nextLevelButton = null;
    [SerializeField]
    Button RestartLevelButton = null;
    [SerializeField]
    UnityEvent nextLevelEvent = new UnityEvent();
    [SerializeField]
    UnityEvent restartLevelEvent = new UnityEvent();
    

    public void NextLevel()
    {
        nextLevelEvent.Invoke();
    }

    public void RestartLevel()
    {
        restartLevelEvent.Invoke();
    }
}
