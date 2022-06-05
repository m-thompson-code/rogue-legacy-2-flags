using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020004B4 RID: 1204
public class RoomEnemyManager : MonoBehaviour, IRoomConsumer, ILevelConsumer, ISetSpawnType
{
	// Token: 0x17001124 RID: 4388
	// (get) Token: 0x06002CC2 RID: 11458 RVA: 0x000978DC File Offset: 0x00095ADC
	public EnemySpawnController[] EnemySpawnControllers
	{
		get
		{
			if (!this.Room.IsNativeNull())
			{
				return this.Room.SpawnControllerManager.EnemySpawnControllers;
			}
			return null;
		}
	}

	// Token: 0x17001125 RID: 4389
	// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000978FD File Offset: 0x00095AFD
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x00097905 File Offset: 0x00095B05
	public void SetRoom(BaseRoom room)
	{
		if (room is MergeRoom)
		{
			return;
		}
		this.m_room = room;
		if (room is Room && this.HasRequiredReferences)
		{
			this.Initialise();
		}
	}

	// Token: 0x17001126 RID: 4390
	// (get) Token: 0x06002CC5 RID: 11461 RVA: 0x0009792D File Offset: 0x00095B2D
	public bool HasRequiredReferences
	{
		get
		{
			return this.Room != null && this.Level != -1;
		}
	}

	// Token: 0x17001127 RID: 4391
	// (get) Token: 0x06002CC6 RID: 11462 RVA: 0x0009794B File Offset: 0x00095B4B
	// (set) Token: 0x06002CC7 RID: 11463 RVA: 0x00097953 File Offset: 0x00095B53
	public bool IsInitialised { get; private set; }

