using System;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
public static class CDGTransformExtensions
{
	// Token: 0x060041FF RID: 16895 RVA: 0x000EB41C File Offset: 0x000E961C
	public static void SetPositionX(this Transform transform, float xPos)
	{
		Vector3 position = transform.position;
		position.x = xPos;
		transform.position = position;
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x000EB440 File Offset: 0x000E9640
	public static void SetPositionY(this Transform transform, float yPos)
	{
		Vector3 position = transform.position;
		position.y = yPos;
		transform.position = position;
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x000EB464 File Offset: 0x000E9664
	public static void SetPositionZ(this Transform transform, float zPos)
	{
		Vector3 position = transform.position;
		position.z = zPos;
		transform.position = position;
	}

	// Token: 0x06004202 RID: 16898 RVA: 0x000EB488 File Offset: 0x000E9688
	public static void SetLocalPositionX(this Transform transform, float xPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x = xPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06004203 RID: 16899 RVA: 0x000EB4AC File Offset: 0x000E96AC
	public static void SetLocalPositionY(this Transform transform, float yPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.y = yPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06004204 RID: 16900 RVA: 0x000EB4D0 File Offset: 0x000E96D0
	public static void SetLocalPositionZ(this Transform transform, float zPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.z = zPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06004205 RID: 16901 RVA: 0x000EB4F4 File Offset: 0x000E96F4
	public static void SetLocalEulerX(this Transform transform, float xRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.x = xRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06004206 RID: 16902 RVA: 0x000EB518 File Offset: 0x000E9718
	public static void SetLocalEulerY(this Transform transform, float yRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.y = yRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06004207 RID: 16903 RVA: 0x000EB53C File Offset: 0x000E973C
	public static void SetLocalEulerZ(this Transform transform, float zRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.z = zRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06004208 RID: 16904 RVA: 0x000EB560 File Offset: 0x000E9760
	public static void SetLocalScaleX(this Transform transform, float xScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.x = xScale;
		transform.localScale = localScale;
	}

	// Token: 0x06004209 RID: 16905 RVA: 0x000EB584 File Offset: 0x000E9784
	public static void SetLocalScaleY(this Transform transform, float yScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.y = yScale;
		transform.localScale = localScale;
	}

	// Token: 0x0600420A RID: 16906 RVA: 0x000EB5A8 File Offset: 0x000E97A8
	public static void SetLocalScaleZ(this Transform transform, float zScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.z = zScale;
		transform.localScale = localScale;
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x000EB5CC File Offset: 0x000E97CC
	public static void SetLocalRotationX(this Transform transform, float xRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.x = xRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x0600420C RID: 16908 RVA: 0x000EB5F0 File Offset: 0x000E97F0
	public static void SetLocalRotationY(this Transform transform, float yRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.y = yRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x0600420D RID: 16909 RVA: 0x000EB614 File Offset: 0x000E9814
	public static void SetLocalRotationZ(this Transform transform, float zRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.z = zRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x000EB638 File Offset: 0x000E9838
	public static void SetLocalRotationW(this Transform transform, float wRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.w = wRotation;
		transform.localRotation = localRotation;
	}
}
