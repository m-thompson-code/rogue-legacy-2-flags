using System;
using UnityEngine;

// Token: 0x02000770 RID: 1904
public class ShaderID_RL
{
	// Token: 0x0600414A RID: 16714 RVA: 0x000E81CC File Offset: 0x000E63CC
	public static int GetShaderID(ShaderID shaderID)
	{
		switch (shaderID)
		{
		case ShaderID.AlphaBlendColor:
			return ShaderID_RL._AlphaBlendColor;
		case ShaderID.RimLightColor:
			return ShaderID_RL._RimLightColor;
		case ShaderID.MultiplyColor:
			return ShaderID_RL._MultiplyColor;
		case ShaderID.AddColor:
			return ShaderID_RL._AddColor;
		case ShaderID.MainColor:
			return ShaderID_RL._MainColor;
		case ShaderID.ShieldToggle:
			return ShaderID_RL._ShieldToggle;
		case ShaderID.Dissolve:
			return ShaderID_RL._Dissolve;
		case ShaderID.Opacity:
			return ShaderID_RL._Opacity;
		case ShaderID.RimBias:
			return ShaderID_RL._RimBias;
		case ShaderID.RimScale:
			return ShaderID_RL._RimScale;
		case ShaderID.ArmorColor:
			return ShaderID_RL._ArmorColor;
		case ShaderID.CapeColor:
			return ShaderID_RL._CapeColor;
		case ShaderID.HelmetColor:
			return ShaderID_RL._HelmetColor;
		case ShaderID.OutlineScale:
			return ShaderID_RL._OutlineScale;
		default:
			return -1;
		}
	}

	// Token: 0x040036A1 RID: 13985
	public static readonly int _AlphaBlendColor = Shader.PropertyToID("_AlphaBlendColor");

	// Token: 0x040036A2 RID: 13986
	public static readonly int _RimLightColor = Shader.PropertyToID("_RimLightColor");

	// Token: 0x040036A3 RID: 13987
	public static readonly int _MultiplyColor = Shader.PropertyToID("_MultiplyColor");

	// Token: 0x040036A4 RID: 13988
	public static readonly int _AddColor = Shader.PropertyToID("_AddColor");

	// Token: 0x040036A5 RID: 13989
	public static readonly int _MainColor = Shader.PropertyToID("_MainColor");

	// Token: 0x040036A6 RID: 13990
	public static readonly int _ShieldToggle = Shader.PropertyToID("_ShieldToggle");

	// Token: 0x040036A7 RID: 13991
	public static readonly int _Dissolve = Shader.PropertyToID("_Dissolve");

	// Token: 0x040036A8 RID: 13992
	public static readonly int _Opacity = Shader.PropertyToID("_Opacity");

	// Token: 0x040036A9 RID: 13993
	public static readonly int _RimBias = Shader.PropertyToID("_RimBias");

	// Token: 0x040036AA RID: 13994
	public static readonly int _RimScale = Shader.PropertyToID("_RimScale");

	// Token: 0x040036AB RID: 13995
	public static readonly int _ArmorColor = Shader.PropertyToID("_ArmorColor");

	// Token: 0x040036AC RID: 13996
	public static readonly int _CapeColor = Shader.PropertyToID("_CapeColor");

	// Token: 0x040036AD RID: 13997
	public static readonly int _HelmetColor = Shader.PropertyToID("_HelmetColor");

	// Token: 0x040036AE RID: 13998
	public static readonly int _OutlineScale = Shader.PropertyToID("_OutlineScale");
}
