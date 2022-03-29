using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    [SerializeField]
    List<ScreenController> screens = new List<ScreenController>();
    Dictionary<ScreenIdentifier, ScreenController> screenDictionary = new Dictionary<ScreenIdentifier, ScreenController>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < screens.Count; i++)
        {
            screens[i].DisableScreen();
            screenDictionary.Add(screens[i]._ScreenIdentifier, screens[i]);
        }
    }

    void Start()
    {
        screenDictionary[ScreenIdentifier.InfoPanel].EnableScreen();
    }

    public ScreenController RequestScreen(ScreenIdentifier screenIdentifier)
    {
        return screenDictionary[screenIdentifier];
    }

    public void OpenOrCloseScreen(ScreenIdentifier screenIdentifier, bool open)
    {
        if (open)
        {
            screenDictionary[screenIdentifier].EnableScreen();
        }
        else
        {
            screenDictionary[screenIdentifier].DisableScreen();
        }
    }
}
