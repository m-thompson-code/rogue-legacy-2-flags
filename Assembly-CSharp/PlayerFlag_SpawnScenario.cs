using System;
using UnityEngine;

// Token: 0x02000640 RID: 1600
public class PlayerFlag_SpawnScenario : SpawnScenario
{
	// Token: 0x1700146B RID: 5227
	// (get) Token: 0x060039D3 RID: 14803 RVA: 0x000C4E7D File Offset: 0x000C307D
	// (set) Token: 0x060039D4 RID: 14804 RVA: 0x000C4E85 File Offset: 0x000C3085
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

	// Token: 0x1700146C RID: 5228
	// (get) Token: 0x060039D5 RID: 14805 RVA: 0x000C4E8E File Offset: 0x000C308E
	// (set) Token: 0x060039D6 RID: 14806 RVA: 0x000C4E96 File Offset: 0x000C3096
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

	// Token: 0x1700146D RID: 5229
	// (get) Token: 0x060039D7 RID: 14807 RVA: 0x000C4E9F File Offset: 0x000C309F
	public override SpawnScenarioType Type
	{
		get
		{
			return SpawnScenarioType.PlayerFlag;
		}
	}

	// Token: 0x1700146E RID: 5230
	// (get) Token: 0x060039D8 RID: 14808 RVA: 0x000C4EA3 File Offset: 0x000C30A3
	public override string GizmoDescription
	{
		get
		{
			return string.Format("FLAG: {0}, ISTRUE: {1}", this.m_playerFlag, this.m_flagIsTrue);
		}
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x000C4EC0 File Offset: 0x000C30C0
	public override void RunIsTrueCheck(BaseRoom room)
	{
		this.RunIsTrueCheck();
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x000C4EC8 File Offset: 0x000C30C8
	public override void RunIsTrueCheck(GridPointManager gridPointManager)
	{
		this.RunIsTrueCheck();
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x000C4ED0 File Offset: 0x000C30D0
	private void RunIsTrueCheck()
	{
		PlayerSaveFlag flag;
		if (Enum.TryParse<PlayerSaveFlag>(this.PlayerFlag, out flag))
		{
			this.IsTrue = (SaveManager.PlayerSaveData.GetFlag(flag) == this.FlagIsTrue);
		}
	}

	// Token: 0x04002C76 RID: 11382
	[SerializeField]
	private string m_playerFlag;

	// Token: 0x04002C77 RID: 11383
	[SerializeField]
	private bool m_flagIsTrue = true;
}
