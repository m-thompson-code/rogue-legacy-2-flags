using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameEventTracking
{
	// Token: 0x02000DD7 RID: 3543
	public class EnemyPreviewController : MonoBehaviour
	{
		// Token: 0x1700201A RID: 8218
		// (get) Token: 0x0600638F RID: 25487 RVA: 0x00003713 File Offset: 0x00001913
		public GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		// Token: 0x06006390 RID: 25488 RVA: 0x00036DFE File Offset: 0x00034FFE
		public void Initialise(EnemyTrackerData enemyData)
		{
			this.m_image.sprite = AssetPreviewManager.GetPreviewImage(enemyData.EnemyType, enemyData.EnemyRank);
		}

		// Token: 0x04005145 RID: 20805
		[SerializeField]
		private Image m_image;
	}
}
