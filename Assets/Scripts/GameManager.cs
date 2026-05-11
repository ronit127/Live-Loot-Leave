using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InputActionProperty restartButton;
    public InputActionProperty quitButton;

    void Update()
    {
        if (quitButton.action.WasPressedThisFrame())
        {
            QuitGame();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit triggered");
    }
}
