using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class AnimationController : MonoBehaviour
{
	// Token: 0x17000984 RID: 2436
	// (get) Token: 0x0600110E RID: 4366 RVA: 0x0003132D File Offset: 0x0002F52D
	// (set) Token: 0x0600110F RID: 4367 RVA: 0x00031335 File Offset: 0x0002F535
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

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06001110 RID: 4368 RVA: 0x0003133E File Offset: 0x0002F53E
	// (set) Token: 0x06001111 RID: 4369 RVA: 0x00031346 File Offset: 0x0002F546
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

	// Token: 0x06001112 RID: 4370 RVA: 0x00031350 File Offset: 0x0002F550
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

	// Token: 0x06001113 RID: 4371 RVA: 0x00031390 File Offset: 0x0002F590
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

	// Token: 0x06001114 RID: 4372 RVA: 0x00031450 File Offset: 0x0002F650
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

	// Token: 0x06001115 RID: 4373 RVA: 0x000314EC File Offset: 0x0002F6EC
	public bool GetParameterExistsInAnimator(string animatorParameter)
	{
		return this.Animator != null && this.Animator.parameters.Any((AnimatorControllerParameter parameter) => parameter.name == animatorParameter);
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00031534 File Offset: 0x0002F734
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

	// Token: 0x040011FF RID: 4607
	[SerializeField]
	private Animator m_animator;

	// Token: 0x04001200 RID: 4608
	[SerializeField]
	private List<AnimationControllerEntry> m_entries;
}
