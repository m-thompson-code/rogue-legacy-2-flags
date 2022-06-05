using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class ClownRoomController : MonoBehaviour
{
	// Token: 0x170011B6 RID: 4534
	// (get) Token: 0x06002F98 RID: 12184 RVA: 0x000A2F27 File Offset: 0x000A1127
	public int BronzeGoal
	{
		get
		{
			return this.m_bronzeGoal;
		}
	}

	// Token: 0x170011B7 RID: 4535
	// (get) Token: 0x06002F99 RID: 12185 RVA: 0x000A2F2F File Offset: 0x000A112F
	public int SilverGoal
	{
		get
		{
			return this.m_silverGoal;
		}
	}

	// Token: 0x170011B8 RID: 4536
	// (get) Token: 0x06002F9A RID: 12186 RVA: 0x000A2F37 File Offset: 0x000A1137
	public int GoldGoal
	{
		get
		{
			return this.m_goldGoal;
		}
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x000A2F3F File Offset: 0x000A113F
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

	// Token: 0x170011B9 RID: 4537
	// (get) Token: 0x06002F9C RID: 12188 RVA: 0x000A2F72 File Offset: 0x000A1172
	// (set) Token: 0x06002F9D RID: 12189 RVA: 0x000A2F7A File Offset: 0x000A117A
	public int CurrentGoalAmount { get; protected set; }

	// Token: 0x170011BA RID: 4538
	// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000A2F83 File Offset: 0x000A1183
	public int StartingAmmo
	{
		get
		{
			return this.m_startingAmmo;
		}
	}

	// Token: 0x170011BB RID: 4539
	// (get) Token: 0x06002F9F RID: 12191 RVA: 0x000A2F8B File Offset: 0x000A118B
	// (set) Token: 0x06002FA0 RID: 12192 RVA: 0x000A2F93 File Offset: 0x000A1193
	public int CurrentAmmo { get; protected set; }

	// Token: 0x170011BC RID: 4540
	// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x000A2F9C File Offset: 0x000A119C
	// (set) Token: 0x06002FA2 RID: 12194 RVA: 0x000A2FA4 File Offset: 0x000A11A4
	public ClownRoomEventHandler BronzeGoalReachedEvent { get; set; }

	// Token: 0x170011BD RID: 4541
	// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x000A2FAD File Offset: 0x000A11AD
	// (set) Token: 0x06002FA4 RID: 12196 RVA: 0x000A2FB5 File Offset: 0x000A11B5
	public ClownRoomEventHandler SilverGoalReachedEvent { get; set; }

	// Token: 0x170011BE RID: 4542
	// (get) Token: 0x06002FA5 RID: 12197 RVA: 0x000A2FBE File Offset: 0x000A11BE
	// (set) Token: 0x06002FA6 RID: 12198 RVA: 0x000A2FC6 File Offset: 0x000A11C6
	public ClownRoomEventHandler GoldGoalReachedEvent { get; set; }

	// Token: 0x170011BF RID: 4543
	// (get) Token: 0x06002FA7 RID: 12199 RVA: 0x000A2FCF File Offset: 0x000A11CF
	// (set) Token: 0x06002FA8 RID: 12200 RVA: 0x000A2FD7 File Offset: 0x000A11D7
	public ClownRoomEventHandler CurrentAmmoChangedEvent { get; set; }

	// Token: 0x170011C0 RID: 4544
	// (get) Token: 0x06002FA9 RID: 12201 RVA: 0x000A2FE0 File Offset: 0x000A11E0
	// (set) Token: 0x06002FAA RID: 12202 RVA: 0x000A2FE8 File Offset: 0x000A11E8
	public ClownRoomEventHandler CurrentGoalAmountChangedEvent { get; set; }

	// Token: 0x170011C1 RID: 4545
	// (get) Token: 0x06002FAB RID: 12203 RVA: 0x000A2FF1 File Offset: 0x000A11F1
	// (set) Token: 0x06002FAC RID: 12204 RVA: 0x000A2FF9 File Offset: 0x000A11F9
	public BaseRoom Room { get; private set; }

	// Token: 0x06002FAD RID: 12205 RVA: 0x000A3004 File Offset: 0x000A1204
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

	// Token: 0x06002FAE RID: 12206 RVA: 0x000A30A3 File Offset: 0x000A12A3
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => PlayerManager.IsInstantiated);
		this.InitializeChallengeAbility();
		this.m_waitUntilAttackReleasedYield = new WaitUntil(() => PlayerManager.GetPlayerController().CastAbility.GetAbility(CastAbilityType.Weapon, false).CurrentAbilityAnimState == AbilityAnimState.Attack);
		this.m_clownRoomEndedYield = new WaitUntil(() => PlayerManager.GetPlayerController().IsGrounded && ProjectileManager.ActiveProjectileCount == 0);
		yield break;
	}

	// Token: 0x06002FAF RID: 12207 RVA: 0x000A30B2 File Offset: 0x000A12B2
	private void InitializeChallengeAbility()
	{
		this.m_challengeAbility = PlayerManager.GetPlayerController().CastAbility.CreateAbilityInstance(CastAbilityType.Weapon, this.m_challengeAbilityType);
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x000A30D0 File Offset: 0x000A12D0
	protected virtual void OnPlayerEnter(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.ResetClownRoom();
		this.AssignRoomAbilities();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerEnterClownRoom, this, this.m_clownRoomArgs);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.AddListener(GameEvent.PlayerWeaponAbilityCast, this.m_onCastAbility);
	}

	// Token: 0x06002FB1 RID: 12209 RVA: 0x000A3106 File Offset: 0x000A1306
	protected virtual void OnPlayerExit(object sender, RoomViaDoorEventArgs eventArgs)
	{
		this.RevertAbilities();
		Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.PlayerExitClownRoom, this, this.m_clownRoomArgs);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.EnemyDeath, this.m_onEnemyDeath);
		Messenger<GameMessenger, GameEvent>.RemoveListener(GameEvent.PlayerWeaponAbilityCast, this.m_onCastAbility);
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000A3138 File Offset: 0x000A1338
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

	// Token: 0x06002FB3 RID: 12211 RVA: 0x000A31F5 File Offset: 0x000A13F5
	protected virtual void OnCastAbility(object sender, EventArgs args)
	{
		base.StartCoroutine(this.OnCastAbilityCoroutine());
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x000A3204 File Offset: 0x000A1404
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

	// Token: 0x06002FB5 RID: 12213 RVA: 0x000A3213 File Offset: 0x000A1413
	protected virtual void EndClownRoom()
	{
		base.StartCoroutine(this.EndClownRoomCoroutine());
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x000A3222 File Offset: 0x000A1422
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

	// Token: 0x06002FB7 RID: 12215 RVA: 0x000A3234 File Offset: 0x000A1434
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

	// Token: 0x06002FB8 RID: 12216 RVA: 0x000A335E File Offset: 0x000A155E
	private void ExitClownRoom()
	{
		if (this.m_tunnelSpawnController.Tunnel != null)
		{
			this.m_tunnelSpawnController.Tunnel.ForceEnterTunnel(true, null);
		}
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x000A3388 File Offset: 0x000A1588
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

	// Token: 0x06002FBA RID: 12218 RVA: 0x000A345C File Offset: 0x000A165C
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

	// Token: 0x06002FBB RID: 12219 RVA: 0x000A34CB File Offset: 0x000A16CB
	public virtual void ResetClownRoom()
	{
		this.CurrentAmmo = this.StartingAmmo;
		this.CurrentGoalAmount = 0;
	}

	// Token: 0x06002FBC RID: 12220 RVA: 0x000A34E0 File Offset: 0x000A16E0
	private void OnDestroy()
	{
		if (this.Room != null)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnter));
			this.Room.PlayerExitRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerExit));
		}
	}

	// Token: 0x040025EF RID: 9711
	[SerializeField]
	private AbilityType m_challengeAbilityType;

	// Token: 0x040025F0 RID: 9712
	[SerializeField]
	private int m_bronzeGoal;

	// Token: 0x040025F1 RID: 9713
	[SerializeField]
	private int m_silverGoal;

	// Token: 0x040025F2 RID: 9714
	[SerializeField]
	private int m_goldGoal;

	// Token: 0x040025F3 RID: 9715
	[SerializeField]
	private int m_startingAmmo;

	// Token: 0x040025F4 RID: 9716
	private ClownRoomEnteredEventArgs m_clownRoomArgs;

	// Token: 0x040025F5 RID: 9717
	private BaseAbility_RL m_challengeAbility;

	// Token: 0x040025F6 RID: 9718
	private BaseAbility_RL m_storedWeaponAbility;

	// Token: 0x040025F7 RID: 9719
	private BaseAbility_RL m_storedTalentAbility;

	// Token: 0x040025F8 RID: 9720
	private BaseAbility_RL m_storedSpellAbility;

	// Token: 0x040025F9 RID: 9721
	private TunnelSpawnController m_tunnelSpawnController;

	// Token: 0x040025FA RID: 9722
	private WaitUntil m_waitUntilAttackReleasedYield;

	// Token: 0x040025FB RID: 9723
	private WaitUntil m_clownRoomEndedYield;

	// Token: 0x040025FC RID: 9724
	private Action<MonoBehaviour, EventArgs> m_onEnemyDeath;

	// Token: 0x040025FD RID: 9725
	private Action<MonoBehaviour, EventArgs> m_onCastAbility;
}
