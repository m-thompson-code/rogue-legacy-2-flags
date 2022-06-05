using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000242 RID: 578
[CreateAssetMenu(menuName = "Custom/Libraries/RelicLibrary")]
public class RelicLibrary : ScriptableObject
{
	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06001725 RID: 5925 RVA: 0x0004807E File Offset: 0x0004627E
	private static RelicLibrary Instance
	{
		get
		{
			if (RelicLibrary.m_instance == null)
			{
				RelicLibrary.m_instance = CDGResources.Load<RelicLibrary>("Scriptable Objects/Libraries/RelicLibrary", "", true);
			}
			return RelicLibrary.m_instance;
		}
	}

	// Token: 0x06001726 RID: 5926 RVA: 0x000480A8 File Offset: 0x000462A8
	public static RelicData GetRelicData(RelicType relicType)
	{
		RelicData result;
		if (RelicLibrary.Instance.m_relicLibrary.TryGetValue(relicType, out result))
		{
			return result;
		}
		Debug.LogWarningFormat("<color=red>{0}: ({1}) Could not find RelicData ({2}) in Relic Library.</color>", new object[]
		{
			Time.frameCount,
			RelicLibrary.Instance,
			relicType
		});
		return null;
	}

	// Token: 0x06001727 RID: 5927 RVA: 0x000480FC File Offset: 0x000462FC
	public static RelicType GetRandomRelic(RngID rngIDToUse, bool limitMaxStacks, IEnumerable<RelicType> exceptionList = null)
	{
		RelicType relicType = RelicType.None;
		RelicLibrary.m_rarityOneRelicHelper.Clear();
		RelicLibrary.m_rarityTwoRelicHelper.Clear();
		RelicLibrary.m_rarityThreeRelicHelper.Clear();
		foreach (RelicType relicType2 in RelicType_RL.TypeArray)
		{
			if (relicType2 != RelicType.None && (exceptionList == null || !exceptionList.Contains(relicType2)))
			{
				RelicData relicData = RelicLibrary.GetRelicData(relicType2);
				if (relicData && (!limitMaxStacks || SaveManager.PlayerSaveData.GetRelic(relicType2).Level < relicData.MaxStack))
				{
					switch (relicData.Rarity)
					{
					case 1:
						RelicLibrary.m_rarityOneRelicHelper.Add(relicType2);
						break;
					case 2:
						RelicLibrary.m_rarityTwoRelicHelper.Add(relicType2);
						break;
					case 3:
						RelicLibrary.m_rarityThreeRelicHelper.Add(relicType2);
						break;
					}
				}
			}
		}
		float num = 1f;
		float num2;
		if (rngIDToUse == RngID.None)
		{
			num2 = UnityEngine.Random.Range(0f, num);
		}
		else
		{
			num2 = RNGManager.GetRandomNumber(rngIDToUse, "RelicLibrary.GetRandomRelic()", 0f, num);
		}
		List<RelicType> list;
		if (num2 <= 1f)
		{
			list = RelicLibrary.m_rarityOneRelicHelper;
		}
		else if (num2 > 1f && num2 <= 1f)
		{
			list = RelicLibrary.m_rarityTwoRelicHelper;
			if (list.Count == 0)
			{
				list = RelicLibrary.m_rarityOneRelicHelper;
			}
		}
		else
		{
			list = RelicLibrary.m_rarityThreeRelicHelper;
			if (list.Count == 0)
			{
				list = RelicLibrary.m_rarityTwoRelicHelper;
			}
			if (list.Count == 0)
			{
				list = RelicLibrary.m_rarityOneRelicHelper;
			}
		}
		int count = list.Count;
		int num3 = 0;
		while (!RelicLibrary.IsRelicAllowed(relicType))
		{
			int index;
			if (rngIDToUse == RngID.None)
			{
				index = UnityEngine.Random.Range(0, count);
			}
			else
			{
				index = RNGManager.GetRandomNumber(rngIDToUse, "RelicLibrary.GetRandomRelic()", 0, count);
			}
			relicType = list[index];
			num3++;
			if (num3 > 50)
			{
				break;
			}
		}
		if (!RelicLibrary.IsRelicAllowed(relicType))
		{
			Debug.Log("<color=red>Could not find valid random relic.</color>");
		}
		return relicType;
	}

	// Token: 0x06001728 RID: 5928 RVA: 0x000482C4 File Offset: 0x000464C4
	public static bool IsRelicAllowed(RelicType relicType)
	{
		if (relicType == RelicType.None)
		{
			return false;
		}
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		return !(relicData == null) && relicData.Rarity <= 3;
	}

	// Token: 0x0400169D RID: 5789
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RelicLibrary";

	// Token: 0x0400169E RID: 5790
	[SerializeField]
	private RelicTypeRelicDataDictionary m_relicLibrary;

	// Token: 0x0400169F RID: 5791
	private static RelicLibrary m_instance = null;

	// Token: 0x040016A0 RID: 5792
	private static List<RelicType> m_rarityOneRelicHelper = new List<RelicType>();

	// Token: 0x040016A1 RID: 5793
	private static List<RelicType> m_rarityTwoRelicHelper = new List<RelicType>();

	// Token: 0x040016A2 RID: 5794
	private static List<RelicType> m_rarityThreeRelicHelper = new List<RelicType>();
}
