using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SDF
{
    public static float EPSILON = 0.00001f;
    float signedDistanceAt(Vector3 point);

    Vector3 normalAt(Vector3 point){
        float dx = signedDistanceAt(point + new Vector3(EPSILON, 0, 0)) - signedDistanceAt(point - new Vector3(EPSILON, 0, 0));
        float dy = signedDistanceAt(point + new Vector3(0, EPSILON, 0)) - signedDistanceAt(point - new Vector3(0, EPSILON, 0));
        float dz = signedDistanceAt(point + new Vector3(0, 0, EPSILON)) - signedDistanceAt(point - new Vector3(0, 0, EPSILON));

        return new Vector3(dx, dy, dz).normalized;
    }
}
