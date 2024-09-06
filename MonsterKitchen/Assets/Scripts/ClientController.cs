using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private Transform waitForTablePosition;
    [SerializeField] private bool entering;
    [SerializeField] private bool waitForATable;
    private float speed = 3f;

    private void OnEnable()
    {
        waitForATable = false;
        entering = true;
        waitForTablePosition = GameObject.Find("WaitForATable").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (entering)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waitForTablePosition.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "WaitForATable")
        {
            entering = false;
            waitForATable = true;
        }
    }
}
