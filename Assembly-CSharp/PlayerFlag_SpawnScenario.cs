using System;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public class PlayerFlag_SpawnScenario : SpawnScenario
{
	// Token: 0x17001BD2 RID: 7122
	// (get) Token: 0x060050B2 RID: 20658 RVA: 0x0002C0BE File Offset: 0x0002A2BE
	// (set) Token: 0x060050B3 RID: 20659 RVA: 0x0002C0C6 File Offset: 0x0002A2C6
	public string PlayerFlag
	{
		get
		{
			return this.m_playerFlag;
		}
		set
		{
			this.m_playerFlag = value;
		}
	}

	// Token: 0x17001BD3 RID: 7123
	// (get) Token: 0x060050B4 RID: 20660 RVA: 0x0002C0CF File Offset: 0x0002A2CF
	// (set) Token: 0x060050B5 RID: 20661 RVA: 0x0002C0D7 File Offset: 0x0002A2D7
	public bool FlagIsTrue
	{
		get
		{
			return this.m_flagIsTrue;
		}
		set
		{
			this.m_flagIsTrue = value;
		}
	}

	// Token: 0x17001BD4 RID: 7124
	// (get) Token: 0x060050B6 RID: 20662 RVA: 0x000054AD File Offset: 0x000036AD
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.PlayerFlag;
		}
	}

	// Token: 0x17001BD5 RID: 7125
	// (get) Token: 0x060050B7 RID: 20663 RVA: 0x0002C0E0 File Offset: 0x0002A2E0
	public override string GizmoDescription
	{
		get
		{
			return string.Format("FLAG: {0}, ISTRUE: {1}", this.m_playerFlag, this.m_flagIsTrue);
		}
	}

	// Token: 0x060050B8 RID: 20664 RVA: 0x0002C0FD File Offset: 0x0002A2FD
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck();
	}

	// Token: 0x060050B9 RID: 20665 RVA: 0x0002C0FD File Offset: 0x0002A2FD
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck();
	}

	// Token: 0x060050BA RID: 20666 RVA: 0x001330BC File Offset: 0x001312BC
	private void RunIsTrueCheck()
	{
		PlayerSaveFlag flag;
		if (Enum.TryParse<PlayerSaveFlag>(this.PlayerFlag, out flag))
		{
			this.IsTrue = (SaveManager.PlayerSaveData.GetFlag(flag) == this.FlagIsTrue);
		}
	}

	// Token: 0x04003D08 RID: 15624
	[SerializeField]
	private string m_playerFlag;

	// Token: 0x04003D09 RID: 15625
	[SerializeField]
	private bool m_flagIsTrue = true;
}
