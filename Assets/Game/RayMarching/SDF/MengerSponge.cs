using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MengerSponge : SDF
{
    public int iterations;

    public MengerSponge(int iterations)
    {
        this.iterations = iterations;
    }

    public MengerSponge() : this(4) { }

    public float signedDistanceAt(Vector3 p)
    {
        float d = new Box().signedDistanceAt(p);
        float s = 1.0f;

        for (int m = 0; m < iterations; m++)
        {
            Vector3 a = mod(p * s, 2.0f) - Vector3.one;
            s *= 3.0f;

            Vector3 r = new Vector3(Mathf.Abs(1.0f - 3.0f * Mathf.Abs(a.x)), Mathf.Abs(1.0f - 3.0f * Mathf.Abs(a.y)), Mathf.Abs(1.0f - 3.0f * Mathf.Abs(a.z)));
            float da = Mathf.Max(r.x, r.y);
            float db = Mathf.Max(r.y, r.z);
            float dc = Mathf.Max(r.z, r.x);
            float c = (Mathf.Min(da, Mathf.Min(db, dc)) - 1.0f) / s;

            d = Mathf.Max(d, c);
        }
        return d;
    }

    Vector3 mod(Vector3 p, float b)
    {
        return p - b * new Vector3(Mathf.Floor(p.x / b), Mathf.Floor(p.y / b), Mathf.Floor(p.z / b));
    }
}
