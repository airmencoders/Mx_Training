using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NewMenuUI : MonoBehaviour
{
    public bool DebugMode = false;

    public Transform Canvas;

    public enum MenuModes
    {
        MainMenu,
        MCDMenu,
        ICSMenu,
        RefuelMenu,
        About,
        Quit
    };

    public MenuModes CurrentMenu = MenuModes.MainMenu;

    public Button Button_MCD;
    public Button Button_ICS;
    public Button Button_Refuel;
    public Button Button_About;
    public Button Button_Quit;

    // Start is called before the first frame update
    void Start()
    {
        if (DebugMode) Debug.Log("CAUTION: LOCAL MENUS DEBUGGER ON!");
        Button_MCD.onClick.AddListener(MCDButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MCDButton()
    {
        CurrentMenu = MenuModes.MCDMenu;
    }
}
