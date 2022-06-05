using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200033F RID: 831
public class CDGSpriteAnimator : MonoBehaviour
{
	// Token: 0x06001AE3 RID: 6883 RVA: 0x0000DF15 File Offset: 0x0000C115
	private void OnEnable()
	{
		this.m_frameCounter = 0f;
		this.m_lastSpriteIndex = -1;
		base.StartCoroutine(this.Animate());
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x0000DF36 File Offset: 0x0000C136
	private IEnumerator Animate()
	{
		for (;;)
		{
			int num = (int)this.m_frameCounter;
			if (num >= this.m_sprites.Length)
			{
				if (!this.m_loop)
				{
					break;
				}
				if (this.m_loopingDelay > 0f)
				{
					float delayTime = Time.time + this.m_loopingDelay;
					while (Time.time < delayTime)
					{
						yield return null;
					}
				}
				this.m_frameCounter = (float)this.m_introFrames + (this.m_frameCounter - (float)this.m_introFrames) % (float)(this.m_sprites.Length - this.m_introFrames);
				num = (int)this.m_frameCounter;
			}
			num = Mathf.Clamp(num, 0, this.m_sprites.Length - 1);
			if (num != this.m_lastSpriteIndex)
			{
				this.m_renderer.sprite = this.m_sprites[num];
			}
			this.m_lastSpriteIndex = num;
			this.m_frameCounter += Time.deltaTime * (float)this.m_fps;
			yield return null;
		}
		yield break;
		yield break;
	}

	// Token: 0x040018FA RID: 6394
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x040018FB RID: 6395
	[SerializeField]
	private Sprite[] m_sprites;

	// Token: 0x040018FC RID: 6396
	[SerializeField]
	private int m_introFrames;

	// Token: 0x040018FD RID: 6397
	[SerializeField]
	private int m_fps;

	// Token: 0x040018FE RID: 6398
	[SerializeField]
	private bool m_loop;

	// Token: 0x040018FF RID: 6399
	[SerializeField]
	private float m_loopingDelay;

	// Token: 0x04001900 RID: 6400
	private float m_frameCounter;

	// Token: 0x04001901 RID: 6401
	private int m_lastSpriteIndex;
}
