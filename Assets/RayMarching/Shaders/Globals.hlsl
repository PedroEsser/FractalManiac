#ifndef GLOBALS_INCLUDED

#define GLOBALS_INCLUDED

#define MAX_STEPS 300
#define MAX_DIST 1000.0
#define EPSILON 0.00001
#define INFINITY 3.402823466e+38

float4x4 _CameraToWorld;
float4x4 _CameraInverseProjection;
float3 _CameraWorldPos;
float4 _ScreenParams;
float3 _PlayerPos;
float _PlayerSize;
float3 _LightDirection;


float4 Debug;


#endif 