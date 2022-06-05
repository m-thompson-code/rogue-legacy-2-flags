using System;
using UnityEngine;

namespace RLTests
{
	// Token: 0x02000DA2 RID: 3490
	public class AnimatorMemoryAllocation_Test : MonoBehaviour
	{
		// Token: 0x060062A7 RID: 25255 RVA: 0x00036569 File Offset: 0x00034769
		private void Awake()
		{
			this.m_animator = base.GetComponent<Animator>();
		}

		// Token: 0x060062A8 RID: 25256 RVA: 0x00036577 File Offset: 0x00034777
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.GetValue();
			}
		}

		// Token: 0x060062A9 RID: 25257 RVA: 0x001708DC File Offset: 0x0016EADC
		private void GetValue()
		{
			string name = this.m_animator.name;
			string tag = this.m_animator.tag;
			if (this.m_animator.parameters.Length != 0)
			{
				string name2 = this.m_animator.parameters[0].name;
			}
			switch (this.m_parameterType)
			{
			case AnimatorParameterType.Float:
				this.m_animator.GetFloat(this.m_parameterName);
				return;
			case AnimatorParameterType.Bool:
				this.m_animator.GetBool(this.m_parameterName);
				return;
			case AnimatorParameterType.Int:
				this.m_animator.GetInteger(this.m_parameterName);
				return;
			default:
				return;
			}
		}

		// Token: 0x0400509B RID: 20635
		[SerializeField]
		private string m_parameterName;

		// Token: 0x0400509C RID: 20636
		[SerializeField]
		private AnimatorParameterType m_parameterType;

		// Token: 0x0400509D RID: 20637
		private Animator m_animator;
	}
}
