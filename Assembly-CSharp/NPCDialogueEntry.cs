using System;

// Token: 0x02000074 RID: 116
public class NPCDialogueEntry
{
	// Token: 0x060001A8 RID: 424 RVA: 0x00010730 File Offset: 0x0000E930
	public NPCDialogueEntry(string locID, params NPCDialogueCondition[] unlockConditions)
	{
		this.LocID = locID;
		this.UnlockConditions = unlockConditions;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00010746 File Offset: 0x0000E946
	public NPCDialogueEntry(string locID, string titleLocID, string conditionValue, params NPCDialogueCondition[] unlockConditions)
	{
		this.LocID = locID;
		this.TitleLocID = titleLocID;
		this.ConditionValue = conditionValue;
		this.UnlockConditions = unlockConditions;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0001076C File Offset: 0x0000E96C
	public bool IsUnlocked()
	{
		if (this.UnlockConditions != null)
		{
			foreach (NPCDialogueCondition condition in this.UnlockConditions)
			{
				if (!NPCType_RL.IsNPCDialogueConditionUnlocked(this, condition))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040003CA RID: 970
	public string LocID;

	// Token: 0x040003CB RID: 971
	public NPCDialogueCondition[] UnlockConditions;

	// Token: 0x040003CC RID: 972
	public string TitleLocID;

	// Token: 0x040003CD RID: 973
	public string ConditionValue;
}
