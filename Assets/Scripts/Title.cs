using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public Button newGameButton, loadButton, exitButton;

    // Start is called before the first frame update
    void Start()
    {
        newGameButton.onClick.AddListener(OnClickNewGameButton);
        loadButton.onClick.AddListener(OnClickLoadGameButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNewGameButton() {
        Debug.Log("new game");
        SceneManager.LoadScene("Field", LoadSceneMode.Single);
    }
    public void OnClickLoadGameButton() {
        Debug.Log("load game");
        Application.Quit();
    }
    
    public void OnClickExitButton() {
        Debug.Log("exit game");
        Application.Quit();
    }
}
