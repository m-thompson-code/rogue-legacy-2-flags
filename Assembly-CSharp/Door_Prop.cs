using System;
using UnityEngine;

// Token: 0x02000A27 RID: 2599
public class Door_Prop : Prop, IMirror
{
	// Token: 0x06004E9A RID: 20122 RVA: 0x0012DE44 File Offset: 0x0012C044
	public override void Mirror()
	{
		float num = base.transform.rotation.eulerAngles.z;
		num *= -1f;
		Quaternion rotation = Quaternion.Euler(0f, 0f, num);
		base.transform.rotation = rotation;
	}
}
