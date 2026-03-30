#version 330

in vec3 a_Pos;
in vec2 a_TPos;

out vec2 v_TPos;

void main()
{
	vec4 newPosition;
	newPosition = vec4(a_Pos, 1);

	v_TPos = a_TPos;

	gl_Position = newPosition;
}
