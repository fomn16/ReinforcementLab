using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIGridContainer : MonoBehaviour
{
    public UIGridRenderer nStepsPerEpisodeGrid;
    public UIGridRenderer accRewardsGrid;
    public UIGridRenderer avgRewardsGrid;

    public void InitGridContainer()
    {
        var gridPrefab = Resources.Load("Prefabs/Grid", typeof(Graphic)) as Graphic;
        this.transform.position = new Vector3(0, 0, 0);

        this.nStepsPerEpisodeGrid = Instantiate(gridPrefab).GetComponent<UIGridRenderer>();
        this.nStepsPerEpisodeGrid.transform.SetParent(this.transform);

        this.accRewardsGrid = Instantiate(gridPrefab).GetComponent<UIGridRenderer>();
        this.accRewardsGrid.transform.SetParent(this.transform);
        this.accRewardsGrid.transform.position += Vector3.right * 200;

        this.avgRewardsGrid = Instantiate(gridPrefab).GetComponent<UIGridRenderer>();
        this.avgRewardsGrid.transform.SetParent(this.transform);
        this.avgRewardsGrid.transform.position += Vector3.right * 400;
    }
}