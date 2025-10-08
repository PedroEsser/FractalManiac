using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandelbulb : SDF
{

    public int iterations = 10;
    public float bailout = 10;
    public float power = 8;

    public float signedDistance(Vector3 p)
    {
        Complex3D c = new Complex3D(p);
        Complex3D z = new Complex3D(p);
        float dr = 1.0f;
        float r = 0f;

        for (int i = 0; i < iterations; i++)
        {
            r = z.Magnitude;
            if (r > bailout) break;
            if (r == 0f)
            {
                z += new Vector3(1e-6f, 0, 0);
                r = z.Magnitude;
            }

            float rp = Mathf.Pow(r, power - 1f);
            dr = rp * power * dr + 1.0f;

            Complex3D newZ = z ^ power;

            z = newZ + c;
        }
        
        if (r == 0f) r = 1e-6f;
        float distanceEstimate = 0.5f * Mathf.Log(r) * r / dr;
        return distanceEstimate;
    }
}
