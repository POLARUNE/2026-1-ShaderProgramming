#version 330

layout(location=0) out vec4 FragColor;

uniform sampler2D u_AhnTex;

in float v_Grey;
in vec2 v_Tex;

const float c_PI = 3.141592;

mat2 rotate2D(float radian) {
	return mat2(cos(radian), -sin(radian), sin(radian), cos(radian));
}

float LinePattern(){
	//FragColor = vec4(v_TPos, 0, 1);
	float lineCountH = 10;
	float lineCountV = 5;
	float lineWidth = 1;
	lineCountH = lineCountH / 2;
	lineCountV = lineCountV / 2;
	lineWidth = 50 / lineWidth;
	float per = -0.5*c_PI;

	float grey = pow(abs(sin((v_Tex.y*2*c_PI+per)*lineCountH)), lineWidth);
	float grey1 = pow(abs(sin((v_Tex.x*2*c_PI+per)*lineCountV)), lineWidth);
	return (grey+grey1);
}

void flag()
{	
	//vec2 newTex = rotate2D(-c_PI/2.0) * v_Tex;
	FragColor =  texture(u_AhnTex, v_Tex);
}

void main()
{
	FragColor = vec4(v_Grey);
}
