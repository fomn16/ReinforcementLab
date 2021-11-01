using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgentManager
{
    public int nEpisodes { get; set; }
    public int maxSteps { get; set; }
    public IAgent agent { get; set; }

    // TODO adicionar decalracao do metodo init na interface
}