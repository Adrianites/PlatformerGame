using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneManager : MonoBehaviour
{
    [SerializeField] Scene scene;
    enum Scene
    {
        MainMenu,
        Level0,
        Level1,
        Level2,
        Level3, 
        Level4
    }

    public void LoadScene()
    {
        switch (scene)
        {
            case Scene.Level0:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level0");
                break;
            case Scene.MainMenu:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                break;
            case Scene.Level1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
                break;
            case Scene.Level2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
                break;
            case Scene.Level3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
                break;
            case Scene.Level4:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level4");
                break;
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            LoadScene();
        }
    }
}
