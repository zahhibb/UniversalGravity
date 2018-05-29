using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    /* 
    FORMULA: F = G * (m1 * m2) / r^2
        F:  Force between the masses.
        G:  Gravitational constant. Standard: ~6.674 * 10^-11 N²•(m/kg)²
        m1: First mass.
        m2: Second mass.
        r:  Distance between the centers of the masses.

    GENERAL INFORMATION:
    Earth        
        Mass:               5.972 * 10^24 = 1 Solar mass
        Radius:             6371 km
        Avg. orbital speed: 107200 km/h
    Moon        
        Mass:               7.342 * 10^22
        Radius:             1737 km
        Avg. orbital speed: 3679200 km/h
    Gravity
        Constant:           6.674 * 10^-11
    Astronomical Units
        1 AU (Earth->Sun):  149 597 870 700 m
                            1.5813 * 10^-5 ly        
    */

    [SerializeField] private float m_overlapRadius = 20f;
    [SerializeField] private Vector3 m_startForce;
    [SerializeField] private bool m_showWireSphere;
    [SerializeField] private bool m_showSelectedLines;
    [SerializeField] private bool m_showSelectedWireSphere;

    private float GRAVITY_CONSTANT = 50;
    private Collider[] m_entities;
    private bool m_activateSimulation;

    public bool ActivateSimulation
    {
        set { m_activateSimulation = value; }
    }

    void Start()
    {
        m_showWireSphere = true;
        m_showSelectedLines = true;
        m_showSelectedWireSphere = true;
        
        GetComponent<Rigidbody>().AddForce(m_startForce, ForceMode.Impulse);
    }

    void Update()
    {
        if (m_activateSimulation)
        {
            m_entities = Physics.OverlapSphere(transform.position, m_overlapRadius);
            foreach (Collider entity in m_entities)
            {
                if (entity.GetComponent<Rigidbody>() != null)
                {
                    if (entity == gameObject)
                    {
                        continue;
                    }

                    float firstMass = GetComponent<Rigidbody>().mass;                                               // Mass of the first object.
                    float secondMass = entity.GetComponent<Rigidbody>().mass;                                       // Mass of the second object.

                    float distance = Vector3.Distance(transform.position, entity.transform.position);               // Distance between the center of the masses.

                    // To prevent NaN force values.
                    if (distance <= 0)
                    {
                        distance = 1;
                    }

                    float force = GRAVITY_CONSTANT * ((firstMass * secondMass) / Mathf.Pow(distance, 2));           // Force between the masses.
                    Vector3 forceDirection = Vector3.Normalize(entity.transform.position - transform.position);     // Direction of gravitational force.

                    Vector3 acceleration = (forceDirection * force) * Time.deltaTime;                               // Acceleration based on gravitational force and direction.

                    GetComponent<Rigidbody>().AddForce(acceleration, ForceMode.Force);

                    // Draws lines between "interacting" gameobjects.
                    if (gameObject.tag == "Planet")
                    {
                        Ray ray = new Ray(transform.position, entity.transform.position - transform.position);
                        RaycastHit hit;
                        Physics.Raycast(ray, out hit);
                        Debug.DrawRay(transform.position, entity.transform.position - transform.position, Color.red);
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (m_showWireSphere)
        {
            if (gameObject.tag == "Planet")
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, m_overlapRadius);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (m_showSelectedWireSphere)
        {
            Gizmos.DrawWireSphere(transform.position, m_overlapRadius);
        }

        if (m_showSelectedLines)
        {
            m_entities = Physics.OverlapSphere(transform.position, m_overlapRadius);
            foreach (Collider entity in m_entities)
            {
                Ray ray = new Ray(transform.position, entity.transform.position - transform.position);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);
                Debug.DrawRay(transform.position, entity.transform.position - transform.position, Color.magenta);
            }
        }
    }
}
