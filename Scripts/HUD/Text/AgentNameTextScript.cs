using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AgentNameTextScript : MonoBehaviour
{
    public TextMeshProUGUI textmeshPro;
    public GameMenu contextMenu;
    // Start is called before the first frame update
    void Start()
    {
        this.contextMenu = GameObject.FindObjectOfType<GameMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        string name = GameMenu.managerTypes[contextMenu.selectedAgent].ToString();
        name = name.Replace("AgentManager","");
        textmeshPro.SetText(name);
    }
}
