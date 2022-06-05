using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameEventTracking
{
	// Token: 0x020008A6 RID: 2214
	public class EnemyPreviewController : MonoBehaviour
	{
		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x00103D4D File Offset: 0x00101F4D
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x00103D55 File Offset: 0x00101F55
		public void Initialise(EnemyTrackerData enemyData)
		{
			this.m_image.sprite = AssetPreviewManager.GetPreviewImage(enemyData.EnemyType, enemyData.EnemyRank);
		}

		// Token: 0x04003D05 RID: 15621
		[SerializeField]
		private Image m_image;
	}
}
