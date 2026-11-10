using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class StageData
{
    public GameObject roomPrefab;
    public int enemyCount;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> guarenteedEnemyPrefab;
}
