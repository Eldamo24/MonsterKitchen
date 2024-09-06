using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private bool isEmpty;

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    void Start()
    {
        isEmpty = true;
    }
}
