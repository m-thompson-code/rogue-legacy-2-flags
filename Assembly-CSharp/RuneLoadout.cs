using System;
using System.Collections.Generic;

// Token: 0x020002CE RID: 718
[Serializable]
public class RuneLoadout
{
	// Token: 0x06001C83 RID: 7299 RVA: 0x0005C768 File Offset: 0x0005A968
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

	// Token: 0x06001C84 RID: 7300 RVA: 0x0005C7F0 File Offset: 0x0005A9F0
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

	// Token: 0x06001C85 RID: 7301 RVA: 0x0005C884 File Offset: 0x0005AA84
	public RuneLoadout Clone()
	{
		return base.MemberwiseClone() as RuneLoadout;
	}

	// Token: 0x040019E3 RID: 6627
	public List<SerializableVector2Int> RuneLevels = new List<SerializableVector2Int>(20);
}
