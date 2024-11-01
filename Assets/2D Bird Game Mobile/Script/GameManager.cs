using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text textOver;

    public Image imageOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResGame()
    {
        SceneManager.LoadScene("FlyBird");
        Time.timeScale = 1;
    }

    public void Play()
    {
        SceneManager.LoadScene("FlyBird");
        
    }


    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }



    public void GameOver()
    {
        textOver.gameObject.SetActive(true);
        Time.timeScale = 0;
        imageOver.gameObject.SetActive(true);
    }
}
