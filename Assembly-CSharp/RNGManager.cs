using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020006A7 RID: 1703
public class RNGManager : MonoBehaviour
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06003E70 RID: 15984 RVA: 0x000DBF4C File Offset: 0x000DA14C
	// (remove) Token: 0x06003E71 RID: 15985 RVA: 0x000DBF80 File Offset: 0x000DA180
	public static event EventHandler<EventArgs> InitialisedEvent;

	// Token: 0x1700156F RID: 5487
	// (get) Token: 0x06003E72 RID: 15986 RVA: 0x000DBFB3 File Offset: 0x000DA1B3
	// (set) Token: 0x06003E73 RID: 15987 RVA: 0x000DBFBA File Offset: 0x000DA1BA
	private static RNGManager Instance { get; set; }

	// Token: 0x17001570 RID: 5488
	// (get) Token: 0x06003E74 RID: 15988 RVA: 0x000DBFC2 File Offset: 0x000DA1C2
	public static bool IsInstantiated
	{
		get
		{
			return RNGManager.Instance != null;
		}
	}

	// Token: 0x17001571 RID: 5489
	// (get) Token: 0x06003E75 RID: 15989 RVA: 0x000DBFD0 File Offset: 0x000DA1D0
	public static List<RNGController> RNGControllers
	{
		get
		{
			if (Application.isPlaying && RNGManager.m_rngControllers == null)
			{
				RNGManager.m_rngControllers = (from entry in RNGManager.m_rngControllerTable.Values
				select entry).ToList<RNGController>();
			}
			return RNGManager.m_rngControllers;
		}
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x000DC028 File Offset: 0x000DA228
	private void Awake()
	{
		if (RNGManager.Instance == null)
		{
			RNGManager.Instance = this;
			if (base.transform.parent == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			if (RNGManager.InitialisedEvent != null)
			{
				RNGManager.InitialisedEvent(RNGManager.Instance, EventArgs.Empty);
				return;
			}
		}
		else
		{
			Debug.LogFormat("<color=red>|{0}| Multiple instances present in Scene. Destroying duplicate</color>", new object[]
			{
				this
			});
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x000DC0A1 File Offset: 0x000DA2A1
	public static int GetRandomNumber(RngID randomNumberGenerator, string callerDescription, int minInclusive, int maxExclusive)
	{
		if (!RNGManager.m_rngControllerTable[randomNumberGenerator].IsInitialized)
		{
			RNGManager.InitializeRandomNumberGenerators();
		}
		return RNGManager.m_rngControllerTable[randomNumberGenerator].GetRandomNumber(callerDescription, minInclusive, maxExclusive);
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x000DC0CD File Offset: 0x000DA2CD
	public static float GetRandomNumber(RngID randomNumberGenerator, string callerDescription, float min, float max)
	{
		if (!RNGManager.m_rngControllerTable[randomNumberGenerator].IsInitialized)
		{
			RNGManager.InitializeRandomNumberGenerators();
		}
		return RNGManager.m_rngControllerTable[randomNumberGenerator].GetRandomNumber(callerDescription, min, max);
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x000DC0F9 File Offset: 0x000DA2F9
	public static int GetSeed(RngID id)
	{
		if (RNGManager.m_rngControllerTable.ContainsKey(id))
		{
			return RNGManager.m_rngControllerTable[id].Seed;
		}
		return -1;
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x000DC11C File Offset: 0x000DA31C
	public static void InitializeRandomNumberGenerators()
	{
		int currentSeed = RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name);
		if (currentSeed == -1)
		{
			RNGSeedManager.GenerateNewSeed(SceneLoadingUtility.ActiveScene.name);
			currentSeed = RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name);
		}
		RNGManager.InitializeRandomNumberGenerators(currentSeed);
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x000DC16C File Offset: 0x000DA36C
	public static void InitializeRandomNumberGenerators(int masterSeed)
	{
		System.Random random = new System.Random(masterSeed);
		foreach (KeyValuePair<RngID, RNGController> keyValuePair in RNGManager.m_rngControllerTable)
		{
			keyValuePair.Value.SetSeed(random.Next(0, RNGManager.Instance.m_maxRngControllerSeed));
		}
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x000DC1DC File Offset: 0x000DA3DC
	public static void SetSeed(RngID id, int seed)
	{
		RNGController rngcontroller;
		if (RNGManager.m_rngControllerTable.TryGetValue(id, out rngcontroller))
		{
			rngcontroller.SetSeed(seed);
			return;
		}
		string.Format("<color=red>| RNGManager | The RNGController table does not contain an entry for RngID (<b>{0}</b>). If you see this message, please add a bug report to Pivotal.</color>", id);
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x000DC211 File Offset: 0x000DA411
	private void OnDestroy()
	{
		RNGManager.m_rngControllerTable = null;
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x000DC21C File Offset: 0x000DA41C
	public static void PrintSeedsToConsole()
	{
		string text = string.Format("<color=blue>| {0} |: Master Seed = (HEX = {1}, INT = {2}) ", RNGManager.Instance, RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name).ToString("X") + "-" + BurdenManager.GetBurdenLevel(BurdenType.RoomCount).ToString(), RNGSeedManager.GetCurrentSeed(SceneLoadingUtility.ActiveScene.name));
		foreach (KeyValuePair<RngID, RNGController> keyValuePair in RNGManager.m_rngControllerTable)
		{
			RNGController value = keyValuePair.Value;
			text += string.Format("<color=blue>({0}) RNG Seed = ({1}) </color>", value.ID, value.Seed);
		}
		text += "</color>";
		Debug.LogFormat("{0}", new object[]
		{
			text
		});
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x000DC31C File Offset: 0x000DA51C
	public static void Reset()
	{
		RNGSeedManager.GenerateNewSeed(SceneLoadingUtility.ActiveScene.name);
		RNGManager.InitializeRandomNumberGenerators();
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x000DC340 File Offset: 0x000DA540
	public static void Reset(int seedOverride)
	{
		RNGSeedManager.SetTempSeed(seedOverride.ToString("X"));
		RNGManager.Reset();
	}

	// Token: 0x04002E6E RID: 11886
	[SerializeField]
	[FormerlySerializedAs("m_maxRandomNumber")]
	private int m_maxRngControllerSeed = 1000;

	// Token: 0x04002E6F RID: 11887
	private static List<RNGController> m_rngControllers = null;

	// Token: 0x04002E70 RID: 11888
	private static Dictionary<RngID, RNGController> m_rngControllerTable = new Dictionary<RngID, RNGController>
	{
		{
			RngID.BiomeCreation,
			new RNGController(RngID.BiomeCreation)
		},
		{
			RngID.MergeRooms,
			new RNGController(RngID.MergeRooms)
		},
		{
			RngID.Chest,
			new RNGController(RngID.Chest)
		},
		{
			RngID.Enemy,
			new RNGController(RngID.Enemy)
		},
		{
			RngID.Lineage,
			new RNGController(RngID.Lineage)
		},
		{
			RngID.Prop,
			new RNGController(RngID.Prop)
		},
		{
			RngID.SpecialRoomProps,
			new RNGController(RngID.SpecialRoomProps)
		},
		{
			RngID.Room_RNGSeed,
			new RNGController(RngID.Room_RNGSeed)
		},
		{
			RngID.Room_RNGSeed_Generator,
			new RNGController(RngID.Room_RNGSeed_Generator)
		},
		{
			RngID.Prop_RoomSeed,
			new RNGController(RngID.Prop_RoomSeed)
		},
		{
			RngID.Deco_RoomSeed,
			new RNGController(RngID.Deco_RoomSeed)
		},
		{
			RngID.Enemy_RoomSeed,
			new RNGController(RngID.Enemy_RoomSeed)
		},
		{
			RngID.Chest_RoomSeed,
			new RNGController(RngID.Chest_RoomSeed)
		},
		{
			RngID.SpecialProps_RoomSeed,
			new RNGController(RngID.SpecialProps_RoomSeed)
		},
		{
			RngID.Hazards_RoomSeed,
			new RNGController(RngID.Hazards_RoomSeed)
		},
		{
			RngID.Tunnel_RoomSeed,
			new RNGController(RngID.Tunnel_RoomSeed)
		}
	};
}
