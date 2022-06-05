using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public static class BiomeUtility
{
	// Token: 0x06001223 RID: 4643 RVA: 0x000346C8 File Offset: 0x000328C8
	public static bool IsBiomeInBiomeLayerMask(BiomeType biome, BiomeLayer biomeLayerMask)
	{
		if (BiomeUtility.m_biomeTypeToLayerTable.ContainsKey(biome))
		{
			return (BiomeUtility.m_biomeTypeToLayerTable[biome] & biomeLayerMask) != BiomeLayer.None;
		}
		Debug.LogWarningFormat("{0}: Biome ({1}) is not in Table", new object[]
		{
			Time.frameCount,
			biome
		});
		return false;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x0003471C File Offset: 0x0003291C
	public static BiomeType GetBiomeType(BiomeLayer biomeLayer)
	{
		if (BiomeUtility.m_biomeLayerToTypeTable.ContainsKey(biomeLayer))
		{
			return BiomeUtility.m_biomeLayerToTypeTable[biomeLayer];
		}
		throw new ArgumentException("biomeLayer", string.Format("Table doesn't contain an entry with Key ({0})", biomeLayer));
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x00034751 File Offset: 0x00032951
	public static BiomeLayer GetBiomeLayer(BiomeType biomeType)
	{
		if (BiomeUtility.m_biomeTypeToLayerTable.ContainsKey(biomeType))
		{
			return BiomeUtility.m_biomeTypeToLayerTable[biomeType];
		}
		throw new ArgumentException("biomeType", string.Format("Table doesn't contain an entry with Key ({0})", biomeType));
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x00034788 File Offset: 0x00032988
	public static List<BiomeType> GetAllBiomesThatConnectToBiome(BiomeType biome)
	{
		if (BiomeUtility.m_biomeToBiomesThatConnectTable == null)
		{
			BiomeUtility.m_biomeToBiomesThatConnectTable = new Dictionary<BiomeType, List<BiomeType>>();
		}
		if (!BiomeUtility.m_biomeToBiomesThatConnectTable.ContainsKey(biome))
		{
			BiomeUtility.m_biomeToBiomesThatConnectTable.Add(biome, new List<BiomeType>());
			foreach (object obj in Enum.GetValues(typeof(BiomeType)))
			{
				BiomeType biomeType = (BiomeType)obj;
				if (BiomeUtility.IsConnectsToBiome(biomeType, biome))
				{
					BiomeUtility.m_biomeToBiomesThatConnectTable[biome].Add(biomeType);
				}
			}
		}
		return BiomeUtility.m_biomeToBiomesThatConnectTable[biome];
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00034838 File Offset: 0x00032A38
	public static bool IsConnectsToBiome(BiomeType biome, BiomeType connectsTo)
	{
		if (biome == BiomeType.None || biome == BiomeType.Any)
		{
			return false;
		}
		BiomeData data = BiomeDataLibrary.GetData(biome);
		if (data)
		{
			return data.ConnectsTo == connectsTo;
		}
		if (BiomeType_RL.IsValidBiome(biome))
		{
			Debug.LogFormat("Failed to find BiomeData for Biome ({0})", new object[]
			{
				biome
			});
		}
		return false;
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x00034890 File Offset: 0x00032A90
	public static BiomeType GetConnectsToBiome(BiomeType biome)
	{
		if (biome == BiomeType.None || biome == BiomeType.Any)
		{
			throw new ArgumentException("biome");
		}
		BiomeData data = BiomeDataLibrary.GetData(biome);
		if (data)
		{
			return data.ConnectsTo;
		}
		Debug.LogFormat("Failed to find BiomeData for Biome ({0})", new object[]
		{
			biome
		});
		return BiomeType.None;
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x000348E4 File Offset: 0x00032AE4
	public static RoomSide GetConnectDirection(BiomeType biome)
	{
		if (biome == BiomeType.None || biome == BiomeType.Any)
		{
			throw new ArgumentException("biome");
		}
		BiomeData data = BiomeDataLibrary.GetData(biome);
		if (data)
		{
			return data.ConnectDirection;
		}
		throw new ArgumentException(string.Format("Failed to find BiomeData for Biome ({0})", biome));
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x00034934 File Offset: 0x00032B34
	public static List<SpecialRoomType> GetPotentialSpecialRoomTypesInBiome(BiomeType biome)
	{
		List<SpecialRoomType> list = new List<SpecialRoomType>();
		BiomeData data = BiomeDataLibrary.GetData(biome);
		if (data.BonusRoomWeights != null && data.BonusRoomWeights.Length != 0)
		{
			for (int i = 0; i < data.BonusRoomWeights.Length; i++)
			{
				if (data.BonusRoomWeights[i].Weight != 0 && !list.Contains(data.BonusRoomWeights[i].Type))
				{
					list.Add(data.BonusRoomWeights[i].Type);
				}
			}
		}
		foreach (SpecialRoomType specialRoomType in RoomType_RL.SpecialRoomTypeArray)
		{
			if (BiomeCreation_EV.GetDefaultSpecialRoomWeight(specialRoomType) != 0 && !list.Contains(specialRoomType))
			{
				list.Add(specialRoomType);
			}
		}
		if (!BiomeUtility.m_biomeToPotentialSpecialRoomTypesTable.ContainsKey(biome))
		{
			BiomeUtility.m_biomeToPotentialSpecialRoomTypesTable.Add(biome, list);
		}
		return BiomeUtility.m_biomeToPotentialSpecialRoomTypesTable[biome];
	}

	// Token: 0x0400129D RID: 4765
	private static Dictionary<BiomeType, BiomeLayer> m_biomeTypeToLayerTable = new Dictionary<BiomeType, BiomeLayer>
	{
		{
			BiomeType.None,
			BiomeLayer.None
		},
		{
			BiomeType.Any,
			BiomeLayer.Any
		},
		{
			BiomeType.Castle,
			BiomeLayer.Castle
		},
		{
			BiomeType.Cave,
			BiomeLayer.Cave
		},
		{
			BiomeType.Dragon,
			BiomeLayer.Dragon
		},
		{
			BiomeType.Forest,
			BiomeLayer.Forest
		},
		{
			BiomeType.Garden,
			BiomeLayer.Garden
		},
		{
			BiomeType.DriftHouse,
			BiomeLayer.DriftHouse
		},
		{
			BiomeType.HubTown,
			BiomeLayer.HubTown
		},
		{
			BiomeType.Lake,
			BiomeLayer.Lake
		},
		{
			BiomeType.Stone,
			BiomeLayer.Stone
		},
		{
			BiomeType.Study,
			BiomeLayer.Study
		},
		{
			BiomeType.Sunken,
			BiomeLayer.Sunken
		},
		{
			BiomeType.Tower,
			BiomeLayer.Tower
		},
		{
			BiomeType.TowerExterior,
			BiomeLayer.Tower
		},
		{
			BiomeType.Town,
			BiomeLayer.Town
		},
		{
			BiomeType.Tutorial,
			BiomeLayer.Tutorial
		}
	};

	// Token: 0x0400129E RID: 4766
	private static Dictionary<BiomeLayer, BiomeType> m_biomeLayerToTypeTable = new Dictionary<BiomeLayer, BiomeType>
	{
		{
			BiomeLayer.None,
			BiomeType.None
		},
		{
			BiomeLayer.Any,
			BiomeType.Any
		},
		{
			BiomeLayer.Castle,
			BiomeType.Castle
		},
		{
			BiomeLayer.Cave,
			BiomeType.Cave
		},
		{
			BiomeLayer.Dragon,
			BiomeType.Dragon
		},
		{
			BiomeLayer.Forest,
			BiomeType.Forest
		},
		{
			BiomeLayer.Garden,
			BiomeType.Garden
		},
		{
			BiomeLayer.DriftHouse,
			BiomeType.DriftHouse
		},
		{
			BiomeLayer.HubTown,
			BiomeType.HubTown
		},
		{
			BiomeLayer.Lake,
			BiomeType.Lake
		},
		{
			BiomeLayer.Stone,
			BiomeType.Stone
		},
		{
			BiomeLayer.Study,
			BiomeType.Study
		},
		{
			BiomeLayer.Sunken,
			BiomeType.Sunken
		},
		{
			BiomeLayer.Tower,
			BiomeType.Tower
		},
		{
			BiomeLayer.Town,
			BiomeType.Town
		},
		{
			BiomeLayer.Tutorial,
			BiomeType.Tutorial
		}
	};

	// Token: 0x0400129F RID: 4767
	private static Dictionary<BiomeType, List<BiomeType>> m_biomeToBiomesThatConnectTable = null;

	// Token: 0x040012A0 RID: 4768
	private static Dictionary<BiomeType, List<SpecialRoomType>> m_biomeToPotentialSpecialRoomTypesTable = new Dictionary<BiomeType, List<SpecialRoomType>>();
}
