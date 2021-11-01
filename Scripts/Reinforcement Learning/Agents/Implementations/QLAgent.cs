using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QLAgent : IAgent
{
    public Environment env { get; set; }
    public double learningRate { get; set; }
    public double discountFactor { get; set; }
    public double epsilon { get; set; }
    public double epsilonDecay { get; set; }
    public int    currentState { get; set; }

    // Metrics
    public int[] nStepsPerEpisode { get; set; }
    public double[] accumulatedRewards { get; set; }
    public double[] averageRewards { get; set; }
    // Number of episodes to find the goal for the first time (if 0, goal was not found)
    public int episodesToGoal { get; set; }

    // TODO privatizar
    public double[][] qTable;
    public int nActions;
    public int nextState;

    private int currentEpisode;

    // Reward definitions
    static double goalReward = 100;     // Reward for reaching the goal
    static double wallReward = -10000;  // Reward for hitting a wall
    static double normReward = -1;      // Normal reward for walking to empty cell

    public QLAgent(int nStates, int nActions, int initState,
                   Dictionary<string, object> kwargs = null)
    {
        this.env          = GameObject.FindObjectOfType<Environment>();
        this.qTable = new double[nStates][];
        for (int i = 0; i < nStates; i++)
        {
            this.qTable[i] = new double[nActions];
        }
        this.nActions     = nActions;
        this.currentState = initState;

        // Q-Learning specific parameters
        this.learningRate   = (double) kwargs["learningRate"];
        this.discountFactor = (double) kwargs["discountFactor"];
        this.epsilon        = (double) kwargs["epsilon"];
        this.epsilonDecay   = (double) kwargs["epsilonDecay"];
    }

    public void RunEpisodes(int nEpisodes, int maxSteps)
    {
        // TODO o epsilon que o usuario mexe no menu eh um initEpsilon
        // this.epsilon = this.initEpsilon
        
        // Reset metric variables
        episodesToGoal = 0;
        nStepsPerEpisode = new int[nEpisodes];
        accumulatedRewards = new double[nEpisodes];
        averageRewards = new double[nEpisodes];
        
        for(int episode = 0; episode < nEpisodes; episode++)
        {
            this.currentEpisode = episode;
            for(int step = 0; step < maxSteps; step++)
            {
                nStepsPerEpisode[episode] += 1;
                // Break if agent reached goal
                if(this.Iterate())
                {
                    episodesToGoal = episode + 1;
                    env.EndEpisode();
                    break;
                }
            }
            epsilon *= epsilonDecay;

            averageRewards[episode] = accumulatedRewards[episode] / nStepsPerEpisode[episode];

            // Avoids episode end if the user is going step by step
            if(maxSteps > 1)
            {
                env.EndEpisode();
            }
        }
    }

    public bool Iterate()
    {
        int action      = ChooseAction(currentState);
        this.nextState  = this.env.UpdateState(action);
        double reward   = ComputeReward();
        double oldValue = qTable[currentState][action];

        double optimalFutureValue = qTable[nextState].Max();

        qTable[currentState][action] =
            (1-learningRate)*oldValue +
            learningRate*(reward + discountFactor*optimalFutureValue);

        this.currentState = this.nextState;

        this.accumulatedRewards[this.currentEpisode] += reward;

        // If the reward was the goal reward, continue to next episode
        if((reward == goalReward)){
            return true;    // Episode has ended
        }

        return false;         // Continue episode
    }

    public double ComputeReward()
    {
        if(nextState == currentState)
            return wallReward;

        if(env.CheckGoal(nextState))
            return goalReward;

        return normReward;
    }

    public int ChooseAction(int state)
    {
        int action;
        double rand = Random.Range(0,1f);
        if(rand > epsilon)
        {
            // Choose optimal action with probability 1-epsilon
            action = (int)qTable[state].ToList().IndexOf(qTable[state].Max());
        } 
        else
        {
            // Choose random action with probability epsilon
            action = Random.Range(0,nActions);
        }
        return action;
    }
}