using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000B43 RID: 2883
public class RNGManager : MonoBehaviour
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06005768 RID: 22376 RVA: 0x0014C41C File Offset: 0x0014A61C
	// (remove) Token: 0x06005769 RID: 22377 RVA: 0x0014C450 File Offset: 0x0014A650
	public static event EventHandler<EventArgs> InitialisedEvent;

	// Token: 0x17001D59 RID: 7513
	// (get) Token: 0x0600576A RID: 22378 RVA: 0x0002F97B File Offset: 0x0002DB7B
	// (set) Token: 0x0600576B RID: 22379 RVA: 0x0002F982 File Offset: 0x0002DB82
	private static RNGManager Instance { get; set; }

	// Token: 0x17001D5A RID: 7514
	// (get) Token: 0x0600576C RID: 22380 RVA: 0x0002F98A File Offset: 0x0002DB8A
	public static bool IsInstantiated
	{
		get
		{
			return RNGManager.Instance != null;
		}
	}

	// Token: 0x17001D5B RID: 7515
	// (get) Token: 0x0600576D RID: 22381 RVA: 0x0014C484 File Offset: 0x0014A684
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

	// Token: 0x0600576E RID: 22382 RVA: 0x0014C4DC File Offset: 0x0014A6DC
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

	// Token: 0x0600576F RID: 22383 RVA: 0x0002F997 File Offset: 0x0002DB97
	public static int GetRandomNumber(RngID randomNumberGenerator, string callerDescription, int minInclusive, int maxExclusive)
	{
		if (!RNGManager.m_rngControllerTable[randomNumberGenerator].IsInitialized)
		{
			RNGManager.InitializeRandomNumberGenerators();
		}
		return RNGManager.m_rngControllerTable[randomNumberGenerator].GetRandomNumber(callerDescription, minInclusive, maxExclusive);
	}

	// Token: 0x06005770 RID: 22384 RVA: 0x0002F9C3 File Offset: 0x0002DBC3
	public static float GetRandomNumber(RngID randomNumberGenerator, string callerDescription, float min, float max)
	{
		if (!RNGManager.m_rngControllerTable[randomNumberGenerator].IsInitialized)
		{
			RNGManager.InitializeRandomNumberGenerators();
		}
		return RNGManager.m_rngControllerTable[randomNumberGenerator].GetRandomNumber(callerDescription, min, max);
	}

	// Token: 0x06005771 RID: 22385 RVA: 0x0002F9EF File Offset: 0x0002DBEF
	public static int GetSeed(RngID id)
	{
		if (RNGManager.m_rngControllerTable.ContainsKey(id))
		{
			return RNGManager.m_rngControllerTable[id].Seed;
		}
		return -1;
	}

	// Token: 0x06005772 RID: 22386 RVA: 0x0014C558 File Offset: 0x0014A758
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

	// Token: 0x06005773 RID: 22387 RVA: 0x0014C5A8 File Offset: 0x0014A7A8
	public static void InitializeRandomNumberGenerators(int masterSeed)
	{
		System.Random random = new System.Random(masterSeed);
		foreach (KeyValuePair<RngID, RNGController> keyValuePair in RNGManager.m_rngControllerTable)
		{
			keyValuePair.Value.SetSeed(random.Next(0, RNGManager.Instance.m_maxRngControllerSeed));
		}
	}

	// Token: 0x06005774 RID: 22388 RVA: 0x0014C618 File Offset: 0x0014A818
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

	// Token: 0x06005775 RID: 22389 RVA: 0x0002FA10 File Offset: 0x0002DC10
	private void OnDestroy()
	{
		RNGManager.m_rngControllerTable = null;
	}

	// Token: 0x06005776 RID: 22390 RVA: 0x0014C650 File Offset: 0x0014A850
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

	// Token: 0x06005777 RID: 22391 RVA: 0x0014C750 File Offset: 0x0014A950
	public static void Reset()
	{
		RNGSeedManager.GenerateNewSeed(SceneLoadingUtility.ActiveScene.name);
		RNGManager.InitializeRandomNumberGenerators();
	}

	// Token: 0x06005778 RID: 22392 RVA: 0x0002FA18 File Offset: 0x0002DC18
	public static void Reset(int seedOverride)
	{
		RNGSeedManager.SetTempSeed(seedOverride.ToString("X"));
		RNGManager.Reset();
	}

	// Token: 0x0400408E RID: 16526
	[SerializeField]
	[FormerlySerializedAs("m_maxRandomNumber")]
	private int m_maxRngControllerSeed = 1000;

	// Token: 0x0400408F RID: 16527
	private static List<RNGController> m_rngControllers = null;

	// Token: 0x04004090 RID: 16528
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
