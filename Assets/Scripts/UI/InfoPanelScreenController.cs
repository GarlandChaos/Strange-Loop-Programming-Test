using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoPanelScreenController : ScreenController
{
    [SerializeField]
    TMP_Text stateText = null;
    [SerializeField]
    TMP_Text commentaryText = null;
    [SerializeField]
    Color waitingTextColor = Color.black;
    [SerializeField]
    Color readyToMoveTextColor = Color.black;
    [SerializeField]
    Color movingTextColor = Color.black;
    [SerializeField]
    LevelCommentarySO levelCommentaries = null;

    public void SetPlayerState(State previousState)
    {
        if(previousState.GetType() == typeof(MoveState))
        {
            stateText.text = "Waiting for minotaur";
            stateText.color = waitingTextColor;
        }
        else if(previousState.GetType() == typeof(WaitState))
        {
            stateText.text = "Ready to move";
            stateText.color = readyToMoveTextColor;
        }
        else if (previousState.GetType() == typeof(NullState))
        {
            stateText.text = "Moving";
            stateText.color = movingTextColor;
        }
    }

    public void SetLevelCommentary(int index)
    {
        commentaryText.text = levelCommentaries._LevelCommentaries[index];
    }
}
