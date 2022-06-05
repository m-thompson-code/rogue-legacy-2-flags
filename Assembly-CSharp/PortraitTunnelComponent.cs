using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class PortraitTunnelComponent : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170015D7 RID: 5591
	// (get) Token: 0x06003B45 RID: 15173 RVA: 0x00020847 File Offset: 0x0001EA47
	// (set) Token: 0x06003B46 RID: 15174 RVA: 0x0002084F File Offset: 0x0001EA4F
	public BaseRoom Room { get; private set; }

	// Token: 0x170015D8 RID: 5592
	// (get) Token: 0x06003B47 RID: 15175 RVA: 0x000F381C File Offset: 0x000F1A1C
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

	// Token: 0x06003B48 RID: 15176 RVA: 0x00020858 File Offset: 0x0001EA58
	private void Awake()
	{
		this.m_waitYield = new WaitRL_Yield(0f, true);
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x0002086B File Offset: 0x0001EA6B
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x00020892 File Offset: 0x0001EA92
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x000F3884 File Offset: 0x000F1A84
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		foreach (KeyValuePair<PlayerSaveFlag, Animator> keyValuePair in this.m_rootsTable)
		{
			this.UpdateBossDefeatedState(keyValuePair.Key);
		}
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x000208BE File Offset: 0x0001EABE
	private void UpdateBossDefeatedState(PlayerSaveFlag bossDefeatedFlag)
	{
		this.SetBossDefeated(bossDefeatedFlag, SaveManager.PlayerSaveData.GetFlag(bossDefeatedFlag));
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x000F38E0 File Offset: 0x000F1AE0
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

	// Token: 0x06003B4E RID: 15182 RVA: 0x000208D2 File Offset: 0x0001EAD2
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

	// Token: 0x04002F1C RID: 12060
	[SerializeField]
	private PlayerSaveFlagAnimatorDictionary m_rootsTable;

	// Token: 0x04002F1D RID: 12061
	[SerializeField]
	private PlayerSaveFlagAnimatorDictionary m_portraitsTable;

	// Token: 0x04002F1E RID: 12062
	[SerializeField]
	private Sprite m_bridgeBossMurkyPortraitSprite;

	// Token: 0x04002F1F RID: 12063
	[SerializeField]
	private Sprite m_bridgeBossClearPortraitSprite;

	// Token: 0x04002F20 RID: 12064
	[SerializeField]
	private SpriteRenderer m_bridgeBossPortraitRenderer;

	// Token: 0x04002F21 RID: 12065
	private WaitRL_Yield m_waitYield;
}
