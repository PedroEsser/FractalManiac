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

float3 PlayerColorAt(float4 playerHitInfo)
{
    float3 p = normalize(playerHitInfo.xyz - _PlayerPos);
    float u = atan2(p.z, p.x) / (2.0 * PI) + 0.5;
    float v = p.y * 0.5 + 0.5;
    float2 uv = float2(u, v);
    int2 texSize = int2(0, 0); // width, height
    SpaceshipTex.GetDimensions(texSize.x, texSize.y);
    int2 texel = int2(uv * float2(texSize - 1));
    float3 playerColor = SpaceshipTex.Load(int3(texel, 0)).rgb;
    return playerColor;
}