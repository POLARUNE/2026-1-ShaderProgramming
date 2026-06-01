#version 330

layout(location=0) out vec4 FragColor;

in vec2 v_TPos;

uniform float u_Time;

uniform vec4 u_DropInfo[1000]; //vec4(x, y, sT, lT)

uniform sampler2D u_RGBTex; // 0
uniform sampler2D u_CurrNumTex;
uniform sampler2D u_NumsTex;
uniform int u_InputNum;

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
		float lTime = u_DropInfo[i].w; //������Ÿ��
		float sTime = u_DropInfo[i].z; //���� �ð�
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

			float value = pow(abs(sin(d * 2 * c_PI * 8 - t*100)), 16); // 8�� ������� ����, t�� �ð��� ���� ��ȭ��
			accum += value * fade * oneMinus;
		}

		else {
		}
	}
	FragColor = vec4(accum);
}

// ȸ�� ����� �����ϴ� ���� �Լ�
mat2 rotate2d(float angle) {
    return mat2(cos(angle), -sin(angle),
                sin(angle),  cos(angle));
}

void FractalPattern() {
    // 1. ��ǥ ����ȭ (0~1 ������ -1~1 ������ �����Ͽ� �߾� ����)
    vec2 uv = (v_TPos - 0.5) * 2.0;
    
    // �ð��� �帧�� ���� ��ü���� ȸ�� �߰�
    uv *= rotate2d(u_Time * 0.2);

    vec3 finalColor = vec3(0.0);

    // 2. ����Ż �ݺ� ���� (���� Ƚ���� ���������� ��������)
    for (float i = 0.0; i < 4.0; i++) {
        // ��ǥ ���� (ȭ���� �ɰ���)
        uv = fract(uv * 1.5) - 0.5;

        // ���� �Ÿ� ��� (Circle2�� �Ÿ� ���� ����)
        float d = length(uv) * exp(-length(v_TPos - 0.5));

        // ���� ���� (��ġ�� �ð��� ���� ��ȭ)
        vec3 col = 0.5 + 0.5 * cos(u_Time + i * 0.5 + vec3(0, 2, 4));

        // �׿� ���� ȿ�� (LinePattern�� sin/pow ���� ����)
        d = sin(d * 8.0 + u_Time) / 8.0;
        d = abs(d);
        d = pow(0.01 / d, 1.2); // ������ �� ȿ��

        finalColor += col * d;
    }

    FragColor = vec4(finalColor, 1.0);
}

