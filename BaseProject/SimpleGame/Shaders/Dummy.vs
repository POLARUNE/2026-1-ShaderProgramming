#version 330

uniform float u_Time;

in vec3 a_Pos; // -0.5~0.5

out float v_Grey;

const float c_PI = 3.1415926;

void main()
{
	float value = a_Pos.x + 0.5; // 0.0 ~ 1.0

	float newX = a_Pos.x;
	float newY = a_Pos.y +  value * 0.25 * sin((newX + 0.5) * 2 * c_PI - u_Time); // -0.25 ~ 0.25

	vec4 final = vec4(newX, newY, 0.0, 1.0);

	vec4 newPosition = final;

	float grey = (sin((newX + 0.5) * 2 * c_PI - u_Time) + 1.0) / 2.0; // 0.0 ~ 1.0
	v_Grey = grey;

	gl_Position = newPosition;
}
