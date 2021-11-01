// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using static Tensorflow.Binding;
// using static Tensorflow.KerasApi;
// using Tensorflow;
// using Tensorflow.NumPy;

// public class DQNAgent : IAgent
// {
//     public Environment env { get; set; }
//     public double learningRate { get; set; }
//     public double discountFactor { get; set; }
//     public double epsilon { get; set; }
//     public double epsilonDecay { get; set; }
//     public int    currentState { get; set; }
//     public int[]  nStepsPerEpisode { get; set; }
//     public int[]  accRewardPerEpisode { get; set; }

//     // TODO privatizar
//     public Tensorflow.Keras.Engine.Sequential model;
//     public int nActions;
//     public int nextState;

//     // Reward definitions
//     static double goalReward = 100;     // Reward for reaching the goal
//     static double wallReward = -10000;  // Reward for hitting a wall
//     static double normReward = -1;      // Normal reward for walking to empty cell

//     public DQNAgent(int nStates, int nActions, int initState,
//                     Dictionary<string, object> kwargs = null)
//     {
//         this.env          = GameObject.FindObjectOfType<Environment>();
//         this.nActions     = nActions;
//         this.currentState = initState;

//         // DQN specific parameters
//         this.learningRate   = (double) kwargs["learningRate"];
//         this.discountFactor = (double) kwargs["discountFactor"];
//         this.epsilon        = (double) kwargs["epsilon"];
//         this.epsilonDecay   = (double) kwargs["epsilonDecay"];

//         // Build neural network model
//         this.model = keras.Sequential();
//         this.model.add(keras.layers.Dense(5, activation: "relu"));
//         this.model.add(keras.layers.Dense(5, activation: "relu"));
//         this.model.add(keras.layers.Dense(4, activation: "linear"));
        
//     }

//     public void RunEpisodes(int nEpisodes, int maxSteps)
//     {
//         for(int episode = 0; episode < nEpisodes; episode++) {
//             for(int step = 0; step < maxSteps; step++) {
//                 // break if agent reached goal
//                 if(this.Iterate())
//                 {
//                     env.EndEpisode();
//                     break;
//                 }
//             }
//             epsilon *= epsilonDecay;
//         }
//     }

//     public bool Iterate()
//     {
//         int action     = ChooseAction(currentState);
//         this.nextState = this.env.UpdateState(action);
//         double reward  = ComputeReward();

//         Tensor nextStateTensor = tf.convert_to_tensor(this.nextState);
//         double futureReward =
//             (reward + this.discountFactor*np.amax(this.model.predict(nextStateTensor)[0].numpy()));

//         Tensor currentStateTensor = tf.convert_to_tensor(this.currentState);
//         NDArray targets = this.model.predict(currentStateTensor)[0].numpy();
//         targets[0][action] = futureReward;

//         this.model.fit(this.currentState, targets, verbose: 0);

//         this.currentState = this.nextState;

//         // If the reward was the goal reward, continue to next episode
//         if((reward == goalReward)){
//             return true;    // Episode has ended
//         }

//         return false;         // Continue episode
//     }

//     public double ComputeReward()
//     {
//         if(nextState == currentState)
//             return wallReward;

//         if(env.CheckGoal(nextState))
//             return goalReward;

//         return normReward;
//     }

//     public int ChooseAction(int state)
//     {
//         int action;
//         double rand = Random.Range(0,1f);
//         if(rand > epsilon)
//         {
//             // Choose optimal action with probability 1-epsilon
//             NDArray stateArray = np.array(state);
//             Tensor stateTensor = tf.convert_to_tensor(stateArray);
//             Tensors temp_pred = this.model.predict(stateTensor);
            
           
//             action = np.argmax(temp_pred[0].numpy());
//         } 
//         else
//         {
//             // Choose random action with probability epsilon
//             action = Random.Range(0,nActions);
//         }
//         return action;
//     }
// }