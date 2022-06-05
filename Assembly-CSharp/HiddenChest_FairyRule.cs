using System;
using System.Collections;

// Token: 0x02000702 RID: 1794
public class HiddenChest_FairyRule : FairyRule
{
	// Token: 0x1700148B RID: 5259
	// (get) Token: 0x060036C6 RID: 14022 RVA: 0x0001E242 File Offset: 0x0001C442
	public override string Description
	{
		get
		{
			return "Hidden Chest";
		}
	}

	// Token: 0x1700148C RID: 5260
	// (get) Token: 0x060036C7 RID: 14023 RVA: 0x00006732 File Offset: 0x00004932
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.HiddenChest;
		}
	}

	// Token: 0x1700148D RID: 5261
	// (get) Token: 0x060036C8 RID: 14024 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x0001E249 File Offset: 0x0001C449
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.SetIsRuleActive(true);
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x000E4D00 File Offset: 0x000E2F00
	private void SetIsRuleActive(bool isRuleActive)
	{
		float opacity = 1f;
		float particleOpacity = 1f;
		bool forceDisable = true;
		if (isRuleActive)
		{
			opacity = 0.125f;
			particleOpacity = 0.0875f;
			forceDisable = false;
		}
		foreach (ChestSpawnController chestSpawnController in PlayerManager.GetCurrentPlayerRoom().SpawnControllerManager.ChestSpawnControllers)
		{
			if (chestSpawnController.ShouldSpawn)
			{
				ChestObj chestInstance = chestSpawnController.ChestInstance;
				chestInstance.SetOpacity(opacity);
				chestInstance.Interactable.ForceDisableInteractPrompt(forceDisable);
				base.StartCoroutine(this.SetParticleOpacity(chestInstance, particleOpacity));
			}
		}
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x0001E252 File Offset: 0x0001C452
	private IEnumerator SetParticleOpacity(ChestObj chest, float particleOpacity)
	{
		yield return null;
		chest.SetSparkleParticleOpacity(particleOpacity);
		yield break;
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x0001E268 File Offset: 0x0001C468
	public override void StopRule()
	{
		base.StopRule();
		this.SetIsRuleActive(false);
		base.StopAllCoroutines();
	}
}
