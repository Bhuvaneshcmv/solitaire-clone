using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class C_GameManagerScript : MonoBehaviour
{
    public GameObject g_CardSlotPrefab;
    public Transform g_CardsSlotsParent;
    int g_numOfSlots;
    public GameObject[] g_cardSlots;
    Vector3 g_Pos;
    public int g_NumOfCardsPerSuite;
    public int g_NumOfSuites;
    public GameObject g_CardPrefab;
    public GameObject[] g_ArrayOfCards;
    public List<Material> g_suiteMaterials = new List<Material>();
    public Transform g_CardParent;
    GameObject g_temp;
    int g_NumOfCards;
    // Start is called before the first frame update
    void Awake()
    {        
        g_numOfSlots = 13;
        g_cardSlots = new GameObject[g_numOfSlots];
        g_Pos = new Vector3(-15, 7.5f, 0.5f);
        g_NumOfCardsPerSuite = 13;
        g_NumOfSuites = 4;
        g_NumOfCards = g_NumOfCardsPerSuite * g_NumOfSuites;
        g_ArrayOfCards = new GameObject[g_NumOfCards];
        m_CreateSlots();
        m_InitCards();
        m_shuffle();
        m_PushCardsToDeck();
        m_PushCardsToBottomRow();
        m_OpenFacesInTopCards();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void m_CreateSlots()
    {
        int l_XCardSize = 5;
        Vector3 l_pos = Vector3.zero;
        for (int i = 0; i < g_numOfSlots; i++)
        {

            g_cardSlots[i] = Instantiate(g_CardSlotPrefab, g_Pos, Quaternion.identity);
            g_cardSlots[i].transform.parent = g_CardsSlotsParent;
            g_cardSlots[i].GetComponent<C_CardSlotScript>().g_Num = i;
            l_pos = g_Pos;
            if (i == 1) l_pos.x += 2 * l_XCardSize;
            else l_pos.x += l_XCardSize;
            if (i == 5)
            {
                l_pos.y -= l_XCardSize;
                l_pos.x = -15;
            }
            if (i <= 5) g_cardSlots[i].GetComponent<C_CardSlotScript>().g_SlotType = e_SlotType.top;
            else g_cardSlots[i].GetComponent<C_CardSlotScript>().g_SlotType = e_SlotType.bottom;
            g_Pos = l_pos;
        }
    }
    void m_InitCards()
    {
        int l_count = 0;
        for (int j = 0; j < g_NumOfSuites; j++)
        {
            for (int i = 0; i < g_NumOfCardsPerSuite; i++)
            {
                g_ArrayOfCards[l_count] = Instantiate(g_CardPrefab, Vector3.zero, Quaternion.identity);
                g_ArrayOfCards[l_count].transform.SetParent(g_CardParent);
                g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material = g_suiteMaterials[j];
                g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(2).GetComponent<MeshRenderer>().material = g_suiteMaterials[j];
                g_ArrayOfCards[l_count].gameObject.GetComponent<C_CardScript>().g_color = e_color.black;
                if( j==1||j==2) g_ArrayOfCards[l_count].gameObject.GetComponent<C_CardScript>().g_color = e_color.red;
                g_ArrayOfCards[l_count].gameObject.GetComponent<C_CardScript>().g_SuiteNum = j;
                g_ArrayOfCards[l_count].gameObject.GetComponent<C_CardScript>().g_Value = i+1;
                if (i < 10 && i > 0) g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = (i + 1).ToString();
                else
                {
                    switch (i)
                    {
                        case 0:
                            g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "A";
                            break;
                        case 10:
                            g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "J";
                            break;
                        case 11:
                            g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "Q";
                            break;
                        case 12:
                            g_ArrayOfCards[l_count].gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshPro>().text = "K";
                            break;
                    }
                }

                g_ArrayOfCards[l_count].gameObject.GetComponent<C_CardScript>().m_changeColor();
               l_count++;
            }
        }
    }
    void m_shuffle()
    {
        for (int i = 0; i < g_NumOfCards; i++)
        {
            int rnd = Random.Range(0, g_NumOfCards);
            g_temp = g_ArrayOfCards[rnd];
            g_ArrayOfCards[rnd] = g_ArrayOfCards[i];
            g_ArrayOfCards[i] = g_temp;
        }
    }
    void m_PushCardsToDeck()
    {
        for (int i = 0; i < g_NumOfCards; i++)
        {
            g_cardSlots[0].GetComponent<C_CardSlotScript>().m_Push(g_ArrayOfCards[i]);
        }
    }
    void m_PushCardsToBottomRow()
    {
        for(int i = 0; i < 7; i++)
        {
            for (int j = i; j >= 0; j--)
            {
                g_temp = g_cardSlots[0].GetComponent<C_CardSlotScript>().m_Pop();
                g_cardSlots[i+6].GetComponent<C_CardSlotScript>().m_Push(g_temp);
            }
        }        
    }
    void m_OpenFacesInTopCards()
    {
        for(int i = 0; i < 7; i++)
        {
            g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top.transform.GetChild(0).gameObject.SetActive(true);
            g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void m_LoadMainMenu()
    {
        SceneManager.LoadScene("main menu1");
    }
}