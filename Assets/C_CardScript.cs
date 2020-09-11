using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_color
{
    red,
    black
}
public class C_CardScript : MonoBehaviour
{
    public int g_SlotNum;
    public int g_Value;
    public e_color g_color;
    public int g_SuiteNum;
    [SerializeField]
    public Material g_CardTextColor;
    public  int g_NumInStack;
    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void m_changeColor()
    {
        if (g_color == e_color.red)
        {                    
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshPro>().faceColor = Color.red;
        }
    }
}