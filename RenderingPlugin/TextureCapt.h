#pragma once
#define WIDTH_MAX 1024
#define HEIGHT_MAX 1024
#include <d3d11.h>
typedef unsigned char byte;
class TextureCapt
{
public:
	TextureCapt();
	~TextureCapt(void);
	int doCapturing (ID3D11Device* g_D3D11Device);
	byte data[WIDTH_MAX*HEIGHT_MAX*4];
	int width,height;
	bool isCapt;
	void setTexture(ID3D11Texture2D* tp);
private:
	ID3D11Texture2D* g_TexturePointer;
	int size;
};

