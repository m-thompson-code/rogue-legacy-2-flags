using System;
using System.Collections.Generic;

// Token: 0x020004C3 RID: 1219
[Serializable]
public class RuneLoadout
{
	// Token: 0x0600274D RID: 10061 RVA: 0x000B8BFC File Offset: 0x000B6DFC
	public void LoadLoadout()
	{
		foreach (RuneType runeType in RuneType_RL.TypeArray)
		{
			if (runeType != RuneType.None)
			{
				RuneManager.SetRuneEquippedLevel(runeType, 0, false, false);
			}
		}
		foreach (SerializableVector2Int serializableVector2Int in this.RuneLevels)
		{
			RuneManager.SetRuneEquippedLevel((RuneType)serializableVector2Int.x, serializableVector2Int.y, false, false);
		}
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000B8C84 File Offset: 0x000B6E84
	public void SaveLoadout()
	{
		this.RuneLevels.Clear();
		foreach (KeyValuePair<RuneType, RuneObj> keyValuePair in SaveManager.EquipmentSaveData.RuneDict)
		{
			RuneType key = keyValuePair.Key;
			RuneObj value = keyValuePair.Value;
			if (!value.IsNativeNull())
			{
				int equippedLevel = value.EquippedLevel;
				if (equippedLevel > 0)
				{
					this.RuneLevels.Add(new SerializableVector2Int((int)key, equippedLevel));
				}
			}
		}
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x0001620F File Offset: 0x0001440F
	public RuneLoadout Clone()
	{
		return base.MemberwiseClone() as RuneLoadout;
	}

	// Token: 0x040021EA RID: 8682
	public List<SerializableVector2Int> RuneLevels = new List<SerializableVector2Int>(20);
}
