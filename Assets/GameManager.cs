using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverCanvas;
    private PlayerInfo _playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        _playerInfo = FindObjectOfType<PlayerInfo>();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        if (Score.score > _playerInfo.highScore)
        {
            _playerInfo.Data.highScore = Score.score;
            if (_playerInfo.Data.id != "")
            {
                FindObjectOfType<DataSaver>().SavePlayerData(_playerInfo.Data);
            }
        }
    }
    public void Replay()
    {
        SceneManager.LoadScene(1);
    }
}
