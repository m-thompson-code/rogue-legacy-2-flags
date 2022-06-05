using System;
using System.Collections;
using RL_Windows;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class SkillTreeAboveGroundController : MonoBehaviour
{
	// Token: 0x0600406B RID: 16491 RVA: 0x00102CCC File Offset: 0x00100ECC
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

	// Token: 0x0600406C RID: 16492 RVA: 0x00023944 File Offset: 0x00021B44
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

	// Token: 0x0600406D RID: 16493 RVA: 0x00102DA0 File Offset: 0x00100FA0
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

	// Token: 0x0400326F RID: 12911
	[SerializeField]
	private GameObject m_skillTreeCastle_SurfaceGrass;

	// Token: 0x04003270 RID: 12912
	[SerializeField]
	private GameObject m_skillTreeCastle_SurfaceRock;

	// Token: 0x04003271 RID: 12913
	private Weather m_weather;
}
