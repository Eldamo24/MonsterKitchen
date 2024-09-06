using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private Transform waitForTablePosition;
    [SerializeField] private bool entering;
    [SerializeField] private bool waitForATable;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text temporizador;

    private int timeToWaitTable;
    private float speed = 3f;

    private void OnEnable()
    {
        waitForATable = false;
        entering = true;
        waitForTablePosition = GameObject.Find("WaitForATable").GetComponent<Transform>();
        text = GameObject.Find("Time").GetComponent<TMP_Text>();
        temporizador = GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>();
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
            StartCoroutine("WaitingTable");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.text ="E to interact";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.text = "";
        }
    }

    IEnumerator WaitingTable()
    {
        timeToWaitTable = Random.Range(15, 31);
        while(waitForATable && timeToWaitTable > 0)
        {
            temporizador.text = timeToWaitTable.ToString();
            yield return new WaitForSeconds(1f);
            timeToWaitTable--;
        }
    }

}
