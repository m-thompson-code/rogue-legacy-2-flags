using System;
using System.Collections;

// Token: 0x02000367 RID: 871
public class Vampire_Trait : BaseTrait
{
	// Token: 0x17000DF9 RID: 3577
	// (get) Token: 0x060020B6 RID: 8374 RVA: 0x00066F2A File Offset: 0x0006512A
	public override TraitType TraitType
	{
		get
		{
			return TraitType.Vampire;
		}
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x00066F31 File Offset: 0x00065131
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

	// Token: 0x060020B8 RID: 8376 RVA: 0x00066F40 File Offset: 0x00065140
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

	// Token: 0x04001C6B RID: 7275
	private float m_healthDropTick = 1f;

	// Token: 0x04001C6C RID: 7276
	private WaitRL_Yield m_waitYield;
}
