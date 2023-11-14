using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AssemblyPoint : MonoBehaviour
{
    public Piston.Part pistonPart;

    public PistonPart inPart {  get; private set; }

    private void Start()
    {
        inPart = GetComponentInParent<PistonPart>();
    }
}
