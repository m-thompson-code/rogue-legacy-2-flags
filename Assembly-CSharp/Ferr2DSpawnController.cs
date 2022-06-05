using System;
using Spawn;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class Ferr2DSpawnController : SimpleSpawnController
{
	// Token: 0x06003838 RID: 14392 RVA: 0x000C0059 File Offset: 0x000BE259
	private void Awake()
	{
		this.m_ferr2D = base.GetComponent<Ferr2DT_PathTerrain>();
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x000C0068 File Offset: 0x000BE268
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

	// Token: 0x04002AEC RID: 10988
	[SerializeField]
	private PropSpawnControllerData m_spawnControllerData;

	// Token: 0x04002AED RID: 10989
	private Ferr2DT_PathTerrain m_ferr2D;
}
