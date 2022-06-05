using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ABC RID: 2748
public class CreateCameras_BuildStage : IBiomeBuildStage
{
	// Token: 0x060052CD RID: 21197 RVA: 0x0002D12D File Offset: 0x0002B32D
	public IEnumerator Run(BiomeController biomeController)
	{
		if (biomeController.Rooms != null && biomeController.Rooms.Count > 0)
		{
			using (List<BaseRoom>.Enumerator enumerator = biomeController.Rooms.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BaseRoom room = enumerator.Current;
					biomeController.CinemachineVirtualCameraCreatedByWorldBuilder(BiomeCreatorTools.CreateCinemachineVirtualCamera(room));
				}
				yield break;
			}
		}
		Debug.LogFormat("<color=red>[{0}] BiomeController ({1})'s Rooms Property is null or empty.</color>", new object[]
		{
			this,
			biomeController.Biome
		});
		yield break;
	}
}
