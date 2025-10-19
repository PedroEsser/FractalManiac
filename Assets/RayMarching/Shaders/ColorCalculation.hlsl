#include "Assets/RayMarching/Shaders/Globals.hlsl"

float ShadowAt(float3 pos)
{
    float3 rayOrigin = pos + -_LightDirection * 2;
    float4 playerHitInfo = RaySphereIntersection(rayOrigin, _LightDirection, _PlayerPos, _PlayerSize);
    if(playerHitInfo.w < 2){
        return 0.5;
    }

    RayHitInfo lightHitInfo = RayMarchScene(rayOrigin, _LightDirection);
    if(abs(lightHitInfo.distanceTraveled - 2) < 2 * EPSILON){
        return 1;
    }
    return 0.5;
}

float3 NormalAt(float3 p)
{
    float2 eps = float2(0.001, 0);

    float dx = sceneSDF(p + eps.xyy) - sceneSDF(p - eps.xyy);
    float dy = sceneSDF(p + eps.yxy) - sceneSDF(p - eps.yxy);
    float dz = sceneSDF(p + eps.yyx) - sceneSDF(p - eps.yyx);

    return normalize(float3(dx, dy, dz));
}


float3 SceneColorAt(RayHitInfo sceneHitInfo)
{
    if(sceneHitInfo.distanceTraveled == INFINITY){
        return float3(0, 0, 0);
    }

    float distAttenuation = 1 / (1 + sceneHitInfo.distanceTraveled * Debug.z);
    return NormalAt(sceneHitInfo.hitPosition) * ShadowAt(sceneHitInfo.hitPosition) * distAttenuation;
}


float3 CalculateColor(float3 rayOrigin, float3 rayDir)
{
    float4 playerHitInfo = RaySphereIntersection(rayOrigin, rayDir, _PlayerPos, _PlayerSize);
    RayHitInfo sceneHitInfo = RayMarchScene(rayOrigin, rayDir);
    if(playerHitInfo.w != INFINITY){
        if(playerHitInfo.w < sceneHitInfo.distanceTraveled){
            return PlayerColorAt(playerHitInfo);
        }

        float3 sceneColor = SceneColorAt(sceneHitInfo);
        return (sceneColor + PlayerColorAt(playerHitInfo) + float3(0, 1, 1)) / 3;
    }
    return SceneColorAt(sceneHitInfo);
}