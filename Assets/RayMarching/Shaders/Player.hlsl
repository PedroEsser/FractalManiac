#include "Assets/RayMarching/Shaders/Globals.hlsl"

float4 RaySphereIntersection(float3 rayOrigin, float3 rayDir, float3 sphereCenter, float sphereRadius)
{
    float3 oc = rayOrigin - sphereCenter;
    float b = dot(oc, rayDir);
    float c = dot(oc, oc) - sphereRadius * sphereRadius;
    float h = b * b - c;

    if (h < 0.0)
        return float4(0, 0, 0, INFINITY); // no intersection

    h = sqrt(h);

    // nearest intersection distance
    float t = -b - h;
    if (t < 0.0)
        t = -b + h;

    if(t > 0.0){
        return float4(rayOrigin + rayDir * t, t);
    }

    return float4(0, 0, 0, INFINITY);
}
