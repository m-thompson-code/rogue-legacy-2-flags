using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000858 RID: 2136
public class ClownRoomController : MonoBehaviour
{
	// Token: 0x1700179F RID: 6047
	// (get) Token: 0x060041CB RID: 16843 RVA: 0x000246D2 File Offset: 0x000228D2
	public int BronzeGoal
	{
		get
		{
			return this.m_bronzeGoal;
		}
	}

	// Token: 0x170017A0 RID: 6048
	// (get) Token: 0x060041CC RID: 16844 RVA: 0x000246DA File Offset: 0x000228DA
	public int SilverGoal
	{
		get
		{
			return this.m_silverGoal;
		}
	}

	// Token: 0x170017A1 RID: 6049
	// (get) Token: 0x060041CD RID: 16845 RVA: 0x000246E2 File Offset: 0x000228E2
	public int GoldGoal
	{
		get
		{
			return this.m_goldGoal;
		}
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x000246EA File Offset: 0x000228EA
	public ClownGoalType GetClownRewardType()
	{
		if (this.CurrentGoalAmount >= this.GoldGoal)
		{
			return ClownGoalType.Gold;
		}
		if (this.CurrentGoalAmount >= this.SilverGoal)
		{
			return ClownGoalType.Silver;
		}
		if (this.CurrentGoalAmount >= this.BronzeGoal)
		{
			return ClownGoalType.Bronze;
		}
		return ClownGoalType.None;
	}

	// Token: 0x170017A2 RID: 6050
	// (get) Token: 0x060041CF RID: 16847 RVA: 0x0002471D File Offset: 0x0002291D
	// (set) Token: 0x060041D0 RID: 16848 RVA: 0x00024725 File Offset: 0x00022925
	public int CurrentGoalAmount { get; protected set; }

	// Token: 0x170017A3 RID: 6051
	// (get) Token: 0x060041D1 RID: 16849 RVA: 0x0002472E File Offset: 0x0002292E
	public int StartingAmmo
	{
		get
		{
			return this.m_startingAmmo;
		}
	}

	// Token: 0x170017A4 RID: 6052
	// (get) Token: 0x060041D2 RID: 16850 RVA: 0x00024736 File Offset: 0x00022936
	// (set) Token: 0x060041D3 RID: 16851 RVA: 0x0002473E File Offset: 0x0002293E
	public int CurrentAmmo { get; protected set; }

	// Token: 0x170017A5 RID: 6053
	// (get) Token: 0x060041D4 RID: 16852 RVA: 0x00024747 File Offset: 0x00022947
	// (set) Token: 0x060041D5 RID: 16853 RVA: 0x0002474F File Offset: 0x0002294F
	public ClownRoomEventHandler BronzeGoalReachedEvent { get; set; }

	// Token: 0x170017A6 RID: 6054
	// (get) Token: 0x060041D6 RID: 16854 RVA: 0x00024758 File Offset: 0x00022958
	// (set) Token: 0x060041D7 RID: 16855 RVA: 0x00024760 File Offset: 0x00022960
	public ClownRoomEventHandler SilverGoalReachedEvent { get; set; }

	// Token: 0x170017A7 RID: 6055
	// (get) Token: 0x060041D8 RID: 16856 RVA: 0x00024769 File Offset: 0x00022969
	// (set) Token: 0x060041D9 RID: 16857 RVA: 0x00024771 File Offset: 0x00022971
	public ClownRoomEventHandler GoldGoalReachedEvent { get; set; }

	// Token: 0x170017A8 RID: 6056
	// (get) Token: 0x060041DA RID: 16858 RVA: 0x0002477A File Offset: 0x0002297A
	// (set) Token: 0x060041DB RID: 16859 RVA: 0x00024782 File Offset: 0x00022982
	public ClownRoomEventHandler CurrentAmmoChangedEvent { get; set; }

	// Token: 0x170017A9 RID: 6057
	// (get) Token: 0x060041DC RID: 16860 RVA: 0x0002478B File Offset: 0x0002298B
	// (set) Token: 0x060041DD RID: 16861 RVA: 0x00024793 File Offset: 0x00022993
	public ClownRoomEventHandler CurrentGoalAmountChangedEvent { get; set; }

	// Token: 0x170017AA RID: 6058
	// (get) Token: 0x060041DE RID: 16862 RVA: 0x0002479C File Offset: 0x0002299C
	// (set) Token: 0x060041DF RID: 16863 RVA: 0x000247A4 File Offset: 0x000229A4
	public BaseRoom Room { get; private set; }

	// Token: 0x060041E0 RID: 16864 RVA: 0x00108B28 File Offset: 0x00106D28
	private void Awake()
	{
		this.Room = base.GetComponent<Room>();
		this.m_onCastAbility = new Action<MonoBehaviour, EventArgs>(this.OnCastAbility);
		this.m_onEnemyDeath = new Action<MonoBehaviour, EventArgs>(this.OnEnemyDeath);
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter), false);
		this.Room.PlayerExitRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit), false);
		this.m_clownRoomArgs = new ClownRoomEnteredEventArgs(this);
		this.m_tunnelSpawnController = this.Room.gameObject.GetComponentInChildren<TunnelSpawnController>();
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x000247AD File Offset: 0x000229AD
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		this.InitializeChallengeAbility();
		this.m_waitUntilAttackReleasedYield = new WaitUntil(() => PlayerManager.GetPlayerController().CastAbility.GetAbility(CastAbilityType.Weapon, false).CurrentAbilityAnimState == AbilityAnimState.Attack);
		this.m_clownRoomEndedYield = new WaitUntil(() => PlayerManager.GetPlayerController().IsGrounded && ProjectileManager.ActiveProjectileCount == 0);
		yield break;
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x000247BC File Offset: 0x000229BC
	private void InitializeChallengeAbility()
	{
		this.m_challengeAbility = PlayerManager.GetPlayerController().CastAbility.CreateAbilityInstance(CastAbilityType.Weapon, this.m_challengeAbilityType);
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x000247DA File Offset: 0x000229DA
	protected virtual void OnPlayerEnter(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.ResetClownRoom();
		this.AssignRoomAbilities();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterClownRoom, this, this.m_clownRoomArgs);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onCastAbility);
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x00024810 File Offset: 0x00022A10
	protected virtual void OnPlayerExit(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.RevertAbilities();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitClownRoom, this, this.m_clownRoomArgs);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onCastAbility);
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x00108BC8 File Offset: 0x00106DC8
	protected virtual void OnEnemyDeath(object sender, EventArgs args)
	{
		EnemyRank enemyRank = (args as EnemyDeathEventArgs).Victim.EnemyRank;
		if (enemyRank == EnemyRank.Basic || enemyRank != EnemyRank.Advanced)
		{
			int currentGoalAmount = this.CurrentGoalAmount;
			this.CurrentGoalAmount = currentGoalAmount + 1;
		}
		else
		{
			int currentGoalAmount = this.CurrentGoalAmount;
			this.CurrentGoalAmount = currentGoalAmount - 1;
		}
		if (this.CurrentGoalAmount == this.BronzeGoal && this.BronzeGoalReachedEvent != null)
		{
			this.BronzeGoalReachedEvent();
		}
		if (this.CurrentGoalAmount == this.SilverGoal && this.SilverGoalReachedEvent != null)
		{
			this.SilverGoalReachedEvent();
		}
		if (this.CurrentGoalAmount == this.GoldGoal && this.GoldGoalReachedEvent != null)
		{
			this.GoldGoalReachedEvent();
		}
		if (this.CurrentGoalAmountChangedEvent != null)
		{
			this.CurrentAmmoChangedEvent();
		}
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x00024840 File Offset: 0x00022A40
	protected virtual void OnCastAbility(object sender, EventArgs args)
	{
		base.StartCoroutine(this.OnCastAbilityCoroutine());
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x0002484F File Offset: 0x00022A4F
	private IEnumerator OnCastAbilityCoroutine()
	{
		yield return this.m_waitUntilAttackReleasedYield;
		int currentAmmo = this.CurrentAmmo;
		this.CurrentAmmo = currentAmmo - 1;
		if (this.CurrentAmmoChangedEvent != null)
		{
			this.CurrentAmmoChangedEvent();
		}
		if (this.CurrentAmmo <= 0)
		{
			this.EndClownRoom();
		}
		yield break;
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x0002485E File Offset: 0x00022A5E
	protected virtual void EndClownRoom()
	{
		base.StartCoroutine(this.EndClownRoomCoroutine());
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x0002486D File Offset: 0x00022A6D
	protected virtual IEnumerator EndClownRoomCoroutine()
	{
		RewiredMapController.SetMapEnabled(GameInputMode.Game, false);
		yield return this.m_clownRoomEndedYield;
		yield return new WaitForSeconds(1f);
		if (this.m_tunnelSpawnController.Tunnel != null)
		{
			this.m_tunnelSpawnController.Tunnel.Destination.Room.gameObject.GetComponentInChildren<ClownEntranceRoomController>().UnlockChestReward(this.GetClownRewardType());
		}
		this.SetEndingDialogue();
		if (this.m_tunnelSpawnController.Tunnel != null)
		{
			DialogueManager.AddDialogueCompleteEndHandler(new Action(this.ExitClownRoom));
		}
		WindowManager.SetWindowIsOpen(WindowID.Dialogue, true);
		yield break;
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x00108C88 File Offset: 0x00106E88
	protected virtual void SetEndingDialogue()
	{
		DialogueManager.StartNewDialogue(null, NPCState.Idle);
		DialogueManager.AddNonLocDialogue("Clown", "TIMES UP!!", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddNonLocDialogue("Clown", "Let's see how you did.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		DialogueManager.AddNonLocDialogue("Clown", string.Format("You destroyed a total of {0} out of {1} targets", this.CurrentGoalAmount, this.GoldGoal), false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
		switch (this.GetClownRewardType())
		{
		case ClownGoalType.Bronze:
			DialogueManager.AddNonLocDialogue("Clown", "MEDIOCRE!!", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			break;
		case ClownGoalType.Silver:
			DialogueManager.AddNonLocDialogue("Clown", "Not bad, not bad.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			break;
		case ClownGoalType.Gold:
			DialogueManager.AddNonLocDialogue("Clown", "You did FANTASTIC.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			DialogueManager.AddNonLocDialogue("Clown", "Don't get cocky, kid.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			break;
		default:
			DialogueManager.AddNonLocDialogue("Clown", "Y-you couldn't even get a bronze!?", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
			break;
		}
		DialogueManager.AddNonLocDialogue("Clown", "Well, you might as well come back for your reward.", false, DialogueWindowStyle.HorizontalUpper, DialoguePortraitType.None, NPCState.None, 0.015f);
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x0002487C File Offset: 0x00022A7C
	private void ExitClownRoom()
	{
		if (this.m_tunnelSpawnController.Tunnel != null)
		{
			this.m_tunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
		}
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x00108DB4 File Offset: 0x00106FB4
	protected void AssignRoomAbilities()
	{
		if (this.m_challengeAbility == null)
		{
			this.InitializeChallengeAbility();
		}
		PlayerController playerController = PlayerManager.GetPlayerController();
		this.m_storedWeaponAbility = playerController.CastAbility.GetAbility(CastAbilityType.Weapon, false);
		this.m_storedSpellAbility = playerController.CastAbility.GetAbility(CastAbilityType.Spell, false);
		this.m_storedTalentAbility = playerController.CastAbility.GetAbility(CastAbilityType.Talent, false);
		playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, this.m_challengeAbility, false);
		playerController.CharacterClass.SetAbility(CastAbilityType.Spell, null, false);
		playerController.CharacterClass.SetAbility(CastAbilityType.Talent, null, false);
		if (playerController.LookController != null)
		{
			CharacterData characterData = SaveManager.PlayerSaveData.CurrentCharacter.Clone();
			characterData.Weapon = this.m_challengeAbilityType;
			characterData.Spell = AbilityType.None;
			characterData.Talent = AbilityType.None;
			playerController.LookController.InitializeEquipmentLook(characterData);
		}
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x00108E88 File Offset: 0x00107088
	protected void RevertAbilities()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, this.m_storedWeaponAbility, false);
		playerController.CharacterClass.SetAbility(CastAbilityType.Spell, this.m_storedSpellAbility, false);
		playerController.CharacterClass.SetAbility(CastAbilityType.Talent, this.m_storedTalentAbility, false);
		if (playerController.LookController != null)
		{
			playerController.LookController.InitializeEquipmentLook(SaveManager.PlayerSaveData.CurrentCharacter);
		}
	}

	// Token: 0x060041EE RID: 16878 RVA: 0x000248A3 File Offset: 0x00022AA3
	public virtual void ResetClownRoom()
	{
		this.CurrentAmmo = this.StartingAmmo;
		this.CurrentGoalAmount = 0;
	}

	// Token: 0x060041EF RID: 16879 RVA: 0x00108EF8 File Offset: 0x001070F8
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit));
		}
	}

	// Token: 0x04003381 RID: 13185
	[SerializeField]
	private AbilityType m_challengeAbilityType;

	// Token: 0x04003382 RID: 13186
	[SerializeField]
	private int m_bronzeGoal;

	// Token: 0x04003383 RID: 13187
	[SerializeField]
	private int m_silverGoal;

	// Token: 0x04003384 RID: 13188
	[SerializeField]
	private int m_goldGoal;

	// Token: 0x04003385 RID: 13189
	[SerializeField]
	private int m_startingAmmo;

	// Token: 0x04003386 RID: 13190
	private ClownRoomEnteredEventArgs m_clownRoomArgs;

	// Token: 0x04003387 RID: 13191
	private BaseAbility_RL m_challengeAbility;

	// Token: 0x04003388 RID: 13192
	private BaseAbility_RL m_storedWeaponAbility;

	// Token: 0x04003389 RID: 13193
	private BaseAbility_RL m_storedTalentAbility;

	// Token: 0x0400338A RID: 13194
	private BaseAbility_RL m_storedSpellAbility;

	// Token: 0x0400338B RID: 13195
	private TunnelSpawnController m_tunnelSpawnController;

	// Token: 0x0400338C RID: 13196
	private WaitUntil m_waitUntilAttackReleasedYield;

	// Token: 0x0400338D RID: 13197
	private WaitUntil m_clownRoomEndedYield;

	// Token: 0x0400338E RID: 13198
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;

	// Token: 0x0400338F RID: 13199
	private Action<MonoBehaviour, EventArgs> m_onCastAbility;
}
