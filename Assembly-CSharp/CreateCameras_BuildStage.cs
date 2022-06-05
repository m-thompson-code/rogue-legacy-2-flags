using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000661 RID: 1633
public class CreateCameras_BuildStage : IBiomeBuildStage
{
	// Token: 0x06003B2C RID: 15148 RVA: 0x000CB72D File Offset: 0x000C992D
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
