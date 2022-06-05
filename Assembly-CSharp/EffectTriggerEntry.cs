using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
[Serializable]
public class EffectTriggerEntry
{
	// Token: 0x04002B51 RID: 11089
	public EffectCategoryType EffectCategory;

	// Token: 0x04002B52 RID: 11090
	public string EffectTypeName = "";

	// Token: 0x04002B53 RID: 11091
	public EffectOffsetType OffsetType;

	// Token: 0x04002B54 RID: 11092
	public Vector2 PositionOffset;

	// Token: 0x04002B55 RID: 11093
	public bool DestroyOnRoomChange = true;

	// Token: 0x04002B56 RID: 11094
	public EffectTriggerDirection TriggerDirection = EffectTriggerDirection.Anywhere;

	// Token: 0x04002B57 RID: 11095
	public float LeftRotationZ;

	// Token: 0x04002B58 RID: 11096
	public bool LeftFlip;

	// Token: 0x04002B59 RID: 11097
	public float RightRotationZ;

	// Token: 0x04002B5A RID: 11098
	public bool RightFlip;

	// Token: 0x04002B5B RID: 11099
	public float UpRotationZ;

	// Token: 0x04002B5C RID: 11100
	public bool UpFlip;

	// Token: 0x04002B5D RID: 11101
	public float DownRotationZ;

	// Token: 0x04002B5E RID: 11102
	public bool DownFlip;

	// Token: 0x04002B5F RID: 11103
	public EffectTargetType DeriveFacing = EffectTargetType.Other;

	// Token: 0x04002B60 RID: 11104
	public EffectTargetType DeriveScale;

	// Token: 0x04002B61 RID: 11105
	public EffectTargetType DeriveRotation;

	// Token: 0x04002B62 RID: 11106
	public bool DeriveRotationFromHeading;

	// Token: 0x04002B63 RID: 11107
	public float BaseScaleToDerive = 1f;

	// Token: 0x04002B64 RID: 11108
	public EffectTargetType FollowTargetType;

	// Token: 0x04002B65 RID: 11109
	public EffectDurationType DurationType;

	// Token: 0x04002B66 RID: 11110
	[Range(0f, 1f)]
	public float StartingPosLerp;

	// Token: 0x04002B67 RID: 11111
	public float Duration;

	// Token: 0x04002B68 RID: 11112
	public int AnimatorLayer;

	// Token: 0x04002B69 RID: 11113
	public EffectStartType EffectStartType;

	// Token: 0x04002B6A RID: 11114
	public float NormalizedStartTime;

	// Token: 0x04002B6B RID: 11115
	public EffectStopType EffectStopType = EffectStopType.Gracefully;
}