void Flag() {
	float amp = 0.5; // ����
	float speed = 15;
	float sinInput = v_TPos.x * c_PI * 2 - u_Time * speed;
	float sinValue = v_TPos.x * amp * ((sin(sinInput) + 1) / 2 - 0.5) + 0.5;

	float fWidth = 0.0; // ��� �� �κ� �� ����
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
	float amp = 0.5; // ����
	float speed = 15;
	float newY = 1 - v_TPos.y;
	float sinInput = newY * c_PI * 2 - u_Time * speed;
	float sinValue = newY * amp * ((sin(sinInput) + 1) / 2 - 0.5) + 0.5;

	float fWidth = 0.0; // ��� �� �κ� �� ����
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

void TextureSampling() {
	vec4 c0;
	vec4 c1;
	vec4 c2;
	vec4 c3;
	vec4 c4;

	float offsetX = 0.01;

	c0 = texture(u_RGBTex, vec2(v_TPos.x - offsetX * 2, v_TPos.y));
	c1 = texture(u_RGBTex, vec2(v_TPos.x - offsetX * 1, v_TPos.y));
	c2 = texture(u_RGBTex, vec2(v_TPos.x - offsetX * 0, v_TPos.y));
	c3 = texture(u_RGBTex, vec2(v_TPos.x - offsetX * 1, v_TPos.y));
	c4 = texture(u_RGBTex, vec2(v_TPos.x - offsetX * 2, v_TPos.y));

	vec4 sum = c0 + c1 + c2 + c3 + c4;
	sum = sum/5;

	FragColor = sum;
}

void TextureQ1() {
	float tx = v_TPos.x;
	float ty = 1 - 2 * abs(v_TPos.y - 0.5); // y��ǥ�� 0~1 �������� 0~1~0 ������ ��ȯ
	vec2 newTex = vec2(tx, ty);

	FragColor = texture(u_RGBTex, newTex);
}

void TextureQ2() {
	float tx = fract(v_TPos.x * 3);
	float ty = v_TPos.y/3;

	float offsetX = 0;
	float offsetY = (2 - floor(v_TPos.x * 3))/3;

	vec2 newTex = vec2(tx+offsetX, ty+offsetY);

	FragColor = texture(u_RGBTex, newTex);
}

void TextureQ3() {
	float tx = fract(v_TPos.x * 3);
	float ty = v_TPos.y/3;

	float offsetX = 0;
	float offsetY = floor(v_TPos.x * 3)/3;

	vec2 newTex = vec2(tx+offsetX, ty+offsetY);

	FragColor = texture(u_RGBTex, newTex);
}

void TextureQ4() {
	float resolX = 5; //�ݺ��Ǵ� ����
	float resolY = 5;
	float shear = 0.5 * u_Time;

	float offsetX = fract(ceil(v_TPos.y * resolY) * shear); //offset
	float offsetY = 0;

	float tx = fract(v_TPos.x * resolX + offsetX); //range
	float ty = fract(v_TPos.y * resolY + offsetY);


	vec2 newTex = vec2(tx, ty);

	FragColor = texture(u_RGBTex, newTex);
}


void Num() {
	float tx = v_TPos.x;
	float ty = v_TPos.y;

	float offsetX = 0;
	float offsetY = 0;

	vec2 newTex = vec2(tx+offsetX, ty+offsetY);

	FragColor = texture(u_CurrNumTex, newTex);
}

void Nums() {
	float index = float(u_InputNum);
	float tx = v_TPos.x / 5;
	float ty = v_TPos.y / 2;

	float offsetX = fract(index/5.0);
	float offsetY = floor(index/5.0)/2.0;

	vec2 newTex = vec2(tx+offsetX, ty+offsetY);

	FragColor = texture(u_NumsTex, newTex);
}

void FS_01_Q6() {
	float tx = fract(v_TPos.x*3); //0~1 0~1 0~1
	float ty = v_TPos.y/3;
	float offsetX = 0;
	float offsetY = abs(floor(v_TPos.x * 3)/3 - 2);
	FragColor = texture(u_RGBTex, vec2(tx+offsetX, ty+offsetY));
}

void FS_01_Q7() {
	//	G
	//	B
	//	R
	float tx = v_TPos.x;
	float ty = fract(v_TPos.y*3)/3; //0~1/3 0~1/3 0~1/3 
	float offsetX = 0;
	float offsetY = 0;
	FragColor = texture(u_RGBTex, vec2(tx+offsetX, ty+offsetY));
}

void FS_01_Q8() {
	float tx = v_TPos.x/5; //������
	float ty = v_TPos.y/2;
	float offsetX = 2.0/5.0; //��ġ
	float offsetY = 1.0/2.0;
	FragColor = texture(u_NumsTex, vec2(tx+offsetX, ty+offsetY));
}

void FS_01_Q9() {
	float tx = v_TPos.x/5 * 2; //������
	float ty = v_TPos.y/2;
	float offsetX = 2.0/5.0; //��ġ
	float offsetY = 0;
	FragColor = texture(u_NumsTex, vec2(tx+offsetX, ty+offsetY));
}

void FS_01_Q10() {
	float index = float(8);

	float tx = v_TPos.x/5; //������
	float ty = v_TPos.y/2;

	float offsetX = fract(index/5.0); //��ġ, fract: �Ҽ��κи� ����
	float offsetY = floor(index/5.0)/2.0;

	FragColor = texture(u_NumsTex, vec2(tx+offsetX, ty+offsetY));
}

void main()
{
	//Nums();
	FS_01_Q10();
	//FractalPattern();
}
