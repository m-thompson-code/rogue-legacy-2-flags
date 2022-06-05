using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000501 RID: 1281
public class GardenRoomController : BaseSpecialRoomController
{
	// Token: 0x06002FE4 RID: 12260 RVA: 0x000A3E88 File Offset: 0x000A2088
	protected override void OnPlayerEnterRoom(object sender, RoomViaDoorEventArgs eventArgs)
	{
		base.OnPlayerEnterRoom(sender, eventArgs);
		SaveManager.PlayerSaveData.EndingSpawnRoom = EndingSpawnRoomType.None;
		base.StartCoroutine(this.FlipPlayer());
		if (MusicManager.CurrentSong != SongID.TraitorBoss_Tettix)
		{
			base.StartCoroutine(this.PlaySong());
		}
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000A3EC3 File Offset: 0x000A20C3
	private IEnumerator FlipPlayer()
	{
		yield return null;
		PlayerController playerController = PlayerManager.GetPlayerController();
		if (playerController.IsFacingRight)
		{
			playerController.CharacterCorgi.Flip(true, true);
		}
		yield break;
	}

	// Token: 0x06002FE6 RID: 12262 RVA: 0x000A3ECB File Offset: 0x000A20CB
	private IEnumerator PlaySong()
	{
		float delay = Time.time + 2f;
		while (Time.time < delay)
		{
			yield return null;
		}
		MusicManager.PlayMusic(SongID.TraitorBoss_Tettix, false, false);
		if (MusicManager.CurrentMusicInstance.isValid())
		{
			RuntimeManager.StudioSystem.setParameterByName("bossEncounterProgress_cain", 0f, false);
		}
		base.GetComponent<RoomMusicOverrideController>().SetOverride(SongID.TraitorBoss_Tettix);
		yield break;
	}
}
