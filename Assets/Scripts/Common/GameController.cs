using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int _sceneIndex;
    private void Awake()
    {
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //reload currently active scene
            SceneManager.LoadScene(_sceneIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //quit application
            Debug.Log("Quit!");
            Application.Quit();
        }
    }
}
