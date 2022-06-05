using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000442 RID: 1090
public class TimeLimit_FairyRule : FairyRule
{
	// Token: 0x17000FC2 RID: 4034
	// (get) Token: 0x060027F9 RID: 10233 RVA: 0x00084776 File Offset: 0x00082976
	public override string Description
	{
		get
		{
			return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TIME_LIMIT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), this.m_timeLimit);
		}
	}

	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x060027FA RID: 10234 RVA: 0x000847A4 File Offset: 0x000829A4
	public virtual string TimeRemainingDescription
	{
		get
		{
			if (this.TimeRemaining <= 0f)
			{
				return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TIME_LIMIT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), 0);
			}
			return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TIME_LIMIT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), (int)this.TimeRemaining);
		}
	}

	// Token: 0x17000FC4 RID: 4036
	// (get) Token: 0x060027FB RID: 10235 RVA: 0x0008480F File Offset: 0x00082A0F
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.TimeLimit;
		}
	}

	// Token: 0x17000FC5 RID: 4037
	// (get) Token: 0x060027FC RID: 10236 RVA: 0x00084813 File Offset: 0x00082A13
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000FC6 RID: 4038
	// (get) Token: 0x060027FD RID: 10237 RVA: 0x00084816 File Offset: 0x00082A16
	// (set) Token: 0x060027FE RID: 10238 RVA: 0x0008481E File Offset: 0x00082A1E
	public float TimeElapsed { get; private set; }

	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x060027FF RID: 10239 RVA: 0x00084827 File Offset: 0x00082A27
	public float TimeRemaining
	{
		get
		{
			return (float)Mathf.Clamp(Mathf.CeilToInt((float)this.m_timeLimit - this.TimeElapsed), 0, this.m_timeLimit);
		}
	}

	// Token: 0x06002800 RID: 10240 RVA: 0x00084849 File Offset: 0x00082A49
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.TimeElapsed = 0f;
		base.State = FairyRoomState.Running;
		base.StartCoroutine(this.RunTimer(fairyRoomController));
	}

	// Token: 0x06002801 RID: 10241 RVA: 0x0008486C File Offset: 0x00082A6C
	private IEnumerator RunTimer(FairyRoomController fairyRoomController)
	{
		while (this.TimeElapsed <= (float)this.m_timeLimit)
		{
			bool flag = true;
			foreach (FairyRoomRuleEntry fairyRoomRuleEntry in fairyRoomController.FairyRoomRuleEntries)
			{
				if (!(fairyRoomRuleEntry.FairyRule == this) && fairyRoomRuleEntry.FairyRule.State != FairyRoomState.Passed)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				base.SetIsPassed();
				base.StopAllCoroutines();
				yield break;
			}
			this.TimeElapsed += Time.deltaTime;
			yield return null;
		}
		base.SetIsFailed();
		yield break;
	}

	// Token: 0x06002802 RID: 10242 RVA: 0x00084882 File Offset: 0x00082A82
	public override void StopRule()
	{
		base.StopRule();
		base.StopAllCoroutines();
	}

	// Token: 0x06002803 RID: 10243 RVA: 0x00084890 File Offset: 0x00082A90
	public override void ResetRule()
	{
		this.TimeElapsed = 0f;
		base.ResetRule();
	}

	// Token: 0x0400213E RID: 8510
	[SerializeField]
	protected int m_timeLimit = 10;
}
