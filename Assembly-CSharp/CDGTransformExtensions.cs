using System;
using UnityEngine;

// Token: 0x02000C66 RID: 3174
public static class CDGTransformExtensions
{
	// Token: 0x06005B88 RID: 23432 RVA: 0x0015A55C File Offset: 0x0015875C
	public static void SetPositionX(this Transform transform, float xPos)
	{
		Vector3 position = transform.position;
		position.x = xPos;
		transform.position = position;
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x0015A580 File Offset: 0x00158780
	public static void SetPositionY(this Transform transform, float yPos)
	{
		Vector3 position = transform.position;
		position.y = yPos;
		transform.position = position;
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x0015A5A4 File Offset: 0x001587A4
	public static void SetPositionZ(this Transform transform, float zPos)
	{
		Vector3 position = transform.position;
		position.z = zPos;
		transform.position = position;
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x0015A5C8 File Offset: 0x001587C8
	public static void SetLocalPositionX(this Transform transform, float xPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.x = xPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x0015A5EC File Offset: 0x001587EC
	public static void SetLocalPositionY(this Transform transform, float yPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.y = yPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x0015A610 File Offset: 0x00158810
	public static void SetLocalPositionZ(this Transform transform, float zPos)
	{
		Vector3 localPosition = transform.localPosition;
		localPosition.z = zPos;
		transform.localPosition = localPosition;
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x0015A634 File Offset: 0x00158834
	public static void SetLocalEulerX(this Transform transform, float xRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.x = xRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06005B8F RID: 23439 RVA: 0x0015A658 File Offset: 0x00158858
	public static void SetLocalEulerY(this Transform transform, float yRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.y = yRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06005B90 RID: 23440 RVA: 0x0015A67C File Offset: 0x0015887C
	public static void SetLocalEulerZ(this Transform transform, float zRot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		localEulerAngles.z = zRot;
		transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x06005B91 RID: 23441 RVA: 0x0015A6A0 File Offset: 0x001588A0
	public static void SetLocalScaleX(this Transform transform, float xScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.x = xScale;
		transform.localScale = localScale;
	}

	// Token: 0x06005B92 RID: 23442 RVA: 0x0015A6C4 File Offset: 0x001588C4
	public static void SetLocalScaleY(this Transform transform, float yScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.y = yScale;
		transform.localScale = localScale;
	}

	// Token: 0x06005B93 RID: 23443 RVA: 0x0015A6E8 File Offset: 0x001588E8
	public static void SetLocalScaleZ(this Transform transform, float zScale)
	{
		Vector3 localScale = transform.localScale;
		localScale.z = zScale;
		transform.localScale = localScale;
	}

	// Token: 0x06005B94 RID: 23444 RVA: 0x0015A70C File Offset: 0x0015890C
	public static void SetLocalRotationX(this Transform transform, float xRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.x = xRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x06005B95 RID: 23445 RVA: 0x0015A730 File Offset: 0x00158930
	public static void SetLocalRotationY(this Transform transform, float yRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.y = yRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x06005B96 RID: 23446 RVA: 0x0015A754 File Offset: 0x00158954
	public static void SetLocalRotationZ(this Transform transform, float zRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.z = zRotation;
		transform.localRotation = localRotation;
	}

	// Token: 0x06005B97 RID: 23447 RVA: 0x0015A778 File Offset: 0x00158978
	public static void SetLocalRotationW(this Transform transform, float wRotation)
	{
		Quaternion localRotation = transform.localRotation;
		localRotation.w = wRotation;
		transform.localRotation = localRotation;
	}
}
