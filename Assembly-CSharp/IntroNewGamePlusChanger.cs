using System;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class IntroNewGamePlusChanger : MonoBehaviour
{
	// Token: 0x06001FC2 RID: 8130 RVA: 0x00010C26 File Offset: 0x0000EE26
	private void Awake()
	{
		this.m_castleGO.SetActive(false);
		this.m_moonGO.SetActive(true);
		this.UpdateCrowns(true);
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000A3868 File Offset: 0x000A1A68
	public void UpdateMainMenu()
	{
		if (SaveManager.PlayerSaveData.HighestNGPlusBeaten > -1)
		{
			if (!this.m_changedToNewGamePlus)
			{
				if (HolidayLookController.IsHoliday(HolidayType.Christmas) || HolidayLookController.IsHoliday(HolidayType.Halloween))
				{
					this.m_introSkyColourChanger.SetSkyColor(this.m_introSkyColourChanger.PatchTypeToUse);
				}
				else
				{
					this.m_introSkyColourChanger.SetSkyColor(PatchType.NewGamePlus_BlueSky);
				}
				this.m_castleGO.SetActive(true);
				this.m_moonGO.SetActive(false);
				this.UpdateCrowns(false);
				this.m_changedToNewGamePlus = true;
				return;
			}
		}
		else if (this.m_changedToNewGamePlus)
		{
			this.m_introSkyColourChanger.SetSkyColor(this.m_introSkyColourChanger.PatchTypeToUse);
			this.m_castleGO.SetActive(false);
			this.m_moonGO.SetActive(true);
			this.UpdateCrowns(false);
			this.m_changedToNewGamePlus = false;
		}
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000A392C File Offset: 0x000A1B2C
	private void UpdateCrowns(bool setToDefault)
	{
		int num = this.m_crownGOArray.Length;
		int num2 = Mathf.Clamp(SaveManager.PlayerSaveData.HighestNGPlusBeaten + 1, 0, num + 1);
		if (setToDefault)
		{
			num2 = 0;
		}
		for (int i = 0; i < num; i++)
		{
			if (i < num2)
			{
				this.m_crownGOArray[i].SetActive(true);
			}
			else
			{
				this.m_crownGOArray[i].SetActive(false);
			}
		}
	}

	// Token: 0x04001C4C RID: 7244
	[SerializeField]
	private IntroSkyColourChanger m_introSkyColourChanger;

	// Token: 0x04001C4D RID: 7245
	[SerializeField]
	private GameObject m_castleGO;

	// Token: 0x04001C4E RID: 7246
	[SerializeField]
	private GameObject m_moonGO;

	// Token: 0x04001C4F RID: 7247
	[SerializeField]
	private GameObject[] m_crownGOArray;

	// Token: 0x04001C50 RID: 7248
	private bool m_changedToNewGamePlus;
}
