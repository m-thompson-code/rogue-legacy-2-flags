using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070C RID: 1804
public class TimeLimit_FairyRule : FairyRule
{
	// Token: 0x170014A7 RID: 5287
	// (get) Token: 0x06003711 RID: 14097 RVA: 0x0001E49E File Offset: 0x0001C69E
	public override string Description
	{
		get
		{
			return string.Format(LocalizationManager.GetString("LOC_ID_FAIRY_RULE_TIME_LIMIT_1", SaveManager.PlayerSaveData.CurrentCharacter.IsFemale, false), this.m_timeLimit);
		}
	}

	// Token: 0x170014A8 RID: 5288
	// (get) Token: 0x06003712 RID: 14098 RVA: 0x000E503C File Offset: 0x000E323C
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

	// Token: 0x170014A9 RID: 5289
	// (get) Token: 0x06003713 RID: 14099 RVA: 0x000046FA File Offset: 0x000028FA
	public override FairyRuleID ID
	{
		get
		{
			return FairyRuleID.TimeLimit;
		}
	}

	// Token: 0x170014AA RID: 5290
	// (get) Token: 0x06003714 RID: 14100 RVA: 0x00003CD2 File Offset: 0x00001ED2
	public override bool LockChestAtStart
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170014AB RID: 5291
	// (get) Token: 0x06003715 RID: 14101 RVA: 0x0001E4CA File Offset: 0x0001C6CA
	// (set) Token: 0x06003716 RID: 14102 RVA: 0x0001E4D2 File Offset: 0x0001C6D2
	public float TimeElapsed { get; private set; }

	// Token: 0x170014AC RID: 5292
	// (get) Token: 0x06003717 RID: 14103 RVA: 0x0001E4DB File Offset: 0x0001C6DB
	public float TimeRemaining
	{
		get
		{
			return (float)Mathf.Clamp(Mathf.CeilToInt((float)this.m_timeLimit - this.TimeElapsed), 0, this.m_timeLimit);
		}
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x0001E4FD File Offset: 0x0001C6FD
	public override void RunRule(FairyRoomController fairyRoomController)
	{
		this.TimeElapsed = 0f;
		base.State = FairyRoomState.Running;
		base.StartCoroutine(this.RunTimer(fairyRoomController));
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0001E520 File Offset: 0x0001C720
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

	// Token: 0x0600371A RID: 14106 RVA: 0x0001E479 File Offset: 0x0001C679
	public override void StopRule()
	{
		base.StopRule();
		base.StopAllCoroutines();
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x0001E536 File Offset: 0x0001C736
	public override void ResetRule()
	{
		this.TimeElapsed = 0f;
		base.ResetRule();
	}

	// Token: 0x04002C72 RID: 11378
	[SerializeField]
	protected int m_timeLimit = 10;
}
