using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    public Environment env { get; set; }

    public double learningRate { get; set; }
    public double discountFactor { get; set; }
    public double epsilon { get; set; }
    public double epsilonDecay { get; set; }
    public int    currentState { get; set; }
    public int[] nStepsPerEpisode { get; set; }
    public double[] accumulatedRewards { get; set; }
    public double[] averageRewards { get; set; }

    // Number of episodes to find the goal for the first time (if 0, goal was not found)
    public int episodesToGoal { get; set; }

    public void   RunEpisodes(int nEpisodes, int maxSteps);
    public bool   Iterate();
    public double ComputeReward();
    public int    ChooseAction(int state);     
}
