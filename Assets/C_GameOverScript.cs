using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class C_GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void m_LoadGameScene()
    {
        SceneManager.LoadScene("solitaireGame");
    }
    public void m_LoadMainMenu()
    {
        SceneManager.LoadScene("main menu1");
    }
}
