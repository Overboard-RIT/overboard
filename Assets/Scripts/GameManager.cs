using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool startGame = false;

    private void OnValidate()
    {
        if (startGame)
        {
            startGame = false;
            SceneManager.LoadScene("GameScene");
        }
    }
}
