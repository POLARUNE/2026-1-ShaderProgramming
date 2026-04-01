#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_TPos;

uniform float u_Time;

const float c_PI = 3.141592;

void Simple(){
	if (v_TPos.x + v_TPos.y > 0.5)
	{
		FragColor = vec4(0);
	}
	else
	{
		FragColor = vec4(v_TPos, 0, 1);
	}
}

void LinePattern(){
	//FragColor = vec4(v_TPos, 0, 1);
	float lineCountH = 10;
	float lineCountV = 5;
	float lineWidth = 1;
	lineCountH = lineCountH / 2;
	lineCountV = lineCountV / 2;
	lineWidth = 50 / lineWidth;
	float per = -0.5*c_PI;

	float grey = pow(abs(sin((v_TPos.y*2*c_PI+per)*lineCountH)), lineWidth);
	float grey1 = pow(abs(sin((v_TPos.x*2*c_PI+per)*lineCountV)), lineWidth);
	FragColor = vec4(grey+grey1);
}

void Circle1(){
	vec2 center = vec2(0.5, 0.5);
	vec2 currpos = v_TPos.xy;
	float d = distance(center, currpos);
	float lineWidth = 0.01;
	float radius = 0.5;

	if (d > radius - lineWidth && d < radius)
	{
		FragColor = vec4(v_TPos.xy, 1, 1);
		
	}
	else
	{
		FragColor = vec4(0);
	}
}

void Circle2(){
	vec2 center = vec2(0.5, 0.5);
	vec2 currpos = v_TPos.xy;
	float d = distance(center, currpos);
	float value = abs(sin(d * c_PI * 16 - u_Time*20));
	FragColor = vec4(pow(value, 16));
}

// 회전 행렬을 생성하는 보조 함수
mat2 rotate2d(float angle) {
    return mat2(cos(angle), -sin(angle),
                sin(angle),  cos(angle));
}

void FractalPattern() {
    // 1. 좌표 정규화 (0~1 범위를 -1~1 범위로 변경하여 중앙 정렬)
    vec2 uv = (v_TPos - 0.5) * 2.0;
    
    // 시간의 흐름에 따라 전체적인 회전 추가
    uv *= rotate2d(u_Time * 0.2);

    vec3 finalColor = vec3(0.0);

    // 2. 프랙탈 반복 루프 (루프 횟수가 많아질수록 복잡해짐)
    for (float i = 0.0; i < 4.0; i++) {
        // 좌표 복제 (화면을 쪼개기)
        uv = fract(uv * 1.5) - 0.5;

        // 원형 거리 계산 (Circle2의 거리 개념 응용)
        float d = length(uv) * exp(-length(v_TPos - 0.5));

        // 색상 결정 (위치와 시간에 따른 변화)
        vec3 col = 0.5 + 0.5 * cos(u_Time + i * 0.5 + vec3(0, 2, 4));

        // 네온 라인 효과 (LinePattern의 sin/pow 개념 응용)
        d = sin(d * 8.0 + u_Time) / 8.0;
        d = abs(d);
        d = pow(0.01 / d, 1.2); // 빛나는 선 효과

        finalColor += col * d;
    }

    FragColor = vec4(finalColor, 1.0);
}

void main()
{
	FractalPattern();
}
