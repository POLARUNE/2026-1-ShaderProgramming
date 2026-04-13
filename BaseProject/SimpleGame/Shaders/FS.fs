#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_TPos;

uniform float u_Time;

uniform vec4 u_DropInfo[1000]; //vec4(x, y, sT, lT)

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

void Raindrop(){
	float accum = 0;

	for (int i = 0; i < 1000; i++) {
		float lTime = u_DropInfo[i].w; //라이프타임
		float sTime = u_DropInfo[i].z; //시작 시간
		float newTime = u_Time - sTime;

		if (newTime > 0) {
			newTime = fract(newTime/lTime); //0~1
			float oneMinus = 1 - newTime; //1~0
			float t = newTime * lTime;

			vec2 center = u_DropInfo[i].xy;
			vec2 currpos = v_TPos.xy;

			float range = t/2;
			float d = distance(center, currpos);

			float fade = 5 * clamp(range - d, 0, 1);

			float value = pow(abs(sin(d * 2 * c_PI * 8 - t*100)), 16); // 8은 물방울의 개수, t는 시간에 따른 변화량
			accum += value * fade * oneMinus;
		}

		else {
		}
	}
	FragColor = vec4(accum);
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

void Flag() {
	float amp = 0.5; // 진폭
	float speed = 15;
	float sinInput = v_TPos.x * c_PI * 2 - u_Time * speed;
	float sinValue = v_TPos.x * amp * ((sin(sinInput) + 1) / 2 - 0.5) + 0.5;

	float fWidth = 0.0; // 깃발 끝 부분 폭 설정
	float width = 0.5 * mix(1, fWidth, v_TPos.x);
	float grey = 0;


	if (v_TPos.y < sinValue + width/2 && v_TPos.y > sinValue - width/2)
	{
		grey = 1;
	}

	else {
		grey = 0;
		discard;
	}

	FragColor = vec4(grey);
}

void Flame() {
	float amp = 0.5; // 진폭
	float speed = 15;
	float newY = 1 - v_TPos.y;
	float sinInput = newY * c_PI * 2 - u_Time * speed;
	float sinValue = newY * amp * ((sin(sinInput) + 1) / 2 - 0.5) + 0.5;

	float fWidth = 0.0; // 깃발 끝 부분 폭 설정
	float width = 0.5 * mix(fWidth, 1, newY);
	float grey = 0;


	if (v_TPos.x < sinValue + width/2 && v_TPos.x > sinValue - width/2)
	{
		grey = 1;
	}

	else {
		grey = 0;
		discard;
	}

	FragColor = vec4(grey);
}

void main()
{
	Flame();
	//FractalPattern();
}
