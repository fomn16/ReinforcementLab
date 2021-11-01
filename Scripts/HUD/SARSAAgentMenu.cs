using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SARSAAgentMenu : MonoBehaviour
{
    public double learningRate;
    public double discountFactor;
    public double epsilon;
    public double epsilonDecay;

    public SliderScript learningRateSlider;
    public SliderScript discountFactorSlider;
    public SliderScript epsilonSlider;
    public SliderScript epsilonDecaySlider;

    void Start()
    {
        // Learning rate: double (0:1)
        // Discount factor: double (0:1)
        // Epsilon init: double (0:1)
        // Epsilon decay: double (0:1)

        var sliderPrefab = Resources.Load("Prefabs/Slider", typeof(GameObject)) as GameObject;
        this.transform.position = new Vector3(0,0,0);


        this.learningRateSlider = Instantiate(sliderPrefab).GetComponent<SliderScript>();
        this.learningRateSlider.transform.SetParent(this.transform);
        this.learningRateSlider.InitSlider("(Learning Rate)", 0.1f);
        this.learningRateSlider.transform.position += Vector3.up * 80;
        this.learningRateSlider.transform.position += Vector3.right * 90;

        this.discountFactorSlider = Instantiate(sliderPrefab).GetComponent<SliderScript>();
        this.discountFactorSlider.transform.SetParent(this.transform);
        this.discountFactorSlider.InitSlider("(Discount Factor)", 0.9f);
        this.discountFactorSlider.transform.position += Vector3.up * 30;
        this.discountFactorSlider.transform.position += Vector3.right * 90;

        this.epsilonSlider = Instantiate(sliderPrefab).GetComponent<SliderScript>();
        this.epsilonSlider.transform.SetParent(this.transform);
        this.epsilonSlider.InitSlider("(Epsilon)", 0.8f);
        this.epsilonSlider.transform.position += Vector3.up * 80;
        this.epsilonSlider.transform.position += Vector3.right * 300;

        this.epsilonDecaySlider = Instantiate(sliderPrefab).GetComponent<SliderScript>();
        this.epsilonDecaySlider.transform.SetParent(this.transform);
        this.epsilonDecaySlider.InitSlider("(Epsilon Decay)", 0.9f);
        this.epsilonDecaySlider.transform.position += Vector3.up * 30;
        this.epsilonDecaySlider.transform.position += Vector3.right * 300;
    }

    void Update()
    {
        this.learningRate = (double) this.learningRateSlider.value;
        this.discountFactor = (double) this.discountFactorSlider.value;
        this.epsilon = (double) this.epsilonSlider.value;
        this.epsilonDecay = (double) this.epsilonDecaySlider.value;
    }
}