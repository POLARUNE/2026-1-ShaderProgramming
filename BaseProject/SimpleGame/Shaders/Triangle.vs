#version 330

// uniform은 CPU에서 직접 shader로 값을 전달할 때 사용
uniform float u_Time; 


// attribute에서 받아온 값, vbo 거쳐서 들어옴
in vec3 a_Position; 
in float a_Mass;
in vec2 a_Vel;
in float a_RV; // 0 ~ 1 랜덤값 (Random Value)
in float a_RV1; // 0 ~ 1 랜덤값 (Random Value)
in float a_RV2; // 0 ~ 1 랜덤값 (Random Value)

out float v_Grey;

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

void sin3()
{
	float startTime = a_RV1 * 2; // 범위	: 0 ~ 2초, 각 파티클마다 시작 시간이 다르게 됨
	float newTime = u_Time - startTime; // 각 파티클마다 시작 시간이 다르게 됨

	if (newTime > 0) {
		float t = mod(newTime * 2, 1.0); // 0 ~ 1 구간 반복, 2를 곱해서 속도를 2배로 빠르게 함
		float a = (1 - t) * 0.2 * (a_RV - 0.5) * 2; // 범위: -0.2 ~ 0.2, t가 0에 가까울수록 a(진폭)가 커짐
		float period = a_RV1; // 범위: 0 ~ 1


		vec4 newPosition;
		newPosition.x = a_Position.x * a_RV2 * 0.2 + t; // x축으로는 t에 따라 오른쪽으로 이동, a_RV2에 따라 약간의 랜덤 이동
		newPosition.y = a_Position.y * a_RV2 * 0.2 + sin(t * 2 * c_PI * period) * a; // y축으로는 사인 곡선을 따라 이동, a에 따라 진폭이 달라짐
		newPosition.z = a_Position.z;
		newPosition.w = 1;

		gl_Position = newPosition;
		v_Grey = 1 - t; // t가 0에 가까울수록 밝게, 1에 가까울수록 어둡게
	}

	else {
		//보이지 않는 곳으로 이동
		gl_Position = vec4(-1000, -1000, 0, 1);
		v_Grey = 0;
	}
}

void sin4()
{
	// a_RV1(0~1)에 따라 0~2초 사이의 지연 시간 생성
	float startTime = a_RV1 * 2.0;
	float newTime = u_Time - startTime;

	if (newTime > 0.0) {
		// 0~1 사이로 반복되는 내부 시간 (속도 2배)
		float t = mod(newTime * 2.0, 1.0);

		// 진폭: 소멸 효과를 위해 (1-t) 곱함
		float a = (1.0 - t) * 0.2 * (a_RV - 0.5) * 2.0;
		float period = a_RV1;

		vec4 newPosition;

		// 시작점(startX)이 u_Time에 따라 점점 더 왼쪽으로 이동
		// u_Time이 커질수록 1.0에서 작아짐
		// 0.2는 밀려나는 속도이므로 적절히 조절
		float startX = 1.0 - (u_Time * 0.8);

		// x축: 점점 왼쪽으로 밀리는 시작점 + t에 따른 오른쪽 이동 거리(2.0배)
		newPosition.x = (a_Position.x * a_RV2 * 0.2) + startX + (t * 2.5);

		// y축: 사인파 운동
		newPosition.y = (a_Position.y * a_RV2 * 0.2) + sin(t * 2.0 * c_PI * period) * a;
		newPosition.z = a_Position.z;
		newPosition.w = 1.0;

		gl_Position = newPosition;
	}
	else {
		// 아직 시작되지 않은 파티클은 숨김
		gl_Position = vec4(-1000.0, -1000.0, 0.0, 1.0);
	}
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

void RoundFalling()
{
	float startTime = a_RV1;
	float newTime = u_Time - startTime; // 각 파티클마다 시작 시간이 다르게 됨
	
	if (newTime > 0) {
		float lifeScale = 2.0;
		float lifeTime = 0.5 + a_RV2 * lifeScale; // 파티클이 1초 동안 움직인다고 가정
		float t = lifeTime * fract(newTime / lifeTime); // 0~lifeTime 구간 반복
		float tt = t * t;
		float vx, vy;
		float sx, sy;
		vx = a_Vel.x / 30;
		vy = a_Vel.y / 30;

		sx = a_Position.x * (1 - random(a_RV)) + sin(a_RV * 2 * c_PI) * 0.8;
		sy = a_Position.y * (1 - random(a_RV)) + cos(a_RV * 2 * c_PI) * 0.8;

		vec4 newPos;
		newPos.x = sx + vx * t;
		newPos.y = sy + vy * t + 0.5 * c_G * tt;
		newPos.z = 0;
		newPos.w = 1;

		gl_Position = newPos;
	}
	else
	{
		//보이지 않는 곳으로 이동
		gl_Position = vec4(-1000, -1000, 0, 1);
		return;
	}
}

void RoundPop() 
{
    // 1. 시간 계산 (0 ~ 1초 반복)
    float t = mod(u_Time, 1.0); 
    float tt = t * t;

    // 2. 원형 시작점(Origin) 계산
    // a_RV(0 ~ 1)에 2*PI를 곱해 완전한 원(0~360도)의 각도를 만듦
    float angle = a_RV * 2.0 * c_PI; 
    float radius = 0.8; // 원의 반지름

    // 원주 위의 한 점 (시작 위치)
    float sx = cos(angle) * radius;
    float sy = sin(angle) * radius;

    // 3. 터지는 속도 설정 (VBO에서 넘어온 랜덤 속도)
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

    // 3. 터지는 속도 설정
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
	//Falling();
	//RoundFalling();
	//RoundPop2();
	sin3();
}
