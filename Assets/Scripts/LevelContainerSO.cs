using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LevelContainer")]
public class LevelContainerSO : ScriptableObject
{
    [SerializeField]
    List<TextAsset> levels = new List<TextAsset>();
    public List<TextAsset> _Levels { get => levels; }
}
