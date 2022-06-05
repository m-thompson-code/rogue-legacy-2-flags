using System;
using UnityEngine;

// Token: 0x02000C31 RID: 3121
public class ShaderID_RL
{
	// Token: 0x06005AC7 RID: 23239 RVA: 0x00157728 File Offset: 0x00155928
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

	// Token: 0x04004951 RID: 18769
	public static readonly int _AlphaBlendColor = Shader.PropertyToID("_AlphaBlendColor");

	// Token: 0x04004952 RID: 18770
	public static readonly int _RimLightColor = Shader.PropertyToID("_RimLightColor");

	// Token: 0x04004953 RID: 18771
	public static readonly int _MultiplyColor = Shader.PropertyToID("_MultiplyColor");

	// Token: 0x04004954 RID: 18772
	public static readonly int _AddColor = Shader.PropertyToID("_AddColor");

	// Token: 0x04004955 RID: 18773
	public static readonly int _MainColor = Shader.PropertyToID("_MainColor");

	// Token: 0x04004956 RID: 18774
	public static readonly int _ShieldToggle = Shader.PropertyToID("_ShieldToggle");

	// Token: 0x04004957 RID: 18775
	public static readonly int _Dissolve = Shader.PropertyToID("_Dissolve");

	// Token: 0x04004958 RID: 18776
	public static readonly int _Opacity = Shader.PropertyToID("_Opacity");

	// Token: 0x04004959 RID: 18777
	public static readonly int _RimBias = Shader.PropertyToID("_RimBias");

	// Token: 0x0400495A RID: 18778
	public static readonly int _RimScale = Shader.PropertyToID("_RimScale");

	// Token: 0x0400495B RID: 18779
	public static readonly int _ArmorColor = Shader.PropertyToID("_ArmorColor");

	// Token: 0x0400495C RID: 18780
	public static readonly int _CapeColor = Shader.PropertyToID("_CapeColor");

	// Token: 0x0400495D RID: 18781
	public static readonly int _HelmetColor = Shader.PropertyToID("_HelmetColor");

	// Token: 0x0400495E RID: 18782
	public static readonly int _OutlineScale = Shader.PropertyToID("_OutlineScale");
}
