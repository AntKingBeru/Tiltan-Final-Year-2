using UnityEngine;

public class MainManuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}