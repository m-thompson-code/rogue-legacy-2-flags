using System;
using UnityEngine;

// Token: 0x0200061B RID: 1563
public class LineHazardSpawnController : HazardSpawnControllerBase, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray, IWidthConsumer, IMirror, IPivotConsumer
{
	// Token: 0x170013EB RID: 5099
	// (get) Token: 0x06003868 RID: 14440 RVA: 0x000C0BF9 File Offset: 0x000BEDF9
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Line;
		}
	}

	// Token: 0x170013EC RID: 5100
	// (get) Token: 0x06003869 RID: 14441 RVA: 0x000C0BFD File Offset: 0x000BEDFD
	// (set) Token: 0x0600386A RID: 14442 RVA: 0x000C0C05 File Offset: 0x000BEE05
	public int Width
	{
		get
		{
			return this.m_width;
		}
		private set
		{
			this.m_width = value;
		}
	}

	// Token: 0x170013ED RID: 5101
	// (get) Token: 0x0600386B RID: 14443 RVA: 0x000C0C0E File Offset: 0x000BEE0E
	// (set) Token: 0x0600386C RID: 14444 RVA: 0x000C0C16 File Offset: 0x000BEE16
	public PivotPoint PivotPoint
	{
		get
		{
			return this.m_pivot;
		}
		private set
		{
			this.m_pivot = value;
		}
	}

	// Token: 0x170013EE RID: 5102
	// (get) Token: 0x0600386D RID: 14445 RVA: 0x000C0C1F File Offset: 0x000BEE1F
	public virtual string SpriteSheetPath
	{
		get
		{
			return "Assets/Content/Textures/Hazards/LineHazard_Editor_Texture.png";
		}
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x000C0C28 File Offset: 0x000BEE28
	public HazardType GetLineHazardType(HazardType hazardType)
	{
		if (hazardType <= HazardType.BreakableSpikeTall)
		{
			if (hazardType <= HazardType.WallTurret_Standard)
			{
				if (hazardType != HazardType.SpikeTrap)
				{
					if (hazardType == HazardType.WallTurret_Standard)
					{
						hazardType = HazardType.Multi_StandardWallTurret;
					}
				}
				else
				{
					hazardType = HazardType.Multi_SpikeTrap;
				}
			}
			else if (hazardType != HazardType.WallTurret_FlameThrower)
			{
				if (hazardType != HazardType.BreakableSpike)
				{
					if (hazardType == HazardType.BreakableSpikeTall)
					{
						hazardType = HazardType.Multi_BreakableSpikeTall;
					}
				}
				else
				{
					hazardType = HazardType.Multi_BreakableSpike;
				}
			}
			else
			{
				hazardType = HazardType.Multi_FlameThrowerWallTurret;
			}
		}
		else if (hazardType <= HazardType.RaycastTurret_Arrow)
		{
			if (hazardType != HazardType.PressurePlate)
			{
				if (hazardType == HazardType.RaycastTurret_Arrow)
				{
					hazardType = HazardType.Multi_RaycastTurret_Arrow;
				}
			}
			else
			{
				hazardType = HazardType.PressurePlate_Trigger;
			}
		}
		else if (hazardType != HazardType.SpringTrap)
		{
			if (hazardType != HazardType.Bodies)
			{
				if (hazardType == HazardType.RaycastTurret_Curse)
				{
					hazardType = HazardType.Multi_RaycastTurret_Curse;
				}
			}
			else
			{
				hazardType = HazardType.Multi_Bodies;
			}
		}
		else
		{
			hazardType = HazardType.Multi_SpringTrap;
		}
		return hazardType;
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x000C0CDA File Offset: 0x000BEEDA
	protected override IHazard GetHazard(HazardType hazardType)
	{
		hazardType = this.GetLineHazardType(hazardType);
		return HazardManager.GetHazard(hazardType);
	}

	// Token: 0x06003870 RID: 14448 RVA: 0x000C0CEC File Offset: 0x000BEEEC
	public void Mirror()
	{
		if (this.PivotPoint == PivotPoint.Left)
		{
			this.PivotPoint = PivotPoint.Right;
		}
		else if (this.PivotPoint == PivotPoint.Right)
		{
			this.PivotPoint = PivotPoint.Left;
		}
		base.transform.rotation = Quaternion.Euler(0f, 0f, -1f * base.transform.rotation.eulerAngles.z);
	}

	// Token: 0x06003871 RID: 14449 RVA: 0x000C0D58 File Offset: 0x000BEF58
	protected override void Reset()
	{
		if (base.Hazard != null && base.Hazard.gameObject != null)
		{
			Multi_Hazard multi_Hazard = base.Hazard as Multi_Hazard;
			if (multi_Hazard != null)
			{
				multi_Hazard.Reset();
			}
			base.Hazard.gameObject.SetActive(false);
		}
		base.Hazard = null;
	}

	// Token: 0x06003872 RID: 14450 RVA: 0x000C0DB3 File Offset: 0x000BEFB3
	public void SetPivot(PivotPoint pivotPoint)
	{
		this.PivotPoint = pivotPoint;
	}

	// Token: 0x06003873 RID: 14451 RVA: 0x000C0DBC File Offset: 0x000BEFBC
	public void SetWidth(int width)
	{
	}

	// Token: 0x06003874 RID: 14452 RVA: 0x000C0DC0 File Offset: 0x000BEFC0
	protected override void Spawn()
	{
		if (base.Type == HazardType.None)
		{
			return;
		}
		base.CreateHazard();
		if (base.Hazard != null)
		{
			if (this.m_hazardArgs == null)
			{
				this.m_hazardArgs = new HazardArgs(base.InitialState);
			}
			Multi_Hazard multi_Hazard = base.Hazard as Multi_Hazard;
			if (multi_Hazard != null)
			{
				multi_Hazard.Initialize(this.PivotPoint, this.Width, this.m_hazardArgs);
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Hazard ({1}) is <b>NOT</b> a <b>Multi Hazard</b>, but should be. Please add a bug report to Pivotal</color>", new object[]
			{
				this,
				base.Hazard.gameObject.name
			});
			return;
		}
		else
		{
			if (base.Room != null)
			{
				Debug.LogFormat("<color=red>| {0} | Hazard is null in Room <b>({1})</b>, but should not be. Please add a bug report to Pivotal</color>", new object[]
				{
					this,
					base.Room.gameObject.name
				});
				return;
			}
			Debug.LogFormat("<color=red>| {0} | Hazard is null, but should not be. Please add a bug report to Pivotal</color>", new object[]
			{
				this
			});
			return;
		}
	}

	// Token: 0x06003876 RID: 14454 RVA: 0x000C0EB1 File Offset: 0x000BF0B1
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002BA8 RID: 11176
	[SerializeField]
	[ReadOnly]
	private int m_width = 1;

	// Token: 0x04002BA9 RID: 11177
	[SerializeField]
	[ReadOnly]
	private PivotPoint m_pivot;

	// Token: 0x04002BAA RID: 11178
	public const int MIN_WIDTH = 1;

	// Token: 0x04002BAB RID: 11179
	public const int MAX_WIDTH = 32;
}
