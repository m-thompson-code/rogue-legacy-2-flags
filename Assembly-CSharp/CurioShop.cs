using System;
using UnityEngine;

// Token: 0x02000902 RID: 2306
public class CurioShop : MonoBehaviour, IRootObj
{
	// Token: 0x06004613 RID: 17939 RVA: 0x0002682C File Offset: 0x00024A2C
	private void OnEnable()
	{
		this.UpdateFlip();
	}

	// Token: 0x06004614 RID: 17940 RVA: 0x00026834 File Offset: 0x00024A34
	private void Update()
	{
		if (PlayerManager.IsInstantiated)
		{
			this.UpdateFlip();
		}
	}

	// Token: 0x06004615 RID: 17941 RVA: 0x0011298C File Offset: 0x00110B8C
	private void UpdateFlip()
	{
		Component playerController = PlayerManager.GetPlayerController();
		bool flag = base.transform.localScale.x > 0f;
		bool flag2 = playerController.transform.localPosition.x > base.transform.localPosition.x;
		if ((flag && !flag2) || (!flag && flag2))
		{
			base.transform.SetLocalScaleX(base.transform.localScale.x * -1f);
		}
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x00003713 File Offset: 0x00001913
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
