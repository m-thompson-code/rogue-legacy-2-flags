using System;
using UnityEngine;

// Token: 0x02000A3E RID: 2622
public class LineHazardSpawnController : HazardSpawnControllerBase, IHazardSpawnController, IComplexSpawnController, ISpawnController, IRoomConsumer, IIDConsumer, IStateConsumer, ISetSpawnType, IHasProjectileNameArray, IWidthConsumer, IMirror, IPivotConsumer
{
	// Token: 0x17001B42 RID: 6978
	// (get) Token: 0x06004F0C RID: 20236 RVA: 0x00005315 File Offset: 0x00003515
	public override HazardCategory Category
	{
		get
		{
			return HazardCategory.Line;
		}
	}

	// Token: 0x17001B43 RID: 6979
	// (get) Token: 0x06004F0D RID: 20237 RVA: 0x0002B1B0 File Offset: 0x000293B0
	// (set) Token: 0x06004F0E RID: 20238 RVA: 0x0002B1B8 File Offset: 0x000293B8
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

	// Token: 0x17001B44 RID: 6980
	// (get) Token: 0x06004F0F RID: 20239 RVA: 0x0002B1C1 File Offset: 0x000293C1
	// (set) Token: 0x06004F10 RID: 20240 RVA: 0x0002B1C9 File Offset: 0x000293C9
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

	// Token: 0x17001B45 RID: 6981
	// (get) Token: 0x06004F11 RID: 20241 RVA: 0x0002B1D2 File Offset: 0x000293D2
	public virtual string SpriteSheetPath
	{
		get
		{
			return "Assets/Content/Textures/Hazards/LineHazard_Editor_Texture.png";
		}
	}

	// Token: 0x06004F12 RID: 20242 RVA: 0x0012EEEC File Offset: 0x0012D0EC
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

	// Token: 0x06004F13 RID: 20243 RVA: 0x0002B1D9 File Offset: 0x000293D9
	protected override IHazard GetHazard(HazardType hazardType)
	{
		hazardType = this.GetLineHazardType(hazardType);
		return HazardManager.GetHazard(hazardType);
	}

	// Token: 0x06004F14 RID: 20244 RVA: 0x0012EFA0 File Offset: 0x0012D1A0
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

	// Token: 0x06004F15 RID: 20245 RVA: 0x0012F00C File Offset: 0x0012D20C
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

	// Token: 0x06004F16 RID: 20246 RVA: 0x0002B1EA File Offset: 0x000293EA
	public void SetPivot(PivotPoint pivotPoint)
	{
		this.PivotPoint = pivotPoint;
	}

	// Token: 0x06004F17 RID: 20247 RVA: 0x00002FCA File Offset: 0x000011CA
	public void SetWidth(int width)
	{
	}

	// Token: 0x06004F18 RID: 20248 RVA: 0x0012F068 File Offset: 0x0012D268
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

	// Token: 0x06004F1A RID: 20250 RVA: 0x00003713 File Offset: 0x00001913
	GameObject ISpawnController.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003C15 RID: 15381
	[SerializeField]
	[ReadOnly]
	private int m_width = 1;

	// Token: 0x04003C16 RID: 15382
	[SerializeField]
	[ReadOnly]
	private PivotPoint m_pivot;

	// Token: 0x04003C17 RID: 15383
	public const int MIN_WIDTH = 1;

	// Token: 0x04003C18 RID: 15384
	public const int MAX_WIDTH = 32;
}
