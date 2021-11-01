using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public IAgentManager manager;
    public MazeManager mazeManager;

    public Environment environment;

    public int selectedAgent = 0;
    public int prevAgent = 0;

    public int mazeSize = 4;
    public int newMazeSize = 4;

    public bool stepByStep;

    public int nEpisodes = 1;
    public int nSteps = 1;

    public List<UIGridContainer> metricsContainers = new List<UIGridContainer>();
    public int currentMetrics = 0;

    public bool isShowingMetrics = false;

    public static List<Type> managerTypes = new List<Type>()
    {
        typeof(QLAgentManager),
        typeof(SARSAAgentManager)
    };

    void Start()
    {    
        this.stepByStep = false;
        this.InitSimulation();
    }


    // UI events

        // Simulation events
    public void OnNEpisodesChanged(string newNEpisodes){
        int n = 1;

        if (int.TryParse(newNEpisodes, out n)){
            this.nEpisodes = n;
        }
    }

    public void OnNStepsChanged(string newNSteps){
        int n = 1;

        if (int.TryParse(newNSteps, out n)){
            this.nSteps = n;
        }
    }

    public void OnResetSimulation(){
        this.environment.EndEpisode();
    }

    public void OnRunSimulation(){
        int maxSteps = this.nEpisodes * this.nSteps;

        if(maxSteps == 1){
            this.environment.animate = true;
            this.stepByStep = true;
            /*double tempEpsilon = this.environment.agent.epsilon;
            this.environment.agent.epsilon = 0;*/
            this.environment.agent.RunEpisodes(this.nEpisodes, this.nSteps);
            //this.environment.agent.epsilon = tempEpsilon;
        }
        else if(this.nEpisodes == 1){
            this.OnResetSimulation();
            this.environment.animate = true;
            this.stepByStep = false;
            this.environment.agent.RunEpisodes(this.nEpisodes, this.nSteps);
        }
        else{
            this.OnResetSimulation();
            this.environment.animate = false;
            this.stepByStep = false;
            this.environment.agent.RunEpisodes(this.nEpisodes, this.nSteps);
        }
    }

    public void OnChangeAgent(){
        this.environment.EndEpisode();
        this.prevAgent = this.selectedAgent;
        this.selectedAgent++;
        this.selectedAgent %= managerTypes.Count;

        this.InitAgentManager();
    }

    // Maze events
    public void OnChangeMazeSize(float newMazeSize){
        this.newMazeSize = (int)newMazeSize;
    }

    public void OnApplyMazeSize(){
        this.mazeSize = this.newMazeSize;
        this.mazeManager.ResetMazeManager(this.mazeSize);
        
        this.environment.InitEnvironment(this.mazeManager.cells,
                                         this.mazeManager.goalPos,
                                         this.mazeSize,
                                         this.mazeManager.initPos);
        this.prevAgent = this.selectedAgent;
        this.InitAgentManager();
    }

    // Metrics Events
    public void OnToggleMetrics()
    {
        if(!this.isShowingMetrics && this.metricsContainers.Count == 0)
        {
            this.OnNewMetric();
        }

        this.isShowingMetrics = !this.isShowingMetrics;

        RefreshMetricsButtons();
    }
    
    public void RefreshMetricsButtons()
    {
        transform.Find("MetricsUICenter").Find("MetricsBackground").gameObject.SetActive(this.isShowingMetrics);
        transform.Find("MetricsUICenter").Find("NewMetric").gameObject.SetActive(this.isShowingMetrics);
        transform.Find("MetricsUICenter").Find("DeleteMetric").gameObject.SetActive(this.isShowingMetrics && this.metricsContainers.Count > 1);
        transform.Find("MetricsUICenter").Find("NextMetric").gameObject.SetActive(this.isShowingMetrics && this.metricsContainers.Count > 1 && this.currentMetrics < this.metricsContainers.Count - 1);
        transform.Find("MetricsUICenter").Find("PrevMetric").gameObject.SetActive(this.isShowingMetrics && this.currentMetrics > 0);
        this.metricsContainers[this.currentMetrics].gameObject.SetActive(this.isShowingMetrics);
    }

    public void SelectContainder(int id)
    {
        if(id != this.currentMetrics && this.currentMetrics < this.metricsContainers.Count)
        {
            this.metricsContainers[this.currentMetrics].gameObject.SetActive(false);
        }

        this.metricsContainers[id].gameObject.SetActive(true);
        this.currentMetrics = id;
    }

    public void OnNewMetric()
    {
        var metricsContainerPrefab = Resources.Load("Prefabs/GridContainer", typeof(GameObject)) as GameObject;
        this.transform.position = new Vector3(0, 0, 0);
        UIGridContainer tempContainer = Instantiate(metricsContainerPrefab).GetComponent<UIGridContainer>();
        tempContainer.transform.SetParent(this.transform);
        tempContainer.transform.Translate(new Vector3(-25, 20));

        tempContainer.InitGridContainer();

        this.PlotNStepsPerEpisode(tempContainer);
        this.PlotAccumulatedRewards(tempContainer);
        this.PlotAverageRewards(tempContainer);

        tempContainer.transform.Find("ContainerName").GetComponent<Text>().text =   this.mazeSize.ToString() + " x " + this.mazeSize.ToString()
                                                                                    + " " + GameMenu.managerTypes[this.selectedAgent].ToString().Replace("AgentManager", "")
                                                                                    + " lr: " + this.manager.agent.learningRate.ToString("0.##")
                                                                                    + " df: " + this.manager.agent.discountFactor.ToString("0.##")
                                                                                    + " eps: " + this.manager.agent.epsilon.ToString("0.##")
                                                                                    + " eps dec: " + this.manager.agent.epsilonDecay.ToString("0.##")
                                                                                    + ", " + this.nEpisodes.ToString() + " episode" + (this.nEpisodes > 1 ? "s":"")
                                                                                    + ", " + this.nSteps.ToString() + " step" + (this.nSteps > 1 ? "s" : "");

        this.metricsContainers.Add(tempContainer);

        SelectContainder(this.metricsContainers.Count - 1);

        RefreshMetricsButtons();
    }

    public void OnDeleteMetric()
    {
        var container = this.metricsContainers[this.currentMetrics];
        this.metricsContainers.Remove(container);
        DestroyImmediate(container.gameObject);

        if(this.currentMetrics == 0)
        {
            SelectContainder(0);
        }
        else if (this.currentMetrics == this.metricsContainers.Count)
        {
            SelectContainder(this.metricsContainers.Count - 1);
        }
        else
        {
            SelectContainder(this.currentMetrics);
        }

        RefreshMetricsButtons();
    }

    public void OnNextMetric()
    {
        SelectContainder(this.currentMetrics + 1);
        RefreshMetricsButtons();
    }

    public void OnPrevMetric()
    {
        SelectContainder(this.currentMetrics - 1);
        RefreshMetricsButtons();
    }

    public void PlotNStepsPerEpisode(UIGridContainer container)
    {
        int[] nStepsPerEpisode = this.manager.agent.nStepsPerEpisode;
        if (nStepsPerEpisode != null)
        {
            Vector2[] pointsToPlot = new Vector2[nStepsPerEpisode.Length];
            for (int i = 0; i < nStepsPerEpisode.Length; i++)
                pointsToPlot[i] = new Vector2(i, nStepsPerEpisode[i]);
            container.nStepsPerEpisodeGrid.points = pointsToPlot;
        }
        container.nStepsPerEpisodeGrid.graphNameStr = "Steps to Goal";
    }

    public void PlotAccumulatedRewards(UIGridContainer container)
    {
        double[] accumulatedRewards = this.manager.agent.accumulatedRewards;
        if (accumulatedRewards != null)
        {
            Vector2[] pointsToPlot = new Vector2[accumulatedRewards.Length];
            for (int i = 0; i < accumulatedRewards.Length; i++)
                pointsToPlot[i] = new Vector2(i, (float)accumulatedRewards[i]);

            container.accRewardsGrid.points = pointsToPlot;
        }
        container.accRewardsGrid.graphNameStr = "Accumulated Reward";
    }

    public void PlotAverageRewards(UIGridContainer container)
    {
        double[] averageRewards = this.manager.agent.averageRewards;
        if (averageRewards != null)
        {
            Vector2[] pointsToPlot = new Vector2[averageRewards.Length];
            for (int i = 0; i < averageRewards.Length; i++)
                pointsToPlot[i] = new Vector2(i, (float)averageRewards[i]);

            container.avgRewardsGrid.points = pointsToPlot;
        }
        container.avgRewardsGrid.graphNameStr = "Average Rewards";
    }

    void InitAgentManager()
    {
        // Initializes selected agent and destroys previously active agent
        if(this.manager != null)
        {
            DestroyImmediate(GetComponent(managerTypes[this.prevAgent]));
        }
        this.manager = gameObject.AddComponent(managerTypes[this.selectedAgent]) as IAgentManager;
    }

    void InitSimulation()
    {
        this.mazeManager.InitMazeManager(mazeSize);
        this.environment.InitEnvironment(this.mazeManager.cells,
                                         this.mazeManager.goalPos,
                                         this.mazeSize,
                                         this.mazeManager.initPos);
        this.InitAgentManager();
    }
}