//--------------------------------------------------------------
//              Sunao Shader Fragment
//                      Copyright (c) 2020 揚茄子研究所
//--------------------------------------------------------------


float4 frag (VOUT IN) : COLOR {

//-------------------------------------メインカラー
	float4 OUT          = float4(0.0f , 0.0f , 0.0f , 1.0f);

	#if defined(TRANSPARENT) || defined(CUTOUT)
	       OUT.a        = saturate(tex2D(_MainTex  , IN.uv).a * _Color.a * _Alpha);
	       OUT.a       *= lerp(1.0f , MonoColor(tex2D(_AlphaMask  , IN.uv).rgb) , _AlphaMaskStrength);
	#endif

	float3 Color        = tex2D(_MainTex  , IN.uv).rgb;
	       Color        = Color * _Color.rgb * _Bright * IN.color;

	if (_OcclusionMode == 1) Color *= lerp(1.0f , tex2D(_OcclusionMap  , IN.uv).rgb , _OcclusionStrength);

//-------------------------------------カットアウト
	#ifdef CUTOUT
	       clip(OUT.a - _Cutout);
	#endif

//-------------------------------------ノーマルマップ
	float3 Normal       = UnityObjectToWorldNormal(IN.normal);

	float3 tan_sx       = float3(IN.tanW.x , IN.tanB.x , Normal.x);
	float3 tan_sy       = float3(IN.tanW.y , IN.tanB.y , Normal.y);
	float3 tan_sz       = float3(IN.tanW.z , IN.tanB.z , Normal.z);

	float3 NormalMap    = normalize(UnpackScaleNormal(tex2D(_BumpMap, IN.uv) , _BumpScale));
	       Normal.x     = dot(tan_sx , NormalMap);
	       Normal.y     = dot(tan_sy , NormalMap);
	       Normal.z     = dot(tan_sz , NormalMap);

//-------------------------------------シェーディング
	float3 ShadeMask    = tex2D(_ShadeMask, IN.uv).rgb * _Shade;
	float3 LightBoost   = 1.0f + (tex2D(_LightMask, IN.uv).rgb * (_LightBoost - 1.0f));

//----ディフューズ
	float  Diffuse      = DiffuseCalc(Normal , IN.ldir , _ShadeGradient , _ShadeWidth);

	#ifdef PASS_FB
		float  SHDiffuse    = DiffuseCalc(Normal , IN.shdir , _ShadeGradient , _ShadeWidth);

		float4 VLDiffuse    = IN.vldirX * Normal.x;
		       VLDiffuse   += IN.vldirY * Normal.y;
		       VLDiffuse   += IN.vldirZ * Normal.z;
		       VLDiffuse    = max((float4)0.0f , VLDiffuse * IN.vlcorr);
	#endif

//----トゥーンシェーディング
	if (_ToonEnable) {
		Diffuse   = ToonCalc(Diffuse , IN.toon);
		#ifdef PASS_FB
			SHDiffuse = ToonCalc(SHDiffuse , IN.toon);
			VLDiffuse = ToonCalc(VLDiffuse , IN.toon);
		#endif
	}

//----影の色
	float3 ShadeColor   = saturate(Color * 3.0f - 1.5f) * _ShadeColor;
	       ShadeColor   = lerp(ShadeColor , _CustomShadeColor.rgb , _CustomShadeColor.a);

//-------------------------------------ライティング
	#ifdef PASS_FB
		float3 LightBase    = _LightColor0 * _DirectionalLight;
		float3 VLight0      = unity_LightColor[0].rgb * IN.vlatn.x;
		float3 VLight1      = unity_LightColor[1].rgb * IN.vlatn.y;
		float3 VLight2      = unity_LightColor[2].rgb * IN.vlatn.z;
		float3 VLight3      = unity_LightColor[3].rgb * IN.vlatn.w;
		float3 VLightBase   = saturate(VLight0 + VLight1 + VLight2 + VLight3);
	#endif
	#ifdef PASS_FA
		float3 LightBase    = _LightColor0 * LIGHT_ATTENUATION(IN) * _PointLight * 0.9f;
	#endif

//----モノクロライティング
	if (_MonochromeLit) {
		LightBase  = MonoColor(LightBase);
		#ifdef PASS_FB
			VLight0    = MonoColor(VLight0);
			VLight1    = MonoColor(VLight1);
			VLight2    = MonoColor(VLight2);
			VLight3    = MonoColor(VLight3);
			VLightBase = MonoColor(VLightBase);
		#endif
	}

//----ライト反映
	float3 Lighting     = LightBase;

	float3 DiffColor    = LightingCalc(Lighting , Diffuse , ShadeColor , ShadeMask);

	#ifdef PASS_FB
		float3 SHDiffColor  = LightingCalc(IN.shmax , SHDiffuse , ShadeColor , ShadeMask);
		       SHDiffColor  = saturate(SHDiffColor - IN.shmin) + IN.shmin;
		if (_OcclusionMode == 0) SHDiffColor *= lerp(1.0f , tex2D(_OcclusionMap  , IN.uv).rgb , _OcclusionStrength);

		float3 VL4Diff[4];
		       VL4Diff[0]   = LightingCalc(VLight0 , VLDiffuse.x , ShadeColor , ShadeMask);
		       VL4Diff[1]   = LightingCalc(VLight1 , VLDiffuse.y , ShadeColor , ShadeMask);
		       VL4Diff[2]   = LightingCalc(VLight2 , VLDiffuse.z , ShadeColor , ShadeMask);
		       VL4Diff[3]   = LightingCalc(VLight3 , VLDiffuse.w , ShadeColor , ShadeMask);
		float3 VLDiffColor  = saturate(VL4Diff[0] + VL4Diff[1] + VL4Diff[2] + VL4Diff[3]);

		       Lighting     = (DiffColor + SHDiffColor + VLDiffColor) * LightBoost;
	#endif
	#ifdef PASS_FA
		       Lighting     = DiffColor * LightBoost;
	#endif

	if (_LightLimitter) Lighting = saturate(Lighting);

//-------------------------------------エミッション
	float3 Emission     = (float3)0.0f;

	bool   EmissionFlag = _EmissionEnable;
	#ifdef PASS_FA
		EmissionFlag = _EmissionEnable && _EmissionLighting;
	#endif

	if (EmissionFlag) {
		float2 EmissionUV  = IN.uv + IN.eprm.yz;
		       EmissionUV  = MixingTransformTex(EmissionUV , _MainTex_ST , _EmissionMap_ST);
		       Emission    = _Emission * _EmissionColor.rgb;
		       Emission   *= tex2D(_EmissionMap  , EmissionUV).rgb * tex2D(_EmissionMap  , EmissionUV).a * IN.eprm.x;
		       Emission   *= tex2D(_EmissionMap2 , IN.euv    ).rgb * tex2D(_EmissionMap2 , IN.euv    ).a;

		if (_EmissionLighting) {
			#ifdef PASS_FB
				Emission   *= saturate(MonoColor(LightBase) + MonoColor(IN.shmax) + MonoColor(VLightBase));
			#endif
			#ifdef PASS_FA
				Emission   *= saturate(MonoColor(LightBase));
			#endif
		}
	}

//-------------------------------------視差エミッション
	float3 Parallax     = (float3)0.0f;

	bool   ParallaxFlag = _ParallaxEnable;
	#ifdef PASS_FA
		ParallaxFlag = _ParallaxEnable && _ParallaxLighting;
	#endif

	if (ParallaxFlag) {
		float  Height      = (1.0f - MonoColor(tex2D(_ParallaxDepthMap , IN.pduv).rgb)) * _ParallaxDepth;
		float2 ParallaxUV  = IN.uv + IN.peprm.yz;
		       ParallaxUV -= normalize(IN.pview).xz * Height;
		       ParallaxUV  = MixingTransformTex(ParallaxUV , _MainTex_ST , _ParallaxMap_ST);
		       Parallax    = _ParallaxEmission * _ParallaxColor.rgb;
		       Parallax   *= tex2D(_ParallaxMap  , ParallaxUV).rgb * tex2D(_ParallaxMap  , ParallaxUV).a * IN.peprm.x;
		       Parallax   *= tex2D(_ParallaxMap2 , IN.peuv   ).rgb * tex2D(_ParallaxMap2 , IN.peuv   ).a;

		if (_ParallaxLighting) {
			#ifdef PASS_FB
				Parallax   *= saturate(MonoColor(LightBase) + MonoColor(IN.shmax) + MonoColor(VLightBase));
			#endif
			#ifdef PASS_FA
				Parallax   *= saturate(MonoColor(LightBase));
			#endif
		}
	}

//-------------------------------------リフレクション
	float3 SpecularMask = (float3)0.0f;
	float3 Specular     = (float3)0.0f;
	float3 Reflection   = (float3)0.0f;
	float3 MatCapture   = (float3)0.0f;

	if (_ReflectionEnable) {

//----スペキュラ反射
		       SpecularMask = tex2D(_MetallicGlossMap , IN.uv).rgb * tex2D(_MetallicGlossMap , IN.uv).a;

		float3 RLSpecular   = SpecularCalc(Normal , IN.ldir , IN.view , _GlossMapScale) * LightBase;

		#ifdef PASS_FB
			float3 SHSpecular   = (float3)0.0f;
			if (_SpecularSH) {
			       SHSpecular   = SpecularCalc(Normal , IN.shdir , IN.view , _GlossMapScale) * IN.shmax;
			}
			       Specular     = (RLSpecular + SHSpecular) * _Specular * ((_GlossMapScale * _GlossMapScale * _GlossMapScale) + 0.25f);
		#endif
		#ifdef PASS_FA
			       Specular     =  RLSpecular               * _Specular * ((_GlossMapScale * _GlossMapScale * _GlossMapScale) + 0.25f);
		#endif

//----環境マッピング
		#ifdef PASS_FB
			       Reflection   = ReflectionCalc(Normal , IN.view , _GlossMapScale);

			if (_ReflectLit == 1) Reflection *= saturate(LightBase + VLightBase);
			if (_ReflectLit == 2) Reflection *= saturate(IN.shmax);
			if (_ReflectLit == 3) Reflection *= saturate(LightBase + IN.shmax + VLightBase);
		#endif
		#ifdef PASS_FA
			if ((_ReflectLit == 1) || (_ReflectLit == 3)) {
			       Reflection   = ReflectionCalc(Normal , IN.view , _GlossMapScale);
				   Reflection  *= saturate(LightBase);
			}
		#endif

//----マットキャップ
		#ifdef PASS_FB
			float2 MatCapUV     = float2(dot(IN.matcaph , Normal), dot(IN.matcapv , Normal)) * 0.5f + 0.5f;
			       MatCapture   = tex2D(_MatCap , MatCapUV).rgb * _MatCapStrength;

			if (_MatCapLit == 1) MatCapture *= saturate(LightBase + VLightBase);
			if (_MatCapLit == 2) MatCapture *= saturate(IN.shmax);
			if (_MatCapLit == 3) MatCapture *= saturate(LightBase + IN.shmax + VLightBase);
		#endif
		#ifdef PASS_FA
			if ((_MatCapLit  == 1) || (_MatCapLit  == 3)) {
				float2 MatCapUV    = float2(dot(IN.matcaph , Normal), dot(IN.matcapv , Normal)) * 0.5f + 0.5f;
				       MatCapture  = tex2D(_MatCap , MatCapUV).rgb * _MatCapStrength;
				       MatCapture *= saturate(LightBase);
			}
		#endif

		if (_SpecularTexColor ) Specular    *= Color;
		if (_MetallicTexColor ) Reflection  *= Color;
		if (_MatCapTexColor   ) MatCapture  *= Color;
	}

//-------------------------------------リムライティング
	float3 RimLight = (float3)0.0f;
	#ifdef PASS_FB
		if (_RimLitEnable) {
			       RimLight  = RimLightCalc(Normal , IN.view , _RimLit , _RimLitGradient);
			       RimLight *= _RimLitColor.rgb * _RimLitColor.a * tex2D(_RimLitMask , IN.uv).rgb;
			if (_RimLitLighthing) RimLight *= saturate(LightBase + IN.shmax + VLightBase);
			if (_RimLitTexColor ) RimLight *= Color;
		}
	#endif
	#ifdef PASS_FA
		if (_RimLitEnable && _RimLitLighthing) {
			       RimLight  = RimLightCalc(Normal , IN.view , _RimLit , _RimLitGradient);
			       RimLight *= _RimLitColor.rgb * _RimLitColor.a * tex2D(_RimLitMask , IN.uv).rgb;
			       RimLight *= saturate(LightBase);
			if (_RimLitTexColor ) RimLight *= Color;
		}
	#endif

//-------------------------------------最終カラー計算
	       OUT.rgb      = Color * Lighting;
	       OUT.rgb      = lerp(OUT.rgb , Color , _Unlit);
	       OUT.rgb      = lerp(OUT.rgb , Reflection , (_Metallic * SpecularMask)) + ((Specular + MatCapture) * SpecularMask);

//----リムライティング混合
	if (_RimLitEnable) {
		if (_RimLitMode == 0) OUT.rgb += RimLight;
		if (_RimLitMode == 1) OUT.rgb *= RimLight;
		if (_RimLitMode == 2) OUT.rgb  = saturate(OUT.rgb - RimLight);
	}

//----エミッション混合
	if (EmissionFlag) {

		float EmissionRev   = MonoColor(LightBase);
		#ifdef PASS_FB
			EmissionRev += MonoColor(IN.shmax) + MonoColor(VLightBase);
		#endif

		      EmissionRev   = 1.0f - pow(saturate(EmissionRev) , 0.44964029f);
		      EmissionRev   = saturate((EmissionRev - _EmissionInTheDark + 0.1f) * 10.0f);
		      Emission     *= EmissionRev;

		if (_EmissionMode == 0) OUT.rgb += Emission;
		if (_EmissionMode == 1) {
			OUT.rgb *= saturate(1.0f - Emission);
			OUT.rgb += (lerp(Color , Reflection , (_Metallic * SpecularMask)) + ((Specular + MatCapture) * SpecularMask)) * Emission;
		}
		if (_EmissionMode == 2) OUT.rgb  = saturate(OUT.rgb - Emission);
	}

//----視差エミッション混合
	if (ParallaxFlag) {

		float ParallaxRev   = MonoColor(LightBase);
		#ifdef PASS_FB
			ParallaxRev += MonoColor(IN.shmax) + MonoColor(VLightBase);
		#endif

		      ParallaxRev   = 1.0f - pow(saturate(ParallaxRev) , 0.44964029f);
		      ParallaxRev   = saturate((ParallaxRev - _ParallaxInTheDark + 0.1f) * 10.0f);
		      Parallax     *= ParallaxRev;

		if (_ParallaxMode == 0) OUT.rgb += Parallax;
		if (_ParallaxMode == 1) {
			OUT.rgb *= saturate(1.0f - Parallax);
			OUT.rgb += (lerp(Color , Reflection , (_Metallic * SpecularMask)) + ((Specular + MatCapture) * SpecularMask)) * Parallax;
		}
		if (_ParallaxMode == 2) OUT.rgb  = saturate(OUT.rgb - Parallax);
	}

//----オクルージョンマスク
	if (_OcclusionMode == 2) OUT.rgb *= lerp(1.0f , tex2D(_OcclusionMap  , IN.uv).rgb , _OcclusionStrength);

//----エミッションのテクスチャアルファ無視
	#ifdef TRANSPARENT

		if (EmissionFlag && _IgnoreTexAlphaE) {
			float EmissionAlpha    = MonoColor(Emission);
			OUT.a = saturate(OUT.a + EmissionAlpha  );
		}

//----視差エミッションのテクスチャアルファ無視
		if (ParallaxFlag && _IgnoreTexAlphaPE) {
			float ParallaxAlpha    = MonoColor(Parallax);
			OUT.a = saturate(OUT.a + ParallaxAlpha  );
		}

//----リフレクションのテクスチャアルファ無視
		if (_ReflectionEnable && _IgnoreTexAlphaR) {
			float ReflectionAlpha  = 0.0f;
			      ReflectionAlpha += MonoColor(Reflection) * _Metallic;
			      ReflectionAlpha += MonoColor(Specular);
			      ReflectionAlpha += MonoColor(MatCapture);
			      ReflectionAlpha *= SpecularMask;
			OUT.a = saturate(OUT.a + ReflectionAlpha);
		}

//----リムライトのテクスチャアルファ無視
		if (_RimLitEnable && _IgnoreTexAlphaRL) {
			float RimLightAlpha    = MonoColor(RimLight);
			OUT.a = saturate(OUT.a + RimLightAlpha  );
		}

	#endif

//-------------------------------------出力オプション
//----ガンマ修正
	if (_EnableGammaFix) {
		_GammaR = max(_GammaR , 0.00001f);
		_GammaG = max(_GammaG , 0.00001f);
		_GammaB = max(_GammaB , 0.00001f);

	       OUT.r    = pow(OUT.r , 1.0f / (1.0f / _GammaR));
	       OUT.g    = pow(OUT.g , 1.0f / (1.0f / _GammaG));
	       OUT.b    = pow(OUT.b , 1.0f / (1.0f / _GammaB));
	}

//----明度修正
	if (_EnableBlightFix) {
	       OUT.rgb *= _BlightOutput;
	       OUT.rgb  = max(OUT.rgb + _BlightOffset , 0.0f);
	}

//----出力リミッタ
	if (_LimitterEnable) {
	       OUT.rgb  = min(OUT.rgb , _LimitterMax);
	}

//-------------------------------------フォグ
	UNITY_APPLY_FOG(IN.fogCoord, OUT);


	return OUT;
}
