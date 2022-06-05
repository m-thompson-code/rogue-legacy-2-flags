using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020003FF RID: 1023
[CreateAssetMenu(menuName = "Custom/Libraries/RelicLibrary")]
public class RelicLibrary : ScriptableObject
{
	// Token: 0x17000E78 RID: 3704
	// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000116BA File Offset: 0x0000F8BA
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

	// Token: 0x060020D9 RID: 8409 RVA: 0x000A5D78 File Offset: 0x000A3F78
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

	// Token: 0x060020DA RID: 8410 RVA: 0x000A5DCC File Offset: 0x000A3FCC
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

	// Token: 0x060020DB RID: 8411 RVA: 0x000A5F94 File Offset: 0x000A4194
	public static bool IsRelicAllowed(RelicType relicType)
	{
		if (relicType == RelicType.None)
		{
			return false;
		}
		RelicData relicData = RelicLibrary.GetRelicData(relicType);
		return !(relicData == null) && relicData.Rarity <= 3;
	}

	// Token: 0x04001DB5 RID: 7605
	public const string RESOURCES_PATH = "Scriptable Objects/Libraries/RelicLibrary";

	// Token: 0x04001DB6 RID: 7606
	[SerializeField]
	private RelicTypeRelicDataDictionary m_relicLibrary;

	// Token: 0x04001DB7 RID: 7607
	private static RelicLibrary m_instance = null;

	// Token: 0x04001DB8 RID: 7608
	private static List<RelicType> m_rarityOneRelicHelper = new List<RelicType>();

	// Token: 0x04001DB9 RID: 7609
	private static List<RelicType> m_rarityTwoRelicHelper = new List<RelicType>();

	// Token: 0x04001DBA RID: 7610
	private static List<RelicType> m_rarityThreeRelicHelper = new List<RelicType>();
}
