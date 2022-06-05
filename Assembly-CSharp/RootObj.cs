using System;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class RootObj : MonoBehaviour, IRootObj
{
	// Token: 0x06002D1E RID: 11550 RVA: 0x00098F07 File Offset: 0x00097107
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
