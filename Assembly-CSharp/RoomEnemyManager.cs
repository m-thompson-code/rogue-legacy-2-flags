using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020007CB RID: 1995
public class RoomEnemyManager : MonoBehaviour, IRoomConsumer, ILevelConsumer, ISetSpawnType
{
	// Token: 0x1700168B RID: 5771
	// (get) Token: 0x06003D6B RID: 15723 RVA: 0x00021F8C File Offset: 0x0002018C
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

	// Token: 0x1700168C RID: 5772
	// (get) Token: 0x06003D6C RID: 15724 RVA: 0x00021FAD File Offset: 0x000201AD
	public BaseRoom Room
	{
		get
		{
			return this.m_room;
		}
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x00021FB5 File Offset: 0x000201B5
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

	// Token: 0x1700168D RID: 5773
	// (get) Token: 0x06003D6E RID: 15726 RVA: 0x00021FDD File Offset: 0x000201DD
	public bool HasRequiredReferences
	{
		get
		{
			return this.Room != null && this.Level != -1;
		}
	}

	// Token: 0x1700168E RID: 5774
	// (get) Token: 0x06003D6F RID: 15727 RVA: 0x00021FFB File Offset: 0x000201FB
	// (set) Token: 0x06003D70 RID: 15728 RVA: 0x00022003 File Offset: 0x00020203
	public bool IsInitialised { get; private set; }

	// Token: 0x1700168F RID: 5775
	// (get) Token: 0x06003D71 RID: 15729 RVA: 0x0002200C File Offset: 0x0002020C
	public int Level
	{
		get
		{
			return this.m_level;
		}
	}

	// Token: 0x06003D72 RID: 15730 RVA: 0x00022014 File Offset: 0x00020214
	public void SetLevel(int value)
	{
		this.m_level = value;
		if (this.HasRequiredReferences)
		{
			this.Initialise();
		}
	}

	// Token: 0x06003D73 RID: 15731 RVA: 0x0002202B File Offset: 0x0002022B
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

	// Token: 0x06003D74 RID: 15732 RVA: 0x000F8A94 File Offset: 0x000F6C94
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

	// Token: 0x06003D75 RID: 15733 RVA: 0x000F8BE8 File Offset: 0x000F6DE8
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

	// Token: 0x06003D76 RID: 15734 RVA: 0x000F8C5C File Offset: 0x000F6E5C
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

	// Token: 0x06003D77 RID: 15735 RVA: 0x000F8D24 File Offset: 0x000F6F24
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

	// Token: 0x04003073 RID: 12403
	private int m_level = -1;

	// Token: 0x04003074 RID: 12404
	private BaseRoom m_room;

	// Token: 0x04003076 RID: 12406
	private static List<EnemySpawnController> m_enemySpawnerListHelper_STATIC = new List<EnemySpawnController>();
}
