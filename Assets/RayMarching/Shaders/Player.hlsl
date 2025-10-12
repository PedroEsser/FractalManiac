bool RaySphereIntersect(float3 rayOrigin, float3 rayDir, float3 sphereCenter, float sphereRadius)
{
    float3 oc = rayOrigin - sphereCenter;
    float b = dot(oc, rayDir);
    float c = dot(oc, oc) - sphereRadius * sphereRadius;
    float h = b * b - c;

    float t;
    if (h < 0.0)
    {
        t = 0.0;
        return false; // no intersection
    }

    h = sqrt(h);
    // choose nearest intersection point
    t = -b - h;
    if (t < 0.0) t = -b + h;
    return t > 0.0;
}
