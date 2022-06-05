using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200081E RID: 2078
public class WaitRL_Yield : IEnumerator
{
	// Token: 0x17001710 RID: 5904
	// (get) Token: 0x060044D9 RID: 17625 RVA: 0x000F4DBE File Offset: 0x000F2FBE
	// (set) Token: 0x060044DA RID: 17626 RVA: 0x000F4DC6 File Offset: 0x000F2FC6
	public float WaitTime { get; private set; }

	// Token: 0x060044DB RID: 17627 RVA: 0x000F4DCF File Offset: 0x000F2FCF
	public WaitRL_Yield(float waitTime, bool useUnscaledTime = false)
	{
		this.CreateNew(waitTime, useUnscaledTime);
		this.m_needsReset = true;
	}

	// Token: 0x060044DC RID: 17628 RVA: 0x000F4DE6 File Offset: 0x000F2FE6
	public void Pause()
	{
		this.m_isPaused = true;
		if (this.m_useUnscaledTime)
		{
			this.m_pauseTime = Time.unscaledTime;
			return;
		}
		this.m_pauseTime = Time.time;
	}

	// Token: 0x060044DD RID: 17629 RVA: 0x000F4E0E File Offset: 0x000F300E
	public void Unpause()
	{
		this.m_isPaused = false;
		if (this.m_useUnscaledTime)
		{
			this.m_pauseTime = Time.unscaledTime - this.m_pauseTime;
			return;
		}
		this.m_pauseTime = Time.time - this.m_pauseTime;
	}

	// Token: 0x17001711 RID: 5905
	// (get) Token: 0x060044DE RID: 17630 RVA: 0x000F4E44 File Offset: 0x000F3044
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060044DF RID: 17631 RVA: 0x000F4E48 File Offset: 0x000F3048
	public bool MoveNext()
	{
		if (this.m_needsReset)
		{
			this.CreateNew(this.WaitTime, this.m_useUnscaledTime);
		}
		if (this.m_isPaused)
		{
			return true;
		}
		bool flag;
		if (!this.m_useUnscaledTime)
		{
			flag = (Time.time < this.m_startingTime + this.m_pauseTime + this.WaitTime);
		}
		else
		{
			flag = (Time.unscaledTime < this.m_startingTime + this.m_pauseTime + this.WaitTime);
		}
		if (!flag)
		{
			this.m_needsReset = true;
		}
		return flag;
	}

	// Token: 0x060044E0 RID: 17632 RVA: 0x000F4EC8 File Offset: 0x000F30C8
	public void CreateNew(float waitTime, bool useUnscaledTime = false)
	{
		this.m_isPaused = false;
		this.m_pauseTime = 0f;
		this.m_needsReset = false;
		this.m_useUnscaledTime = useUnscaledTime;
		if (!this.m_useUnscaledTime)
		{
			this.m_startingTime = Time.time;
		}
		else
		{
			this.m_startingTime = Time.unscaledTime;
		}
		this.WaitTime = waitTime;
	}

	// Token: 0x060044E1 RID: 17633 RVA: 0x000F4F1C File Offset: 0x000F311C
	public void Reset()
	{
		this.CreateNew(this.WaitTime, false);
	}

	// Token: 0x04003AB7 RID: 15031
	private float m_startingTime;

	// Token: 0x04003AB8 RID: 15032
	private bool m_needsReset;

	// Token: 0x04003AB9 RID: 15033
	private bool m_useUnscaledTime;

	// Token: 0x04003ABA RID: 15034
	private bool m_isPaused;

	// Token: 0x04003ABB RID: 15035
	private float m_pauseTime;
}
