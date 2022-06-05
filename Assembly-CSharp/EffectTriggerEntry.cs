using System;
using UnityEngine;

// Token: 0x0200040E RID: 1038
[Serializable]
public class EffectTriggerEntry
{
	// Token: 0x0400206E RID: 8302
	public EffectCategoryType EffectCategory;

	// Token: 0x0400206F RID: 8303
	public string EffectTypeName = "";

	// Token: 0x04002070 RID: 8304
	public EffectOffsetType OffsetType;

	// Token: 0x04002071 RID: 8305
	public Vector2 PositionOffset;

	// Token: 0x04002072 RID: 8306
	public bool DestroyOnRoomChange = true;

	// Token: 0x04002073 RID: 8307
	public EffectTriggerDirection TriggerDirection = EffectTriggerDirection.Anywhere;

	// Token: 0x04002074 RID: 8308
	public float LeftRotationZ;

	// Token: 0x04002075 RID: 8309
	public bool LeftFlip;

	// Token: 0x04002076 RID: 8310
	public float RightRotationZ;

	// Token: 0x04002077 RID: 8311
	public bool RightFlip;

	// Token: 0x04002078 RID: 8312
	public float UpRotationZ;

	// Token: 0x04002079 RID: 8313
	public bool UpFlip;

	// Token: 0x0400207A RID: 8314
	public float DownRotationZ;

	// Token: 0x0400207B RID: 8315
	public bool DownFlip;

	// Token: 0x0400207C RID: 8316
	public EffectTargetType DeriveFacing = EffectTargetType.Other;

	// Token: 0x0400207D RID: 8317
	public EffectTargetType DeriveScale;

	// Token: 0x0400207E RID: 8318
	public EffectTargetType DeriveRotation;

	// Token: 0x0400207F RID: 8319
	public bool DeriveRotationFromHeading;

	// Token: 0x04002080 RID: 8320
	public float BaseScaleToDerive = 1f;

	// Token: 0x04002081 RID: 8321
	public EffectTargetType FollowTargetType;

	// Token: 0x04002082 RID: 8322
	public EffectDurationType DurationType;

	// Token: 0x04002083 RID: 8323
	[Range(0f, 1f)]
	public float StartingPosLerp;

	// Token: 0x04002084 RID: 8324
	public float Duration;

	// Token: 0x04002085 RID: 8325
	public int AnimatorLayer;

	// Token: 0x04002086 RID: 8326
	public EffectStartType EffectStartType;

	// Token: 0x04002087 RID: 8327
	public float NormalizedStartTime;

	// Token: 0x04002088 RID: 8328
	public EffectStopType EffectStopType = EffectStopType.Gracefully;
}
