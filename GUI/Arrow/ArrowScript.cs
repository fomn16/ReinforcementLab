using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ArrowScript : MonoBehaviour
{
    private HUDArrowScript parent;
    public int index;
    public TextMeshProUGUI textmeshPro;
    public GameMenu contextMenu;

    // Start is called before the first frame update
    void Start()
    {
        this.contextMenu = GameObject.FindObjectOfType<GameMenu>();
        this.parent = transform.parent.GetComponent<HUDArrowScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.contextMenu.stepByStep)
        {
            transform.GetChild(0).gameObject.SetActive(true);

            double max = 0;

            for (int i =0; i< 4; i++)
            {
                double directionReward = Math.Abs(this.parent.values[i]);
                if (directionReward > max)
                    max = directionReward;
            }
            if (max == 0) max++;
            double number = (this.parent.values[index]/max);
            textmeshPro.SetText(number.ToString("0.####"));
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }
}
