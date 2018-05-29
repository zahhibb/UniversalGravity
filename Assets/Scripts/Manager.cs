using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
    
    [SerializeField] private bool m_activateSimulation;

    private GameObject[] m_debris;
    private GameObject m_planet;

    void Start()
    {
        m_activateSimulation = true;
        m_planet = GameObject.FindGameObjectWithTag("Planet");
        m_debris = GameObject.FindGameObjectsWithTag("Debris");        
    }

    void Update()
    {
        foreach (GameObject obj in m_debris)
        {
            obj.GetComponent<Base>().ActivateSimulation = m_activateSimulation;
        }
        m_planet.GetComponent<Base>().ActivateSimulation = m_activateSimulation;
    }
}
