using System;
using System.Collections;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x0200088D RID: 2189
	public class BiomeTransitionController : MonoBehaviour
	{
		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x060047D3 RID: 18387 RVA: 0x00102258 File Offset: 0x00100458
		// (set) Token: 0x060047D4 RID: 18388 RVA: 0x0010225F File Offset: 0x0010045F
		private static BiomeTransitionController Instance { get; set; }

		// Token: 0x060047D5 RID: 18389 RVA: 0x00102267 File Offset: 0x00100467
		private void Awake()
		{
			if (BiomeTransitionController.Instance == null)
			{
				BiomeTransitionController.Instance = this;
			}
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x0010227C File Offset: 0x0010047C
		private IEnumerator TransitionCoroutine(Door door)
		{
			if (!door)
			{
				Debug.Log("Biome Transition: door is null!");
			}
			if (!door.Room)
			{
				Debug.Log("Biome Transition: door.Room is null!");
			}
			Debug.Log(string.Concat(new string[]
			{
				"Biome Transition: From ",
				door.Room.BiomeType.ToString(),
				" to ",
				door.TransitionsToBiome.ToString(),
				" (seed: ",
				RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name).ToString("X"),
				"-",
				BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString()
			}));
			door.Room.BroadcastPlayerExitRoomEvents(door);
			yield return BiomeTransitionController.BiomeTransitionCoroutine(door.Room.BiomeType, door.TransitionsToBiome);
			PlayerManager.GetPlayerController().ControllerCorgi.GravityActive(true);
			PlayerManager.GetPlayerController().StopActiveAbilities(true);
			if (!door.ConnectedRoom)
			{
				Debug.Log("Biome Transition: door.ConnectedRoom is null!");
			}
			Door connectedDoor = door.ConnectedDoor;
			door.ConnectedRoom.PlacePlayerInRoom(connectedDoor);
			BiomeTransitionController.DestroyOldCastleTransitionRoom(door.TransitionsToBiome);
			BiomeTransitionController.DestroyBiomeRoomInstances(door.Room.BiomeType);
			GC.Collect();
			yield break;
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0010228B File Offset: 0x0010048B
		public static void DestroyBiomeRoomInstances(BiomeType prevBiome)
		{
			WorldBuilder.GetBiomeController(prevBiome).Reset();
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00102298 File Offset: 0x00100498
		public static void DestroyOldCastleTransitionRoom(BiomeType nextBiome)
		{
			if (nextBiome == BiomeType.Castle && WorldBuilder.OldCastleTransitionRoom != null && !WorldBuilder.OldCastleTransitionRoom.Equals(null))
			{
				UnityEngine.Object.Destroy(WorldBuilder.OldCastleTransitionRoom.gameObject);
			}
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x001022C8 File Offset: 0x001004C8
		public static IEnumerator BiomeTransitionCoroutine(BiomeType originBiome, BiomeType destinationBiome)
		{
			originBiome = BiomeType_RL.GetGroupedBiomeType(originBiome);
			destinationBiome = BiomeType_RL.GetGroupedBiomeType(destinationBiome);
			if (!WorldBuilder.GetBiomeController(destinationBiome).IsInstantiated)
			{
				yield return WorldBuilder.InstantiateBiome(destinationBiome);
			}
			Messenger<GameMessenger, GameEvent>.Broadcast(GameEvent.UpdatePools, null, null);
			BiomeTransitionController.UpdateObjectPools(originBiome, destinationBiome);
			if ((GameUtility.IsInGame && WorldBuilder.DeactivateRoomGameObjects) || GameUtility.IsInLevelEditor)
			{
				BaseRoom currentPlayerRoom = PlayerManager.GetCurrentPlayerRoom();
				if (currentPlayerRoom != null)
				{
					currentPlayerRoom.gameObject.SetActive(false);
				}
			}
			yield break;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x001022DE File Offset: 0x001004DE
		public static void UpdateObjectPools(BiomeType previousBiome, BiomeType newBiome)
		{
			SharedWorldObjectPoolManager.CreateBiomePools(newBiome);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x001022E8 File Offset: 0x001004E8
		public static void Run(Door door)
		{
			if (BiomeTransitionController.Instance != null)
			{
				PlayerManager.GetPlayerController().CharacterDash.StopDash();
				PlayerManager.GetPlayerController().ControllerCorgi.GravityActive(false);
				PlayerManager.GetPlayerController().ControllerCorgi.SetForce(Vector2.zero);
				BiomeTransitionController.TransitionStartRelay.Dispatch(door.Room.BiomeType, door.TransitionsToBiome);
				SceneLoader_RL.RunTransitionWithLogic(BiomeTransitionController.Instance.TransitionCoroutine(door), TransitionID.FadeToBlackWithLoading, true);
				return;
			}
			Debug.LogFormat("<color=red>| BiomeTransitionController | No instance of <b>BiomeTransitionController</b> exists in Scene</color>", Array.Empty<object>());
		}

		// Token: 0x04003CC2 RID: 15554
		public static Relay<BiomeType, BiomeType> TransitionStartRelay = new Relay<BiomeType, BiomeType>();
	}
}
