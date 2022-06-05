using System;
using UnityEngine;

// Token: 0x02000C4E RID: 3150
public class AnimatorInfo
{
	// Token: 0x17001E49 RID: 7753
	// (get) Token: 0x06005ADF RID: 23263 RVA: 0x00031E02 File Offset: 0x00030002
	// (set) Token: 0x06005AE0 RID: 23264 RVA: 0x00031E0A File Offset: 0x0003000A
	public AnimatorInfo.AnimatorInfoParameterInternal[] Parameters { get; private set; }

	// Token: 0x17001E4A RID: 7754
	// (get) Token: 0x06005AE1 RID: 23265 RVA: 0x00031E13 File Offset: 0x00030013
	// (set) Token: 0x06005AE2 RID: 23266 RVA: 0x00031E1B File Offset: 0x0003001B
	public int StartingStateHash { get; private set; }

	// Token: 0x06005AE3 RID: 23267 RVA: 0x00031E24 File Offset: 0x00030024
	public bool HasState(Animator animator, string stateName)
	{
		return this.GetStateLayerIndex(animator, stateName) != -1;
	}

	// Token: 0x06005AE4 RID: 23268 RVA: 0x00031E34 File Offset: 0x00030034
	public bool HasState(Animator animator, int stateNameHash)
	{
		return this.GetStateLayerIndex(animator, stateNameHash) != -1;
	}

	// Token: 0x06005AE5 RID: 23269 RVA: 0x001587E4 File Offset: 0x001569E4
	public int GetStateLayerIndex(Animator animator, string stateName)
	{
		int stateNameHash = Animator.StringToHash(stateName);
		return this.GetStateLayerIndex(animator, stateNameHash);
	}

	// Token: 0x06005AE6 RID: 23270 RVA: 0x00158800 File Offset: 0x00156A00
	public int GetStateLayerIndex(Animator animator, int stateNameHash)
	{
		if ((long)(this.m_stateIndex + 1) == (long)((ulong)this.m_stateCount))
		{
			this.m_states = this.Expand(this.m_states, this.m_stateCount * 2U, this.m_stateCount);
			this.m_stateCount *= 2U;
		}
		for (int i = 0; i <= this.m_stateIndex; i++)
		{
			AnimatorInfo.AnimatorInfoStateInternal animatorInfoStateInternal = this.m_states[i];
			if (animatorInfoStateInternal.StateNameHash == stateNameHash)
			{
				return animatorInfoStateInternal.LayerIndex;
			}
		}
		int layerIndex = -1;
		int layerCount = animator.layerCount;
		for (int j = 0; j < layerCount; j++)
		{
			if (animator.HasState(j, stateNameHash))
			{
				layerIndex = j;
				break;
			}
		}
		AnimatorInfo.AnimatorInfoStateInternal animatorInfoStateInternal2 = new AnimatorInfo.AnimatorInfoStateInternal
		{
			StateNameHash = stateNameHash,
			LayerIndex = layerIndex
		};
		this.m_stateIndex++;
		this.m_states[this.m_stateIndex] = animatorInfoStateInternal2;
		return animatorInfoStateInternal2.LayerIndex;
	}

	// Token: 0x06005AE7 RID: 23271 RVA: 0x001588E8 File Offset: 0x00156AE8
	public bool HasParameter(int parameterNameHash)
	{
		int num = this.Parameters.Length;
		for (int i = 0; i < num; i++)
		{
			if (this.Parameters[i].NameHash == parameterNameHash)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x00031E44 File Offset: 0x00030044
	public bool HasParameter(string parameterName)
	{
		return this.HasParameter(Animator.StringToHash(parameterName));
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x00158924 File Offset: 0x00156B24
	public AnimatorInfo(Animator animator)
	{
		this.StartingStateHash = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
		int parameterCount = animator.parameterCount;
		this.Parameters = new AnimatorInfo.AnimatorInfoParameterInternal[parameterCount];
		AnimatorControllerParameter[] parameters = animator.parameters;
		for (int i = 0; i < parameterCount; i++)
		{
			AnimatorControllerParameter animatorControllerParameter = parameters[i];
			AnimatorInfo.AnimatorInfoParameterInternal animatorInfoParameterInternal = default(AnimatorInfo.AnimatorInfoParameterInternal);
			animatorInfoParameterInternal.NameHash = animatorControllerParameter.nameHash;
			animatorInfoParameterInternal.ParameterType = animatorControllerParameter.type;
			animatorInfoParameterInternal.DefaultBool = animatorControllerParameter.defaultBool;
			animatorInfoParameterInternal.DefaultFloat = animatorControllerParameter.defaultFloat;
			animatorInfoParameterInternal.DefaultInt = animatorControllerParameter.defaultInt;
			this.Parameters[i] = animatorInfoParameterInternal;
		}
		this.m_stateCount = 1U;
		this.m_states = new AnimatorInfo.AnimatorInfoStateInternal[this.m_stateCount];
	}

	// Token: 0x06005AEA RID: 23274 RVA: 0x001589F4 File Offset: 0x00156BF4
	private AnimatorInfo.AnimatorInfoStateInternal[] Expand(AnimatorInfo.AnimatorInfoStateInternal[] arr, uint cap, uint count)
	{
		AnimatorInfo.AnimatorInfoStateInternal[] array = new AnimatorInfo.AnimatorInfoStateInternal[cap];
		int num = 0;
		while ((long)num < (long)((ulong)count))
		{
			array[num] = arr[num];
			num++;
		}
		return array;
	}

	// Token: 0x04004BD4 RID: 19412
	private AnimatorInfo.AnimatorInfoStateInternal[] m_states;

	// Token: 0x04004BD5 RID: 19413
	private int m_stateIndex = -1;

	// Token: 0x04004BD6 RID: 19414
	private uint m_stateCount;

	// Token: 0x02000C4F RID: 3151
	private struct AnimatorInfoStateInternal
	{
		// Token: 0x04004BD9 RID: 19417
		public int StateNameHash;

		// Token: 0x04004BDA RID: 19418
		public int LayerIndex;
	}

	// Token: 0x02000C50 RID: 3152
	public struct AnimatorInfoParameterInternal
	{
		// Token: 0x04004BDB RID: 19419
		public int NameHash;

		// Token: 0x04004BDC RID: 19420
		public int DefaultInt;

		// Token: 0x04004BDD RID: 19421
		public bool DefaultBool;

		// Token: 0x04004BDE RID: 19422
		public float DefaultFloat;

		// Token: 0x04004BDF RID: 19423
		public AnimatorControllerParameterType ParameterType;
	}
}
