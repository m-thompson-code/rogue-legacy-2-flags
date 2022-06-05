using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200031B RID: 795
public class AnimationController : MonoBehaviour
{
	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06001955 RID: 6485 RVA: 0x0000CBE7 File Offset: 0x0000ADE7
	// (set) Token: 0x06001956 RID: 6486 RVA: 0x0000CBEF File Offset: 0x0000ADEF
	public Animator Animator
	{
		get
		{
			return this.m_animator;
		}
		set
		{
			this.m_animator = value;
		}
	}

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x06001957 RID: 6487 RVA: 0x0000CBF8 File Offset: 0x0000ADF8
	// (set) Token: 0x06001958 RID: 6488 RVA: 0x0000CC00 File Offset: 0x0000AE00
	public List<AnimationControllerEntry> AnimationEntries
	{
		get
		{
			return this.m_entries;
		}
		set
		{
			this.m_entries = value;
		}
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0008F8E8 File Offset: 0x0008DAE8
	private void OnValidate()
	{
		if (this.m_entries != null)
		{
			for (int i = 0; i < this.m_entries.Count; i++)
			{
				this.m_entries[i].Id = i;
			}
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0008F928 File Offset: 0x0008DB28
	public void Trigger(int animationEntryID)
	{
		if (this.Animator != null)
		{
			AnimationControllerEntry entry = this.GetEntry(animationEntryID);
			if (entry != null)
			{
				AnimatorControllerParameterType parameterType = entry.ParameterType;
				switch (parameterType)
				{
				case AnimatorControllerParameterType.Float:
					this.Animator.SetFloat(entry.AnimationParameter, entry.FloatValue);
					return;
				case (AnimatorControllerParameterType)2:
					break;
				case AnimatorControllerParameterType.Int:
					this.Animator.SetInteger(entry.AnimationParameter, entry.IntValue);
					return;
				case AnimatorControllerParameterType.Bool:
					this.Animator.SetBool(entry.AnimationParameter, entry.BoolValue);
					return;
				default:
					if (parameterType != AnimatorControllerParameterType.Trigger)
					{
						return;
					}
					this.Animator.SetTrigger(entry.AnimationParameter);
					return;
				}
			}
		}
		else
		{
			Debug.LogFormat("<color=red>[{0}] Animator Field is null</color>", new object[]
			{
				this
			});
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x0008F9E8 File Offset: 0x0008DBE8
	private AnimationControllerEntry GetEntry(int animationEntryID)
	{
		if (this.AnimationEntries != null && this.AnimationEntries.Count > 0 && animationEntryID >= 0 && animationEntryID < this.AnimationEntries.Count)
		{
			return this.AnimationEntries[animationEntryID];
		}
		if (this.AnimationEntries == null || this.AnimationEntries.Count == 0)
		{
			Debug.LogFormat("<color=red>[{0}] Animation Entries are null or empty</color>", new object[]
			{
				this
			});
		}
		if (animationEntryID < 0 || animationEntryID >= this.AnimationEntries.Count)
		{
			Debug.LogFormat("<color=red>[{0}] animationEntryID ({1}) is out of bounds</color>", new object[]
			{
				this,
				animationEntryID
			});
		}
		return null;
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x0008FA84 File Offset: 0x0008DC84
	public bool GetParameterExistsInAnimator(string animatorParameter)
	{
		return this.Animator != null && this.Animator.parameters.Any((AnimatorControllerParameter parameter) => parameter.name == animatorParameter);
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x0008FACC File Offset: 0x0008DCCC
	public bool GetIsParameterTypeValid(AnimationControllerEntry entry)
	{
		if (entry != null && this.Animator != null && this.AnimationEntries != null && this.AnimationEntries.Count > 0 && this.GetParameterExistsInAnimator(entry.AnimationParameter))
		{
			int index = 0;
			for (int i = 0; i < this.Animator.parameters.Length; i++)
			{
				if (entry.AnimationParameter == this.Animator.parameters[i].name)
				{
					index = i;
					break;
				}
			}
			AnimatorControllerParameter parameter = this.Animator.GetParameter(index);
			return entry.ParameterType == parameter.type;
		}
		return false;
	}

	// Token: 0x04001807 RID: 6151
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04001808 RID: 6152
	[SerializeField]
	private List<AnimationControllerEntry> m_entries;
}
