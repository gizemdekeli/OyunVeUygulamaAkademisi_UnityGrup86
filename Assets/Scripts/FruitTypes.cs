using GameManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

[CreateAssetMenu(fileName = "New Fruit Type", menuName = "Fruit Type")]

public class FruitTypes : ScriptableObject
{
    public Mesh _mesh;
    public Material[] _materials;
    public MinMaxGradient _gradient;
}