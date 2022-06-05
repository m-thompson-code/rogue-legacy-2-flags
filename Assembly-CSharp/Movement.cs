using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class Movement : MonoBehaviour
{
	// Token: 0x06000010 RID: 16 RVA: 0x00003AF1 File Offset: 0x00001CF1
	private void Update()
	{
		base.transform.position += this.movementVector * Time.deltaTime;
	}

	// Token: 0x0400000D RID: 13
	public Vector3 movementVector;
}
