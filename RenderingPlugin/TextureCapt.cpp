#include "TextureCapt.h"

TextureCapt::TextureCapt()
{
	isCapt=false;	
	g_TexturePointer=NULL;
}

void TextureCapt::setTexture(ID3D11Texture2D* tp){
	g_TexturePointer=tp;
	if (g_TexturePointer){
		ID3D11Texture2D* d3dtex = g_TexturePointer;
		D3D11_TEXTURE2D_DESC desc;
		d3dtex->GetDesc (&desc);
		width=desc.Width;
		height=desc.Height;
		size=desc.Width*desc.Height*4;
	}
}

TextureCapt::~TextureCapt(void)
{
}

int TextureCapt::doCapturing(ID3D11Device* g_D3D11Device){
	int flag=0;
	if(!isCapt)
		flag= -1;
	else if (g_TexturePointer){
		ID3D11DeviceContext* ctx = NULL;
		g_D3D11Device->GetImmediateContext (&ctx);
		ID3D11Texture2D* d3dtex = g_TexturePointer;
		D3D11_TEXTURE2D_DESC bufferDesc;
		ZeroMemory(&bufferDesc,sizeof(bufferDesc));
		d3dtex->GetDesc(&bufferDesc);
		bufferDesc.CPUAccessFlags=D3D11_CPU_ACCESS_READ;
		bufferDesc.Usage=D3D11_USAGE_STAGING;
		bufferDesc.BindFlags=0;
		bufferDesc.MiscFlags=0;

		ID3D11Texture2D* pDebugBuffer=NULL;

		if( SUCCEEDED(g_D3D11Device->CreateTexture2D (&bufferDesc,NULL,&pDebugBuffer)) ){
			ctx->CopyResource(pDebugBuffer,d3dtex);
		}else
			flag= -2;
		D3D11_MAPPED_SUBRESOURCE mappedOutResource;
		byte * pOut=NULL;
		ctx->Map(pDebugBuffer,0,D3D11_MAP_READ,0,&mappedOutResource);
		pOut=(byte *)mappedOutResource.pData;	
		memcpy(data,pOut,size);
		ctx->Unmap(pDebugBuffer,0);
		pDebugBuffer->Release();
		pDebugBuffer=NULL;
		ctx->Release();
		flag= 1;
	}
	return flag;
}