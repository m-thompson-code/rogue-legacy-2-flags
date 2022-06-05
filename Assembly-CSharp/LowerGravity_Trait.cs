using System;
using System.Collections;

// Token: 0x0200034A RID: 842
public class LowerGravity_Trait : BaseTrait
{
	// Token: 0x17000DCC RID: 3532
	// (get) Token: 0x0600203E RID: 8254 RVA: 0x000665ED File Offset: 0x000647ED
	public override TraitType TraitType
	{
		get
		{
			return TraitType.LowerGravity;
		}
	}

	// Token: 0x17000DCD RID: 3533
	// (get) Token: 0x0600203F RID: 8255 RVA: 0x000665F4 File Offset: 0x000647F4
	// (set) Token: 0x06002040 RID: 8256 RVA: 0x000665FC File Offset: 0x000647FC
	public bool LowerGravityApplied { get; set; }

	// Token: 0x06002041 RID: 8257 RVA: 0x00066605 File Offset: 0x00064805
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

	// Token: 0x06002042 RID: 8258 RVA: 0x00066614 File Offset: 0x00064814
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
