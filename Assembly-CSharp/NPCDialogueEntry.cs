using System;

// Token: 0x0200007C RID: 124
public class NPCDialogueEntry
{
	// Token: 0x060001BC RID: 444 RVA: 0x00003B13 File Offset: 0x00001D13
	public NPCDialogueEntry(string locID, params NPCDialogueCondition[] unlockConditions)
	{
		this.LocID = locID;
		this.UnlockConditions = unlockConditions;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x00003B29 File Offset: 0x00001D29
	public NPCDialogueEntry(string locID, string titleLocID, string conditionValue, params NPCDialogueCondition[] unlockConditions)
	{
		this.LocID = locID;
		this.TitleLocID = titleLocID;
		this.ConditionValue = conditionValue;
		this.UnlockConditions = unlockConditions;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0004CC7C File Offset: 0x0004AE7C
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

	// Token: 0x040003EB RID: 1003
	public string LocID;

	// Token: 0x040003EC RID: 1004
	public NPCDialogueCondition[] UnlockConditions;

	// Token: 0x040003ED RID: 1005
	public string TitleLocID;

	// Token: 0x040003EE RID: 1006
	public string ConditionValue;
}
