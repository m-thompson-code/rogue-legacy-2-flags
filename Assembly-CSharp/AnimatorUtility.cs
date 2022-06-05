using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public static class AnimatorUtility
{
	// Token: 0x0600416E RID: 16750 RVA: 0x000E9678 File Offset: 0x000E7878
	private static bool AddAnimatorInfo(Animator animator)
	{
		if (!animator || !animator.isInitialized)
		{
			return false;
		}
		string cachedAnimatorName = animator.GetCachedAnimatorName();
		if (string.IsNullOrEmpty(cachedAnimatorName))
		{
			return false;
		}
		if (!global::AnimatorUtility.m_animatorInfoDict.ContainsKey(cachedAnimatorName))
		{
			AnimatorInfo value = new AnimatorInfo(animator);
			global::AnimatorUtility.m_animatorInfoDict.Add(cachedAnimatorName, value);
		}
		return true;
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x000E96CC File Offset: 0x000E78CC
	public static AnimatorInfo GetAnimatorInfo(Animator animator)
	{
		if (!animator)
		{
			return null;
		}
		string cachedAnimatorName = animator.GetCachedAnimatorName();
		if (string.IsNullOrEmpty(cachedAnimatorName))
		{
			return null;
		}
		if (global::AnimatorUtility.AddAnimatorInfo(animator))
		{
			return global::AnimatorUtility.m_animatorInfoDict[cachedAnimatorName];
		}
		return null;
	}

	// Token: 0x06004170 RID: 16752 RVA: 0x000E970C File Offset: 0x000E790C
	public static bool HasParameter(Animator animator, int paramHash)
	{
		if (!animator)
		{
			return false;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		return animatorInfo != null && animatorInfo.HasParameter(paramHash);
	}

	// Token: 0x06004171 RID: 16753 RVA: 0x000E9738 File Offset: 0x000E7938
	public static bool HasParameter(Animator animator, string parameterName)
	{
		int paramHash = Animator.StringToHash(parameterName);
		return global::AnimatorUtility.HasParameter(animator, paramHash);
	}

	// Token: 0x06004172 RID: 16754 RVA: 0x000E9754 File Offset: 0x000E7954
	public static bool HasState(Animator animator, int stateHash)
	{
		if (!animator)
		{
			return false;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		return animatorInfo != null && animatorInfo.HasState(animator, stateHash);
	}

	// Token: 0x06004173 RID: 16755 RVA: 0x000E9780 File Offset: 0x000E7980
	public static bool HasState(Animator animator, string stateName)
	{
		int stateHash = Animator.StringToHash(stateName);
		return global::AnimatorUtility.HasState(animator, stateHash);
	}

	// Token: 0x06004174 RID: 16756 RVA: 0x000E979C File Offset: 0x000E799C
	public static int GetLayerIndex(Animator animator, int stateHash)
	{
		if (!animator)
		{
			return -1;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		if (animatorInfo == null)
		{
			return -1;
		}
		return animatorInfo.GetStateLayerIndex(animator, stateHash);
	}

	// Token: 0x06004175 RID: 16757 RVA: 0x000E97C8 File Offset: 0x000E79C8
	public static int GetLayerIndex(Animator animator, string stateName)
	{
		int stateHash = Animator.StringToHash(stateName);
		return global::AnimatorUtility.GetLayerIndex(animator, stateHash);
	}

	// Token: 0x06004176 RID: 16758 RVA: 0x000E97E4 File Offset: 0x000E79E4
	public static void ResetAnimator(Animator animator)
	{
		if (!animator)
		{
			return;
		}
		if (!animator.gameObject.activeInHierarchy)
		{
			animator.Rebind();
			return;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		if (animatorInfo == null)
		{
			return;
		}
		int num = animatorInfo.Parameters.Length;
		int i = 0;
		while (i < num)
		{
			AnimatorInfo.AnimatorInfoParameterInternal animatorInfoParameterInternal = animatorInfo.Parameters[i];
			int nameHash = animatorInfoParameterInternal.NameHash;
			AnimatorControllerParameterType parameterType = animatorInfoParameterInternal.ParameterType;
			switch (parameterType)
			{
			case AnimatorControllerParameterType.Float:
				animator.SetFloat(nameHash, animatorInfoParameterInternal.DefaultFloat);
				break;
			case (AnimatorControllerParameterType)2:
				break;
			case AnimatorControllerParameterType.Int:
				animator.SetInteger(nameHash, animatorInfoParameterInternal.DefaultInt);
				break;
			case AnimatorControllerParameterType.Bool:
				goto IL_71;
			default:
				if (parameterType == AnimatorControllerParameterType.Trigger)
				{
					goto IL_71;
				}
				break;
			}
			IL_9F:
			i++;
			continue;
			IL_71:
			animator.SetBool(nameHash, animatorInfoParameterInternal.DefaultBool);
			goto IL_9F;
		}
		animator.Play(animatorInfo.StartingStateHash, 0, 0f);
	}

	// Token: 0x06004177 RID: 16759 RVA: 0x000E98AC File Offset: 0x000E7AAC
	public static string GetCachedAnimatorName(this Animator animator)
	{
		if (!animator.runtimeAnimatorController)
		{
			return null;
		}
		string name;
		if (global::AnimatorUtility.m_animatorNameTable.TryGetValue(animator.runtimeAnimatorController, out name))
		{
			return name;
		}
		name = animator.runtimeAnimatorController.name;
		global::AnimatorUtility.m_animatorNameTable.Add(animator.runtimeAnimatorController, name);
		return name;
	}

	// Token: 0x06004178 RID: 16760 RVA: 0x000E98FC File Offset: 0x000E7AFC
	public static void ClearAnimatorTables()
	{
		global::AnimatorUtility.m_animatorNameTable.Clear();
		global::AnimatorUtility.m_animatorInfoDict.Clear();
	}

	// Token: 0x04003929 RID: 14633
	private static Dictionary<RuntimeAnimatorController, string> m_animatorNameTable = new Dictionary<RuntimeAnimatorController, string>();

	// Token: 0x0400392A RID: 14634
	private static Dictionary<string, AnimatorInfo> m_animatorInfoDict = new Dictionary<string, AnimatorInfo>();
}
