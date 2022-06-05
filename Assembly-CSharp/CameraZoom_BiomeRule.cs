using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
[CreateAssetMenu(menuName = "Custom/Biome Rules/Camera Zoom")]
public class CameraZoom_BiomeRule : BiomeRule
{
	// Token: 0x1700136F RID: 4975
	// (get) Token: 0x060036E1 RID: 14049 RVA: 0x000BCAB2 File Offset: 0x000BACB2
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			return BiomeRuleExecutionTime.PlayerEnterBiome;
		}
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x000BCAB6 File Offset: 0x000BACB6
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

	// Token: 0x060036E3 RID: 14051 RVA: 0x000BCACC File Offset: 0x000BACCC
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

	// Token: 0x04002A3D RID: 10813
	[SerializeField]
	private float m_zoomLevel = 1f;

	// Token: 0x04002A3E RID: 10814
	private float m_previousZoomLevel;
}
