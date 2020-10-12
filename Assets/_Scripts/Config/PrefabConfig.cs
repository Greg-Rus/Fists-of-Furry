using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabConfig", menuName = "ScriptableObjects/PrefabConfig", order = 1)]
public class PrefabConfig : ScriptableObject
{
    public GameObject CharacterPrefab;
    public GameObject EnemyPrefab;
}