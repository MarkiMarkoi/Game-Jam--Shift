using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
    }
}
