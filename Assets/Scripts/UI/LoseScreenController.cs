using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoseScreenController : ScreenController
{
    [SerializeField]
    Button RestartLevelButton = null;
    [SerializeField]
    UnityEvent restartLevelEvent = new UnityEvent();

    public void RestartLevel()
    {
        restartLevelEvent.Invoke();
    }
}
