using System;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x02000EE9 RID: 3817
	[AddComponentMenu("")]
	public class Bullet : MonoBehaviour
	{
		// Token: 0x06006E6F RID: 28271 RVA: 0x0003CBC0 File Offset: 0x0003ADC0
		private void Start()
		{
			if (this.lifeTime > 0f)
			{
				this.deathTime = Time.time + this.lifeTime;
				this.die = true;
			}
		}

		// Token: 0x06006E70 RID: 28272 RVA: 0x0003CBE8 File Offset: 0x0003ADE8
		private void Update()
		{
			if (this.die && Time.time >= this.deathTime)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040058C5 RID: 22725
		public float lifeTime = 3f;

		// Token: 0x040058C6 RID: 22726
		private bool die;

		// Token: 0x040058C7 RID: 22727
		private float deathTime;
	}
}
