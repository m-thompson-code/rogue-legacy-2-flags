using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006A5 RID: 1701
[Serializable]
public class RNGController
{
	// Token: 0x17001566 RID: 5478
	// (get) Token: 0x06003E59 RID: 15961 RVA: 0x000DBD3B File Offset: 0x000D9F3B
	// (set) Token: 0x06003E5A RID: 15962 RVA: 0x000DBD43 File Offset: 0x000D9F43
	public List<RNGData> CallerData { get; private set; }

	// Token: 0x17001567 RID: 5479
	// (get) Token: 0x06003E5B RID: 15963 RVA: 0x000DBD4C File Offset: 0x000D9F4C
	// (set) Token: 0x06003E5C RID: 15964 RVA: 0x000DBD54 File Offset: 0x000D9F54
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

	// Token: 0x17001568 RID: 5480
	// (get) Token: 0x06003E5D RID: 15965 RVA: 0x000DBD5D File Offset: 0x000D9F5D
	// (set) Token: 0x06003E5E RID: 15966 RVA: 0x000DBD65 File Offset: 0x000D9F65
	public bool IsInitialized { get; private set; }

	// Token: 0x17001569 RID: 5481
	// (get) Token: 0x06003E5F RID: 15967 RVA: 0x000DBD6E File Offset: 0x000D9F6E
	// (set) Token: 0x06003E60 RID: 15968 RVA: 0x000DBD76 File Offset: 0x000D9F76
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

	// Token: 0x06003E61 RID: 15969 RVA: 0x000DBD7F File Offset: 0x000D9F7F
	public RNGController(RngID id)
	{
		this.ID = id;
		this.Seed = -1;
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x000DBD9C File Offset: 0x000D9F9C
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

	// Token: 0x06003E63 RID: 15971 RVA: 0x000DBDF8 File Offset: 0x000D9FF8
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

	// Token: 0x06003E64 RID: 15972 RVA: 0x000DBE59 File Offset: 0x000DA059
	private void InitialiseRandomNumberGenerator(int seed)
	{
		this.Seed = seed;
		this.m_randomNumberGenerator = new System.Random(this.Seed);
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x000DBE74 File Offset: 0x000DA074
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

	// Token: 0x04002E63 RID: 11875
	private RngID m_id;

	// Token: 0x04002E64 RID: 11876
	private System.Random m_randomNumberGenerator;

	// Token: 0x04002E65 RID: 11877
	private int m_seed = -1;
}
