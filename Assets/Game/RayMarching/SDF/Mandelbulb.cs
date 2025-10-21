using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mandelbulb : SDF
{

    public int iterations = 20;
    public float bailout = 2;
    public float power = 8;

    public Mandelbulb(int iterations, float bailout, float power){
        this.iterations = iterations;
        this.bailout = bailout;
        this.power = power;
    }

    public Mandelbulb() : this(20, 2, 8) { }

    public float signedDistanceAt(Vector3 p)
    {
        Complex3D c = new Complex3D(p);
        Complex3D z = new Complex3D(p);
        float dr = 1.0f;
        float r = 0f;

        for (int i = 0; i < iterations; i++)
        {
            r = z.Magnitude;
            if (r > bailout) break;

            float rp = Mathf.Pow(r, power - 1f);
            dr = rp * power * dr + 1.0f;

            Complex3D newZ = z ^ power;

            z = newZ + c;
        }
        
        float distanceEstimate = 0.5f * Mathf.Log(r) * r / dr;
        return distanceEstimate;
    }
}
