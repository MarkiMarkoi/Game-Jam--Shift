using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject restartButton;
    private void Awake()
    {
        gameOverText.enabled = false;
        restartButton.SetActive(false);
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0f;
        gameOverText.enabled = true;
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene);
    }
}
