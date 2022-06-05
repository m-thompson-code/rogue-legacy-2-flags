using System;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class TutorialRoomController : MonoBehaviour, IRoomConsumer
{
	// Token: 0x170017E5 RID: 6117
	// (get) Token: 0x060042FE RID: 17150 RVA: 0x000250B3 File Offset: 0x000232B3
	// (set) Token: 0x060042FF RID: 17151 RVA: 0x000250BB File Offset: 0x000232BB
	public BaseRoom Room { get; private set; }

	// Token: 0x06004300 RID: 17152 RVA: 0x000250C4 File Offset: 0x000232C4
	public void SetRoom(BaseRoom room)
	{
		this.Room = room;
		this.Room.PlayerEnterRelay.AddListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom), false);
	}

	// Token: 0x06004301 RID: 17153 RVA: 0x000250EB File Offset: 0x000232EB
	private void OnDestroy()
	{
		if (this.Room)
		{
			this.Room.PlayerEnterRelay.RemoveListener(new Action<object, RoomViaDoorEventArgs>(this.OnPlayerEnterRoom));
		}
	}

	// Token: 0x06004302 RID: 17154 RVA: 0x00025117 File Offset: 0x00023317
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

	// Token: 0x06004303 RID: 17155 RVA: 0x0010C720 File Offset: 0x0010A920
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

	// Token: 0x06004304 RID: 17156 RVA: 0x00025146 File Offset: 0x00023346
	private void OnDisable()
	{
		if (!PlayerManager.IsDisposed)
		{
			PlayerManager.GetPlayerController().TakesNoDamage = false;
			this.m_playerIsInvincible = false;
		}
	}

	// Token: 0x04003444 RID: 13380
	public static bool InitializeTutorialPlayer;

	// Token: 0x04003445 RID: 13381
	private bool m_playerIsInvincible;
}
