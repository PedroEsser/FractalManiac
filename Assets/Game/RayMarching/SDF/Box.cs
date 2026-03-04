using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : SDF
{
    public Vector3 size;

    public Box(Vector3 size)
    {
        this.size = size;
    }

    public Box() : this(Vector3.one) { }

    public float signedDistanceAt(Vector3 p)
    {
        float x = Mathf.Max(Mathf.Abs(p.x) - size.x, 0);
        float y = Mathf.Max(Mathf.Abs(p.y) - size.y, 0);
        float z = Mathf.Max(Mathf.Abs(p.z) - size.z, 0);
        return Mathf.Sqrt(x*x + y*y + z*z);
    }
}
