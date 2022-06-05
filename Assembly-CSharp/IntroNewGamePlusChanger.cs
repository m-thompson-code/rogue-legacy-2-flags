using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class IntroNewGamePlusChanger : MonoBehaviour
{
	// Token: 0x06001626 RID: 5670 RVA: 0x00045089 File Offset: 0x00043289
	private void Awake()
	{
		this.m_castleGO.SetActive(false);
		this.m_moonGO.SetActive(true);
		this.UpdateCrowns(true);
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x000450AC File Offset: 0x000432AC
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

	// Token: 0x06001628 RID: 5672 RVA: 0x00045170 File Offset: 0x00043370
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

	// Token: 0x04001549 RID: 5449
	[SerializeField]
	private IntroSkyColourChanger m_introSkyColourChanger;

	// Token: 0x0400154A RID: 5450
	[SerializeField]
	private GameObject m_castleGO;

	// Token: 0x0400154B RID: 5451
	[SerializeField]
	private GameObject m_moonGO;

	// Token: 0x0400154C RID: 5452
	[SerializeField]
	private GameObject[] m_crownGOArray;

	// Token: 0x0400154D RID: 5453
	private bool m_changedToNewGamePlus;
}
