#version 330

uniform float u_Time;
uniform vec4 u_DropInfo[1000]; // x, y, sT, lT

in vec3 a_Pos; // -0.5~0.5

out float v_Grey;
out vec2 v_Tex;

const float c_PI = 3.1415926;

void flag()
{
	float tX, tY;
	tX = a_Pos.x + 0.5;
	tY = 1 - (a_Pos.y + 0.5);
	v_Tex = vec2(tX, tY);

	float value = a_Pos.x + 0.5; // 0.0 ~ 1.0

	float newX = a_Pos.x;
	float newY = a_Pos.y * (1 - (value * 0.5)) +  value * 0.25 * sin((newX + 0.5) * 2 * c_PI - u_Time); // -0.25 ~ 0.25

	vec4 final = vec4(newX, newY, 0.0, 1.0);

	vec4 newPosition = final;

	float grey = (sin((newX + 0.5) * 2 * c_PI - u_Time) + 1.0) / 2.0; // 0.0 ~ 1.0
	v_Grey = grey;

	gl_Position = newPosition;
}

void Circles()
{
	float accum = 0;

	for (int i = 0; i < 1000; i++){
		vec2 center = u_DropInfo[i].xy - vec2(0.5, 0.5);
		vec2 pos = a_Pos.xy;
		float ltime = u_DropInfo[i].z;
		float sTime = u_DropInfo[i].w;
		float nTime = (u_Time - sTime) * 10.0;

		if (nTime > 0) {
			float lVal = fract(nTime / ltime); // 0.0 ~ 1.0
			float oneMinus = 1.0 - lVal;

			float t = lVal * ltime;

			float d = distance(center, pos);

			float range = t/15.0;

			float fade = 15 * clamp(range - d, 0, 1.0);

			float sinValue = pow(abs(sin(d * 4.0 * c_PI * 8.0 + t * 2.0)), 3.0);
			accum += sinValue * fade * oneMinus;
		}
	}

	v_Grey = accum;

	//gl_Position = vec4(a_Pos, 1.0);
	gl_Position = vec4(a_Pos.x, a_Pos.y, a_Pos.z, 1.0);
}

void main()
{
	//flag();
	Circles();
}
