// Example low level rendering Unity plugin
#include<iostream>
#include<fstream>
#include "UnityPluginInterface.h"
#include "TextureCapt.h"
#include <math.h>
#include <stdio.h>
#include <vector>
#include <string>
#include <sstream>
using namespace std;
// --------------------------------------------------------------------------
// Include headers for the graphics APIs we support

#if SUPPORT_D3D9
#include <d3d9.h>
#endif
#if SUPPORT_D3D11
#include <d3d11.h>
#endif
#if SUPPORT_OPENGL
#if UNITY_WIN || UNITY_LINUX
#include <GL/gl.h>
#else
#include <OpenGL/OpenGL.h>
#endif
#endif

// --------------------------------------------------------------------------
// Helper utilities
// Prints a string
static void DebugLog (const char* str)
{
#if UNITY_WIN
	OutputDebugStringA (str);
#else
	printf ("%s", str);
#endif
}

// COM-like Release macro
#ifndef SAFE_RELEASE
#define SAFE_RELEASE(a) if (a) { a->Release(); a = NULL; }
#endif

typedef int (__stdcall * Callback)(const char* text);
Callback Handler = 0;
extern "C" __declspec(dllexport)
void __stdcall SetCallback(Callback handler) {
  Handler = handler;
}
void log(const char* text){
if(Handler!=0)
	Handler(text);
}

//extern "C" __declspec(dllexport)
//void __stdcall TestCallback() {
//  int retval = Handler("hello world");
//}

static void convert(byte* d0,int s0, byte* d1,int* s1);
static void combine(TextureCapt* left,TextureCapt* right,byte* data,int* size);
// --------------------------------------------------------------------------
// SetTimeFromUnity, an example function we export which is called by one of the scripts.
static float g_Time;
TextureCapt tc[2];
byte tempData[WIDTH_MAX*2*HEIGHT_MAX*4];

extern "C" void EXPORT_API SetTimeFromUnity (float t) { g_Time = t; }

extern "C" void EXPORT_API CreateTextureCapt(int id,void* texturePtr){
	if(id>1||id<0)
		return;
	//tc[id]=new TextureCapt();
	tc[id].setTexture((ID3D11Texture2D*)texturePtr);
}

extern "C" void EXPORT_API  StartCapt(int id) {
	if(id>1||id<0)
		return;
	tc[id].isCapt=true;
	log("StartCapt");
}

extern "C" void EXPORT_API  StopCapt(int id) {
	if(id>1||id<0)
		return;
	tc[id].isCapt=false;
	log("StopCapt");
}

extern "C" void EXPORT_API  getTexture(int id,byte* data,int* size) {
	if(id>1||id<0)
		return;
	if(tc[id].isCapt){											     
		int s=tc[id].height*tc[id].width*4;
		byte* d=tc[id].data;
		convert(d,s, data, size);
	}
}

extern "C" void EXPORT_API GetCombinedTexture(byte* data,int* size){
	int width=tc[0].width+tc[1].width;
	int height=(tc[0].height+tc[1].height)/2;
	int tempSize=width*height*4;
	combine(&tc[0],&tc[1],tempData,&tempSize);
	convert(tempData,tempSize,data,size);//cirtical!!
}

static void combine(TextureCapt* left,TextureCapt* right,byte* data,int* size){
	if(!(left->width==right->width&&left->height==right->height))
		return;
	int width=left->width+right->width;
	int height=(left->height+right->height)/2;
	byte* leftMap = left->data;
	byte* rightMap = right->data;
	byte* desP=data;
	int widthSize=(width/2)*4;
	for (int j = 0; j < height; j++){
		memcpy(desP,leftMap,widthSize);
		desP+=widthSize;
		leftMap+=widthSize;
		memcpy(desP,rightMap,widthSize);
		desP+=widthSize;
		rightMap+=widthSize;
	}
	*size=width*height*4;
	log("combine");
}

