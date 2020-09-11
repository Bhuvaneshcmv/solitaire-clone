using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace assign8
{
    public class c_screenSwitchScript1 : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject g_HomePanel;
        public GameObject g_settingsMenuPanel;
        public Toggle g_toggleComponent;

        void Start()
        {
            Screen.orientation = ScreenOrientation.Landscape;
            m_goToHomeMenu();
            m_SetToggleValue();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void m_goToSolarSystemScene()
        {
            SceneManager.LoadScene("solitaireGame");
        }
        public void m_goToMainMenuScene()
        {
            SceneManager.LoadScene("main menu1");
        }

        public void m_goToHomeMenu()
        {
            g_HomePanel.SetActive(true);
            g_settingsMenuPanel.SetActive(false);
        }
        public void m_goToSettingsMenu()
        {
            g_HomePanel.SetActive(false);
            g_settingsMenuPanel.SetActive(true);
        }
        public void m_LoadToggleValue()
        {
            if (g_toggleComponent.isOn == false)
            {
                PlayerPrefs.SetInt("solitaire_Sound", 0);
            }
            else
            {
                PlayerPrefs.SetInt("solitaire_Sound", 1);
            }
        }
        public void m_SetToggleValue()
        {
            if (PlayerPrefs.GetInt("solitaire_Sound") == 0)
            {
                g_toggleComponent.isOn = false;
            }
            else
            {
                g_toggleComponent.isOn = true;
            }
        }

        public void m_changeToggleValue()
        {
            if (PlayerPrefs.GetInt("solitaire_Sound") == 0)
            {
                PlayerPrefs.SetInt("solitaire_Sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("solitaire_Sound", 0);
            }
        }
       
    }
}