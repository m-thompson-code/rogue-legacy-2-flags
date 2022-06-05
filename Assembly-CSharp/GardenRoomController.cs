using System;
using System.Collections;
using FMODUnity;
using RLAudio;
using UnityEngine;

// Token: 0x02000869 RID: 2153
public class GardenRoomController : BaseSpecialRoomController
{
	// Token: 0x06004258 RID: 16984 RVA: 0x00024BE0 File Offset: 0x00022DE0
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

	// Token: 0x06004259 RID: 16985 RVA: 0x00024C1B File Offset: 0x00022E1B
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

	// Token: 0x0600425A RID: 16986 RVA: 0x00024C23 File Offset: 0x00022E23
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
