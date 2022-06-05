using System;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class TutorialRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170011D0 RID: 4560
	// (get) Token: 0x06003042 RID: 12354 RVA: 0x000A5386 File Offset: 0x000A3586
	// (set) Token: 0x06003043 RID: 12355 RVA: 0x000A538E File Offset: 0x000A358E
	public BaseRoom Room { get; private set; }

	// Token: 0x06003044 RID: 12356 RVA: 0x000A5397 File Offset: 0x000A3597
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x000A53BE File Offset: 0x000A35BE
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000A53EA File Offset: 0x000A35EA
	private void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs args)
	{
		if (TutorialRoomController.InitializeTutorialPlayer)
		{
			this.CreateStartingCharacter();
		}
		TutorialRoomController.InitializeTutorialPlayer = false;
		if (!this.m_playerIsInvincible)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = true;
			this.m_playerIsInvincible = true;
		}
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x000A541C File Offset: 0x000A361C
	private void CreateStartingCharacter()
	{
		PlayerController playerController = PlayerManager.GetPlayerController();
		CharacterData currentCharacter = SaveManager.PlayerSaveData.CurrentCharacter;
		currentCharacter.IsFemale = CharacterCreator.GetRandomGender(true);
		CharacterCreator.GenerateRandomLook(currentCharacter);
		string[] availableNames = CharacterCreator.GetAvailableNames(currentCharacter.IsFemale);
		string name = availableNames[UnityEngine.Random.Range(0, availableNames.Length)];
		currentCharacter.Name = name;
		currentCharacter.Spell = AbilityType.FireballSpell;
		currentCharacter.Talent = AbilityType.ShieldBlockTalent;
		currentCharacter.ClassType = ClassType.SwordClass;
		playerController.LookController.InitializeLook(SaveManager.PlayerSaveData.CurrentCharacter);
		playerController.CharacterClass.ClassType = SaveManager.PlayerSaveData.CurrentCharacter.ClassType;
		playerController.CharacterClass.SetAbility(CastAbilityType.Weapon, SaveManager.PlayerSaveData.CurrentCharacter.Weapon, true);
		playerController.CharacterClass.SetAbility(CastAbilityType.Spell, SaveManager.PlayerSaveData.CurrentCharacter.Spell, true);
		playerController.CharacterClass.SetAbility(CastAbilityType.Talent, SaveManager.PlayerSaveData.CurrentCharacter.Talent, true);
		playerController.ResetCharacter();
		playerController.SetMana(0f, false, true, false);
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x000A551C File Offset: 0x000A371C
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_playerIsInvincible = false;
		}
	}

	// Token: 0x0400265C RID: 9820
	public static bool InitializeTutorialPlayer;

	// Token: 0x0400265D RID: 9821
	private bool m_playerIsInvincible;
}
