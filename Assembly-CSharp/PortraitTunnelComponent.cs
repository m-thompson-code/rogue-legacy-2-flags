using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class PortraitTunnelComponent : MonoBehaviour, IRoomConsumer
{
	// Token: 0x17001094 RID: 4244
	// (get) Token: 0x06002B08 RID: 11016 RVA: 0x00091DB0 File Offset: 0x0008FFB0
	// (set) Token: 0x06002B09 RID: 11017 RVA: 0x00091DB8 File Offset: 0x0008FFB8
	public BaseRoom Room { get; private set; }

	// Token: 0x17001095 RID: 4245
	// (get) Token: 0x06002B0A RID: 11018 RVA: 0x00091DC4 File Offset: 0x0008FFC4
	public bool IsPortraitComplete
	{
		get
		{
			foreach (KeyValuePair<PlayerSaveFlag, Animator> keyValuePair in this.m_rootsTable)
			{
				if (!SaveManager.PlayerSaveData.GetFlag(keyValuePair.Key))
				{
					return false;
				}
			}
			return true;
		}
	}

	// Token: 0x06002B0B RID: 11019 RVA: 0x00091E2C File Offset: 0x0009002C
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x00091E3F File Offset: 0x0009003F
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x00091E66 File Offset: 0x00090066
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06002B0E RID: 11022 RVA: 0x00091E94 File Offset: 0x00090094
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		foreach (KeyValuePair<PlayerSaveFlag, Animator> keyValuePair in this.m_rootsTable)
		{
			this.UpdateBossDefeatedState(keyValuePair.Key);
		}
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x00091EF0 File Offset: 0x000900F0
	private void UpdateBossDefeatedState(PlayerSaveFlag bossDefeatedFlag)
	{
		this.SetBossDefeated(bossDefeatedFlag, SaveManager.PlayerSaveData.GetFlag(bossDefeatedFlag));
	}

	// Token: 0x06002B10 RID: 11024 RVA: 0x00091F04 File Offset: 0x00090104
	public void SetBossDefeated(PlayerSaveFlag playerFlag, bool defeated)
	{
		if (!this.m_rootsTable.ContainsKey(playerFlag))
		{
			return;
		}
		if (!this.m_portraitsTable.ContainsKey(playerFlag))
		{
			return;
		}
		if (playerFlag == PlayerSaveFlag.BridgeBoss_Defeated && this.m_bridgeBossPortraitRenderer)
		{
			if (BurdenManager.GetBurdenLevel(BurdenType.BridgeBossUp) > 0)
			{
				this.m_bridgeBossPortraitRenderer.sprite = this.m_bridgeBossClearPortraitSprite;
			}
			else
			{
				this.m_bridgeBossPortraitRenderer.sprite = this.m_bridgeBossMurkyPortraitSprite;
			}
		}
		Animator animator = this.m_rootsTable[playerFlag];
		Animator animator2 = this.m_portraitsTable[playerFlag];
		if (defeated)
		{
			animator2.Play("RevealedIdle");
			animator2.Update(1f);
			animator.Play("RetractedIdle");
			animator.Update(1f);
			return;
		}
		animator2.Play("Idle");
		animator2.Update(1f);
		animator.Play("Idle");
		animator.Update(1f);
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x00091FEB File Offset: 0x000901EB
	public IEnumerator BossDefeatedCoroutine(PlayerSaveFlag playerFlag)
	{
		this.SetBossDefeated(playerFlag, false);
		if (!this.m_rootsTable.ContainsKey(playerFlag))
		{
			yield break;
		}
		if (!this.m_portraitsTable.ContainsKey(playerFlag))
		{
			yield break;
		}
		Animator rootsAnimator = this.m_rootsTable[playerFlag];
		this.m_portraitsTable[playerFlag].SetTrigger("Reveal");
		this.m_waitYield.CreateNew(1f, false);
		yield return this.m_waitYield;
		rootsAnimator.SetTrigger("Retract");
		this.m_waitYield.CreateNew(3f, false);
		yield return this.m_waitYield;
		yield break;
	}

	// Token: 0x04002311 RID: 8977
	[SerializeField]
	private PlayerSaveFlagAnimatorDictionary m_rootsTable;

	// Token: 0x04002312 RID: 8978
	[SerializeField]
	private PlayerSaveFlagAnimatorDictionary m_portraitsTable;

	// Token: 0x04002313 RID: 8979
	[SerializeField]
	private Sprite m_bridgeBossMurkyPortraitSprite;

	// Token: 0x04002314 RID: 8980
	[SerializeField]
	private Sprite m_bridgeBossClearPortraitSprite;

	// Token: 0x04002315 RID: 8981
	[SerializeField]
	private SpriteRenderer m_bridgeBossPortraitRenderer;

	// Token: 0x04002316 RID: 8982
	private WaitRL_Yield m_waitYield;
}