//from rgba to bgr
static void convert(byte* d0,int s0, byte* d1,int* s1){
	int index_d=0;
	int index_data=0;
	int pc=s0/4;
	for(int i=0;i<pc;i++){
		int tempIndex=index_d;
		d1[tempIndex+2]=d0[index_data++];//b-r
		d1[tempIndex+1]=d0[index_data++];//g-g
		d1[tempIndex+0]=d0[index_data++];//r-b
		index_data++;//a
		index_d+=3;
	}
	*s1=(pc*3);
	log("convert");
}

// --------------------------------------------------------------------------
// UnitySetGraphicsDevice
static int g_DeviceType = -1;

// Actual setup/teardown functions defined below
#if SUPPORT_D3D11
static void SetGraphicsDeviceD3D11 (ID3D11Device* device, GfxDeviceEventType eventType);
#endif

extern "C" void EXPORT_API UnitySetGraphicsDevice (void* device, int deviceType, int eventType)
{
	// Set device type to -1, i.e. "not recognized by our plugin"
	g_DeviceType = -1;
#if SUPPORT_D3D11
	// D3D11 device, remember device pointer and device type.
	// The pointer we get is ID3D11Device.
	if (deviceType == kGfxRendererD3D11)
	{
		DebugLog ("Set D3D11 graphics device\n");
		g_DeviceType = deviceType;
		SetGraphicsDeviceD3D11 ((ID3D11Device*)device, (GfxDeviceEventType)eventType);
	}
#endif
}

// --------------------------------------------------------------------------
// UnityRenderEvent
// This will be called for GL.IssuePluginEvent script calls; eventID will
// be the integer passed to IssuePluginEvent. In this example, we just ignore
// that value.

static void SetDefaultGraphicsState ();
static void DoRendering (int);
static void textureDataGet(const byte * data,const int size);

extern "C" void EXPORT_API UnityRenderEvent (int eventID)
{
	// Unknown graphics device type? Do nothing.
	if (g_DeviceType == -1)
		return;
	// Actual functions defined below
	SetDefaultGraphicsState ();
	DoRendering (eventID);
}

// -------------------------------------------------------------------
//  Direct3D 11 setup/teardown code
#if SUPPORT_D3D11

static ID3D11Device* g_D3D11Device;
static ID3D11Buffer* g_D3D11VB; // vertex buffer
static ID3D11Buffer* g_D3D11CB; // constant buffer
static ID3D11VertexShader* g_D3D11VertexShader;
static ID3D11PixelShader* g_D3D11PixelShader;
static ID3D11InputLayout* g_D3D11InputLayout;
static ID3D11RasterizerState* g_D3D11RasterState;
static ID3D11BlendState* g_D3D11BlendState;
static ID3D11DepthStencilState* g_D3D11DepthState;

#if !UNITY_METRO
typedef HRESULT (WINAPI *D3DCompileFunc)(
	const void* pSrcData,
	unsigned long SrcDataSize,
	const char* pFileName,
	const D3D10_SHADER_MACRO* pDefines,
	ID3D10Include* pInclude,
	const char* pEntrypoint,
	const char* pTarget,
	unsigned int Flags1,
	unsigned int Flags2,
	ID3D10Blob** ppCode,
	ID3D10Blob** ppErrorMsgs);

static const char* kD3D11ShaderText =
	"cbuffer MyCB : register(b0) {\n"
	"	float4x4 worldMatrix;\n"
	"}\n"
	"void VS (float3 pos : POSITION, float4 color : COLOR, out float4 ocolor : COLOR, out float4 opos : SV_Position) {\n"
	"	opos = mul (worldMatrix, float4(pos,1));\n"
	"	ocolor = color;\n"
	"}\n"
	"float4 PS (float4 color : COLOR) : SV_TARGET {\n"
	"	return color;\n"
	"}\n";
#elif UNITY_METRO
typedef std::vector<unsigned char> Buffer;
bool LoadFileIntoBuffer(const char* fileName, Buffer& data)
{
	FILE* fp;
	fopen_s(&fp, fileName,"rb");
	if (fp)
	{
		fseek (fp, 0, SEEK_END);
		int size = ftell (fp);
		fseek (fp, 0, SEEK_SET);
		data.resize(size);

		fread(&data[0], size, 1, fp);

		fclose(fp);

		return true;
	}
	else
	{
		return false;
	}
}

