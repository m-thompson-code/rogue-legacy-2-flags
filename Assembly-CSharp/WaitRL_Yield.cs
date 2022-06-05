using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000CF5 RID: 3317
public class WaitRL_Yield : IEnumerator
{
	// Token: 0x17001F1E RID: 7966
	// (get) Token: 0x06005EA5 RID: 24229 RVA: 0x00034290 File Offset: 0x00032490
	// (set) Token: 0x06005EA6 RID: 24230 RVA: 0x00034298 File Offset: 0x00032498
	public float WaitTime { get; private set; }

	// Token: 0x06005EA7 RID: 24231 RVA: 0x000342A1 File Offset: 0x000324A1
	public WaitRL_Yield(float waitTime, bool useUnscaledTime = false)
	{
		this.CreateNew(waitTime, useUnscaledTime);
		this.m_needsReset = true;
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x000342B8 File Offset: 0x000324B8
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

	// Token: 0x06005EA9 RID: 24233 RVA: 0x000342E0 File Offset: 0x000324E0
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

	// Token: 0x17001F1F RID: 7967
	// (get) Token: 0x06005EAA RID: 24234 RVA: 0x0000F49B File Offset: 0x0000D69B
	public object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06005EAB RID: 24235 RVA: 0x00162A4C File Offset: 0x00160C4C
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

	// Token: 0x06005EAC RID: 24236 RVA: 0x00162ACC File Offset: 0x00160CCC
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

	// Token: 0x06005EAD RID: 24237 RVA: 0x00034316 File Offset: 0x00032516
	public void Reset()
	{
		this.CreateNew(this.WaitTime, false);
	}

	// Token: 0x04004DBB RID: 19899
	private float m_startingTime;

	// Token: 0x04004DBC RID: 19900
	private bool m_needsReset;

	// Token: 0x04004DBD RID: 19901
	private bool m_useUnscaledTime;

	// Token: 0x04004DBE RID: 19902
	private bool m_isPaused;

	// Token: 0x04004DBF RID: 19903
	private float m_pauseTime;
}
