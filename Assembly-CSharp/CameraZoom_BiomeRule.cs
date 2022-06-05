using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020009FC RID: 2556
[CreateAssetMenu(menuName = "Custom/Biome Rules/Camera Zoom")]
public class CameraZoom_BiomeRule : BiomeRule
{
	// Token: 0x17001AA4 RID: 6820
	// (get) Token: 0x06004D0B RID: 19723 RVA: 0x000046FA File Offset: 0x000028FA
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x06004D0C RID: 19724 RVA: 0x00029DDF File Offset: 0x00027FDF
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			CameraZoomController component = OnPlayManager.CurrentRoom.GetComponent<CameraZoomController>();
			if (component)
			{
				this.m_previousZoomLevel = component.ZoomLevel;
				component.SetZoomLevel(this.m_zoomLevel);
			}
		}
		else
		{
			BiomeController biomeController = WorldBuilder.GetBiomeController(BiomeType_RL.GetGroupedBiomeType(biome));
			if (biomeController)
			{
				List<BaseRoom> list;
				if (biome != BiomeType.TowerExterior)
				{
					list = (from room in biomeController.Rooms
					where room.AppearanceBiomeType == biome
					select room).ToList<BaseRoom>();
				}
				else
				{
					list = (from room in biomeController.Rooms
					where room.AppearanceBiomeType == BiomeType.TowerExterior
					select room).ToList<BaseRoom>();
				}
				using (List<BaseRoom>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BaseRoom baseRoom = enumerator.Current;
						CameraZoomController component2 = baseRoom.gameObject.GetComponent<CameraZoomController>();
						if (component2)
						{
							component2.SetZoomLevel(this.m_zoomLevel);
						}
					}
					yield break;
				}
			}
		}
		yield break;
	}

	// Token: 0x06004D0D RID: 19725 RVA: 0x0012AD78 File Offset: 0x00128F78
	public override void UndoRule(BiomeType biome)
	{
		if (GameUtility.IsInLevelEditor && OnPlayManager.CurrentRoom)
		{
			CameraZoomController component = OnPlayManager.CurrentRoom.GetComponent<CameraZoomController>();
			if (component && component.OverrideZoomLevel)
			{
				component.SetZoomLevel(this.m_previousZoomLevel);
			}
		}
	}

	// Token: 0x04003A4D RID: 14925
	[SerializeField]
	private float m_zoomLevel = 1f;

	// Token: 0x04003A4E RID: 14926
	private float m_previousZoomLevel;
}
