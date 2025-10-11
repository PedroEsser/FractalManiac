using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex3D
{

    public Vector3 v;

    public float Magnitude => v.magnitude;
    public float AzimuthalAngle => Mathf.Atan2(v.y, v.x);
    public float PolarAngle => Mathf.Acos(v.z / Magnitude);

    public Complex3D(float x, float y, float z){
        this.v = new Vector3(x, y, z);
    }

    public Complex3D(Vector3 v){
        this.v = v;
    }

    public static Complex3D operator +(Complex3D a, Complex3D b){
        return new Complex3D(a.v + b.v);
    }

    public static Complex3D operator +(Complex3D a, Vector3 b){
        return new Complex3D(a.v + b);
    }

    public static Complex3D operator -(Complex3D a){
        return new Complex3D(-a.v);
    }

    public static Complex3D operator -(Complex3D a, Complex3D b){
        return new Complex3D(a.v - b.v);
    }

    public static Complex3D operator -(Complex3D a, Vector3 b){
        return new Complex3D(a.v - b);
    }

    public static Complex3D operator *(float a, Complex3D b){
        return new Complex3D(a * b.v);
    }

    public static Complex3D operator *(Complex3D a, float b){ return b * a; }


    public static Complex3D operator *(Complex3D a, Complex3D b){
        float magnitude = a.Magnitude * b.Magnitude;
        float theta = a.AzimuthalAngle + b.AzimuthalAngle;
        float phi = a.PolarAngle + b.PolarAngle;
        return fromMagnitudeAndAngles(magnitude, theta, phi);
    }

    public static Complex3D operator ^(Complex3D a, float power){
        float theta = a.AzimuthalAngle * power;
        float phi = a.PolarAngle * power;
        float magnitude = Mathf.Pow(a.Magnitude, power);
        return fromMagnitudeAndAngles(magnitude, theta, phi);
    }


    public static Complex3D fromAngles(float azimuthalAngle, float polarAngle){
        float cosPolar = Mathf.Cos(polarAngle);
        return new Complex3D(Mathf.Cos(azimuthalAngle) * cosPolar, Mathf.Sin(azimuthalAngle) * cosPolar, Mathf.Sin(polarAngle));
    }

    public static Complex3D fromMagnitudeAndAngles(float magnitude, float azimuthalAngle, float polarAngle){ return magnitude * fromAngles(azimuthalAngle, polarAngle); }

}
