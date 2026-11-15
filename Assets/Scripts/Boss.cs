using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private string bossName = "The Guardian";

    public string BossName => bossName;
}
