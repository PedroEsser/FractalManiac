float smoothMax(float a, float b, float k)
{
    float h = clamp(0.5 + 0.5*(a - b)/k, 0.0, 1.0);
    return lerp(b, a, h) + k * h * (1.0 - h);
}



float sphere_sdf(float3 d, float radius){
    return length(d) - radius;
}

float box_sdf(float3 d, float3 b){
    return length(max(abs(d) - b, 0.0));
}

float torus_sdf(float3 d, float r1, float r2){
    return length(float2(length(d.xz) - r1, d.y)) - r2;
}

float cross_sdf(float3 d)
{
    d = abs(d);
    float da = max(d.x, d.y);
    float db = max(d.y, d.z);
    float dc = max(d.z, d.x);
    return min(da,min(db,dc))-1.0;
}