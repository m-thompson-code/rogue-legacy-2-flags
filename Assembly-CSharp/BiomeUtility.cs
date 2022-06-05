using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000335 RID: 821
public static class BiomeUtility
{
	// Token: 0x06001A81 RID: 6785 RVA: 0x00092034 File Offset: 0x00090234
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

	// Token: 0x06001A82 RID: 6786 RVA: 0x0000D9F5 File Offset: 0x0000BBF5
	public static BiomeType GetBiomeType(BiomeLayer biomeLayer)
	{
		if (BiomeUtility.m_biomeLayerToTypeTable.ContainsKey(biomeLayer))
		{
			return BiomeUtility.m_biomeLayerToTypeTable[biomeLayer];
		}
		throw new ArgumentException("biomeLayer", string.Format("Table doesn't contain an entry with Key ({0})", biomeLayer));
	}

	// Token: 0x06001A83 RID: 6787 RVA: 0x0000DA2A File Offset: 0x0000BC2A
	public static BiomeLayer GetBiomeLayer(BiomeType biomeType)
	{
		if (BiomeUtility.m_biomeTypeToLayerTable.ContainsKey(biomeType))
		{
			return BiomeUtility.m_biomeTypeToLayerTable[biomeType];
		}
		throw new ArgumentException("biomeType", string.Format("Table doesn't contain an entry with Key ({0})", biomeType));
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x00092088 File Offset: 0x00090288
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

	// Token: 0x06001A85 RID: 6789 RVA: 0x00092138 File Offset: 0x00090338
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

	// Token: 0x06001A86 RID: 6790 RVA: 0x00092190 File Offset: 0x00090390
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

	// Token: 0x06001A87 RID: 6791 RVA: 0x000921E4 File Offset: 0x000903E4
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

	// Token: 0x06001A88 RID: 6792 RVA: 0x00092234 File Offset: 0x00090434
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

	// Token: 0x040018B3 RID: 6323
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

	// Token: 0x040018B4 RID: 6324
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

	// Token: 0x040018B5 RID: 6325
	private static Dictionary<BiomeType, List<BiomeType>> m_biomeToBiomesThatConnectTable = null;

	// Token: 0x040018B6 RID: 6326
	private static Dictionary<BiomeType, List<SpecialRoomType>> m_biomeToPotentialSpecialRoomTypesTable = new Dictionary<BiomeType, List<SpecialRoomType>>();
}
