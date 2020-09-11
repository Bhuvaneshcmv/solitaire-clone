using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class C_MoveCards : MonoBehaviour
{
    RaycastHit g_hit;
    public Transform g_obj;
    public Vector3 g_offset;
    Vector3 g_pointOnHit;
    public bool g_objectSelected;
    Vector3 g_dragPos;
    Vector3 g_DefaultPos;
    Vector3 g_PosDelta;
    public C_GameManagerScript g_GameManagerScript;
    bool g_objectPopped;
    Vector3[] g_OpenPosBottomRow;
    int g_PreviousStackNum;
    public int g_IndexDiff;
    public bool g_isTempNull;
    List<GameObject> g_Temp ;
    bool g_shouldReset;
    int g_NumOfCards;
    // Start is called before the first frame update
    void Start()
    {
        g_NumOfCards = 52;
        Input.multiTouchEnabled = false;
        g_shouldReset = false; 
        g_Temp = new List<GameObject>();
        g_pointOnHit = Vector3.zero;
        g_dragPos = Vector3.zero;
        g_objectPopped = false;
        g_OpenPosBottomRow = new Vector3[7];
        g_isTempNull = true;
        g_Temp.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        m_checkMousePosAndGiveOutput();        
       // if (g_Temp != null)            print(g_Temp.GetComponent<C_CardScript>().g_Value);
    }

    void m_checkMousePosAndGiveOutput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            m_FindingClicksOnDeal(ray);
            if (Physics.Raycast(ray, out g_hit, 100))
            {
                g_obj = g_hit.transform.parent;
                g_DefaultPos = g_obj.transform.position;
                g_objectSelected = true;
                g_pointOnHit = g_hit.point;
                g_offset = g_hit.transform.position - g_pointOnHit;
                g_IndexDiff = g_obj.GetComponent<C_CardScript>().g_NumInStack - g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().g_Index;
                if (g_IndexDiff < 0)
                {
                   for(int i = g_IndexDiff;i<0;i++)
                   {
                        if (i != g_IndexDiff)
                            g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().g_stack[g_obj.GetComponent<C_CardScript>().g_NumInStack - i ].transform.parent = g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().g_stack[g_obj.GetComponent<C_CardScript>().g_NumInStack - i - 1].transform;
                        else
                         g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().g_stack[g_obj.GetComponent<C_CardScript>().g_NumInStack].transform.parent = g_obj.transform;
                   }
                }
            }
        }
        if (g_objectSelected)
        {
            g_dragPos.z = g_pointOnHit.z;
            g_dragPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, g_pointOnHit.z - Camera.main.transform.position.z));
            g_dragPos.z = -19;
            g_obj.position = g_dragPos + g_offset;
            g_PosDelta = g_DefaultPos - g_obj.position;           
            if ((Mathf.Abs(g_PosDelta.x) >= 0.1f || Mathf.Abs(g_PosDelta.y) >= 0.1f ) && g_objectPopped == false)
            {
                
                
                g_objectPopped = true;
                g_PreviousStackNum = g_obj.GetComponent<C_CardScript>().g_SlotNum;                
                g_Temp.Add(g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().m_Pop());
                for (int i = g_IndexDiff; i < 0; i++)
                {
                   g_Temp.Add(g_GameManagerScript.g_cardSlots[g_obj.GetComponent<C_CardScript>().g_SlotNum].GetComponent<C_CardSlotScript>().m_Pop());
                }
                g_isTempNull = false;
            }
        }
        if (g_objectSelected && Input.GetMouseButtonUp(0))
        {
            m_RemoveParent();
            g_objectSelected = false;
            m_FindTheOpenPositions();
            m_ToCheckToPlaceTheCardInTopRow();
            m_ToCheckToPlaceTheCardInBottomRow();
            m_CheckGameOver();
        }
    }
    void m_FindTheOpenPositions()
    {
        for(int i = 0;i<7;i++)
        {
            if (g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top != null)
                g_OpenPosBottomRow[i] = g_GameManagerScript.g_cardSlots[i + 6].transform.position;
        }        
    }
    void m_ToCheckToPlaceTheCardInTopRow()
    {
        if (g_isTempNull == false && g_Temp.Count == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                if ((Mathf.Abs(g_GameManagerScript.g_cardSlots[i + 2].transform.position.x - g_Temp[0].transform.position.x) <= 2.5f) && g_Temp[0].transform.position.y >= 2)
                {
                    if (g_GameManagerScript.g_cardSlots[i + 2].GetComponent<C_CardSlotScript>().g_Top == null && g_Temp[0].GetComponent<C_CardScript>().g_Value == 1)
                    {
                        g_GameManagerScript.g_cardSlots[i+2].GetComponent<C_CardSlotScript>().m_Push(g_Temp[0]);
                        g_isTempNull = true;
                        g_Temp.Clear();
                        g_objectPopped = false;
                        m_OpenTheFaceOfTopCard(g_PreviousStackNum);
                        return;
                    }
                    else if (((g_GameManagerScript.g_cardSlots[i + 2].GetComponent<C_CardSlotScript>().g_Top != null) &&
                        g_GameManagerScript.g_cardSlots[i + 2].GetComponent<C_CardSlotScript>().g_Top.GetComponent<C_CardScript>().g_Value == g_Temp[0].GetComponent<C_CardScript>().g_Value - 1)
                        && (g_GameManagerScript.g_cardSlots[i + 2].GetComponent<C_CardSlotScript>().g_Top.GetComponent<C_CardScript>().g_SuiteNum == g_Temp[0].GetComponent<C_CardScript>().g_SuiteNum))
                    {
                        g_GameManagerScript.g_cardSlots[i + 2].GetComponent<C_CardSlotScript>().m_Push(g_Temp[0]);
                        g_isTempNull = true;
                        g_Temp.Clear();
                        g_objectPopped = false;
                        m_OpenTheFaceOfTopCard(g_PreviousStackNum);
                        return;
                    }
                }
            }
        }
    }
    void m_ToCheckToPlaceTheCardInBottomRow()
    {
        if (g_isTempNull == false)
        {
            for (int i = 0; i < 7; i++)
            {
                if ((Mathf.Abs(g_GameManagerScript.g_cardSlots[i + 6].transform.position.x - g_Temp[g_Temp.Count - 1].transform.position.x) <= 2.5f) && g_Temp[g_Temp.Count - 1].transform.position.y <= 2.5f)
                {
                    if (g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top == null && g_Temp[g_Temp.Count-1].GetComponent<C_CardScript>().g_Value == 13)
                    {
                        for (int j = 0; j < g_Temp.Count; j++)
                        {
                            g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().m_Push(g_Temp[g_Temp.Count-j-1]);
                        }
                        g_isTempNull = true;
                        g_Temp.Clear();
                        g_objectPopped = false;
                        m_OpenTheFaceOfTopCard(g_PreviousStackNum);
                        return;
                    }
                    else if (((g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top != null)&&
                        g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top.GetComponent<C_CardScript>().g_Value == g_Temp[g_Temp.Count - 1].GetComponent<C_CardScript>().g_Value + 1)
                        && (g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().g_Top.GetComponent<C_CardScript>().g_color != g_Temp[g_Temp.Count - 1].GetComponent<C_CardScript>().g_color))
                    {
                        for (int j = 0; j < g_Temp.Count; j++)
                        {
                            g_GameManagerScript.g_cardSlots[i + 6].GetComponent<C_CardSlotScript>().m_Push(g_Temp[g_Temp.Count - j - 1]);
                        }
                        g_isTempNull = true;
                        g_Temp.Clear();
                        g_objectPopped = false;
                        m_OpenTheFaceOfTopCard(g_PreviousStackNum);
                        return;
                    }                   
                }
            }
            for (int j = 0; j < g_Temp.Count; j++)
            {
                g_GameManagerScript.g_cardSlots[g_PreviousStackNum].GetComponent<C_CardSlotScript>().m_Push(g_Temp[g_Temp.Count - j - 1]);
            }
            g_isTempNull = true;
            g_Temp.Clear();
            g_objectPopped = false;
        }
    }
    void m_OpenTheFaceOfTopCard(int l_StackNum)
    {
        if (g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_Top != null)
        {
            g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_Top.transform.GetChild(0).gameObject.SetActive(true);
            g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_Top.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    void m_FindingClicksOnDeal(Ray l_ray)
    { 
        if(l_ray.origin.y>=5.5f && l_ray.origin.y<=9.5f && l_ray.origin.x<=-14 && l_ray.origin.x>=-16)
        {
            if (g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().g_Index <= 0)
            {

                m_ResetDealDeck();
                return;
            }
            g_Temp.Add( g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().m_Pop());
            g_isTempNull = false;
            g_GameManagerScript.g_cardSlots[1].GetComponent<C_CardSlotScript>().m_Push(g_Temp[0]);
            m_OpenTheFaceOfTopCard(1);
           
            
            
            g_isTempNull = true;
            g_Temp.Clear();
            
        }
    }
    void m_ResetDealDeck()
    {
        //print(g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().g_Index);
        if(g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().g_Index <= 0)
        {            
            //      g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().g_Index++;
            var l_NumOfCardsInDeck2 = g_GameManagerScript.g_cardSlots[1].GetComponent<C_CardSlotScript>().g_Index - 1;
            for (int i = l_NumOfCardsInDeck2; i >= 0; i--)
            {
                m_CloseFacesOfCards(i, 1);
                g_Temp.Add( g_GameManagerScript.g_cardSlots[1].GetComponent<C_CardSlotScript>().m_Pop());              
                g_isTempNull = false;
                g_GameManagerScript.g_cardSlots[0].GetComponent<C_CardSlotScript>().m_Push(g_Temp[0]);
                g_isTempNull = true;
                g_Temp.Clear();
            }
            
        }
    }
    void  m_CloseFacesOfCards(int l_CardNum,int l_StackNum)
    {
        if (g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_stack[l_CardNum] != null)
        {
            g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_stack[l_CardNum].transform.GetChild(0).gameObject.SetActive(false);
            g_GameManagerScript.g_cardSlots[l_StackNum].GetComponent<C_CardSlotScript>().g_stack[l_CardNum].transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    void m_RemoveParent()
    {
       for(int i =0;i<g_Temp.Count;i++)
        {
            g_Temp[i].gameObject.transform.parent = this.gameObject.transform;
        }
    }
    void m_CheckGameOver()
    {
        for (int i = 0; i < 4; i++)
        {
            print(g_GameManagerScript.g_cardSlots[i+2].GetComponent<C_CardSlotScript>().g_Index);
            if(g_GameManagerScript.g_cardSlots[i+2].GetComponent<C_CardSlotScript>().g_Index != 13)
            {
                return;
            }
                        
        }
        print("You Won");
        m_LoadGameOverScene();
    }
    
    void m_LoadGameOverScene()
    {
        SceneManager.LoadScene("Game Over");
    }
}