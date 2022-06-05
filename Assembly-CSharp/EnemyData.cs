using System;
using UnityEngine;

// Token: 0x020006D1 RID: 1745
[CreateAssetMenu(menuName = "Custom/XML Processing Behaviours/Enemy Data (Test)")]
public class EnemyData : ScriptableObject
{
	// Token: 0x06003FC8 RID: 16328 RVA: 0x000E2688 File Offset: 0x000E0888
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

	// Token: 0x06003FC9 RID: 16329 RVA: 0x000E2778 File Offset: 0x000E0978
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

	// Token: 0x04002FC3 RID: 12227
	public float SummonValue;

	// Token: 0x04002FC4 RID: 12228
	public int Health;

	// Token: 0x04002FC5 RID: 12229
	public int WeaponDamage;

	// Token: 0x04002FC6 RID: 12230
	public int MagicDamage;

	// Token: 0x04002FC7 RID: 12231
	public float Speed;

	// Token: 0x04002FC8 RID: 12232
	public float TurnSpeed;

	// Token: 0x04002FC9 RID: 12233
	public float RestMinCooldown;

	// Token: 0x04002FCA RID: 12234
	public float RestMaxCooldown;

	// Token: 0x04002FCB RID: 12235
	public bool IsFlying;

	// Token: 0x04002FCC RID: 12236
	public bool AlwaysFace;

	// Token: 0x04002FCD RID: 12237
	public bool FallLedge;

	// Token: 0x04002FCE RID: 12238
	public bool CollidesWithPlatforms;

	// Token: 0x04002FCF RID: 12239
	public int StunDefence;

	// Token: 0x04002FD0 RID: 12240
	public int KnockbackDefence;

	// Token: 0x04002FD1 RID: 12241
	public float StunDuration;

	// Token: 0x04002FD2 RID: 12242
	public float KnockbackModX;

	// Token: 0x04002FD3 RID: 12243
	public float KnockbackModY;

	// Token: 0x04002FD4 RID: 12244
	public float Scale;

	// Token: 0x04002FD5 RID: 12245
	public float AnimationSpeed;

	// Token: 0x04002FD6 RID: 12246
	public int GoldMin;

	// Token: 0x04002FD7 RID: 12247
	public int GoldMax;

	// Token: 0x04002FD8 RID: 12248
	public float DropOdds;

	// Token: 0x04002FD9 RID: 12249
	public float MeleeRadius;

	// Token: 0x04002FDA RID: 12250
	public float ProjectileRadius;

	// Token: 0x04002FDB RID: 12251
	public float FarRadius;

	// Token: 0x04002FDC RID: 12252
	[Space]
	[Header("Spawn in Biome")]
	[Space]
	public bool SpawnInCastle;

	// Token: 0x04002FDD RID: 12253
	public bool SpawnInCave;

	// Token: 0x04002FDE RID: 12254
	public bool SpawnInDragon;

	// Token: 0x04002FDF RID: 12255
	public bool SpawnInForest;

	// Token: 0x04002FE0 RID: 12256
	public bool SpawnInGarden;

	// Token: 0x04002FE1 RID: 12257
	public bool SpawnInHome;

	// Token: 0x04002FE2 RID: 12258
	public bool SpawnInLake;

	// Token: 0x04002FE3 RID: 12259
	public bool SpawnInStone;

	// Token: 0x04002FE4 RID: 12260
	public bool SpawnInStudy;

	// Token: 0x04002FE5 RID: 12261
	public bool SpawnInSunken;

	// Token: 0x04002FE6 RID: 12262
	public bool SpawnInTower;

	// Token: 0x04002FE7 RID: 12263
	public bool SpawnInTowerExterior;

	// Token: 0x04002FE8 RID: 12264
	public bool SpawnInTown;

	// Token: 0x04002FE9 RID: 12265
	[Space]
	[Header("Text Fields")]
	[Space]
	public string Title;

	// Token: 0x04002FEA RID: 12266
	public string MoveList;

	// Token: 0x04002FEB RID: 12267
	public string Description01;

	// Token: 0x04002FEC RID: 12268
	public string Description02;
}
