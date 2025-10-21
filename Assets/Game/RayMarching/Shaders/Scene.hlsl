#include "Assets/Game/RayMarching/Shaders/Globals.hlsl"


float playerClipSDF(float3 pos)
{
    return sphere_sdf(pos - _PlayerPos, 10 * _PlayerSize);
}

float cameraClipSDF(float3 pos)
{
    return sphere_sdf(pos - _CameraWorldPos, 10 * _PlayerSize);
}


float sceneSDF(float3 pos)
{
    /*float fractal_dist = mandelbulbSDF(pos, 40);
    if(fractal_dist < -Debug.x){
        fractal_dist = -fractal_dist - 2 * Debug.x;
    }*/
    //float fractal_dist = mandelbulbSDF(pos, 10);
    float fractal_dist = mengerSpongeSDF(pos, 4);
    float dist_scene = max(fractal_dist, -cameraClipSDF(pos) * Debug.y);
    return dist_scene;
    //return fractal_dist;
}

RayHitInfo RayMarchScene(float3 rayOrigin, float3 rayDir, uint maxSteps = MAX_STEPS, float maxDist = MAX_DIST)
{
    RayHitInfo info;
    float totalDist = 0.0;

    for (int i = 0; i < maxSteps && totalDist < maxDist; i++)
    {
        float3 pos = rayOrigin + rayDir * totalDist;
        float dist = sceneSDF(pos);

        if (dist < EPSILON){
            info.hitPosition = pos;
            info.distanceTraveled = totalDist;
            info.stepsTaken = i;
            return info;
        }

        totalDist += dist;
    }
    info.distanceTraveled = INFINITY;
    return info;
}