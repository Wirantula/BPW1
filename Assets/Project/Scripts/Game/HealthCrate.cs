using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCrate : MonoBehaviour
{
    [Header("Visuals")]
    public GameObject container;
    public float rotaionSpeed = 180f;

    [Header("Gameplay")]
    public int health = 50;

    // Update is called once per frame
    void Update()
    {
        container.transform.Rotate(Vector3.up * rotaionSpeed * Time.deltaTime);
    }
}
