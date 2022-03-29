using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelCommentarySO")]
public class LevelCommentarySO : ScriptableObject
{
    [SerializeField]
    List<string> levelCommentaries = new List<string>();
    public List<string> _LevelCommentaries { get => levelCommentaries; }
}
