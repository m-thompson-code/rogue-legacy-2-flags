using System;
using RLAudio;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
public class EggplantTriggerController : MonoBehaviour
{
	// Token: 0x0600364F RID: 13903 RVA: 0x0001DD85 File Offset: 0x0001BF85
	private void Awake()
	{
		this.m_enemyController = base.GetComponent<EnemyController>();
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x0001DDB1 File Offset: 0x0001BFB1
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x0001DDBF File Offset: 0x0001BFBF
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x000E3BF4 File Offset: 0x000E1DF4
	private void OnPlayerEnterRoom(object sender, EventArgs args)
	{
		PlayerSaveFlag eggplantFlag = this.GetEggplantFlag();
		bool flag = false;
		if (eggplantFlag != PlayerSaveFlag.None)
		{
			flag = !SaveManager.PlayerSaveData.GetFlag(eggplantFlag);
		}
		if (flag)
		{
			this.m_interactable.SetIsInteractableActive(true);
			if (SaveManager.PlayerSaveData.InHubTown)
			{
				this.m_enemyController.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.m_interactable.SetIsInteractableActive(false);
			if (!SaveManager.PlayerSaveData.InHubTown)
			{
				this.m_enemyController.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x000E3C74 File Offset: 0x000E1E74
	private PlayerSaveFlag GetEggplantFlag()
	{
		switch (this.m_enemyController.EnemyRank)
		{
		case EnemyRank.Basic:
			return PlayerSaveFlag.FoundEggplant_Basic;
		case EnemyRank.Advanced:
			return PlayerSaveFlag.FoundEggplant_Advanced;
		case EnemyRank.Expert:
			return PlayerSaveFlag.FoundEggplant_Expert;
		case EnemyRank.Miniboss:
			return PlayerSaveFlag.FoundEggplant_Miniboss;
		default:
			return PlayerSaveFlag.None;
		}
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x000E3CC0 File Offset: 0x000E1EC0
	public void CollectEggplant()
	{
		PlayerSaveFlag eggplantFlag = this.GetEggplantFlag();
		if (eggplantFlag != PlayerSaveFlag.None)
		{
			AudioManager.PlayOneShot(null, "event:/Cut_Scenes/sfx_cutscene_ngPlusTeleport_playerAppear", base.gameObject.transform.position);
			AudioManager.PlayOneShot(null, "event:/SFX/Enemies/sfx_npc_eggplant_vanish", base.gameObject.transform.position);
			this.m_enemyController.gameObject.SetActive(false);
			SaveManager.PlayerSaveData.SetFlag(eggplantFlag, true);
			SaveManager.SaveCurrentProfileGameData(SaveDataType.Player, SavingType.FileOnly, true, null);
		}
	}

	// Token: 0x04002C1C RID: 11292
	private Interactable m_interactable;

	// Token: 0x04002C1D RID: 11293
	private EnemyController m_enemyController;

	// Token: 0x04002C1E RID: 11294
	private Action<object, EventArgs> m_onPlayerEnterRoom;
}
