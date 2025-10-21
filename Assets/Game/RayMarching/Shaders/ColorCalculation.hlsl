#include "Assets/Game/RayMarching/Shaders/Globals.hlsl"

float3 FractalColor = float3(1, 0.7, 0.8);
float3 AmbientLight = float3(.3, .3, .3);
float DiffuseComponent = 0.3;
float SpecularComponent = 0.8;
float Shininess = 32.0;

/*float SoftShadowAt(float3 pos, float mint = 0.01, float tmax = 2, float w = 0.1, int technique = 1)
{
    float3 ro = pos - LightDirection * tmax;
    float3 rd = LightDirection;
    float res = 1.0;
    float t = mint;
    float ph = 1e10; // big, such that y = 0 on the first iteration

    for (int i = 0; i < 32; i++)
    {
        float h = sceneSDF(ro + rd * t);

        // traditional technique
        if (technique == 0)
        {
            res = min(res, h / (w * t));
        }
        // improved technique
        else
        {
            // use this if you are getting artifact on the first iteration, or unroll the
            // first iteration out of the loop
            //float y = (i==0) ? 0.0 : h*h/(2.0*ph); 

            float y = h * h / (2.0 * ph);
            float d = sqrt(h * h - y * y);
            res = min(res, d / (w * max(0.0, t - y)));
            ph = h;
        }

        t += h;

        if (res<0.0001 || t>tmax) break;

    }
    res = clamp(res, 0.0, 1.0);
    return res * res * (3.0 - 2.0 * res);
}*/

float ShadowAt(float3 pos)
{
    float3 rayOrigin = pos - 2 * _LightDirection;
    float4 playerHitInfo = RaySphereIntersection(rayOrigin, _LightDirection, _PlayerPos, _PlayerSize);
    if(playerHitInfo.w < 2){
        return 0.5;
    }

    RayHitInfo lightHitInfo = RayMarchScene(rayOrigin, _LightDirection);
    if(abs(lightHitInfo.distanceTraveled - 2) < 10 * EPSILON){
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


float3 SceneColorAt(RayHitInfo sceneHitInfo, float3 rayDirection)
{
    if(sceneHitInfo.distanceTraveled == INFINITY){
        return float3(0, 0, 0);
    }

    float shadow = ShadowAt(sceneHitInfo.hitPosition);
    float fog = 1 / (1 + sceneHitInfo.distanceTraveled * Debug.z);
    float3 normal = NormalAt(sceneHitInfo.hitPosition);

    float diff = max(dot(normal, _LightDirection), 0.0);
    float3 diffuse = DiffuseComponent * diff * float3(1.0, 0.9, 0.7);
    float3 reflectDir = reflect(-_LightDirection, normal);
    float3 viewDir = -rayDirection;
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), Shininess);
    float3 specular = SpecularComponent * spec * float3(1, 1, 1);

    float3 objectColor = (0.5 + normal/2) + 0.1 * float3(0.8, 0.3, 0.2) * pow(1.0 - abs(dot(normal, viewDir)), 4.0);
    return (AmbientLight + diffuse + specular) * objectColor * fog * shadow;
}


float3 CalculateColor(float3 rayOrigin, float3 rayDir)
{
    float4 playerHitInfo = RaySphereIntersection(rayOrigin, rayDir, _PlayerPos, _PlayerSize);
    RayHitInfo sceneHitInfo = RayMarchScene(rayOrigin, rayDir);
    if(playerHitInfo.w != INFINITY){
        if(playerHitInfo.w < sceneHitInfo.distanceTraveled){
            return PlayerColorAt(playerHitInfo);
        }

        float3 sceneColor = SceneColorAt(sceneHitInfo, rayDir);
        return (sceneColor + PlayerColorAt(playerHitInfo) + float3(0, 1, 1)) / 3;
    }
    return SceneColorAt(sceneHitInfo, rayDir);
}