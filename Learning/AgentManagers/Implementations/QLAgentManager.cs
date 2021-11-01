using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QLAgentManager : MonoBehaviour, IAgentManager
{
    //TODO esse get set nao precisa se o campo for publico
    public int nEpisodes { get; set; }
    public int  maxSteps { get; set; }
    public IAgent agent { get; set; }
    
    private Environment environment;

    // Default parameter values
    private double learningRate;
    private double discountFactor;
    private double epsilon;
    private double epsilonDecay;

    // GUI
    private HUDArrowScript arrows;
    private QLAgentMenu agentMenu;

    void Start()
    {
        this.environment = GameObject.FindObjectOfType<Environment>();

        Dictionary<string, object> QLkwargs = new Dictionary<string, object>()
        {
            {"learningRate",   this.learningRate},
            {"discountFactor", this.discountFactor},
            {"epsilon",        this.epsilon},
            {"epsilonDecay",   this.epsilonDecay}
        };

        this.agent = new QLAgent(this.environment.nStates,
                                 this.environment.nActions,
                                 this.environment.initState,
                                 QLkwargs);

        this.environment.agent = this.agent;

        this.SetupMenu();
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateMenu();
    }
    
    void OnDestroy()
    {
        DestroyImmediate(this.arrows.gameObject);
        DestroyImmediate(this.agentMenu.gameObject);
    }

    void SetupMenu()
    {
        var cameraTransform = GameObject.Find("Main Camera").transform;

        // Setup arrows UI
        var arrowsPrefab = Resources.Load("Prefabs/HUDArrowScript", typeof(GameObject)) as GameObject;
        this.arrows = Instantiate(arrowsPrefab, cameraTransform.position, cameraTransform.rotation).GetComponent<HUDArrowScript>();
        this.arrows.transform.SetParent(transform);

        // Setup agent parameters menu
        var agentMenuPrefab = Resources.Load("Prefabs/QLAgentMenu", typeof(GameObject)) as GameObject;
        this.agentMenu = Instantiate(agentMenuPrefab, cameraTransform.position, cameraTransform.rotation).GetComponent<QLAgentMenu>();
        this.agentMenu.transform.SetParent(transform);
    }

    void UpdateMenu()
    {
        this.arrows.values = (double[])((QLAgent) this.agent).qTable[this.agent.currentState];
        this.agent.learningRate = this.agentMenu.learningRate;
        this.agent.discountFactor = this.agentMenu.discountFactor;
        this.agent.epsilon = this.agentMenu.epsilon;
        this.agent.epsilonDecay = this.agentMenu.epsilonDecay;
    }
}