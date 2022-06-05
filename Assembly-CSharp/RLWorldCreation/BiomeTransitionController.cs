using System;
using System.Collections;
using SceneManagement_RL;
using Sigtrap.Relays;
using UnityEngine;

namespace RLWorldCreation
{
	// Token: 0x02000DAD RID: 3501
	public class BiomeTransitionController : MonoBehaviour
	{
		// Token: 0x17001FF4 RID: 8180
		// (get) Token: 0x060062D1 RID: 25297 RVA: 0x0003677F File Offset: 0x0003497F
		// (set) Token: 0x060062D2 RID: 25298 RVA: 0x00036786 File Offset: 0x00034986
		private static BiomeTransitionController Instance { get; set; }

		// Token: 0x060062D3 RID: 25299 RVA: 0x0003678E File Offset: 0x0003498E
		private void Awake()
		{
			if (BiomeTransitionController.Instance == null)
			{
				BiomeTransitionController.Instance = this;
			}
		}

		// Token: 0x060062D4 RID: 25300 RVA: 0x000367A3 File Offset: 0x000349A3
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

		// Token: 0x060062D5 RID: 25301 RVA: 0x000367B2 File Offset: 0x000349B2
		public static void DestroyBiomeRoomInstances(BiomeType prevBiome)
		{
			WorldBuilder.GetBiomeController(prevBiome).Reset();
		}

		// Token: 0x060062D6 RID: 25302 RVA: 0x000367BF File Offset: 0x000349BF
		public static void DestroyOldCastleTransitionRoom(BiomeType nextBiome)
		{
			if (nextBiome == BiomeType.Castle && WorldBuilder.OldCastleTransitionRoom != null && !WorldBuilder.OldCastleTransitionRoom.Equals(null))
			{
				UnityEngine.Object.Destroy(WorldBuilder.OldCastleTransitionRoom.gameObject);
			}
		}

		// Token: 0x060062D7 RID: 25303 RVA: 0x000367EF File Offset: 0x000349EF
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

		// Token: 0x060062D8 RID: 25304 RVA: 0x00036805 File Offset: 0x00034A05
		public static void UpdateObjectPools(BiomeType previousBiome, BiomeType newBiome)
		{
			SharedWorldObjectPoolManager.CreateBiomePools(newBiome);
		}

		// Token: 0x060062D9 RID: 25305 RVA: 0x00170A10 File Offset: 0x0016EC10
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

		// Token: 0x040050C4 RID: 20676
		public static Relay<BiomeType, BiomeType> TransitionStartRelay = new Relay<BiomeType, BiomeType>();
	}
}
