using System;
using System.Collections;

// Token: 0x020005B0 RID: 1456
public class LowerGravity_Trait : BaseTrait
{
	// Token: 0x17001221 RID: 4641
	// (get) Token: 0x06002D86 RID: 11654 RVA: 0x00019144 File Offset: 0x00017344
	public override TraitType TraitType
	{
		get
		{
			return TraitType.LowerGravity;
		}
	}

	// Token: 0x17001222 RID: 4642
	// (get) Token: 0x06002D87 RID: 11655 RVA: 0x0001914B File Offset: 0x0001734B
	// (set) Token: 0x06002D88 RID: 11656 RVA: 0x00019153 File Offset: 0x00017353
	public bool LowerGravityApplied { get; set; }

	// Token: 0x06002D89 RID: 11657 RVA: 0x0001915C File Offset: 0x0001735C
	private IEnumerator Start()
	{
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		if (!this.LowerGravityApplied)
		{
			PlayerManager.GetPlayerController().AscentMultiplierOverride *= 1f;
			PlayerManager.GetPlayerController().FallMultiplierOverride *= 0.35f;
			this.LowerGravityApplied = true;
		}
		yield break;
	}

	// Token: 0x06002D8A RID: 11658 RVA: 0x000C7230 File Offset: 0x000C5430
	private void OnDestroy()
	{
		if (!PlayerManager.IsDisposed && this.LowerGravityApplied)
		{
			PlayerManager.GetPlayerController().AscentMultiplierOverride /= 1f;
			PlayerManager.GetPlayerController().FallMultiplierOverride /= 0.35f;
			this.LowerGravityApplied = false;
		}
	}
}
