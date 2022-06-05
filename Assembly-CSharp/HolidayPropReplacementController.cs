using System;
using UnityEngine;

// Token: 0x020003BD RID: 957
public class HolidayPropReplacementController : MonoBehaviour
{
	// Token: 0x06001F8A RID: 8074 RVA: 0x000A30F0 File Offset: 0x000A12F0
	private void Awake()
	{
		this.m_propSpawner = base.GetComponent<PropSpawnController>();
		if (!this.m_propSpawner)
		{
			Debug.Log("Could not execute HolidayPropReplacementController.");
			return;
		}
		this.m_propSpawner.BeforePropInstanceInitializedRelay.AddListener(new Action(this.BeforePropInitialized), false);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x00010977 File Offset: 0x0000EB77
	private void OnDestroy()
	{
		if (this.m_propSpawner)
		{
			this.m_propSpawner.BeforePropInstanceInitializedRelay.RemoveListener(new Action(this.BeforePropInitialized));
		}
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000A3140 File Offset: 0x000A1340
	private void BeforePropInitialized()
	{
		if (!this.m_propSpawner)
		{
			return;
		}
		if (!this.m_propStored)
		{
			this.m_storedPropPrefab = this.m_propSpawner.PropPrefab;
			this.m_propStored = true;
		}
		if (HolidayLookController.IsHoliday(this.m_holidayType))
		{
			if (BiomeUtility.IsBiomeInBiomeLayerMask(PlayerManager.GetCurrentPlayerRoom().BiomeType, this.m_biomesToReplace))
			{
				if (!this.m_propsPooled)
				{
					Prop[] propPrefabReplacementArray = this.m_propPrefabReplacementArray;
					for (int i = 0; i < propPrefabReplacementArray.Length; i++)
					{
						PropManager.AddPropToPool(propPrefabReplacementArray[i], 5);
					}
					this.m_propsPooled = true;
				}
				int num = (int)Mathf.Abs(this.m_propSpawner.transform.localPosition.x) % this.m_propPrefabReplacementArray.Length;
				this.m_propSpawner.ForcePropPrefab(this.m_propPrefabReplacementArray[num]);
				return;
			}
		}
		else
		{
			this.m_propSpawner.ForcePropPrefab(this.m_storedPropPrefab);
		}
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000109A3 File Offset: 0x0000EBA3
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing && this.m_propStored)
		{
			this.m_propSpawner.ForcePropPrefab(this.m_storedPropPrefab);
		}
	}

	// Token: 0x04001C29 RID: 7209
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001C2A RID: 7210
	[SerializeField]
	[EnumFlag]
	private BiomeLayer m_biomesToReplace;

	// Token: 0x04001C2B RID: 7211
	[HelpBox("WARNING: The Breakable state of the replacement prop MUST MATCH (i.e. if the replaced prop is breakable then the replacement prop must be breakable as well). This system also DOES NOT support replacing props with Decos on them.", HelpBoxMessageType.Warning)]
	[Space(10f)]
	[SerializeField]
	private Prop[] m_propPrefabReplacementArray;

	// Token: 0x04001C2C RID: 7212
	private PropSpawnController m_propSpawner;

	// Token: 0x04001C2D RID: 7213
	private bool m_propsPooled;

	// Token: 0x04001C2E RID: 7214
	private bool m_propStored;

	// Token: 0x04001C2F RID: 7215
	private Prop m_storedPropPrefab;
}
