using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    LevelContainerSO levelContainer = null;
    int currentLevelIndex = -1;
    public delegate void levelRestartDelegate();
    public levelRestartDelegate levelRestart;
    public delegate void levelLoadedDelegate(int index);
    public levelLoadedDelegate levelLoaded;
    [SerializeField]
    Camera gameCamera = null;
    float cameraOffsetY = 20f;
    float cameraOffsetZ = 5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(UIManager.instance != null)
        {
            InfoPanelScreenController infoPanel = UIManager.instance.RequestScreen(ScreenIdentifier.InfoPanel) as InfoPanelScreenController;
            levelLoaded += infoPanel.SetLevelCommentary;
        }
        LoadNextLevel();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPreviousLevel();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if(currentLevelIndex >= levelContainer._Levels.Count)
        {
            currentLevelIndex = 0;
        }
        MazeManager.instance.CreateMaze(levelContainer._Levels[currentLevelIndex]);
        SetCameraValues();
        levelLoaded.Invoke(currentLevelIndex);
        if (MazeManager.instance._ControllersInstantiated && levelRestart != null)
        {
            RestartLevel();
        }
    }

    public void LoadPreviousLevel()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0)
        {
            currentLevelIndex = levelContainer._Levels.Count - 1;
        }
        MazeManager.instance.CreateMaze(levelContainer._Levels[currentLevelIndex]);
        SetCameraValues();
        levelLoaded.Invoke(currentLevelIndex);
        if (MazeManager.instance._ControllersInstantiated && levelRestart != null)
        {
            RestartLevel();
        }
    }

    public void SetCameraValues()
    {
        var val = MazeManager.instance._MazeWidth * MazeManager.instance._MazeHeight;
        gameCamera.transform.position = new Vector3(gameCamera.transform.position.x, val + cameraOffsetY, (-val / 2) - cameraOffsetZ);
    }

    public void RestartLevel()
    {
        levelRestart.Invoke();
        UIManager.instance.OpenOrCloseScreen(ScreenIdentifier.WinScreen, false);
        UIManager.instance.OpenOrCloseScreen(ScreenIdentifier.LoseScreen, false);
    }

    public void Win()
    {
        UIManager.instance.OpenOrCloseScreen(ScreenIdentifier.WinScreen, true);
    }

    public void Lose()
    {
        UIManager.instance.OpenOrCloseScreen(ScreenIdentifier.LoseScreen, true);
    }
}
