using System;
using Spawn;
using UnityEngine;

// Token: 0x02000A2B RID: 2603
public class Ferr2DSpawnController : SimpleSpawnController
{
	// Token: 0x06004ED5 RID: 20181 RVA: 0x0002AFEC File Offset: 0x000291EC
	private void Awake()
	{
		this.m_ferr2D = base.GetComponent<Ferr2DT_PathTerrain>();
	}

	// Token: 0x06004ED6 RID: 20182 RVA: 0x0012E4F0 File Offset: 0x0012C6F0
	protected override void DoIsSpawned()
	{
		if (base.Room == null)
		{
			throw new ArgumentNullException("Room");
		}
		if (this.m_spawnControllerData == null)
		{
			throw new ArgumentNullException("m_spawnControllerData");
		}
		Prop[] array = this.m_spawnControllerData.DefaultProps;
		for (int i = 0; i < this.m_spawnControllerData.PropsPerBiome.Length; i++)
		{
			if (BiomeType_RL.GetGroupedBiomeType(base.Room.AppearanceBiomeType) == this.m_spawnControllerData.PropsPerBiome[i].Biome)
			{
				array = this.m_spawnControllerData.PropsPerBiome[i].Props;
				break;
			}
		}
		if (array == null || array.Length == 0)
		{
			throw new ArgumentNullException("potentialProps");
		}
		int num = 0;
		if (array.Length > 1)
		{
			num = RNGManager.GetRandomNumber(RngID.Prop_RoomSeed, string.Format("Ferr2D Spawn Controller. Get Material.", Array.Empty<object>()), 0, array.Length);
		}
		Ferr2DT_PathTerrain component = array[num].GetComponent<Ferr2DT_PathTerrain>();
		if (component)
		{
			CameraLayerController component2 = base.GetComponent<CameraLayerController>();
			component2.SetCameraLayer(this.m_spawnControllerData.CameraLayer);
			component2.SetSubLayer(this.m_spawnControllerData.SubLayer, false);
			this.m_ferr2D.SetMaterial(component.TerrainMaterial);
			this.m_ferr2D.BuildMeshOnly(false);
			return;
		}
		throw new ArgumentNullException("ferr2DComponent");
	}

	// Token: 0x04003B4F RID: 15183
	[SerializeField]
	private PropSpawnControllerData m_spawnControllerData;

	// Token: 0x04003B50 RID: 15184
	private Ferr2DT_PathTerrain m_ferr2D;
}
