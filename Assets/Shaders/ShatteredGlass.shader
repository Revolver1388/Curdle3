// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShatteredGlass"
{
	Properties
	{
		_Clean("Clean", 2D) = "white" {}
		_Broken("Broken", 2D) = "white" {}
		_TilingX("TilingX", Range( 0 , 10)) = 2.225986
		_TilingY("TilingY", Range( 0 , 10)) = 2.372235
		_Control("Control", Range( -1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Clean;
		uniform float4 _Clean_ST;
		uniform sampler2D _Broken;
		uniform float4 _Broken_ST;
		uniform float _TilingX;
		uniform float _TilingY;
		uniform float _Control;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Clean = i.uv_texcoord * _Clean_ST.xy + _Clean_ST.zw;
			float2 uv_Broken = i.uv_texcoord * _Broken_ST.xy + _Broken_ST.zw;
			float4 appendResult30 = (float4(_TilingX , _TilingY , 0.0 , 0.0));
			float2 uv_TexCoord31 = i.uv_texcoord * appendResult30.xy;
			float simplePerlin2D27 = snoise( uv_TexCoord31 );
			float clampResult34 = clamp( simplePerlin2D27 , 0.0 , 1.0 );
			float clampResult36 = clamp( ( clampResult34 + _Control ) , 0.0 , 1.0 );
			float4 lerpResult4 = lerp( tex2D( _Clean, uv_Clean ) , tex2D( _Broken, uv_Broken ) , clampResult36);
			o.Albedo = lerpResult4.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
99;68;1219;759;4601.076;-736.2188;3.422986;True;False
Node;AmplifyShaderEditor.RangedFloatNode;29;-2935.779,1846.162;Float;False;Property;_TilingY;TilingY;3;0;Create;True;0;0;False;0;2.372235;2.372235;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-2936.316,1601.607;Float;False;Property;_TilingX;TilingX;2;0;Create;True;0;0;False;0;2.225986;2.225986;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;30;-2600.18,1709.458;Float;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-2350.324,1687.887;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.NoiseGeneratorNode;27;-2078.301,1683.116;Float;False;Simplex2D;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;34;-1848.419,1687.839;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1889.472,1955.196;Float;False;Property;_Control;Control;4;0;Create;True;0;0;False;0;0;0.2109145;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;-2480.641,-233.9123;Float;True;Property;_Clean;Clean;0;0;Create;True;0;0;False;0;None;84508b93f15f2b64386ec07486afc7a3;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TexturePropertyNode;2;-2485.061,-4.617373;Float;True;Property;_Broken;Broken;1;0;Create;True;0;0;False;0;None;5798ded558355430c8a9b13ee12a847c;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-1515.719,1687.839;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-2212.853,-232.4843;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-2212.854,-6.876418;Float;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;36;-1240.955,1694.975;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;4;-886.16,-6.515316;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-335.5484,-4.692984;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ShatteredGlass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;False;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;28;0
WireConnection;30;1;29;0
WireConnection;31;0;30;0
WireConnection;27;0;31;0
WireConnection;34;0;27;0
WireConnection;32;0;34;0
WireConnection;32;1;33;0
WireConnection;6;0;1;0
WireConnection;7;0;2;0
WireConnection;36;0;32;0
WireConnection;4;0;6;0
WireConnection;4;1;7;0
WireConnection;4;2;36;0
WireConnection;0;0;4;0
ASEEND*/
//CHKSM=6FF658B6BDDF3D92EECFC2C2A431AD8985399184