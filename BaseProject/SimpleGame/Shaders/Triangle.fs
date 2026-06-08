#version 330

layout(location=0) out vec4 FragColor;
layout(location=1) out vec4 FragColor1;

in float v_Grey;
in vec2 v_Tex;
in vec3 v_Color;

uniform sampler2D u_ParticleTex;
uniform sampler2D u_ParticleSpriteTex;

void CircleShape(){
	float d = distance(vec2(0.5,0.5), v_Tex);

	if (d < 0.5) {
		FragColor = vec4(v_Color, clamp(0.5-d, 0, 0.5)*2.0);
	}
	else {
		FragColor= vec4(0);
	}
}

void SingleTexture()
{
	FragColor = v_Grey * texture(u_ParticleTex, v_Tex);
}

void AnimTexture()
{
	float resolX = 9.0;
	float resolY = 9.0;
	float index = floor((1.0 - v_Grey) * (resolX*resolY-1));
	float tx = v_Tex.x / resolX;
	float ty = v_Tex.y / resolY;
	float offsetX = fract(index / resolX);
	float offsetY = floor(index / resolX) / resolY;

	vec2 newTex = vec2(tx+offsetX, ty+offsetY);

	float d = distance(vec2(0.5,0.5), v_Tex);
	float value = clamp(0.5 - d, 0, 0.5) * 2.0;

	FragColor = v_Grey * texture(u_ParticleSpriteTex, newTex);
	FragColor.a *= value;
}

void CircleShapeHDR(){
	vec4 newColor;

	float d = distance(vec2(0.5,0.5), v_Tex);

	if (d < 0.5) {
		newColor = vec4(v_Color*1.5, clamp(0.5-d, 0, 0.5)*2.0);
	}
	else {
		newColor= vec4(0);
	}

	float brightness = dot(newColor.rgb, vec3(0.2126, 0.7152, 0.0722));

	FragColor = newColor;

	if(brightness > 1.0){
		FragColor1 = newColor;
	}	
	else{
		FragColor1 = vec4(0);
	}
}

void main()
{
	//AnimTexture();
	CircleShapeHDR();
}
