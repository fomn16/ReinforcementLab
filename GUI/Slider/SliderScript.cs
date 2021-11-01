using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    public TextMeshProUGUI textmeshPro;

    public float value;

    // Start is called before the first frame update
    void Start()
    {
        this.textmeshPro = gameObject.transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        this.textmeshPro.SetText(this.value.ToString("0.##"));
        float currValue = this.gameObject.GetComponent<Slider>().value;
        if(currValue == this.value)
            this.gameObject.GetComponent<Slider>().value = this.value;
        else
        {
            this.value = currValue;
        }
    }

    public void InitSlider(string name, float initValue)
    {
        gameObject.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().SetText(name);
        this.value = initValue;
        this.gameObject.GetComponent<Slider>().value = initValue;
    }
}
