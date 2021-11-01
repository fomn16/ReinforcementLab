// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DQNAgentManager : MonoBehaviour, IAgentManager
// {
//     //TODO esse get set nao precisa se o campo for publico
//     public int nEpisodes { get; set; }
//     public int  maxSteps { get; set; }
//     public IAgent  agent { get; set; }
    
//     private Environment environment;

//     // Default parameter values
//     // TODO tirar f do final do numero
//     private double learningRate = 0.1f;
//     private double discountFactor = 0.99f;
//     private double epsilon = 0.8f;
//     private double epsilonDecay = 0.99f;

//     void Start() {
//         this.nEpisodes = 10;
//         this.maxSteps  = 10;

//         this.environment = GameObject.FindObjectOfType<Environment>();

//         Dictionary<string, object> DQNkwargs = new Dictionary<string, object>()
//         {
//             {"learningRate",   this.learningRate},
//             {"discountFactor", this.discountFactor},
//             {"epsilon",        this.epsilon},
//             {"epsilonDecay",   this.epsilonDecay} //TODO add model parameters
//         };

//         this.agent = new DQNAgent(this.environment.nStates,
//                                  this.environment.nActions,
//                                  this.environment.initState,
//                                  DQNkwargs);

//         this.environment.agent = this.agent;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyDown("f"))
//         {
//             environment.animate = false;
//             environment.showGUI = false;
//             agent.RunEpisodes(nEpisodes, maxSteps);
//         }
//         else if (Input.GetKeyDown("g"))
//         {
//             environment.animate = true;
//             environment.showGUI = false;
//             agent.RunEpisodes(nEpisodes, maxSteps);
//         }
//         else if (Input.GetKeyDown("h"))
//         {
//             environment.animate = true;
//             environment.showGUI = true;
//             double tempEpsilon = agent.epsilon;
//             agent.epsilon = 0;
//             agent.RunEpisodes(1, 1);
//             agent.epsilon = tempEpsilon;
//         }
//         else if (Input.GetKeyDown("r"))
//         {
//             environment.EndEpisode();
//         }
//     }
// }