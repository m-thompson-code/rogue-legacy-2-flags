using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class Movement : MonoBehaviour
{
	// Token: 0x06000011 RID: 17 RVA: 0x00002B57 File Offset: 0x00000D57
	private void Update()
	{
		base.transform.position += this.movementVector * Time.deltaTime;
	}

	// Token: 0x0400000F RID: 15
	public Vector3 movementVector;
}
