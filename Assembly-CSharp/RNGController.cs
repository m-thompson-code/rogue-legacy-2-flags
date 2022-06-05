using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B41 RID: 2881
[Serializable]
public class RNGController
{
	// Token: 0x17001D50 RID: 7504
	// (get) Token: 0x06005751 RID: 22353 RVA: 0x0002F881 File Offset: 0x0002DA81
	// (set) Token: 0x06005752 RID: 22354 RVA: 0x0002F889 File Offset: 0x0002DA89
	public List<RNGData> CallerData { get; private set; }

	// Token: 0x17001D51 RID: 7505
	// (get) Token: 0x06005753 RID: 22355 RVA: 0x0002F892 File Offset: 0x0002DA92
	// (set) Token: 0x06005754 RID: 22356 RVA: 0x0002F89A File Offset: 0x0002DA9A
	public RngID ID
	{
		get
		{
			return this.m_id;
		}
		private set
		{
			this.m_id = value;
		}
	}

	// Token: 0x17001D52 RID: 7506
	// (get) Token: 0x06005755 RID: 22357 RVA: 0x0002F8A3 File Offset: 0x0002DAA3
	// (set) Token: 0x06005756 RID: 22358 RVA: 0x0002F8AB File Offset: 0x0002DAAB
	public bool IsInitialized { get; private set; }

	// Token: 0x17001D53 RID: 7507
	// (get) Token: 0x06005757 RID: 22359 RVA: 0x0002F8B4 File Offset: 0x0002DAB4
	// (set) Token: 0x06005758 RID: 22360 RVA: 0x0002F8BC File Offset: 0x0002DABC
	public int Seed
	{
		get
		{
			return this.m_seed;
		}
		private set
		{
			this.m_seed = value;
		}
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x0002F8C5 File Offset: 0x0002DAC5
	public RNGController(RngID id)
	{
		this.ID = id;
		this.Seed = -1;
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x0014C304 File Offset: 0x0014A504
	public int GetRandomNumber(string callerDescription, int min, int max)
	{
		if (min >= max)
		{
			throw new ArgumentException("min or max", string.Format("|{0}| argument min ({1}) must be less than argument max ({2})", this, min, max));
		}
		if (string.IsNullOrEmpty(callerDescription))
		{
			Debug.LogFormat("<color=red>| {0} | callerDescription argument is empty. Please add a description to your method call</color>", new object[]
			{
				this
			});
		}
		return this.m_randomNumberGenerator.Next(min, max);
	}

	// Token: 0x0600575B RID: 22363 RVA: 0x0014C360 File Offset: 0x0014A560
	public float GetRandomNumber(string callerDescription, float min, float max)
	{
		if (min >= max)
		{
			throw new ArgumentException("min or max", string.Format("|{0}| argument min ({1}) must be less than argument max ({2})", this, min, max));
		}
		if (string.IsNullOrEmpty(callerDescription))
		{
			Debug.LogFormat("<color=red>|{0}| callerDescription argument is empty. Please add a description to your method call</color>", new object[]
			{
				this
			});
		}
		return min + (float)this.m_randomNumberGenerator.NextDouble() * (max - min);
	}

	// Token: 0x0600575C RID: 22364 RVA: 0x0002F8E2 File Offset: 0x0002DAE2
	private void InitialiseRandomNumberGenerator(int seed)
	{
		this.Seed = seed;
		this.m_randomNumberGenerator = new System.Random(this.Seed);
	}

	// Token: 0x0600575D RID: 22365 RVA: 0x0014C3C4 File Offset: 0x0014A5C4
	public void SetSeed(int seed)
	{
		if (Application.isPlaying)
		{
			if (this.CallerData == null)
			{
				this.CallerData = new List<RNGData>();
			}
			else
			{
				this.CallerData.Clear();
			}
			this.InitialiseRandomNumberGenerator(seed);
			this.IsInitialized = true;
			return;
		}
		Debug.LogFormat("<color=red>|{0}| RNGController's Initialise Method was called during Edit Mode, BUT SHOULD NOT BE</color>", new object[]
		{
			this
		});
	}

	// Token: 0x04004083 RID: 16515
	private RngID m_id;

	// Token: 0x04004084 RID: 16516
	private System.Random m_randomNumberGenerator;

	// Token: 0x04004085 RID: 16517
	private int m_seed = -1;
}
