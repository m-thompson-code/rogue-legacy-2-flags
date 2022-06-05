using System;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

// Token: 0x02000825 RID: 2085
public class UserReportingConfigureOnly : MonoBehaviour
{
	// Token: 0x060044F9 RID: 17657 RVA: 0x000F53CC File Offset: 0x000F35CC
	private void Start()
	{
		if (UnityUserReporting.CurrentClient == null)
		{
			UnityUserReporting.Configure();
		}
	}
}
