using System;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public class AnimatorInfo
{
	// Token: 0x1700164D RID: 5709
	// (get) Token: 0x06004162 RID: 16738 RVA: 0x000E93E4 File Offset: 0x000E75E4
	// (set) Token: 0x06004163 RID: 16739 RVA: 0x000E93EC File Offset: 0x000E75EC
	public AnimatorInfo.AnimatorInfoParameterInternal[] Parameters { get; private set; }

	// Token: 0x1700164E RID: 5710
	// (get) Token: 0x06004164 RID: 16740 RVA: 0x000E93F5 File Offset: 0x000E75F5
	// (set) Token: 0x06004165 RID: 16741 RVA: 0x000E93FD File Offset: 0x000E75FD
	public int StartingStateHash { get; private set; }

	// Token: 0x06004166 RID: 16742 RVA: 0x000E9406 File Offset: 0x000E7606
	public bool HasState(Animator animator, string stateName)
	{
		return this.GetStateLayerIndex(animator, stateName) != -1;
	}

	// Token: 0x06004167 RID: 16743 RVA: 0x000E9416 File Offset: 0x000E7616
	public bool HasState(Animator animator, int stateNameHash)
	{
		return this.GetStateLayerIndex(animator, stateNameHash) != -1;
	}

	// Token: 0x06004168 RID: 16744 RVA: 0x000E9428 File Offset: 0x000E7628
	public int GetStateLayerIndex(Animator animator, string stateName)
	{
		int stateNameHash = Animator.StringToHash(stateName);
		return this.GetStateLayerIndex(animator, stateNameHash);
	}

	// Token: 0x06004169 RID: 16745 RVA: 0x000E9444 File Offset: 0x000E7644
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

	// Token: 0x0600416A RID: 16746 RVA: 0x000E952C File Offset: 0x000E772C
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

	// Token: 0x0600416B RID: 16747 RVA: 0x000E9565 File Offset: 0x000E7765
	public bool HasParameter(string parameterName)
	{
		return this.HasParameter(Animator.StringToHash(parameterName));
	}

	// Token: 0x0600416C RID: 16748 RVA: 0x000E9574 File Offset: 0x000E7774
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

	// Token: 0x0600416D RID: 16749 RVA: 0x000E9644 File Offset: 0x000E7844
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

	// Token: 0x04003924 RID: 14628
	private AnimatorInfo.AnimatorInfoStateInternal[] m_states;

	// Token: 0x04003925 RID: 14629
	private int m_stateIndex = -1;

	// Token: 0x04003926 RID: 14630
	private uint m_stateCount;

	// Token: 0x02000E35 RID: 3637
	private struct AnimatorInfoStateInternal
	{
		// Token: 0x04005732 RID: 22322
		public int StateNameHash;

		// Token: 0x04005733 RID: 22323
		public int LayerIndex;
	}

	// Token: 0x02000E36 RID: 3638
	public struct AnimatorInfoParameterInternal
	{
		// Token: 0x04005734 RID: 22324
		public int NameHash;

		// Token: 0x04005735 RID: 22325
		public int DefaultInt;

		// Token: 0x04005736 RID: 22326
		public bool DefaultBool;

		// Token: 0x04005737 RID: 22327
		public float DefaultFloat;

		// Token: 0x04005738 RID: 22328
		public AnimatorControllerParameterType ParameterType;
	}
}
