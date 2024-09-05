using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOReceta", menuName = "ScriptableObjects/Receta", order = 1)]
public class SOReceta : ScriptableObject
{
    public string nameReceta;
    public string ingredient1;
    public string ingredient2;
    public string ingredient3;
    public string ingredient4;
}