#endif
static D3D11_INPUT_ELEMENT_DESC s_DX11InputElementDesc[] = {
	{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, 0, D3D11_INPUT_PER_VERTEX_DATA, 0 },
	{ "COLOR", 0, DXGI_FORMAT_R8G8B8A8_UNORM, 0, 12, D3D11_INPUT_PER_VERTEX_DATA, 0 },
};
static void CreateD3D11Resources()
{
	D3D11_BUFFER_DESC desc;
	memset (&desc, 0, sizeof(desc));

	// vertex buffer
	desc.Usage = D3D11_USAGE_DEFAULT;
	desc.ByteWidth = 1024;
	desc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	g_D3D11Device->CreateBuffer (&desc, NULL, &g_D3D11VB);

	// constant buffer
	desc.Usage = D3D11_USAGE_DEFAULT;
	desc.ByteWidth = 64; // hold 1 matrix
	desc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	desc.CPUAccessFlags = 0;
	g_D3D11Device->CreateBuffer (&desc, NULL, &g_D3D11CB);

#if !UNITY_METRO
	// shaders
	HMODULE compiler = LoadLibraryA("D3DCompiler_43.dll");

	if (compiler == NULL)
	{
		// Try compiler from Windows 8 SDK
		compiler = LoadLibraryA("D3DCompiler_46.dll");
	}
	if (compiler)
	{
		ID3D10Blob* vsBlob = NULL;
		ID3D10Blob* psBlob = NULL;

		D3DCompileFunc compileFunc = (D3DCompileFunc)GetProcAddress (compiler, "D3DCompile");
		if (compileFunc)
		{
			HRESULT hr;
			hr = compileFunc(kD3D11ShaderText, strlen(kD3D11ShaderText), NULL, NULL, NULL, "VS", "vs_4_0", 0, 0, &vsBlob, NULL);
			if (SUCCEEDED(hr))
			{
				g_D3D11Device->CreateVertexShader (vsBlob->GetBufferPointer(), vsBlob->GetBufferSize(), NULL, &g_D3D11VertexShader);
			}

			hr = compileFunc(kD3D11ShaderText, strlen(kD3D11ShaderText), NULL, NULL, NULL, "PS", "ps_4_0", 0, 0, &psBlob, NULL);
			if (SUCCEEDED(hr))
			{
				g_D3D11Device->CreatePixelShader (psBlob->GetBufferPointer(), psBlob->GetBufferSize(), NULL, &g_D3D11PixelShader);
			}
		}

		// input layout
		if (g_D3D11VertexShader && vsBlob)
		{
			g_D3D11Device->CreateInputLayout (s_DX11InputElementDesc, 2, vsBlob->GetBufferPointer(), vsBlob->GetBufferSize(), &g_D3D11InputLayout);
		}

		SAFE_RELEASE(vsBlob);
		SAFE_RELEASE(psBlob);

		FreeLibrary (compiler);
	}
	else
	{
		DebugLog ("D3D11: HLSL shader compiler not found, will not render anything\n");
	}
#elif UNITY_METRO
	HRESULT hr = -1;
	Buffer vertexShader;
	Buffer pixelShader;
	LoadFileIntoBuffer("Data\\StreamingAssets\\SimpleVertexShader.cso", vertexShader);
	LoadFileIntoBuffer("Data\\StreamingAssets\\SimplePixelShader.cso", pixelShader);

	if (vertexShader.size() > 0 && pixelShader.size() > 0)
	{
		hr = g_D3D11Device->CreateVertexShader(&vertexShader[0], vertexShader.size(), nullptr, &g_D3D11VertexShader);
		if (FAILED(hr)) DebugLog("Failed to create vertex shader.");
		hr = g_D3D11Device->CreatePixelShader(&pixelShader[0], pixelShader.size(), nullptr, &g_D3D11PixelShader);
		if (FAILED(hr)) DebugLog("Failed to create pixel shader.");
	}
	else
	{
		DebugLog("Failed to load vertex or pixel shader.");
	}
	// input layout
	if (g_D3D11VertexShader && vertexShader.size() > 0)
	{
		g_D3D11Device->CreateInputLayout (s_DX11InputElementDesc, 2, &vertexShader[0], vertexShader.size(), &g_D3D11InputLayout);
	}
#endif
	// render states
	D3D11_RASTERIZER_DESC rsdesc;
	memset (&rsdesc, 0, sizeof(rsdesc));
	rsdesc.FillMode = D3D11_FILL_SOLID;
	rsdesc.CullMode = D3D11_CULL_NONE;
	rsdesc.DepthClipEnable = TRUE;
	g_D3D11Device->CreateRasterizerState (&rsdesc, &g_D3D11RasterState);

	D3D11_DEPTH_STENCIL_DESC dsdesc;
	memset (&dsdesc, 0, sizeof(dsdesc));
	dsdesc.DepthEnable = TRUE;
	dsdesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ZERO;
	dsdesc.DepthFunc = D3D11_COMPARISON_LESS_EQUAL;
	g_D3D11Device->CreateDepthStencilState (&dsdesc, &g_D3D11DepthState);

	D3D11_BLEND_DESC bdesc;
	memset (&bdesc, 0, sizeof(bdesc));
	bdesc.RenderTarget[0].BlendEnable = FALSE;
	bdesc.RenderTarget[0].RenderTargetWriteMask = 0xF;
	g_D3D11Device->CreateBlendState (&bdesc, &g_D3D11BlendState);
}

