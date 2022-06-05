using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C51 RID: 3153
public static class AnimatorUtility
{
	// Token: 0x06005AEB RID: 23275 RVA: 0x00158A28 File Offset: 0x00156C28
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

	// Token: 0x06005AEC RID: 23276 RVA: 0x00158A7C File Offset: 0x00156C7C
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

	// Token: 0x06005AED RID: 23277 RVA: 0x00158ABC File Offset: 0x00156CBC
	public static bool HasParameter(Animator animator, int paramHash)
	{
		if (!animator)
		{
			return false;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		return animatorInfo != null && animatorInfo.HasParameter(paramHash);
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x00158AE8 File Offset: 0x00156CE8
	public static bool HasParameter(Animator animator, string parameterName)
	{
		int paramHash = Animator.StringToHash(parameterName);
		return global::AnimatorUtility.HasParameter(animator, paramHash);
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x00158B04 File Offset: 0x00156D04
	public static bool HasState(Animator animator, int stateHash)
	{
		if (!animator)
		{
			return false;
		}
		AnimatorInfo animatorInfo = global::AnimatorUtility.GetAnimatorInfo(animator);
		return animatorInfo != null && animatorInfo.HasState(animator, stateHash);
	}

	// Token: 0x06005AF0 RID: 23280 RVA: 0x00158B30 File Offset: 0x00156D30
	public static bool HasState(Animator animator, string stateName)
	{
		int stateHash = Animator.StringToHash(stateName);
		return global::AnimatorUtility.HasState(animator, stateHash);
	}

	// Token: 0x06005AF1 RID: 23281 RVA: 0x00158B4C File Offset: 0x00156D4C
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

	// Token: 0x06005AF2 RID: 23282 RVA: 0x00158B78 File Offset: 0x00156D78
	public static int GetLayerIndex(Animator animator, string stateName)
	{
		int stateHash = Animator.StringToHash(stateName);
		return global::AnimatorUtility.GetLayerIndex(animator, stateHash);
	}

	// Token: 0x06005AF3 RID: 23283 RVA: 0x00158B94 File Offset: 0x00156D94
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

	// Token: 0x06005AF4 RID: 23284 RVA: 0x00158C5C File Offset: 0x00156E5C
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

	// Token: 0x06005AF5 RID: 23285 RVA: 0x00031E52 File Offset: 0x00030052
	public static void ClearAnimatorTables()
	{
		global::AnimatorUtility.m_animatorNameTable.Clear();
		global::AnimatorUtility.m_animatorInfoDict.Clear();
	}

	// Token: 0x04004BE0 RID: 19424
	private static Dictionary<RuntimeAnimatorController, string> m_animatorNameTable = new Dictionary<RuntimeAnimatorController, string>();

	// Token: 0x04004BE1 RID: 19425
	private static Dictionary<string, AnimatorInfo> m_animatorInfoDict = new Dictionary<string, AnimatorInfo>();
}
