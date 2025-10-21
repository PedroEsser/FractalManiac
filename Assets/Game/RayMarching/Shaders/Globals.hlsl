#ifndef GLOBALS_INCLUDED

#define GLOBALS_INCLUDED

#define MAX_STEPS 300
#define MAX_DIST 1000.0
#define EPSILON 0.00001
#define INFINITY 3.402823466e+38
#define PI 3.14159265358979323846

Texture2D<float4> SpaceshipTex;
SamplerState samplerLinear;

float4x4 _CameraToWorld;
float4x4 _CameraInverseProjection;
float3 _CameraWorldPos;
float4 _ScreenParams;
float3 _PlayerPos;
float _PlayerSize;
float3 _LightDirection;

float4 Debug;

struct RayHitInfo {
	float3 hitPosition;
	float distanceTraveled;
	int stepsTaken;
};

#include "Assets/Game/RayMarching/Shaders/SDFs.hlsl"
#include "Assets/Game/RayMarching/Shaders/Fractal_SDFs.hlsl"
#include "Assets/Game/RayMarching/Shaders/Player.hlsl"
#include "Assets/Game/RayMarching/Shaders/Scene.hlsl"
#include "Assets/Game/RayMarching/Shaders/ColorCalculation.hlsl"
#endif 