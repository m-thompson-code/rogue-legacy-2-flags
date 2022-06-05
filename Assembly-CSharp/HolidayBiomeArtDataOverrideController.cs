using System;
using UnityEngine;

// Token: 0x02000208 RID: 520
public class HolidayBiomeArtDataOverrideController : MonoBehaviour
{
	// Token: 0x060015E8 RID: 5608 RVA: 0x00044528 File Offset: 0x00042728
	private void Awake()
	{
		Room component = base.GetComponent<Room>();
		if (component)
		{
			if (HolidayLookController.IsHoliday(this.m_holidayType) && !string.IsNullOrEmpty(component.gameObject.scene.name))
			{
				component.ForceBiomeArtDataOverride(this.m_biomeArtData);
				return;
			}
		}
		else
		{
			Debug.Log("<color=yellow>Could not apply HolidayBiomeArtData.  No room found.</color>");
		}
	}

	// Token: 0x04001518 RID: 5400
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001519 RID: 5401
	[SerializeField]
	private BiomeArtData m_biomeArtData;
}
