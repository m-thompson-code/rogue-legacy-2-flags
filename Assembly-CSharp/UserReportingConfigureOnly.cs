using System;
using Unity.Cloud.UserReporting.Plugin;
using UnityEngine;

// Token: 0x02000CFE RID: 3326
public class UserReportingConfigureOnly : MonoBehaviour
{
	// Token: 0x06005ED1 RID: 24273 RVA: 0x00034457 File Offset: 0x00032657
	private void Start()
	{
		if (UnityUserReporting.CurrentClient == null)
		{
			UnityUserReporting.Configure();
		}
	}
}
