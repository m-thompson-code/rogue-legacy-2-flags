using System;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public class Door_Prop : Prop, IMirror
{
	// Token: 0x060037FD RID: 14333 RVA: 0x000BF688 File Offset: 0x000BD888
	public override void Mirror()
	{
		float num = base.transform.rotation.eulerAngles.z;
		num *= -1f;
		Quaternion rotation = Quaternion.Euler(0f, 0f, num);
		base.transform.rotation = rotation;
	}
}
