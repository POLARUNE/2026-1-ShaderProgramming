#version 330

uniform float u_Time; // uniform은 CPU에서 직접 shader로 값을 전달할 때 사용


in vec3 a_Position; // attribute에서 받아온 값, vbo 거쳐서 들어옴
in float a_Mass;
in vec2 a_Vel;
in float a_RV; //랜덤값

float random(float seed)
{
	return fract(sin(seed) * 43758.5453123);
}

const float c_PI = 3.141592;
const float c_G = -9.8;

void sin1()
{
	float t = u_Time;
	vec4 newPosition;
	newPosition.x = a_Position.x + t;
	newPosition.y = a_Position.y + sin(t * 2 * 3.141592) * 0.5;
	newPosition.z = a_Position.z;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}

void sin2()
{
	float t = u_Time;
	vec4 newPosition;
	newPosition.x = a_Position.x - 1 + t;
	newPosition.y = a_Position.y + sin(t * 3.141592) * 0.5;
	newPosition.z = a_Position.z;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}

void circle()
{
	float t = u_Time;
	vec4 newPosition;
	newPosition.x = a_Position.x + sin(t * 3.141592);
	newPosition.y = a_Position.y + cos(t * 3.141592);
	newPosition.z = a_Position.z;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}

void Falling()
{
	float t = mod(u_Time, 1.0); // 0 ~ 1 구간 반복
	float tt = t*t;
	float vx, vy;
	vx = a_Vel.x;
	vy = a_Vel.y;

	vec4 newPos;
	newPos.x = a_Position.x + vx*t;
	newPos.y = a_Position.y + vy*t + 0.5*c_G*tt;
	newPos.z = 0;
	newPos.w = 1;

	gl_Position = newPos;
}

void RoundPop() 
{
    // 1. 시간 계산 (0~1초 반복)
    float t = mod(u_Time, 1.0); 
    float tt = t * t;

    // 2. 원형 시작점(Origin) 계산
    // a_RV(0~1)에 2*PI를 곱해 완전한 원(0~360도)의 각도를 만듦
    float angle = a_RV * 2.0 * c_PI; 
    float radius = 0.8; // 원의 반지름

    // 원주 위의 한 점 (시작 위치)
    float sx = cos(angle) * radius;
    float sy = sin(angle) * radius;

    // 3. 속도 설정 (VBO에서 넘어온 랜덤 속도)
    float vx = a_Vel.x;
    float vy = a_Vel.y;

    vec4 newPos;

    // 원의 테두리(sx, sy)에서 운동 시작
    newPos.x = a_Position.x + sx + (vx * t);
    newPos.y = a_Position.y + sy + (vy * t) + (0.5 * c_G * tt);
    newPos.z = 0.0;
    newPos.w = 1.0;

    gl_Position = newPos;
}

void RoundPop2() 
{
    // 1. 각도 계산: 0~1 사이의 a_RV를 시계 방향 각도로 변환
    // 윗부분(PI/2)에서 시작하여 시계 방향(-)으로 회전
    float angle = (c_PI / 2.0) - (a_RV * 2.0 * c_PI);
    
    float radius = 0.8; // 원의 반지름
    float sx = cos(angle) * radius;
    float sy = sin(angle) * radius;

    // 2. 지연 시간 로직: a_RV가 클수록 나중에 터짐
    // u_Time이 1초 주기라고 가정할 때, a_RV만큼의 오프셋을 줍니다.
    float t = mod(u_Time - a_RV, 1.0); 
    float tt = t * t;

    // 3. 속도 설정
    float vx = a_Vel.x;
    float vy = a_Vel.y;

    vec4 newPos;

    // 4. 최종 위치 계산
    // t가 0에 가까울수록(막 터지는 순간) sx, sy 위치에 있게 됩니다.
    newPos.x = a_Position.x + sx + (vx * t);
    newPos.y = a_Position.y + sy + (vy * t) + (0.5 * c_G * tt);
    newPos.z = 0.0;
    newPos.w = 1.0;

    gl_Position = newPos;
}

void main()
{
	RoundPop2();
}
