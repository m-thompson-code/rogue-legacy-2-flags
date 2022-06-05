using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class CDGSpriteAnimator : MonoBehaviour
{
	// Token: 0x0600126D RID: 4717 RVA: 0x00035CE4 File Offset: 0x00033EE4
	private void OnEnable()
	{
		this.m_frameCounter = 0f;
		this.m_lastSpriteIndex = -1;
		base.StartCoroutine(this.Animate());
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x00035D05 File Offset: 0x00033F05
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

	// Token: 0x040012D0 RID: 4816
	[SerializeField]
	private SpriteRenderer m_renderer;

	// Token: 0x040012D1 RID: 4817
	[SerializeField]
	private Sprite[] m_sprites;

	// Token: 0x040012D2 RID: 4818
	[SerializeField]
	private int m_introFrames;

	// Token: 0x040012D3 RID: 4819
	[SerializeField]
	private int m_fps;

	// Token: 0x040012D4 RID: 4820
	[SerializeField]
	private bool m_loop;

	// Token: 0x040012D5 RID: 4821
	[SerializeField]
	private float m_loopingDelay;

	// Token: 0x040012D6 RID: 4822
	private float m_frameCounter;

	// Token: 0x040012D7 RID: 4823
	private int m_lastSpriteIndex;
}
