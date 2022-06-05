using System;
using UnityEngine;

// Token: 0x020007D4 RID: 2004
public class RootObj : MonoBehaviour, IRootObj
{
	// Token: 0x06003DCA RID: 15818 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
