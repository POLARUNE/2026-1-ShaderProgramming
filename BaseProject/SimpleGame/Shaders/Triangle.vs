#version 330

uniform float u_Time; // uniform은 CPU에서 직접 shader로 값을 전달할 때 사용

in vec3 a_Position; // attribute에서 받아온 값, vbo 거쳐서 들어옴


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

void star(){
	// 별의 궤적을 계산하기 위한 변수
    // 속도 조절을 위해 u_Time에 적당한 상수를 곱합니다.
    float t = u_Time * 5.0; 
    
    // 별 모양의 매개변수 방정식 (Star Curve)
    // 외경과 내경의 차이를 이용하여 별 모양을 만듭니다.
    // 5개의 모서리를 가지는 별 궤적:
    float r = 0.7; // 별의 크기 (화면에 꽉 차게 0.8 ~ 1.0 사이 조절)
    
    // x, y 좌표를 별 모양 궤적 수식으로 정의
    float x_offset = r * (cos(t) + cos(2.0/3.0 * t) * 0.5); 
    float y_offset = r * (sin(t) - sin(2.0/3.0 * t) * 0.5);

    vec4 newPosition;
    // a_Position이 (0,0)이므로 offset 값이 곧 중심 좌표가 됩니다.
    newPosition.x = a_Position.x + x_offset;
    newPosition.y = a_Position.y + y_offset;
    newPosition.z = a_Position.z;
    newPosition.w = 1.0;
    
    gl_Position = newPosition;}



void main()
{
	star();
}
