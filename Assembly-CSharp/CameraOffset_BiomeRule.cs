using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Token: 0x020009FA RID: 2554
[CreateAssetMenu(menuName = "Custom/Biome Rules/Camera Offset")]
public class CameraOffset_BiomeRule : BiomeRule
{
	// Token: 0x17001AA1 RID: 6817
	// (get) Token: 0x06004D00 RID: 19712 RVA: 0x00029D60 File Offset: 0x00027F60
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

	// Token: 0x06004D01 RID: 19713 RVA: 0x00029D6C File Offset: 0x00027F6C
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

	// Token: 0x06004D02 RID: 19714 RVA: 0x00029D82 File Offset: 0x00027F82
	private void SetOffset(CinemachineVirtualCamera virtualCamera)
	{
		CinemachineFramingTransposer cinemachineComponent = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
		cinemachineComponent.m_ScreenX = this.m_offsetOverride.x;
		cinemachineComponent.m_ScreenY = this.m_offsetOverride.y;
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x00002FCA File Offset: 0x000011CA
	public override void UndoRule(BiomeType biome)
	{
	}

	// Token: 0x04003A48 RID: 14920
	[SerializeField]
	private Vector2 m_offsetOverride = new Vector2(0.5f, 0.6f);
}
