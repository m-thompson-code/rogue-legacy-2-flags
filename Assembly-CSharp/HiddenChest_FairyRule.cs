using System;
using System.Collections;

// Token: 0x0200043A RID: 1082
public class HiddenChest_FairyRule : FairyRule
{
	// Token: 0x17000FAA RID: 4010
	// (get) Token: 0x060027BA RID: 10170 RVA: 0x00084229 File Offset: 0x00082429
	public override string Description
	{
		get
		{
			return "Hidden Chest";
		}
	}

	// Token: 0x17000FAB RID: 4011
	// (get) Token: 0x060027BB RID: 10171 RVA: 0x00084230 File Offset: 0x00082430
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.HiddenChest;
		}
	}

	// Token: 0x17000FAC RID: 4012
	// (get) Token: 0x060027BC RID: 10172 RVA: 0x00084234 File Offset: 0x00082434
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x00084237 File Offset: 0x00082437
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.SetIsRuleActive(true);
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x00084240 File Offset: 0x00082440
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

	// Token: 0x060027BF RID: 10175 RVA: 0x000842CB File Offset: 0x000824CB
	private IEnumerator SetParticleOpacity(ChestObj chest, float particleOpacity)
	{
		yield return null;
		chest.SetSparkleParticleOpacity(particleOpacity);
		yield break;
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x000842E1 File Offset: 0x000824E1
	public override void StopRule()
	{
		base.StopRule();
		this.SetIsRuleActive(false);
		base.StopAllCoroutines();
	}
}
