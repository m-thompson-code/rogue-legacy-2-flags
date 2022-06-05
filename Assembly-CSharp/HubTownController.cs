using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class HubTownController : MonoBehaviour
{
	// Token: 0x0600321A RID: 12826 RVA: 0x000A9DD8 File Offset: 0x000A7FD8
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

	// Token: 0x0600321B RID: 12827 RVA: 0x000A9E5C File Offset: 0x000A805C
	private void OnEnable()
	{
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerDownstrikeBounce, this.m_onBounce);
		this.m_room.PlayerEnterRelay.AddListener(this.m_onPlayerEnterRoom, false);
	}

	// Token: 0x0600321C RID: 12828 RVA: 0x000A9EA8 File Offset: 0x000A80A8
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.SkillTree_Closed, this.m_skillTreeWindowClosed);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerExitHubTown, this.m_onExitHubTown);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerDownstrikeBounce, this.m_onBounce);
		this.m_room.PlayerEnterRelay.RemoveListener(this.m_onPlayerEnterRoom);
	}

	// Token: 0x0600321D RID: 12829 RVA: 0x000A9EE8 File Offset: 0x000A80E8
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

	// Token: 0x0600321E RID: 12830 RVA: 0x000A9F3A File Offset: 0x000A813A
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

	// Token: 0x0600321F RID: 12831 RVA: 0x000A9F6B File Offset: 0x000A816B
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

	// Token: 0x06003220 RID: 12832 RVA: 0x000A9F7C File Offset: 0x000A817C
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

	// Token: 0x06003221 RID: 12833 RVA: 0x000A9FD4 File Offset: 0x000A81D4
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

	// Token: 0x06003222 RID: 12834 RVA: 0x000AA098 File Offset: 0x000A8298
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

	// Token: 0x06003223 RID: 12835 RVA: 0x000AA1A4 File Offset: 0x000A83A4
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

	// Token: 0x04002767 RID: 10087
	[Header("Shops")]
	[SerializeField]
	private SkillTreeShop m_skillTree;

	// Token: 0x04002768 RID: 10088
	private PolygonCollider2D m_collider;

	// Token: 0x04002769 RID: 10089
	private Coroutine m_refreshTownCoroutine;

	// Token: 0x0400276A RID: 10090
	private bool m_firstTimeEntering = true;

	// Token: 0x0400276B RID: 10091
	private BaseRoom m_room;

	// Token: 0x0400276C RID: 10092
	private HubTownEnteredEventArgs m_hubtownEnteredArgs;

	// Token: 0x0400276D RID: 10093
	private ObjectiveCompleteHUDEventArgs m_traitsDisplayHUDArgs;

	// Token: 0x0400276E RID: 10094
	private bool m_traitsDisplayed;

	// Token: 0x0400276F RID: 10095
	private Action<MonoBehaviour, EventArgs> m_skillTreeWindowClosed;

	// Token: 0x04002770 RID: 10096
	private Action<MonoBehaviour, EventArgs> m_onExitHubTown;

	// Token: 0x04002771 RID: 10097
	private Action<MonoBehaviour, EventArgs> m_onBounce;

	// Token: 0x04002772 RID: 10098
	private Action<object, RoomViaDoorEventArgs> m_onPlayerEnterRoom;

	// Token: 0x04002773 RID: 10099
	private float m_regenTick;

	// Token: 0x04002774 RID: 10100
	private const int SPINKICK_ACHIEVEMENT_CONDITION_COUNT = 11;

	// Token: 0x04002775 RID: 10101
	private bool m_hasStartedSpinkickAchievement;

	// Token: 0x04002776 RID: 10102
	private HashSet<SpinkickAchievementObj> m_spinKickAchievementObjs = new HashSet<SpinkickAchievementObj>();
}
