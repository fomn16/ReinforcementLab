using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SARSAAgentManager : MonoBehaviour, IAgentManager
{
    //TODO esse get set nao precisa se o campo for publico
    public int nEpisodes { get; set; }
    public int  maxSteps { get; set; }
    public IAgent  agent { get; set; }
    
    private Environment environment;

    // Default parameter values
    private double learningRate;
    private double discountFactor;
    private double epsilon;
    private double epsilonDecay;

    // GUI
    private HUDArrowScript arrows;
    private SARSAAgentMenu agentMenu;

    void Start() 
    {
        this.environment = GameObject.FindObjectOfType<Environment>();

        Dictionary<string, object> SARSAkwargs = new Dictionary<string, object>()
        {
            {"learningRate",   this.learningRate},
            {"discountFactor", this.discountFactor},
            {"epsilon",        this.epsilon},
            {"epsilonDecay",   this.epsilonDecay}
        };

        this.agent = new SARSAAgent(this.environment.nStates,
                                    this.environment.nActions,
                                    this.environment.initState,
                                    SARSAkwargs);

        this.environment.agent = this.agent;

        this.setupMenu();
    }

    // Update is called once per frame
    void Update()
    {
        updateMenu();
    }

    void OnDestroy()
    {
        DestroyImmediate(this.arrows.gameObject);
        DestroyImmediate(this.agentMenu.gameObject);
    }

    void setupMenu()
    {
        var cameraTransform = GameObject.Find("Main Camera").transform;

        // Setup arrows UI
        var arrowsPrefab = Resources.Load("Prefabs/HUDArrowScript", typeof(GameObject)) as GameObject;
        this.arrows = Instantiate(arrowsPrefab, cameraTransform.position, cameraTransform.rotation).GetComponent<HUDArrowScript>();
        this.arrows.transform.SetParent(transform);

        // Setup agent parameters menu
        var agentMenuPrefab = Resources.Load("Prefabs/SARSAAgentMenu", typeof(GameObject)) as GameObject;
        this.agentMenu = Instantiate(agentMenuPrefab, cameraTransform.position, cameraTransform.rotation).GetComponent<SARSAAgentMenu>();
        this.agentMenu.transform.SetParent(transform);
    }

    void updateMenu()
    {
        this.arrows.values = (double[])((SARSAAgent) this.agent).qTable[this.agent.currentState];
        this.agent.learningRate = this.agentMenu.learningRate;
        this.agent.discountFactor = this.agentMenu.discountFactor;
        this.agent.epsilon = this.agentMenu.epsilon;
        this.agent.epsilonDecay = this.agentMenu.epsilonDecay;
    }
}