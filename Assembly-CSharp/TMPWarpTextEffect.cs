using System;
using TMPro;
using UnityEngine;

// Token: 0x02000CEB RID: 3307
[ExecuteAlways]
public class TMPWarpTextEffect : MonoBehaviour
{
	// Token: 0x06005E2A RID: 24106 RVA: 0x00033DA2 File Offset: 0x00031FA2
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.OnTextChanged));
	}

	// Token: 0x06005E2B RID: 24107 RVA: 0x00033DBA File Offset: 0x00031FBA
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.OnTextChanged));
	}

	// Token: 0x06005E2C RID: 24108 RVA: 0x00033DD2 File Offset: 0x00031FD2
	private void OnTextChanged(UnityEngine.Object obj)
	{
		if (obj == this.m_text)
		{
			this.m_hasTextChanged = true;
		}
	}

	// Token: 0x06005E2D RID: 24109 RVA: 0x00033DE9 File Offset: 0x00031FE9
	private void Awake()
	{
		this.m_text = base.gameObject.GetComponent<TMP_Text>();
		this.VertexCurve.preWrapMode = WrapMode.Once;
		this.VertexCurve.postWrapMode = WrapMode.Once;
	}

	// Token: 0x06005E2E RID: 24110 RVA: 0x00033E14 File Offset: 0x00032014
	private void Start()
	{
		this.WarpText();
	}

	// Token: 0x06005E2F RID: 24111 RVA: 0x00033E1C File Offset: 0x0003201C
	private void Update()
	{
		if (!Application.isPlaying || this.m_hasTextChanged)
		{
			this.WarpText();
		}
	}

	// Token: 0x06005E30 RID: 24112 RVA: 0x00033E33 File Offset: 0x00032033
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		return new AnimationCurve
		{
			keys = curve.keys
		};
	}

	// Token: 0x06005E31 RID: 24113 RVA: 0x0015FEC8 File Offset: 0x0015E0C8
	private void WarpText()
	{
		float curveScale = this.CurveScale;
		AnimationCurve vertexCurve = this.CopyAnimationCurve(this.VertexCurve);
		this.m_text.havePropertiesChanged = true;
		this.CurveScale *= 10f;
		this.m_text.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.m_text.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		float x = this.m_text.bounds.min.x;
		float x2 = this.m_text.bounds.max.x;
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
				vertices[vertexIndex] += -vector;
				vertices[vertexIndex + 1] += -vector;
				vertices[vertexIndex + 2] += -vector;
				vertices[vertexIndex + 3] += -vector;
				float num = (vector.x - x) / (x2 - x);
				float num2 = num + 0.0001f;
				float y = this.VertexCurve.Evaluate(num) * this.CurveScale;
				float y2 = this.VertexCurve.Evaluate(num2) * this.CurveScale;
				Vector3 lhs = new Vector3(1f, 0f, 0f);
				Vector3 rhs = new Vector3(num2 * (x2 - x) + x, y2) - new Vector3(vector.x, y);
				float num3 = Mathf.Acos(Vector3.Dot(lhs, rhs.normalized)) * 57.29578f;
				float z = (Vector3.Cross(lhs, rhs).z > 0f) ? num3 : (360f - num3);
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, y, 0f), Quaternion.Euler(0f, 0f, z), Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
				vertices[vertexIndex] += vector;
				vertices[vertexIndex + 1] += vector;
				vertices[vertexIndex + 2] += vector;
				vertices[vertexIndex + 3] += vector;
			}
		}
		this.m_text.UpdateVertexData();
		this.VertexCurve = vertexCurve;
		this.CurveScale = curveScale;
		this.m_hasTextChanged = false;
	}

	// Token: 0x04004D55 RID: 19797
	private TMP_Text m_text;

	// Token: 0x04004D56 RID: 19798
	public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 2f),
		new Keyframe(0.5f, 0f),
		new Keyframe(0.75f, 2f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04004D57 RID: 19799
	public float AngleMultiplier = 1f;

	// Token: 0x04004D58 RID: 19800
	public float SpeedMultiplier = 1f;

	// Token: 0x04004D59 RID: 19801
	public float CurveScale = 1f;

	// Token: 0x04004D5A RID: 19802
	private bool m_hasTextChanged;
}
