#version 330

layout(location=0) out vec4 FragColor;

uniform sampler2D u_Tex;

in vec2 v_Tex;

void main()
{
	FragColor = texture(u_Tex, v_Tex);
}
