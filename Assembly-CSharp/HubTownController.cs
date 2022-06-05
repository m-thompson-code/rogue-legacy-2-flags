using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200090F RID: 2319
public class HubTownController : MonoBehaviour
{
	// Token: 0x0600466B RID: 18027 RVA: 0x00113D9C File Offset: 0x00111F9C
	private void Awake()
	{
		this.m_room = base.GetComponent<Room>();
		this.m_hubtownEnteredArgs = new HubTownEnteredEventArgs(this);
		this.m_traitsDisplayHUDArgs = new ObjectiveCompleteHUDEventArgs(ObjectiveCompleteHUDType.Traits, 5f, null, null, null);
		this.m_skillTreeWindowClosed = new Action<MonoBehaviour, EventArgs>(this.SkillTreeWindowClosed);
		this.m_onExitHubTown = new Action<MonoBehaviour, EventArgs>(this.OnExitHubTown);
		this.m_onPlayerEnterRoom = new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom);
		this.m_onBounce = new Action<MonoBehaviour, EventArgs>(this.OnBounce);
	}

	// Token: 0x0600466C RID: 18028 RVA: 0x00113E20 File Offset: 0x00112020
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeBounce, this.m_onBounce);
		this.m_room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
	}

	// Token: 0x0600466D RID: 18029 RVA: 0x00026AD2 File Offset: 0x00024CD2
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeBounce, this.m_onBounce);
		this.m_room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600466E RID: 18030 RVA: 0x00113E6C File Offset: 0x0011206C
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs e)
	{
		SaveManager.PlayerSaveData.InCastle = false;
		SaveManager.PlayerSaveData.InHubTown = true;
		if (this.m_refreshTownCoroutine == null)
		{
			this.m_refreshTownCoroutine = base.StartCoroutine(this.RefreshTown());
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Refresh Town Coroutine is already running, but should not be.</color>", new object[]
		{
			this
		});
	}

	// Token: 0x0600466F RID: 18031 RVA: 0x00026B12 File Offset: 0x00024D12
	private void SkillTreeWindowClosed(object sender, EventArgs eventArgs)
	{
		if (this.m_refreshTownCoroutine == null)
		{
			this.m_refreshTownCoroutine = base.StartCoroutine(this.RefreshTown());
			return;
		}
		Debug.LogFormat("<color=red>| {0} | Refresh Town Coroutine is already running, but should not be.</color>", new object[]
		{
			this
		});
	}

	// Token: 0x06004670 RID: 18032 RVA: 0x00026B43 File Offset: 0x00024D43
	private IEnumerator RefreshTown()
	{
		if (PlayerManager.IsInstantiated)
		{
			PlayerManager.GetPlayerController().ResetCharacter();
		}
		if (this.m_firstTimeEntering)
		{
			while (!PlayerManager.IsInstantiated)
			{
				yield return null;
			}
			BaseRoom room = PlayerManager.GetCurrentPlayerRoom();
			while (!room.CinemachineCamera.IsActiveVirtualCamera)
			{
				yield return null;
			}
			if (!SaveManager.PlayerSaveData.FirstTimeGoldReceived && SaveManager.PlayerSaveData.GoldCollected < 200 && SaveManager.PlayerSaveData.TimesDied <= 1)
			{
				SaveManager.PlayerSaveData.FirstTimeGoldReceived = true;
				SaveManager.PlayerSaveData.GoldCollected = 200;
			}
			if (!SaveManager.PlayerSaveData.GoldilocksReceived)
			{
				this.CalculateGoldilocksLimit();
				SaveManager.PlayerSaveData.GoldilocksReceived = true;
			}
			this.m_skillTree.OpenSkillTree(false);
			room = null;
		}
		else
		{
			if (!this.m_traitsDisplayed)
			{
				this.m_traitsDisplayed = true;
				if (TraitLibrary.GetTraitData(SaveManager.PlayerSaveData.CurrentCharacter.TraitOne) != null || TraitLibrary.GetTraitData(SaveManager.PlayerSaveData.CurrentCharacter.TraitTwo) != null)
				{
					Messenger<UIMessenger, UIEvent>.Broadcast(UIEvent.DisplayObjectiveCompleteHUD, this, this.m_traitsDisplayHUDArgs);
				}
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterHubTown, this, this.m_hubtownEnteredArgs);
		}
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldSavedChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.GoldChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.RuneOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.EquipmentOreChanged, this, null);
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.SoulChanged, this, null);
		this.m_firstTimeEntering = false;
		this.m_refreshTownCoroutine = null;
		yield break;
	}

	// Token: 0x06004671 RID: 18033 RVA: 0x00113EC0 File Offset: 0x001120C0
	private void OnExitHubTown(MonoBehaviour sender, EventArgs eventArgs)
	{
		if (SaveManager.PlayerSaveData.CastleLockState != CastleLockState.NotLocked)
		{
			PlayerSaveData playerSaveData = SaveManager.PlayerSaveData;
			playerSaveData.TimesCastleLocked += 1;
		}
		else
		{
			SaveManager.PlayerSaveData.TimesCastleLocked = 0;
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.ResetHealth();
		playerController.ResetMana();
		playerController.ResetAllAbilityAmmo();
		playerController.ResetAbilityCooldowns();
	}

	// Token: 0x06004672 RID: 18034 RVA: 0x00113F18 File Offset: 0x00112118
	private void CalculateGoldilocksLimit()
	{
		int num = int.MaxValue;
		SkillTreeType skillTreeType = SkillTreeType.None;
		foreach (SkillTreeType skillTreeType2 in SkillTreeType_RL.TypeArray)
		{
			SkillTreeObj skillTreeObj = SkillTreeManager.GetSkillTreeObj(skillTreeType2);
			if (skillTreeObj != null && !skillTreeObj.IsLevelLocked && !skillTreeObj.IsLocked && !skillTreeObj.IsSoulLocked)
			{
				int goldCostWithLevelAppreciation = skillTreeObj.GoldCostWithLevelAppreciation;
				if (goldCostWithLevelAppreciation < num)
				{
					num = goldCostWithLevelAppreciation;
					skillTreeType = skillTreeType2;
				}
			}
		}
		int goldCollectedIncludingBank = SaveManager.PlayerSaveData.GoldCollectedIncludingBank;
		if (skillTreeType != SkillTreeType.None && goldCollectedIncludingBank < num)
		{
			int goldilocksLimit = Economy_EV.GetGoldilocksLimit();
			if (goldCollectedIncludingBank + goldilocksLimit >= num)
			{
				SaveManager.PlayerSaveData.GoldCollected += num - goldCollectedIncludingBank;
				Debug.Log("<color=yellow>Awarded goldilocks bonus of " + (num - goldCollectedIncludingBank).ToString() + " gold.</color>");
			}
		}
	}

	// Token: 0x06004673 RID: 18035 RVA: 0x00113FDC File Offset: 0x001121DC
	private void FixedUpdate()
	{
		if (Time.time < this.m_regenTick + 0.05f)
		{
			return;
		}
		this.m_regenTick = Time.time;
		if (PlayerManager.IsInstantiated)
		{
			PlayerController playerController = PlayerManager.GetPlayerController();
			float currentMana = playerController.CurrentMana;
			float num = (float)playerController.ActualMaxMana;
			if (currentMana < num)
			{
				float num2 = 100f * Time.fixedDeltaTime;
				if (currentMana + num2 > num)
				{
					playerController.SetMana(num, false, true, false);
				}
				else
				{
					playerController.SetMana(100f * Time.fixedDeltaTime, true, true, false);
				}
			}
			if (playerController.CurrentHealth < (float)playerController.ActualMaxHealth)
			{
				playerController.SetHealth(100f * Time.fixedDeltaTime, true, true);
			}
			if (playerController.CurrentArmor < playerController.ActualArmor)
			{
				playerController.CurrentArmor += Mathf.CeilToInt(100f * Time.fixedDeltaTime);
				playerController.SetHealth(0f, true, true);
			}
			if (this.m_hasStartedSpinkickAchievement && (playerController.IsGrounded || playerController.CharacterFlight.IsFlying || playerController.CharacterFlight.IsAssistFlying))
			{
				this.m_hasStartedSpinkickAchievement = false;
			}
		}
	}

	// Token: 0x06004674 RID: 18036 RVA: 0x001140E8 File Offset: 0x001122E8
	private void OnBounce(object sender, EventArgs args)
	{
		if (!this.m_hasStartedSpinkickAchievement)
		{
			this.m_hasStartedSpinkickAchievement = true;
			this.m_spinKickAchievementObjs.Clear();
		}
		PlayerDownstrikeEventArgs playerDownstrikeEventArgs = args as PlayerDownstrikeEventArgs;
		if (playerDownstrikeEventArgs != null)
		{
			SpinkickAchievementObj component = playerDownstrikeEventArgs.CollidedObj.GetRoot(false).GetComponent<SpinkickAchievementObj>();
			if (component)
			{
				this.m_spinKickAchievementObjs.Add(component);
				if (this.m_spinKickAchievementObjs.Count >= 11)
				{
					StoreAPIManager.GiveAchievement(AchievementType.HubtownSpinKick, StoreType.All);
				}
			}
		}
	}

	// Token: 0x0400364D RID: 13901
	[Header("Shops")]
	[SerializeField]
	private SkillTreeShop m_skillTree;

	// Token: 0x0400364E RID: 13902
	private PolygonCollider2D m_collider;

	// Token: 0x0400364F RID: 13903
	private Coroutine m_refreshTownCoroutine;

	// Token: 0x04003650 RID: 13904
	private bool m_firstTimeEntering = true;

	// Token: 0x04003651 RID: 13905
	private BaseRoom m_room;

	// Token: 0x04003652 RID: 13906
	private HubTownEnteredEventArgs m_hubtownEnteredArgs;

	// Token: 0x04003653 RID: 13907
	private ObjectiveCompleteHUDEventArgs m_traitsDisplayHUDArgs;

	// Token: 0x04003654 RID: 13908
	private bool m_traitsDisplayed;

	// Token: 0x04003655 RID: 13909
	private Action<MonoBehaviour, EventArgs> m_skillTreeWindowClosed;

	// Token: 0x04003656 RID: 13910
	private Action<MonoBehaviour, EventArgs> m_onExitHubTown;

	// Token: 0x04003657 RID: 13911
	private Action<MonoBehaviour, EventArgs> m_onBounce;

	// Token: 0x04003658 RID: 13912
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04003659 RID: 13913
	private float m_regenTick;

	// Token: 0x0400365A RID: 13914
	private const int SPINKICK_ACHIEVEMENT_CONDITION_COUNT = 11;

	// Token: 0x0400365B RID: 13915
	private bool m_hasStartedSpinkickAchievement;

	// Token: 0x0400365C RID: 13916
	private HashSet<SpinkickAchievementObj> m_spinKickAchievementObjs = new HashSet<SpinkickAchievementObj>();
}
