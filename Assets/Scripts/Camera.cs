using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private GameObject m_planet;
    [SerializeField]
    private float distanceToPlanet;

    void Start()
    {
        distanceToPlanet = 40f;
    }

    void Update()
    {
        transform.LookAt(m_planet.GetComponent<Transform>());

        Vector3 direction = Vector3.Normalize(m_planet.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, m_planet.transform.position);
        if (distance < distanceToPlanet)
        {
            transform.Translate(-direction * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * Time.deltaTime);
        }
    }
}
