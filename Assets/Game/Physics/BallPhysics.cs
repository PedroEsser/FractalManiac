using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics
{
    public static float BOUNCINESS = 0.8f; // 0 = no bounce, 1 = full bounce
    public static float FRICTION = 0.1f;
    public static int MAX_COLLISION_ITERATIONS = 5;
    public static float COLLISION_EPSILON = 0.001f;
    
    public static (Vector3 position, Vector3 velocity) ResolveCollision(ref Vector3 position, ref Vector3 velocity, float radius, SDF sdf)
    {
        int iterations = 0;
        float collisionEpsilon = COLLISION_EPSILON * radius;
        
        while (iterations < MAX_COLLISION_ITERATIONS)
        {
            float distance = sdf.signedDistanceAt(position);
            
            // If we're far enough from the surface, we're done
            if (distance >= radius - collisionEpsilon)
            {
                break;
            }
            
            // Get surface normal
            Vector3 normal = sdf.normalAt(position);
            
            // Push position out of the surface
            float penetration = radius - distance;
            position += normal * (penetration + collisionEpsilon);
            
            // Reflect velocity along the surface normal
            float normalSpeed = Vector3.Dot(velocity, normal);
            
            // Only reflect if moving into the surface
            if (normalSpeed < 0)
            {
                // Reflect velocity with bounciness
                velocity -= normal * normalSpeed * (1.0f + BOUNCINESS);
                
                // Apply friction to tangential velocity
                Vector3 tangent = velocity - normal * Vector3.Dot(velocity, normal);
                velocity -= tangent * FRICTION;
            }
            
            iterations++;
        }
        return (position, velocity);
    }
    
    // Helper method to check if a position is valid (not intersecting)
    public static bool IsValidPosition(Vector3 position, float radius, SDF sdf)
    {
        return sdf.signedDistanceAt(position) >= radius * (1 - COLLISION_EPSILON);
    }
    
    public static bool isTouchingSurface(Vector3 position, float radius, SDF sdf)
    {
        return sdf.signedDistanceAt(position) <= radius * 1.01f;
    }
    // Get the closest valid position on the surface
    public static Vector3 GetClosestSurfacePosition(Vector3 position, float radius, SDF sdf)
    {
        float distance = sdf.signedDistanceAt(position);
        Vector3 normal = sdf.normalAt(position);
        return position - normal * (distance - radius);
    }
}
