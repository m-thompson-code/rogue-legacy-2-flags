using System;
using System.Collections;

// Token: 0x020005DF RID: 1503
public class Vampire_Trait : BaseTrait
{
	// Token: 0x17001268 RID: 4712
	// (get) Token: 0x06002E5B RID: 11867 RVA: 0x00019598 File Offset: 0x00017798
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Vampire;
		}
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x0001959F File Offset: 0x0001779F
	private IEnumerator Start()
	{
		this.m_waitYield = new WaitRL_Yield(0f, false);
		while (!PlayerManager.IsInstantiated)
		{
			yield return null;
		}
		PlayerManager.GetPlayerController().LookController.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		yield break;
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x000195AE File Offset: 0x000177AE
	private IEnumerator VampirismCoroutine(PlayerController playerController)
	{
		for (;;)
		{
			this.m_waitYield.CreateNew(this.m_healthDropTick, false);
			yield return this.m_waitYield;
			if (playerController.CurrentHealth <= 1f)
			{
				break;
			}
			playerController.SetHealth(-1f, true, true);
		}
		yield break;
	}

	// Token: 0x0400260D RID: 9741
	private float m_healthDropTick = 1f;

	// Token: 0x0400260E RID: 9742
	private WaitRL_Yield m_waitYield;
}
