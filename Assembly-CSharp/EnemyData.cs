using System;
using UnityEngine;

// Token: 0x02000B7C RID: 2940
[CreateAssetMenu(menuName = "Custom/XML Processing Behaviours/Enemy Data (Test)")]
public class EnemyData : ScriptableObject
{
	// Token: 0x060058FF RID: 22783 RVA: 0x001523A4 File Offset: 0x001505A4
	public void SetSpawnInBiome(BiomeType biome, bool value)
	{
		if (biome <= BiomeType.DriftHouse)
		{
			if (biome <= BiomeType.Cave)
			{
				if (biome == BiomeType.Editor)
				{
					this.SpawnInCastle = value;
					return;
				}
				if (biome == BiomeType.Castle)
				{
					this.SpawnInCastle = value;
					return;
				}
				if (biome != BiomeType.Cave)
				{
					return;
				}
				this.SpawnInCave = value;
				return;
			}
			else if (biome <= BiomeType.Forest)
			{
				if (biome == BiomeType.Dragon)
				{
					this.SpawnInDragon = value;
					return;
				}
				if (biome != BiomeType.Forest)
				{
					return;
				}
				this.SpawnInForest = value;
				return;
			}
			else
			{
				if (biome == BiomeType.Garden)
				{
					this.SpawnInGarden = value;
					return;
				}
				if (biome != BiomeType.DriftHouse)
				{
					return;
				}
				this.SpawnInHome = value;
				return;
			}
		}
		else if (biome <= BiomeType.Study)
		{
			if (biome == BiomeType.Lake)
			{
				this.SpawnInLake = value;
				return;
			}
			if (biome == BiomeType.Stone)
			{
				this.SpawnInStone = value;
				return;
			}
			if (biome != BiomeType.Study)
			{
				return;
			}
			this.SpawnInStudy = value;
			return;
		}
		else if (biome <= BiomeType.Tower)
		{
			if (biome == BiomeType.Sunken)
			{
				this.SpawnInSunken = value;
				return;
			}
			if (biome != BiomeType.Tower)
			{
				return;
			}
			this.SpawnInTower = value;
			return;
		}
		else
		{
			if (biome == BiomeType.TowerExterior)
			{
				this.SpawnInTowerExterior = value;
				return;
			}
			if (biome != BiomeType.Town)
			{
				return;
			}
			this.SpawnInTown = value;
			return;
		}
	}

	// Token: 0x06005900 RID: 22784 RVA: 0x00152494 File Offset: 0x00150694
	public bool GetSpawnInBiome(BiomeType biome)
	{
		if (biome <= BiomeType.DriftHouse)
		{
			if (biome <= BiomeType.Cave)
			{
				if (biome == BiomeType.Editor)
				{
					return this.SpawnInCastle;
				}
				if (biome == BiomeType.Castle)
				{
					return this.SpawnInCastle;
				}
				if (biome == BiomeType.Cave)
				{
					return this.SpawnInCave;
				}
			}
			else if (biome <= BiomeType.Forest)
			{
				if (biome == BiomeType.Dragon)
				{
					return this.SpawnInDragon;
				}
				if (biome == BiomeType.Forest)
				{
					return this.SpawnInForest;
				}
			}
			else
			{
				if (biome == BiomeType.Garden)
				{
					return this.SpawnInGarden;
				}
				if (biome == BiomeType.DriftHouse)
				{
					return this.SpawnInHome;
				}
			}
		}
		else if (biome <= BiomeType.Study)
		{
			if (biome == BiomeType.Lake)
			{
				return this.SpawnInLake;
			}
			if (biome == BiomeType.Stone)
			{
				return this.SpawnInStone;
			}
			if (biome == BiomeType.Study)
			{
				return this.SpawnInStudy;
			}
		}
		else if (biome <= BiomeType.Tower)
		{
			if (biome == BiomeType.Sunken)
			{
				return this.SpawnInSunken;
			}
			if (biome == BiomeType.Tower)
			{
				return this.SpawnInTower;
			}
		}
		else
		{
			if (biome == BiomeType.TowerExterior)
			{
				return this.SpawnInTowerExterior;
			}
			if (biome == BiomeType.Town)
			{
				return this.SpawnInTown;
			}
		}
		return false;
	}

	// Token: 0x04004212 RID: 16914
	public float SummonValue;

	// Token: 0x04004213 RID: 16915
	public int Health;

	// Token: 0x04004214 RID: 16916
	public int WeaponDamage;

	// Token: 0x04004215 RID: 16917
	public int MagicDamage;

	// Token: 0x04004216 RID: 16918
	public float Speed;

	// Token: 0x04004217 RID: 16919
	public float TurnSpeed;

	// Token: 0x04004218 RID: 16920
	public float RestMinCooldown;

	// Token: 0x04004219 RID: 16921
	public float RestMaxCooldown;

	// Token: 0x0400421A RID: 16922
	public bool IsFlying;

	// Token: 0x0400421B RID: 16923
	public bool AlwaysFace;

	// Token: 0x0400421C RID: 16924
	public bool FallLedge;

	// Token: 0x0400421D RID: 16925
	public bool CollidesWithPlatforms;

	// Token: 0x0400421E RID: 16926
	public int StunDefence;

	// Token: 0x0400421F RID: 16927
	public int KnockbackDefence;

	// Token: 0x04004220 RID: 16928
	public float StunDuration;

	// Token: 0x04004221 RID: 16929
	public float KnockbackModX;

	// Token: 0x04004222 RID: 16930
	public float KnockbackModY;

	// Token: 0x04004223 RID: 16931
	public float Scale;

	// Token: 0x04004224 RID: 16932
	public float AnimationSpeed;

	// Token: 0x04004225 RID: 16933
	public int GoldMin;

	// Token: 0x04004226 RID: 16934
	public int GoldMax;

	// Token: 0x04004227 RID: 16935
	public float DropOdds;

	// Token: 0x04004228 RID: 16936
	public float MeleeRadius;

	// Token: 0x04004229 RID: 16937
	public float ProjectileRadius;

	// Token: 0x0400422A RID: 16938
	public float FarRadius;

	// Token: 0x0400422B RID: 16939
	[Space]
	[Header("Spawn in Biome")]
	[Space]
	public bool SpawnInCastle;

	// Token: 0x0400422C RID: 16940
	public bool SpawnInCave;

	// Token: 0x0400422D RID: 16941
	public bool SpawnInDragon;

	// Token: 0x0400422E RID: 16942
	public bool SpawnInForest;

	// Token: 0x0400422F RID: 16943
	public bool SpawnInGarden;

	// Token: 0x04004230 RID: 16944
	public bool SpawnInHome;

	// Token: 0x04004231 RID: 16945
	public bool SpawnInLake;

	// Token: 0x04004232 RID: 16946
	public bool SpawnInStone;

	// Token: 0x04004233 RID: 16947
	public bool SpawnInStudy;

	// Token: 0x04004234 RID: 16948
	public bool SpawnInSunken;

	// Token: 0x04004235 RID: 16949
	public bool SpawnInTower;

	// Token: 0x04004236 RID: 16950
	public bool SpawnInTowerExterior;

	// Token: 0x04004237 RID: 16951
	public bool SpawnInTown;

	// Token: 0x04004238 RID: 16952
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04004239 RID: 16953
	public string MoveList;

	// Token: 0x0400423A RID: 16954
	public string Description01;

	// Token: 0x0400423B RID: 16955
	public string Description02;
}
