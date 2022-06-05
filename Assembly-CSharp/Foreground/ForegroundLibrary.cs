using System;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000DC5 RID: 3525
	[CreateAssetMenu(menuName = "Custom/Libraries/Foreground Library")]
	public class ForegroundLibrary : ScriptableObject
	{
		// Token: 0x0600634A RID: 25418 RVA: 0x00036B25 File Offset: 0x00034D25
		public ForegroundGroup[] GetForegrounds()
		{
			return this.m_foregroundGroups;
		}

		// Token: 0x04005115 RID: 20757
		[SerializeField]
		private ForegroundGroup[] m_foregroundGroups;
	}
}
