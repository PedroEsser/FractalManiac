float mandelbulbSDF(float3 p, int maxIter = 25, float power = 8.0)
{
    float3 z = p;
    float dr = 1.0;
    float r = 0.0;

    for (int i = 0; i < maxIter; i++)
    {
        r = length(z);
        if (r > 2.0) break;

        // Convert to polar coordinates
        float theta = acos(z.z / r);
        float phi = atan2(z.y, z.x);
        float zr = pow(r, power);

        // Scale derivative
        dr = pow(r, power - 1.0) * power * dr + 1.0;

        // Rotate and scale
        theta *= power;
        phi *= power;

        z = zr * float3(sin(theta) * cos(phi), sin(phi) * sin(theta) + 0.0, cos(theta));
        z += p; // add original point
    }
    return 0.5 * log(r) * r / dr;
}