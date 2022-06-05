using System;
using UnityEngine;

namespace RLTests
{
	// Token: 0x02000882 RID: 2178
	public class AnimatorMemoryAllocation_Test : MonoBehaviour
	{
		// Token: 0x060047A9 RID: 18345 RVA: 0x00101EFD File Offset: 0x001000FD
		private void Awake()
		{
			this.m_animator = base.GetComponent<Animator>();
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x00101F0B File Offset: 0x0010010B
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				this.GetValue();
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x00101F20 File Offset: 0x00100120
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

		// Token: 0x04003C99 RID: 15513
		[SerializeField]
		private string m_parameterName;

		// Token: 0x04003C9A RID: 15514
		[SerializeField]
		private AnimatorParameterType m_parameterType;

		// Token: 0x04003C9B RID: 15515
		private Animator m_animator;
	}
}
