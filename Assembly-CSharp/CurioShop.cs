using System;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class CurioShop : MonoBehaviour, IRootObj
{
	// Token: 0x060031E1 RID: 12769 RVA: 0x000A8AAB File Offset: 0x000A6CAB
	private void OnEnable()
	{
		this.UpdateFlip();
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x000A8AB3 File Offset: 0x000A6CB3
	private void Update()
	{
		if (PlayerManager.IsInstantiated)
		{
			this.UpdateFlip();
		}
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x000A8AC4 File Offset: 0x000A6CC4
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

	// Token: 0x060031E5 RID: 12773 RVA: 0x000A8B45 File Offset: 0x000A6D45
	GameObject IRootObj.get_gameObject()
	{
		return base.gameObject;
	}
}
