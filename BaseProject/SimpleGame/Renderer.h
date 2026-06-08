#pragma once

#include <string>
#include <cstdlib>
#include <fstream>
#include <iostream>

#include "Dependencies\glew.h"

class Renderer
{
public:
	Renderer(int windowSizeX, int windowSizeY);
	~Renderer();

	bool IsInitialized();
	void DrawSolidRect(float x, float y, float z, float size, float r, float g, float b, float a);
	void DrawTriangle();
	void DrawTriangle_Bloom();
	void DrawParticles(int count);
	void DrawFS();
	void DrawDummy();
	void DrawDummy_FBO();
	void DrawAll_FBO();
	void DrawMultipleRenderTarget();

private:
	void Initialize(int windowSizeX, int windowSizeY);
	bool ReadFile(char* filename, std::string *target);
	void AddShader(GLuint ShaderProgram, const char* pShaderText, GLenum ShaderType);
	GLuint CompileShaders(char* filenameVS, char* filenameFS);
	void CreateVertexBufferObjects();
	void GetGLPosition(float x, float y, float *newX, float *newY);

	GLuint CreatePngTexture(char* filePath, GLuint samplingMethod);

	void GenDummyMesh(int x, int y);

	void DrawTexture(GLuint texID, float x, float y, float scale, bool flip);

	void GenFBOs();

	void DrawGaussianBlur(GLuint texID, GLuint targetFBOID, GLuint shader);

	bool m_Initialized = false;
	
	unsigned int m_WindowSizeX = 0;
	unsigned int m_WindowSizeY = 0;

	GLuint m_VBORect = 0;
	GLuint m_SolidRectShader = 0;

	GLuint m_VBOTriangle = 0;
	GLuint m_TriangleShader = 0;

	GLuint m_VBOParticle = 0;
	GLuint m_ParticleShader = 0;

	//Fragment Shader
	GLuint m_VBOFS = 0;
	GLuint m_FSShader = 0;

	//RainDrops
	float m_DropPoints[1000 * 4];

	//Textures
	GLuint m_RgbTexture = 0;
	GLuint m_NumTexture[10];
	GLuint m_NumsTexture = 0;

	GLuint m_ParticleTexture = 0;
	GLuint m_ParticleSpriteTexture = 0;

	GLuint m_AhnTexture = 0;

	//Dummy Vertex
	GLuint m_VBODummy = 0;
	GLuint m_VBODummyCount = 0;

	GLuint m_DummyShader = 0;

	//Texture API
	GLuint m_TextureShader = 0;
	GLuint m_TextureVBO = 0;

	//FBO
	GLuint m_FBO = 0;
	GLuint m_FBO_Texture = 0;

	GLuint m_FBO1 = 0;
	GLuint m_FBO_Texture1 = 0;

	GLuint m_FBO2 = 0;
	GLuint m_FBO_Texture2 = 0;

	GLuint m_MRT_FBO = 0;
	GLuint m_MRT_FBO_Texture0 = 0;
	GLuint m_MRT_FBO_Texture1 = 0;
	GLuint m_MRT_FBO_Texture2 = 0;

	GLuint m_MRT_HDR_FBO = 0;
	GLuint m_MRT_HDR_FBO_High_Texture = 0; // float Texture
	GLuint m_MRT_HDR_FBO_Low_Texture = 0; // float Texture

	GLuint m_PingPongFBO[2];
	GLuint m_PingPongTexture[2];
	GLuint m_BlurH_Shader = 0;
	GLuint m_BlurV_Shader = 0;
};

