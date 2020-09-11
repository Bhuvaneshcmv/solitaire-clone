using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_SlotType
{
    top,
    bottom
}
public class C_CardSlotScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] g_stack;
    public GameObject g_Top;
    public int g_Index;
    public int g_Num;
    public e_SlotType g_SlotType;
    void Awake()
    {
        g_stack = new GameObject[52];
        g_Index = 0;
        g_Top = null;
    }

    // Update is called once per frame
    void Update()
    {
       // m_DisableGameobjectsOnTopRow();
    }
    public void m_Push(GameObject l_object)
    {
        Vector3 l_pos = this.gameObject.transform.position;
        g_stack[g_Index] = l_object;
        l_object.GetComponent<C_CardScript>().g_SlotNum = g_Num;
        if (g_SlotType == e_SlotType.top)
        {            
            l_pos.z = -0.5f * g_Index;
            l_object.transform.position = l_pos;              
        }
        else
        {
            l_pos.z = -0.5f * g_Index;
            l_pos.y -= 1f * g_Index;
            l_object.transform.position = l_pos;
        }
        l_object.GetComponent<C_CardScript>().g_NumInStack = g_Index+1;
        g_Top = g_stack[g_Index];
        g_Index++;       
    }
    public GameObject m_Pop()
    {   if(g_Index > 0)
        {
            g_Index--;
            GameObject l_removedObject = g_stack[g_Index];
            l_removedObject.GetComponent<C_CardScript>().g_SlotNum = -1;
            g_stack[g_Index] = null;
            if (g_Index > 0) g_Top = g_stack[g_Index - 1];
            else
            {
                g_Top = null;
            }
            if (g_Index < 0)
            {
                g_Index = 0;
            }
            return (l_removedObject);
        }    
       return null;        
    }
    public void m_DisableGameobjectsOnTopRow()
    {
        if(this.g_SlotType == e_SlotType.top)
        {
            for(int i = g_Index-2;i>0;i--)
            {
                g_stack[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = g_Index; i >= 0; i--)
            {
                g_stack[i].gameObject.SetActive(true);
            }
        }
    }
}