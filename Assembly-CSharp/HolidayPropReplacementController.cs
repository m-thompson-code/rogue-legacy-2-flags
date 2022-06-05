using System;
using UnityEngine;

// Token: 0x0200020C RID: 524
public class HolidayPropReplacementController : MonoBehaviour
{
	// Token: 0x060015FA RID: 5626 RVA: 0x00044870 File Offset: 0x00042A70
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

	// Token: 0x060015FB RID: 5627 RVA: 0x000448BF File Offset: 0x00042ABF
	private void OnDestroy()
	{
		if (this.m_propSpawner)
		{
			this.m_propSpawner.BeforePropInstanceInitializedRelay.RemoveListener(new Action(this.BeforePropInitialized));
		}
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x000448EC File Offset: 0x00042AEC
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

	// Token: 0x060015FD RID: 5629 RVA: 0x000449C2 File Offset: 0x00042BC2
	private void OnDisable()
	{
		if (!GameManager.IsApplicationClosing && this.m_propStored)
		{
			this.m_propSpawner.ForcePropPrefab(this.m_storedPropPrefab);
		}
	}

	// Token: 0x0400152C RID: 5420
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x0400152D RID: 5421
	[SerializeField]
	[EnumFlag]
	private BiomeLayer m_biomesToReplace;

	// Token: 0x0400152E RID: 5422
	[HelpBox("WARNING: The Breakable state of the replacement prop MUST MATCH (i.e. if the replaced prop is breakable then the replacement prop must be breakable as well). This system also DOES NOT support replacing props with Decos on them.", HelpBoxMessageType.Warning)]
	[Space(10f)]
	[SerializeField]
	private Prop[] m_propPrefabReplacementArray;

	// Token: 0x0400152F RID: 5423
	private PropSpawnController m_propSpawner;

	// Token: 0x04001530 RID: 5424
	private bool m_propsPooled;

	// Token: 0x04001531 RID: 5425
	private bool m_propStored;

	// Token: 0x04001532 RID: 5426
	private Prop m_storedPropPrefab;
}
