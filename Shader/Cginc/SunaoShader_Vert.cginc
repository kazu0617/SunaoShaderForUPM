//--------------------------------------------------------------
//              Sunao Shader Vert
//                      Copyright (c) 2019 揚茄子研究所
//--------------------------------------------------------------


VOUT vert (VIN v) {

	VOUT o;

//-------------------------------------頂点座標変換
	o.pos     = UnityObjectToClipPos(v.vertex);
	float3 PosW = mul(unity_ObjectToWorld , v.vertex).xyz;

//-------------------------------------UV
	o.uv      = (v.uv * _MainTex_ST.xy) + _MainTex_ST.zw;

//-------------------------------------法線
	o.normal  = v.normal;

//-------------------------------------頂点カラー
	o.color   = (float3)1.0f;
	if (_VertexColor) o.color = v.color;

//-------------------------------------ライト方向
	o.ldir    = _WorldSpaceLightPos0.xyz;

	#ifdef PASS_FA
		o.ldir -= PosW;
	#endif

	o.ldir    = normalize(o.ldir);

//-------------------------------------カメラ方向
	o.view    = normalize(_WorldSpaceCameraPos - PosW);

//-------------------------------------SHライト
	#ifdef PASS_FB
		float3 SHColor[6];
		SHColor[0]   = ShadeSH9(float4(-1.0f ,  0.0f ,  0.0f , 1.0f));
		SHColor[1]   = ShadeSH9(float4( 1.0f ,  0.0f ,  0.0f , 1.0f));
		SHColor[2]   = ShadeSH9(float4( 0.0f , -1.0f ,  0.0f , 1.0f));
		SHColor[3]   = ShadeSH9(float4( 0.0f ,  1.0f ,  0.0f , 1.0f));
		SHColor[4]   = ShadeSH9(float4( 0.0f ,  0.0f , -1.0f , 1.0f));
		SHColor[5]   = ShadeSH9(float4( 0.0f ,  0.0f ,  1.0f , 1.0f));

		float SHLength[6];
		SHLength[0]  = MonoColor(SHColor[0]);
		SHLength[1]  = MonoColor(SHColor[1]);
		SHLength[2]  = MonoColor(SHColor[2]);
		SHLength[3]  = MonoColor(SHColor[3]) + 0.000001f;
		SHLength[4]  = MonoColor(SHColor[4]);
		SHLength[5]  = MonoColor(SHColor[5]);

		o.shdir      = SHLightDirection(SHLength);
		o.shmax      = SHLightMax(SHColor) * _SHLight;
		o.shmin      = SHLightMin(SHColor) * _SHLight;

		if (_MonochromeLit) {
			o.shmax  = MonoColor(o.shmax);
			o.shmin  = MonoColor(o.shmin);
		}
	#endif

//-------------------------------------Vertexライト
	#ifdef PASS_FB
		#if defined(UNITY_SHOULD_SAMPLE_SH) && defined(VERTEXLIGHT_ON)

			o.vldirX  = unity_4LightPosX0 - PosW.x;
			o.vldirY  = unity_4LightPosY0 - PosW.y;
			o.vldirZ  = unity_4LightPosZ0 - PosW.z;

			float4 VLLength = VLightLength(o.vldirX , o.vldirY , o.vldirZ);
			o.vlcorr  = rsqrt(VLLength);
			o.vlatn   = VLightAtten(VLLength) * _PointLight;

		#else

			o.vldirX  = (float4)0.0f;
			o.vldirY  = (float4)0.0f;
			o.vldirZ  = (float4)0.0f;
			o.vlcorr  = (float4)0.0f;
			o.vlatn   = (float4)0.0f;

		#endif
	#endif

//-------------------------------------Toon
	o.toon  = Toon(max(float(11 - _Toon) , 0.0f));

//-------------------------------------エミッションUV
	#ifdef PASS_FB
		o.euv.xy  = MixingTransformTex(v.uv , _MainTex_ST , _EmissionMap_ST );
		o.euv.zw  = MixingTransformTex(v.uv , _MainTex_ST , _EmissionMap2_ST);
	#endif

//-------------------------------------エミッション時間変化
	#ifdef PASS_FB
		o.eprm = (float3)0.0f;
		if (_EmissionEnable) {
			o.eprm.x = EmissionWave(_EmissionWaveform , _EmissionBlink , _EmissionFrequency);
		}
	#endif

//-------------------------------------エミッションスクロール
	#ifdef PASS_FB
		o.eprm.yz = float2(_EmissionScrX * _Time.y , _EmissionScrY * _Time.y);
	#endif

//-------------------------------------接ベクトル
	o.tanW    = UnityObjectToWorldDir(v.tangent.xyz);
	o.tanB    = cross(UnityObjectToWorldNormal(v.normal) , o.tanW) * v.tangent.w * unity_WorldTransformParams.w;

//-------------------------------------MatCap
	float3 MatCam = normalize(UNITY_MATRIX_V[1].xyz);
	o.matcapv = normalize(MatCam - o.view * dot(o.view, MatCam));
	o.matcaph = normalize(cross(o.view , o.matcapv));

//-------------------------------------ポイントライト
	#ifdef PASS_FA
		TRANSFER_VERTEX_TO_FRAGMENT(o)
	#endif

//-------------------------------------フォグ
	UNITY_TRANSFER_FOG(o,o.pos)


	return o;
}
