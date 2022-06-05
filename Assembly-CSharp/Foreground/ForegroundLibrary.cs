using System;
using UnityEngine;

namespace Foreground
{
	// Token: 0x02000898 RID: 2200
	[CreateAssetMenu(menuName = "Custom/Libraries/Foreground Library")]
	public class ForegroundLibrary : ScriptableObject
	{
		// Token: 0x0600480F RID: 18447 RVA: 0x0010379B File Offset: 0x0010199B
		public ForegroundGroup[] GetForegrounds()
		{
			return this.m_foregroundGroups;
		}

		// Token: 0x04003CE7 RID: 15591
		[SerializeField]
		private ForegroundGroup[] m_foregroundGroups;
	}
}
