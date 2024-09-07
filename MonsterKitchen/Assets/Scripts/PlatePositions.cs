using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlatePositions : MonoBehaviour
{
    private bool isEmpty;
    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }
    
    private void Start()
    {
        isEmpty = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Plate"))
        {
            isEmpty = true;
        }
    }



}
