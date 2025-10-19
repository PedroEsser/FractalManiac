#include "Assets/RayMarching/Shaders/Globals.hlsl"


float playerClipSDF(float3 pos)
{
    return sphere_sdf(pos - _PlayerPos, 10 * _PlayerSize);
}

float sceneSDF(float3 pos)
{
    /*float fractal_dist = mandelbulbSDF(pos, 40);
    if(fractal_dist < -Debug.x){
        fractal_dist = -fractal_dist - 2 * Debug.x;
    }*/
    float fractal_dist = mandelbulbSDF(pos);
    float dist_scene = max(fractal_dist, -playerClipSDF(pos) * Debug.y);
    return dist_scene;
    //return fractal_dist;
}

float4 RayMarchScene(float3 rayOrigin, float3 rayDir)
{
    float totalDist = 0.0;

    for (int i = 0; i < MAX_STEPS; i++)
    {
        float3 pos = rayOrigin + rayDir * totalDist;
        float dist = sceneSDF(pos);

        if (dist < EPSILON){
            return float4(pos, totalDist);
        }

        totalDist += dist;
        if (totalDist > MAX_DIST)
            return float4(0, 0, 0, INFINITY);
    }

    return float4(0, 0, 0, INFINITY);
}