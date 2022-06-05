using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x020004E1 RID: 1249
public class SkillTreeAboveGroundController : MonoBehaviour
{
	// Token: 0x06002EB7 RID: 11959 RVA: 0x0009F244 File Offset: 0x0009D444
	private void OnEnable()
	{
		bool flag = false;
		if (PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom())
		{
			EndingSpawnRoomTypeController component = PlayerManager.GetCurrentPlayerRoom().GetComponent<EndingSpawnRoomTypeController>();
			if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (!this.m_skillTreeCastle_SurfaceGrass.activeSelf)
			{
				this.m_skillTreeCastle_SurfaceGrass.SetActive(true);
			}
			if (!this.m_skillTreeCastle_SurfaceRock.activeSelf)
			{
				this.m_skillTreeCastle_SurfaceRock.SetActive(true);
			}
			if (WindowManager.GetIsWindowOpen(WindowID.ConfirmMenuBig))
			{
				WindowManager.GetWindowController(WindowID.ConfirmMenuBig).WindowCanvas.sortingOrder = 30;
			}
			base.StartCoroutine(this.SetWeatherCoroutine());
			return;
		}
		if (this.m_skillTreeCastle_SurfaceGrass.activeSelf)
		{
			this.m_skillTreeCastle_SurfaceGrass.SetActive(false);
		}
		if (this.m_skillTreeCastle_SurfaceRock.activeSelf)
		{
			this.m_skillTreeCastle_SurfaceRock.SetActive(false);
		}
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x0009F316 File Offset: 0x0009D516
	private IEnumerator SetWeatherCoroutine()
	{
		float delay = 0.1f + Time.time;
		while (Time.time < delay)
		{
			yield return null;
		}
		if (!this.m_weather)
		{
			this.m_weather = UnityEngine.Object.FindObjectOfType<Weather>();
		}
		if (this.m_weather)
		{
			this.m_weather.gameObject.SetLayerRecursively(5, false);
		}
		yield break;
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x0009F328 File Offset: 0x0009D528
	private void OnDisable()
	{
		bool flag = false;
		if (PlayerManager.IsInstantiated && PlayerManager.GetCurrentPlayerRoom())
		{
			EndingSpawnRoomTypeController component = PlayerManager.GetCurrentPlayerRoom().GetComponent<EndingSpawnRoomTypeController>();
			if (component && component.EndingSpawnRoomType == EndingSpawnRoomType.AboveGround)
			{
				flag = true;
			}
		}
		if (flag && this.m_weather)
		{
			this.m_weather.gameObject.SetLayerRecursively(0, false);
		}
	}

	// Token: 0x04002543 RID: 9539
	[SerializeField]
	private GameObject m_skillTreeCastle_SurfaceGrass;

	// Token: 0x04002544 RID: 9540
	[SerializeField]
	private GameObject m_skillTreeCastle_SurfaceRock;

	// Token: 0x04002545 RID: 9541
	private Weather m_weather;
}
