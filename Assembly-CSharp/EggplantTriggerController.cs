using System;
using RLAudio;
using UnityEngine;

// Token: 0x0200042B RID: 1067
public class EggplantTriggerController : MonoBehaviour
{
	// Token: 0x0600274C RID: 10060 RVA: 0x00082E0B File Offset: 0x0008100B
	private void Awake()
	{
		this.m_enemyController = base.GetComponent<EnemyController>();
		this.m_interactable = base.GetComponent<Interactable>();
		this.m_onPlayerEnterRoom = new Action<object, EventArgs>(this.OnPlayerEnterRoom);
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x00082E37 File Offset: 0x00081037
	private void OnEnable()
	{
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x00082E45 File Offset: 0x00081045
	private void OnDisable()
	{
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerEnterRoom, this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x00082E54 File Offset: 0x00081054
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

	// Token: 0x06002750 RID: 10064 RVA: 0x00082ED4 File Offset: 0x000810D4
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

	// Token: 0x06002751 RID: 10065 RVA: 0x00082F20 File Offset: 0x00081120
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

	// Token: 0x040020F9 RID: 8441
	private Interactable m_interactable;

	// Token: 0x040020FA RID: 8442
	private EnemyController m_enemyController;

	// Token: 0x040020FB RID: 8443
	private Action<object, EventArgs> m_onPlayerEnterRoom;
}
