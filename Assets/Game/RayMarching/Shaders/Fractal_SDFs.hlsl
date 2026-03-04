#include "Assets/Game/RayMarching/Shaders/Globals.hlsl"

float4 hsv_to_rgb(float3 HSV)
{
    float var_h = HSV.x * 6;
    float var_i = floor(var_h);
    float var_1 = HSV.z * (1.0 - HSV.y);
    float var_2 = HSV.z * (1.0 - HSV.y * (var_h - var_i));
    float var_3 = HSV.z * (1.0 - HSV.y * (1 - (var_h - var_i)));

    if (var_i == 0) { return float4(HSV.z, var_3, var_1, 1); }
    else if (var_i == 1) { return float4(var_2, HSV.z, var_1, 1); }
    else if (var_i == 2) { return float4(var_1, HSV.z, var_3, 1); }
    else if (var_i == 3) { return float4(var_1, var_2, HSV.z, 1); }
    else if (var_i == 4) { return float4(var_3, var_1, HSV.z, 1); }
    else { return float4(HSV.z, var_1, var_2, 1); }
}

float mandelbulbSDF(float3 p, int maxIter = 10, float power = 8.0)
{
    float3 z = p;
    float dr = 1.0;
    float r = length(z);
    if (r > 2.0) {
        return r - 1.5;
    }

    for (int i = 0; i < maxIter; i++)
    {
        // Convert to polar coordinates
        float theta = acos(z.z / r);
        float phi = atan2(z.y, z.x);
        float zr = pow(r, power);

        // Scale derivative
        dr = pow(r, power - 1.0) * power * dr + 1.0;

        // Rotate and scale
        theta *= power;
        phi *= power;

        z = zr * float3(sin(theta) * cos(phi), sin(phi) * sin(theta), cos(theta));
        z += p; // add original point

        r = length(z);
        if (r > 2.0)
            break;
    }
    return 0.5 * log(r) * r / dr;
}

float3 mandelbulbColorAt(float3 p, int maxIter = 10, float power = 8.0) {
    float3 z = p;
    float r = length(z);
    float orbitDistance = sphere_sdf(z - Debug.yzw);

    for (int i = 0; i < maxIter; i++)
    {
        // Convert to polar coordinates
        float theta = acos(z.z / r);
        float phi = atan2(z.y, z.x);
        float zr = pow(r, power);

        // Rotate and scale
        theta *= power;
        phi *= power;

        z = zr * float3(sin(theta) * cos(phi), sin(phi) * sin(theta), cos(theta));
        z += p; // add original point

        orbitDistance = min(orbitDistance, sphere_sdf(z - Debug.yzw));
        

        r = length(z);
        if (r > 100.0)
            break;
    }
    float3 hsv = float3((orbitDistance + 1 + Debug.x * 0.1) % 1, 1, 1);
    return hsv_to_rgb(hsv);
}

float3 mod(float3 p, float b)
{
    return p - b * floor(p / b);
}

float mengerSpongeSDF(float3 p, int iterations = 4)
{
    float d = box_sdf(p, float3(1, 1, 1));
    float s = 1.0;

    for (int m = 0; m < iterations; m++)
    {
        float3 a = mod(p * s, 2.0) - 1.0;
        s *= 3.0;

        float3 r = abs(1.0 - 3.0 * abs(a));
        float da = max(r.x, r.y);
        float db = max(r.y, r.z);
        float dc = max(r.z, r.x);
        float c = (min(da, min(db, dc)) - 1.0) / s;

        d = max(d, c);
    }

    return d;
}