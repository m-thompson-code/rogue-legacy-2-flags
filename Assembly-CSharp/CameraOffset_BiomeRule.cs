using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Token: 0x020005EF RID: 1519
[CreateAssetMenu(menuName = "Custom/Biome Rules/Camera Offset")]
public class CameraOffset_BiomeRule : BiomeRule
{
	// Token: 0x1700136E RID: 4974
	// (get) Token: 0x060036DC RID: 14044 RVA: 0x000BCA48 File Offset: 0x000BAC48
	public override BiomeRuleExecutionTime ExecutionTime
	{
		get
		{
			if (GameUtility.IsInLevelEditor)
			{
				return BiomeRuleExecutionTime.WorldCreationComplete;
			}
			return BiomeRuleExecutionTime.BiomeCreationComplete;
		}
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x000BCA54 File Offset: 0x000BAC54
	public override IEnumerator RunRule(BiomeType biome)
	{
		if (GameUtility.IsInLevelEditor && OnPlayManager.BiomeController)
		{
			CinemachineVirtualCamera virtualCamera = OnPlayManager.CurrentRoom.CinemachineCamera.VirtualCamera;
			this.SetOffset(virtualCamera);
		}
		else
		{
			BiomeController biomeController = WorldBuilder.GetBiomeController(biome);
			if (biomeController != null)
			{
				List<CinemachineVirtualCamera> cinemachineVirtualCameras = biomeController.CinemachineVirtualCameras;
				if (cinemachineVirtualCameras != null)
				{
					for (int i = 0; i < cinemachineVirtualCameras.Count; i++)
					{
						this.SetOffset(cinemachineVirtualCameras[i]);
					}
				}
				else
				{
					Debug.LogFormat("<color=red>| {0} | Can't run rule because {1} Biome's BiomeController's CinemachineVirtualCameras property is null.</color>", new object[]
					{
						this,
						biome
					});
				}
			}
		}
		yield break;
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x000BCA6A File Offset: 0x000BAC6A
	private void SetOffset(CinemachineVirtualCamera virtualCamera)
	{
		CinemachineFramingTransposer cinemachineComponent = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		cinemachineComponent.m_ScreenX = this.m_offsetOverride.x;
		cinemachineComponent.m_ScreenY = this.m_offsetOverride.y;
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x000BCA93 File Offset: 0x000BAC93
	public override void UndoRule(BiomeType biome)
	{
	}

	// Token: 0x04002A3C RID: 10812
	[SerializeField]
	private Vector2 m_offsetOverride = new Vector2(0.5f, 0.6f);
}
