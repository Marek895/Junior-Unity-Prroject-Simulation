using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHealth : MonoBehaviour
{
    public static event Action agentDespawn;
    public static event Action<int, string> updateAgentHP;
    public static event Action clearAgentSelection; //allows to clear the UI when the selected agent dies
    [SerializeField] Material agenttInjuredMaterial;
    [SerializeField] Material agenttBadlyInjuredMaterial;
    [SerializeField] int maxHealthPoints = 3;

    public int currentHealthpoints;

    
    void Start()
    {
        currentHealthpoints = maxHealthPoints;
    }

    void OnTriggerEnter(Collider other)
    {
        ProcessCollision();
        ChangeAgentCollor(); // change agent colllor when collides with another
    }

    private void ChangeAgentCollor()
    {
        foreach (Transform bodypart in transform) 
        {

            if (currentHealthpoints == 2)
            {
                bodypart.GetComponent<MeshRenderer>().material = agenttInjuredMaterial;
            }
            else if (currentHealthpoints == 1)
            {
                bodypart.GetComponent<MeshRenderer>().material = agenttBadlyInjuredMaterial;
            }

        }
    }

    private void ProcessCollision()
    {
        currentHealthpoints--;

        if (transform.childCount > 4) //Selected agent can have updated HP on UI when touches another agent
        {
            updateAgentHP?.Invoke(gameObject.GetComponent<AgentHealth>().currentHealthpoints, gameObject.name);
        }
        if (currentHealthpoints <= 0)
        {
            if (transform.childCount > 4) //Agent Selection Mark is always 5th child of agent Object , so when is dies with Agent Selection Mark on it, the UI is cleared
            {
                clearAgentSelection?.Invoke();
            }

            AgentDespawn();
        }
    }

    void AgentDespawn()
    {
        this.gameObject.SetActive(false);
        agentDespawn?.Invoke();
    }
}
