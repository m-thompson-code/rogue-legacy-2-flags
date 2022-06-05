using System;
using TMPro;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public class MapTextUpdater : MonoBehaviour, ILocalizable
{
	// Token: 0x06002F98 RID: 12184 RVA: 0x0001A0E7 File Offset: 0x000182E7
	private void Awake()
	{
		this.m_refreshText = new Action<MonoBehaviour, EventArgs>(this.RefreshText);
	}

	// Token: 0x06002F99 RID: 12185 RVA: 0x0001A0FC File Offset: 0x000182FC
	private void OnEnable()
	{
		this.RefreshText(null, null);
		Messenger<UIMessenger, UIEvent>.AddListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002F9A RID: 12186 RVA: 0x0001A113 File Offset: 0x00018313
	private void OnDisable()
	{
		Messenger<UIMessenger, UIEvent>.RemoveListener(UIEvent.LanguageChanged, this.m_refreshText);
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x000CB1CC File Offset: 0x000C93CC
	public void RefreshText(object sender, EventArgs args)
	{
		this.m_text.text = LocalizationManager.GetString(NowEnteringHUDController.CurrentLocID, false, false);
		int num = NowEnteringHUDController.CurrentRiskLevel;
		bool flag = PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom() && PlayerManager.GetCurrentPlayerRoom().BiomeType == BiomeType.Tutorial;
		EndingSpawnRoomTypeController endingSpawnRoomTypeController = null;
		if (PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom())
		{
			endingSpawnRoomTypeController = PlayerManager.GetCurrentPlayerRoom().GetComponent<EndingSpawnRoomTypeController>();
		}
		if (endingSpawnRoomTypeController)
		{
			if (endingSpawnRoomTypeController.EndingSpawnRoomType == EndingSpawnRoomType.Docks)
			{
				this.m_text.text = LocalizationManager.GetString(BiomeDataLibrary.GetData(BiomeType.HubTown).BiomeNameLocID, false, false);
				num = -2;
			}
			else if (endingSpawnRoomTypeController.EndingSpawnRoomType < EndingSpawnRoomType.AboveGround)
			{
				this.m_text.text = LocalizationManager.GetString("LOC_ID_LOCATION_TITLE_HOME_1", false, false);
				num = 7;
			}
			else if (endingSpawnRoomTypeController.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
			{
				this.m_text.text = LocalizationManager.GetString("LOC_ID_LOCATION_SUBTITLE_HOME_1", false, false);
				num = -2;
			}
		}
		else if (SaveManager.PlayerSaveData.InHubTown)
		{
			this.m_text.text = LocalizationManager.GetString(BiomeDataLibrary.GetData(BiomeType.HubTown).BiomeNameLocID, false, false);
			num = -2;
		}
		else if (flag)
		{
			this.m_text.text = "";
			num = -2;
		}
		if (num == -2)
		{
			this.m_grayStarsGO.SetActive(false);
			this.m_yellowStarsGO.SetActive(false);
			this.m_unknownStarsGO.SetActive(false);
			this.m_dangerGO.SetActive(false);
			return;
		}
		if (num == -1)
		{
			this.m_grayStarsGO.SetActive(false);
			this.m_yellowStarsGO.SetActive(false);
			this.m_unknownStarsGO.SetActive(true);
			this.m_dangerGO.SetActive(true);
			return;
		}
		this.m_grayStarsGO.SetActive(true);
		this.m_yellowStarsGO.SetActive(true);
		this.m_unknownStarsGO.SetActive(false);
		this.m_dangerGO.SetActive(true);
		GameObject gameObject = this.m_grayStarsGO.transform.GetChild(6).gameObject;
		GameObject gameObject2 = this.m_yellowStarsGO.transform.GetChild(6).gameObject;
		if (num > 6)
		{
			gameObject.gameObject.SetActive(true);
			gameObject2.gameObject.SetActive(true);
		}
		else
		{
			gameObject.gameObject.SetActive(false);
			gameObject2.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.m_yellowStarsGO.transform.childCount; i++)
		{
			GameObject gameObject3 = this.m_yellowStarsGO.transform.GetChild(i).gameObject;
			if (i < num)
			{
				gameObject3.SetActive(true);
			}
			else
			{
				gameObject3.SetActive(false);
			}
		}
	}

	// Token: 0x04002700 RID: 9984
	[SerializeField]
	private TMP_Text m_text;

	// Token: 0x04002701 RID: 9985
	[SerializeField]
	private GameObject m_dangerGO;

	// Token: 0x04002702 RID: 9986
	[SerializeField]
	private GameObject m_grayStarsGO;

	// Token: 0x04002703 RID: 9987
	[SerializeField]
	private GameObject m_yellowStarsGO;

	// Token: 0x04002704 RID: 9988
	[SerializeField]
	private GameObject m_unknownStarsGO;

	// Token: 0x04002705 RID: 9989
	private Action<MonoBehaviour, EventArgs> m_refreshText;
}
