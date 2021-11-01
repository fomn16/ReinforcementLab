using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeSizeTextScript : MonoBehaviour
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
        textmeshPro.SetText(contextMenu.newMazeSize.ToString());
    }
}
