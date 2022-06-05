using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000324 RID: 804
[SharedBetweenAnimators]
public class EffectTriggerAnimBehaviour : StateMachineBehaviour
{
	// Token: 0x06001978 RID: 6520 RVA: 0x0000CD72 File Offset: 0x0000AF72
	public static void ClearNormalizedStartCounters()
	{
		EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Clear();
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x06001979 RID: 6521 RVA: 0x0000CD7E File Offset: 0x0000AF7E
	// (set) Token: 0x0600197A RID: 6522 RVA: 0x0000CD85 File Offset: 0x0000AF85
	public static bool DISABLE_GLOBALLY { get; set; } = false;

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x0600197B RID: 6523 RVA: 0x0000CD8D File Offset: 0x0000AF8D
	// (set) Token: 0x0600197C RID: 6524 RVA: 0x0000CD95 File Offset: 0x0000AF95
	public EffectTriggerEntry[] TriggerArray
	{
		get
		{
			return this.m_effectTriggerEntryList;
		}
		set
		{
			this.m_effectTriggerEntryList = value;
		}
	}

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x0600197D RID: 6525 RVA: 0x0000CD9E File Offset: 0x0000AF9E
	// (set) Token: 0x0600197E RID: 6526 RVA: 0x0000CDA6 File Offset: 0x0000AFA6
	public AnimBehaviourCondition[] ConditionArray
	{
		get
		{
			return this.m_animBehaviourConditionArray;
		}
		set
		{
			this.m_animBehaviourConditionArray = value;
		}
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x0008FF10 File Offset: 0x0008E110
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EffectTriggerAnimBehaviour.DISABLE_GLOBALLY)
		{
			return;
		}
		if (EffectManager.AnimatorEffectsDisabled(animator))
		{
			return;
		}
		if (this.m_animBehaviourConditionArray == null)
		{
			Debug.Log("AnimBehaviourConditionArray null on animator: " + animator.name + " StateInfoHash: " + stateInfo.shortNameHash.ToString());
			string text = "";
			foreach (EffectTriggerEntry effectTriggerEntry in this.m_effectTriggerEntryList)
			{
				text = text + effectTriggerEntry.EffectTypeName + ",  ";
			}
			Debug.Log("Effects: " + text);
			return;
		}
		for (int j = 0; j < this.m_animBehaviourConditionArray.Length; j++)
		{
			if (!this.m_animBehaviourConditionArray[j].IsTrue(animator))
			{
				return;
			}
		}
		for (int k = 0; k < this.m_effectTriggerEntryList.Length; k++)
		{
			EffectTriggerEntry effectTriggerEntry2 = this.m_effectTriggerEntryList[k];
			if (effectTriggerEntry2.EffectStartType == EffectStartType.FirstLoopOnly && effectTriggerEntry2.NormalizedStartTime <= 0f)
			{
				effectTriggerEntry2.EffectStartType = EffectStartType.None;
			}
			if (effectTriggerEntry2.EffectStartType == EffectStartType.None)
			{
				this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
			}
			else
			{
				int instanceID = animator.GetInstanceID();
				if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID))
				{
					EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID, 0);
				}
				else
				{
					EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID] = 0;
				}
			}
		}
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x00090064 File Offset: 0x0008E264
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (EffectTriggerAnimBehaviour.DISABLE_GLOBALLY)
		{
			return;
		}
		if (EffectManager.AnimatorEffectsDisabled(animator))
		{
			return;
		}
		if (this.m_animBehaviourConditionArray == null)
		{
			Debug.Log("AnimBehaviourConditionArray null on animator: " + animator.name + " StateInfoHash: " + stateInfo.shortNameHash.ToString());
			string text = "";
			foreach (EffectTriggerEntry effectTriggerEntry in this.m_effectTriggerEntryList)
			{
				text = text + effectTriggerEntry.EffectTypeName + ",  ";
			}
			Debug.Log("Effects: " + text);
			return;
		}
		for (int j = 0; j < this.m_animBehaviourConditionArray.Length; j++)
		{
			if (!this.m_animBehaviourConditionArray[j].IsTrue(animator))
			{
				return;
			}
		}
		for (int k = 0; k < this.m_effectTriggerEntryList.Length; k++)
		{
			EffectTriggerEntry effectTriggerEntry2 = this.m_effectTriggerEntryList[k];
			EffectStartType effectStartType = effectTriggerEntry2.EffectStartType;
			if (effectStartType != EffectStartType.None)
			{
				if (effectStartType != EffectStartType.FirstLoopOnly)
				{
					if (effectStartType == EffectStartType.OnceEveryLoop)
					{
						int instanceID = animator.GetInstanceID();
						if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID))
						{
							EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID, 0);
						}
						if (stateInfo.normalizedTime >= (float)EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID] + effectTriggerEntry2.NormalizedStartTime)
						{
							Dictionary<int, int> normalizedStartCounterDict = EffectTriggerAnimBehaviour.m_normalizedStartCounterDict;
							int num = instanceID;
							int i = normalizedStartCounterDict[num];
							normalizedStartCounterDict[num] = i + 1;
							this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
						}
					}
				}
				else
				{
					int instanceID2 = animator.GetInstanceID();
					if (!EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.ContainsKey(instanceID2))
					{
						EffectTriggerAnimBehaviour.m_normalizedStartCounterDict.Add(instanceID2, 0);
					}
					if (stateInfo.normalizedTime >= effectTriggerEntry2.NormalizedStartTime && EffectTriggerAnimBehaviour.m_normalizedStartCounterDict[instanceID2] <= 0)
					{
						Dictionary<int, int> normalizedStartCounterDict2 = EffectTriggerAnimBehaviour.m_normalizedStartCounterDict;
						int i = instanceID2;
						int num = normalizedStartCounterDict2[i];
						normalizedStartCounterDict2[i] = num + 1;
						this.InvokeTrigger(animator.gameObject, animator, effectTriggerEntry2, layerIndex);
					}
				}
			}
		}
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x00090250 File Offset: 0x0008E450
	private void InvokeTrigger(GameObject parentObj, Animator animator, EffectTriggerEntry entry, int layerIndex)
	{
		Vector3 vector = parentObj.transform.position;
		IMidpointObj component = parentObj.GetComponent<IMidpointObj>();
		if (component != null)
		{
			vector = component.Midpoint;
		}
		EffectTriggerDirection effectDirectionFromObject = EffectTrigger.GetEffectDirectionFromObject(parentObj, entry.TriggerDirection);
		EffectTrigger.InvokeTrigger(entry, parentObj, parentObj, vector, vector, effectDirectionFromObject, animator);
	}

	// Token: 0x04001829 RID: 6185
	private static Dictionary<int, int> m_normalizedStartCounterDict = new Dictionary<int, int>();

	// Token: 0x0400182A RID: 6186
	[SerializeField]
	private EffectTriggerEntry[] m_effectTriggerEntryList = new EffectTriggerEntry[0];

	// Token: 0x0400182B RID: 6187
	[SerializeField]
	private AnimBehaviourCondition[] m_animBehaviourConditionArray = new AnimBehaviourCondition[0];
}