static void ReleaseD3D11Resources()
{
	SAFE_RELEASE(g_D3D11VB);
	SAFE_RELEASE(g_D3D11CB);
	SAFE_RELEASE(g_D3D11VertexShader);
	SAFE_RELEASE(g_D3D11PixelShader);
	SAFE_RELEASE(g_D3D11InputLayout);
	SAFE_RELEASE(g_D3D11RasterState);
	SAFE_RELEASE(g_D3D11BlendState);
	SAFE_RELEASE(g_D3D11DepthState);
}

static void SetGraphicsDeviceD3D11 (ID3D11Device* device, GfxDeviceEventType eventType)
{
	g_D3D11Device = device;

	if (eventType == kGfxDeviceEventInitialize)
		CreateD3D11Resources();
	if (eventType == kGfxDeviceEventShutdown)
		ReleaseD3D11Resources();
}

#endif // #if SUPPORT_D3D11

// --------------------------------------------------------------------------
// SetDefaultGraphicsState
//
// Helper function to setup some "sane" graphics state. Rendering state
// upon call into our plugin can be almost completely arbitrary depending
// on what was rendered in Unity before.
// Before calling into the plugin, Unity will set shaders to null,
// and will unbind most of "current" objects (e.g. VBOs in OpenGL case).
//
// Here, we set culling off, lighting off, alpha blend & test off, Z
// comparison to less equal, and Z writes off.

static void SetDefaultGraphicsState ()
{
#if SUPPORT_D3D11
	// D3D11 case
	if (g_DeviceType == kGfxRendererD3D11)
	{
		ID3D11DeviceContext* ctx = NULL;
		g_D3D11Device->GetImmediateContext (&ctx);
		ctx->OMSetDepthStencilState (g_D3D11DepthState, 0);
		ctx->RSSetState (g_D3D11RasterState);
		ctx->OMSetBlendState (g_D3D11BlendState, NULL, 0xFFFFFFFF);
		ctx->Release();
	}
#endif
}

string int2str(int &i) {
  string s;
  stringstream ss(s);
 ss << i;
  return ss.str();
}

static void DoRendering (int id)
{
#if SUPPORT_D3D11
	// D3D11 case
	if (g_DeviceType == kGfxRendererD3D11 && g_D3D11VertexShader)
	{
		int r=tc[id].doCapturing(g_D3D11Device);
		string s="DoRendering"+int2str(r);
		log(s.c_str());
	}
#endif
}