	// Token: 0x17001128 RID: 4392
	// (get) Token: 0x06002CC8 RID: 11464 RVA: 0x0009795C File Offset: 0x00095B5C
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x00097964 File Offset: 0x00095B64
	public void SetLevel(int value)
	{
		this.m_level = value;
		if (this.HasRequiredReferences)
		{
			this.Initialise();
		}
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x0009797B File Offset: 0x00095B7B
	private void Initialise()
	{
		if (this.Room.IsNativeNull())
		{
			throw new ArgumentNullException("Room");
		}
		if (this.IsInitialised)
		{
			return;
		}
		if (GameUtility.IsInLevelEditor)
		{
			this.SetSpawnType();
		}
		this.IsInitialised = true;
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000979B4 File Offset: 0x00095BB4
	public void SetSpawnType()
	{
		Room room = this.Room as Room;
		if (room)
		{
			BiomeType biomeType = room.BiomeType;
			if (biomeType != BiomeType.Any && biomeType != BiomeType.None)
			{
				using (IEnumerator<IGrouping<int, EnemySpawnController>> enumerator = (from entry in this.EnemySpawnControllers
				group entry by entry.ID).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IGrouping<int, EnemySpawnController> enemySpawnerGroup = enumerator.Current;
						List<EnemySpawnController> enemySpawnerList = this.GetEnemySpawnerList(enemySpawnerGroup, false, false);
						this.SetupEnemySpawnControllers(enemySpawnerList, biomeType, false, false, this.Room.RoomType);
						List<EnemySpawnController> enemySpawnerList2 = this.GetEnemySpawnerList(enemySpawnerGroup, false, true);
						this.SetupEnemySpawnControllers(enemySpawnerList2, biomeType, false, true, this.Room.RoomType);
						List<EnemySpawnController> enemySpawnerList3 = this.GetEnemySpawnerList(enemySpawnerGroup, true, false);
						this.SetupEnemySpawnControllers(enemySpawnerList3, biomeType, true, false, this.Room.RoomType);
						List<EnemySpawnController> enemySpawnerList4 = this.GetEnemySpawnerList(enemySpawnerGroup, true, true);
						this.SetupEnemySpawnControllers(enemySpawnerList4, biomeType, true, true, this.Room.RoomType);
					}
					return;
				}
			}
			Debug.LogFormat("<color=red>({0}) Room Biome ({1}) is invalid</color>", new object[]
			{
				this,
				biomeType
			});
			return;
		}
		throw new InvalidCastException("{0}: Can't cast Room as Room", Time.frameCount);
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x00097B08 File Offset: 0x00095D08
	private List<EnemySpawnController> GetEnemySpawnerList(IGrouping<int, EnemySpawnController> enemySpawnerGroup, bool commanderOnly, bool flyingOnly)
	{
		RoomEnemyManager.m_enemySpawnerListHelper_STATIC.Clear();
		foreach (EnemySpawnController enemySpawnController in enemySpawnerGroup)
		{
			if ((!flyingOnly || enemySpawnController.ForceFlying) && (!commanderOnly || enemySpawnController.ForceCommander))
			{
				RoomEnemyManager.m_enemySpawnerListHelper_STATIC.Add(enemySpawnController);
			}
		}
		return RoomEnemyManager.m_enemySpawnerListHelper_STATIC;
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x00097B7C File Offset: 0x00095D7C
	private void SetupEnemySpawnControllers(List<EnemySpawnController> enemySpawnControllers, BiomeType biome, bool commanderOnly, bool flyingOnly, RoomType roomType)
	{
		if (enemySpawnControllers.Count > 0)
		{
			EnemyTypeAndRank randomEnemyTypeAndRank = this.GetRandomEnemyTypeAndRank(biome, flyingOnly, commanderOnly, roomType);
			for (int i = 0; i < enemySpawnControllers.Count; i++)
			{
				bool flag = BurdenManager.GetBurdenLevel(BurdenType.EnemyEvolve) > 0;
				bool flag2 = false;
				if (flag && randomEnemyTypeAndRank.Rank < EnemyRank.Expert && roomType != RoomType.Fairy)
				{
					float burdenStatGain = BurdenManager.GetBurdenStatGain(BurdenType.EnemyEvolve);
					if (RNGManager.GetRandomNumber(RngID.Enemy_RoomSeed, "(RoomEnemyManager) GetRandomEnemyTypeAndRank.CanEvolve", 0f, 1f) < burdenStatGain)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					enemySpawnControllers[i].SetEnemy(randomEnemyTypeAndRank.Type, randomEnemyTypeAndRank.Rank + 1, this.Level);
				}
				else
				{
					enemySpawnControllers[i].SetEnemy(randomEnemyTypeAndRank.Type, randomEnemyTypeAndRank.Rank, this.Level);
				}
			}
		}
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x00097C44 File Offset: 0x00095E44
	private EnemyTypeAndRank GetRandomEnemyTypeAndRank(BiomeType biome, bool flyingOnly, bool commanderOnly, RoomType roomType)
	{
		EnemyTypeAndRank result = new EnemyTypeAndRank(EnemyType.None, EnemyRank.None);
		List<EnemyTypeAndRank> allEnemiesInBiome = EnemyUtility.GetAllEnemiesInBiome(biome, flyingOnly);
		for (int i = 0; i < allEnemiesInBiome.Count; i++)
		{
			if (allEnemiesInBiome[i].Type == EnemyType.BouncySpike && (roomType == RoomType.Fairy || allEnemiesInBiome[i].Rank == EnemyRank.Expert))
			{
				allEnemiesInBiome.RemoveAt(i);
				i--;
			}
		}
		if (allEnemiesInBiome.Count > 0)
		{
			int randomNumber = RNGManager.GetRandomNumber(RngID.Enemy_RoomSeed, string.Format("(RoomEnemyManager) GetRandomEnemyTypeAndRank", Array.Empty<object>()), 0, allEnemiesInBiome.Count);
			result = allEnemiesInBiome[randomNumber];
		}
		else
		{
			Debug.LogFormat("<color=red>| {0} | No Enemy in Enemy Object Pool matches criteria: Biome = ({1}), Is Flying = ({2})</color>", new object[]
			{
				this,
				biome,
				flyingOnly
			});
		}
		if (commanderOnly && result.Type != EnemyType.BouncySpike)
		{
			return new EnemyTypeAndRank(result.Type, EnemyRank.Expert);
		}
		return result;
	}

	// Token: 0x04002412 RID: 9234
	private int m_level = -1;

	// Token: 0x04002413 RID: 9235
	private BaseRoom m_room;

	// Token: 0x04002415 RID: 9237
	private static List<EnemySpawnController> m_enemySpawnerListHelper_STATIC = new List<EnemySpawnController>();
}
