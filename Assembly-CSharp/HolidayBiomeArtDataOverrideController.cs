using System;
using UnityEngine;

// Token: 0x020003B9 RID: 953
public class HolidayBiomeArtDataOverrideController : MonoBehaviour
{
	// Token: 0x06001F78 RID: 8056 RVA: 0x000A2F20 File Offset: 0x000A1120
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

	// Token: 0x04001C15 RID: 7189
	[SerializeField]
	private HolidayType m_holidayType;

	// Token: 0x04001C16 RID: 7190
	[SerializeField]
	private BiomeArtData m_biomeArtData;
}
