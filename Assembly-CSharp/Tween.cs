using System;
using System.Collections;
using System.Reflection;
using Sigtrap.Relays;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200081C RID: 2076
public class Tween : MonoBehaviour, IGenericPoolObj
{
	// Token: 0x170016FF RID: 5887
	// (get) Token: 0x0600449F RID: 17567 RVA: 0x000F3B8E File Offset: 0x000F1D8E
	// (set) Token: 0x060044A0 RID: 17568 RVA: 0x000F3B96 File Offset: 0x000F1D96
	public bool IsFreePoolObj { get; set; }

	// Token: 0x17001700 RID: 5888
	// (get) Token: 0x060044A1 RID: 17569 RVA: 0x000F3B9F File Offset: 0x000F1D9F
	// (set) Token: 0x060044A2 RID: 17570 RVA: 0x000F3BA7 File Offset: 0x000F1DA7
	public bool IsAwakeCalled { get; protected set; } = true;

	// Token: 0x17001701 RID: 5889
	// (get) Token: 0x060044A3 RID: 17571 RVA: 0x000F3BB0 File Offset: 0x000F1DB0
	// (set) Token: 0x060044A4 RID: 17572 RVA: 0x000F3BB8 File Offset: 0x000F1DB8
	public string TweenedObjectName { get; private set; }

	// Token: 0x17001702 RID: 5890
	// (get) Token: 0x060044A5 RID: 17573 RVA: 0x000F3BC1 File Offset: 0x000F1DC1
	public bool UseUnscaledTime
	{
		get
		{
			return this.m_useUnscaledTime;
		}
	}

	// Token: 0x17001703 RID: 5891
	// (get) Token: 0x060044A6 RID: 17574 RVA: 0x000F3BC9 File Offset: 0x000F1DC9
	// (set) Token: 0x060044A7 RID: 17575 RVA: 0x000F3BD1 File Offset: 0x000F1DD1
	public float CoroutineStartTime { get; private set; }

	// Token: 0x17001704 RID: 5892
	// (get) Token: 0x060044A8 RID: 17576 RVA: 0x000F3BDA File Offset: 0x000F1DDA
	public float TweenStartTime
	{
		get
		{
			return this.m_tweenStartTime;
		}
	}

	// Token: 0x17001705 RID: 5893
	// (get) Token: 0x060044A9 RID: 17577 RVA: 0x000F3BE2 File Offset: 0x000F1DE2
	public float TweenEndTime
	{
		get
		{
			return this.m_tweenEndTime;
		}
	}

	// Token: 0x17001706 RID: 5894
	// (get) Token: 0x060044AA RID: 17578 RVA: 0x000F3BEA File Offset: 0x000F1DEA
	public float CoroutineElapsedTime
	{
		get
		{
			if (this.UseUnscaledTime)
			{
				return Time.unscaledTime;
			}
			return Time.time;
		}
	}

	// Token: 0x17001707 RID: 5895
	// (get) Token: 0x060044AB RID: 17579 RVA: 0x000F3BFF File Offset: 0x000F1DFF
	public object[] PropertyArray
	{
		get
		{
			return this.m_propertiesList;
		}
	}

	// Token: 0x17001708 RID: 5896
	// (get) Token: 0x060044AC RID: 17580 RVA: 0x000F3C07 File Offset: 0x000F1E07
	public IRelayLink OnTweenCompleteRelay
	{
		get
		{
			return this.m_onTweenCompleteRelay.link;
		}
	}

	// Token: 0x17001709 RID: 5897
	// (get) Token: 0x060044AD RID: 17581 RVA: 0x000F3C14 File Offset: 0x000F1E14
	// (set) Token: 0x060044AE RID: 17582 RVA: 0x000F3C1C File Offset: 0x000F1E1C
	public Coroutine TweenCoroutine { get; private set; }

	// Token: 0x1700170A RID: 5898
	// (get) Token: 0x060044AF RID: 17583 RVA: 0x000F3C25 File Offset: 0x000F1E25
	public bool Paused
	{
		get
		{
			return this.m_paused;
		}
	}

	// Token: 0x060044B0 RID: 17584 RVA: 0x000F3C2D File Offset: 0x000F1E2D
	public void SetPaused(bool pause)
	{
		this.m_paused = pause;
	}

	// Token: 0x1700170B RID: 5899
	// (get) Token: 0x060044B1 RID: 17585 RVA: 0x000F3C36 File Offset: 0x000F1E36
	public object TweenedObj
	{
		get
		{
			return this.m_tweenedObject;
		}
	}

	// Token: 0x1700170C RID: 5900
	// (get) Token: 0x060044B2 RID: 17586 RVA: 0x000F3C3E File Offset: 0x000F1E3E
	// (set) Token: 0x060044B3 RID: 17587 RVA: 0x000F3C46 File Offset: 0x000F1E46
	public bool IsType { get; private set; }

	// Token: 0x1700170D RID: 5901
	// (get) Token: 0x060044B4 RID: 17588 RVA: 0x000F3C4F File Offset: 0x000F1E4F
	// (set) Token: 0x060044B5 RID: 17589 RVA: 0x000F3C57 File Offset: 0x000F1E57
	public string ID { get; set; }

	// Token: 0x060044B6 RID: 17590 RVA: 0x000F3C60 File Offset: 0x000F1E60
	public void SetValues(object tweenObject, float duration, bool useUnscaledTime, EaseDelegate ease, bool tweenTo, object[] properties)
	{
		if (tweenObject is Type)
		{
			this.IsType = true;
		}
		else
		{
			this.IsType = false;
		}
		this.m_tweenedObject = tweenObject;
		UnityEngine.Object @object = tweenObject as UnityEngine.Object;
		if (@object != null)
		{
			this.TweenedObjectName = @object.name;
		}
		if (CameraController.IsInstantiated && @object == CameraController.ForegroundPostProcessing)
		{
			this.m_updatesForegroundPostProcessing = true;
		}
		this.m_ease = ease;
		this.m_tweenTo = tweenTo;
		this.m_propertiesList = properties;
		this.m_useUnscaledTime = useUnscaledTime;
		if (this.m_useUnscaledTime)
		{
			this.m_tweenStartTime = Time.unscaledTime;
		}
		else
		{
			this.m_tweenStartTime = Time.time;
		}
		this.m_tweenEndTime = this.m_tweenStartTime + duration;
		this.m_initialValues = new float[this.m_propertiesList.Length / 2];
		this.m_tweenStarted = false;
		this.ID = null;
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.m_propertiesList.Length; i += 2)
		{
			if (this.m_propertiesList[i + 1] is int)
			{
				this.m_propertiesList[i + 1] = (float)((int)this.m_propertiesList[i + 1]);
			}
			if ((string)this.m_propertiesList[i] == "delay")
			{
				num = (float)this.m_propertiesList[i + 1];
			}
			if ((string)this.m_propertiesList[i] == "subtract")
			{
				num2 = (float)this.m_propertiesList[i + 1];
			}
		}
		this.m_tweenStartTime += num;
		this.m_tweenEndTime += num;
		this.m_tweenStartTime -= num2;
		this.m_tweenEndTime -= num2;
	}

	// Token: 0x060044B7 RID: 17591 RVA: 0x000F3E14 File Offset: 0x000F2014
	public void AddCustomEndHandler(object methodObject, string functionName, params object[] args)
	{
		if (methodObject == null)
		{
			throw new Exception("methodObject cannot be null");
		}
		Type[] array = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			Type type = args[i] as Type;
			if (type != null)
			{
				array[i] = type;
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		this.m_methodInfo = methodObject.GetType().GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, array, null);
		if (this.m_methodInfo == null)
		{
			throw new Exception("Function " + functionName + " not found in class " + methodObject.GetType().ToString());
		}
		this.m_methodObject = methodObject;
		this.m_methodArgs = args;
	}

	// Token: 0x060044B8 RID: 17592 RVA: 0x000F3EC0 File Offset: 0x000F20C0
	public void AddCustomEndHandler_Type(Type objectType, string functionName, params object[] args)
	{
		Type[] array = new Type[args.Length];
		for (int i = 0; i < args.Length; i++)
		{
			Type type = args[i] as Type;
			if (type != null)
			{
				array[i] = type;
			}
			else
			{
				array[i] = args[i].GetType();
			}
		}
		this.m_methodInfo = objectType.GetMethod(functionName, array);
		if (this.m_methodInfo == null)
		{
			throw new Exception("Function " + functionName + " not found in class " + objectType.ToString());
		}
		this.m_methodObject = null;
		this.m_methodArgs = args;
	}

	// Token: 0x060044B9 RID: 17593 RVA: 0x000F3F4D File Offset: 0x000F214D
	public void StartTween()
	{
		this.CoroutineStartTime = Time.time;
		if (this.UseUnscaledTime)
		{
			this.CoroutineStartTime = Time.unscaledTime;
		}
		this.TweenCoroutine = base.StartCoroutine(this.UpdateCoroutine());
	}

	// Token: 0x060044BA RID: 17594 RVA: 0x000F3F7F File Offset: 0x000F217F
	public void StopTween(bool runEndHandlers)
	{
		this.m_endTween = true;
		this.m_endTweenImmediately = true;
		this.m_endTweenWithoutEndHandlers = !runEndHandlers;
	}

	// Token: 0x060044BB RID: 17595 RVA: 0x000F3F99 File Offset: 0x000F2199
	public void StopTweenWithConditionChecks(bool runEndHandlers, object tweenedObj, string id = null)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (tweenedObj != this.TweenedObj)
		{
			return;
		}
		if (!string.IsNullOrEmpty(id) && id != this.ID)
		{
			return;
		}
		this.StopTween(runEndHandlers);
	}

	// Token: 0x060044BC RID: 17596 RVA: 0x000F3FCC File Offset: 0x000F21CC
	public void EndTween(bool runEndHandlers)
	{
		this.m_endTween = true;
		this.m_endTweenImmediately = false;
		this.m_endTweenWithoutEndHandlers = !runEndHandlers;
	}

	// Token: 0x060044BD RID: 17597 RVA: 0x000F3FE8 File Offset: 0x000F21E8
	private void InvokeEndHandlers()
	{
		this.m_onTweenCompleteRelay.Dispatch();
		if (this.OnTweenComplete_UnityEvent != null)
		{
			this.OnTweenComplete_UnityEvent.Invoke();
		}
		if (this.m_methodInfo != null)
		{
			this.m_methodInfo.Invoke(this.m_methodObject, this.m_methodArgs);
		}
	}

	// Token: 0x060044BE RID: 17598 RVA: 0x000F403C File Offset: 0x000F223C
	private void InitializeTween()
	{
		int num = 0;
		for (int i = 0; i < this.m_propertiesList.Length; i += 2)
		{
			string a = (string)this.m_propertiesList[i];
			if (a != "delay" && a != "subtract")
			{
				string text = this.m_propertiesList[i] as string;
				string name = null;
				int num2 = text.IndexOf('.');
				Type type = this.m_tweenedObject.GetType();
				this.m_structObjs[num] = null;
				this.m_structPropertyInfo[num] = null;
				this.m_structFieldInfo[num] = null;
				this.m_structName[num] = null;
				bool flag = num2 > 0;
				if (flag)
				{
					name = text.Substring(num2 + 1);
					text = text.Substring(0, num2);
				}
				if (this.IsType)
				{
					this.m_propertyInfo[num] = (this.m_tweenedObject as Type).GetProperty(text);
					if (this.m_propertyInfo[num] == null)
					{
						this.m_fieldInfo[num] = (this.m_tweenedObject as Type).GetField(text);
					}
				}
				else
				{
					this.m_propertyInfo[num] = type.GetProperty(text);
					if (this.m_propertyInfo[num] == null)
					{
						this.m_fieldInfo[num] = type.GetField(text);
					}
				}
				if (flag)
				{
					this.m_structName[num] = text;
					if (this.m_propertyInfo[num] != null)
					{
						this.m_structObjs[num] = this.m_propertyInfo[num].GetValue(this.m_tweenedObject, null);
					}
					else
					{
						this.m_structObjs[num] = type.GetField(this.m_structName[num]).GetValue(this.m_tweenedObject);
					}
					Type type2 = this.m_structObjs[num].GetType();
					this.m_structPropertyInfo[num] = type2.GetProperty(name);
					if (this.m_structPropertyInfo[num] == null)
					{
						this.m_structFieldInfo[num] = type2.GetField(name);
					}
				}
				if (this.m_propertyInfo[num] == null && this.m_fieldInfo[num] == null)
				{
					throw new Exception("Property/Field " + text + " not found on object " + this.m_tweenedObject.GetType().ToString());
				}
				if (this.m_propertyInfo[num] != null)
				{
					if (this.m_structObjs[num] == null)
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_propertyInfo[num].GetValue(this.m_tweenedObject, null));
					}
					else if (this.m_structPropertyInfo[num] != null)
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_structPropertyInfo[num].GetValue(this.m_structObjs[num], null));
					}
					else
					{
						this.m_initialValues[num] = Convert.ToSingle(this.m_structFieldInfo[num].GetValue(this.m_structObjs[num]));
					}
				}
				else if (this.m_structObjs[num] == null)
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_fieldInfo[num].GetValue(this.m_tweenedObject));
				}
				else if (this.m_structPropertyInfo[num] != null)
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_structPropertyInfo[num].GetValue(this.m_structObjs[num], null));
				}
				else
				{
					this.m_initialValues[num] = Convert.ToSingle(this.m_structFieldInfo[num].GetValue(this.m_structObjs[num]));
				}
				num++;
			}
		}
	}

	// Token: 0x060044BF RID: 17599 RVA: 0x000F4386 File Offset: 0x000F2586
	public IEnumerator UpdateCoroutine()
	{
		float coroutineElapsedTime = Time.time;
		if (this.m_useUnscaledTime)
		{
			coroutineElapsedTime = Time.unscaledTime;
		}
		while (coroutineElapsedTime < this.m_tweenEndTime && !this.m_endTween)
		{
			while (this.m_paused)
			{
				if (this.m_endTween)
				{
					break;
				}
				yield return null;
				if (!this.m_useUnscaledTime)
				{
					this.m_tweenEndTime += Time.deltaTime;
					coroutineElapsedTime = Time.time;
				}
				else
				{
					this.m_tweenEndTime += Time.unscaledDeltaTime;
					coroutineElapsedTime = Time.unscaledTime;
				}
			}
			while (coroutineElapsedTime < this.m_tweenStartTime && !this.m_endTween)
			{
				yield return null;
				coroutineElapsedTime = Time.time;
				if (this.m_useUnscaledTime)
				{
					coroutineElapsedTime = Time.unscaledTime;
				}
			}
			if (!this.m_tweenStarted)
			{
				this.m_tweenStarted = true;
				this.InitializeTween();
			}
			if (this.m_tweenedObject == null || this.m_tweenedObject.ToString().Equals("null"))
			{
				base.gameObject.SetActive(false);
				yield break;
			}
			int num = 0;
			for (int i = 0; i < this.m_propertiesList.Length - 1; i += 2)
			{
				string a = (string)this.m_propertiesList[i];
				if (a != "delay" && a != "subtract")
				{
					float num2;
					if (this.m_tweenTo)
					{
						num2 = this.m_ease(coroutineElapsedTime - this.m_tweenStartTime, this.m_initialValues[num], (float)this.m_propertiesList[i + 1] - this.m_initialValues[num], this.m_tweenEndTime - this.m_tweenStartTime);
					}
					else
					{
						num2 = this.m_ease(coroutineElapsedTime - this.m_tweenStartTime, this.m_initialValues[num], (float)this.m_propertiesList[i + 1], this.m_tweenEndTime - this.m_tweenStartTime);
					}
					bool flag = this.m_propertyInfo[num] == null;
					bool flag2 = this.m_structObjs[num] != null;
					bool flag3 = this.m_structPropertyInfo[num] == null;
					if (!flag2)
					{
						if (!flag)
						{
							if (this.m_propertyInfo[num].PropertyType == typeof(int))
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, (int)num2, null);
							}
							else
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, num2, null);
							}
						}
						else if (this.m_fieldInfo[num].FieldType == typeof(int))
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, (int)num2);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, num2);
						}
					}
					else
					{
						this.UpdateStructValues(num);
						if (!flag3)
						{
							if (this.m_structPropertyInfo[num].PropertyType == typeof(int))
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], (int)num2, null);
							}
							else
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], num2, null);
							}
						}
						else if (this.m_structFieldInfo[num].FieldType == typeof(int))
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], (int)num2);
						}
						else
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], num2);
						}
						if (!flag)
						{
							this.m_propertyInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num], null);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num]);
						}
					}
					num++;
				}
			}
			coroutineElapsedTime = Time.time;
			if (this.m_useUnscaledTime)
			{
				coroutineElapsedTime = Time.unscaledTime;
			}
			if (this.m_updatesForegroundPostProcessing)
			{
				CameraController.ForegroundPostProcessing.ApplyShader();
			}
			yield return null;
		}
		if (!this.m_tweenStarted)
		{
			this.m_tweenStarted = true;
			this.InitializeTween();
		}
		this.Complete();
		yield break;
	}

	// Token: 0x060044C0 RID: 17600 RVA: 0x000F4398 File Offset: 0x000F2598
	private void UpdateStructValues(int index)
	{
		if (this.m_propertyInfo[index] != null)
		{
			this.m_structObjs[index] = this.m_propertyInfo[index].GetValue(this.m_tweenedObject, null);
			return;
		}
		this.m_structObjs[index] = this.m_fieldInfo[index].GetValue(this.m_tweenedObject);
	}

	// Token: 0x060044C1 RID: 17601 RVA: 0x000F43F0 File Offset: 0x000F25F0
	public void Complete()
	{
		if (!this.m_endTweenImmediately && this.m_tweenedObject != null && !this.m_tweenedObject.Equals(null))
		{
			int num = 0;
			for (int i = 0; i < this.m_propertiesList.Length - 1; i += 2)
			{
				string a = (string)this.m_propertiesList[i];
				if (a != "delay" && a != "subtract")
				{
					float num2;
					if (this.m_tweenTo)
					{
						num2 = (float)this.m_propertiesList[i + 1];
					}
					else
					{
						num2 = this.m_initialValues[num] + (float)this.m_propertiesList[i + 1];
					}
					this.m_tweenStartTime = this.m_tweenEndTime;
					bool flag = this.m_propertyInfo[num] == null;
					bool flag2 = this.m_structObjs[num] != null;
					bool flag3 = this.m_structPropertyInfo[num] == null;
					if (!flag2)
					{
						if (!flag)
						{
							if (this.m_propertyInfo[num].PropertyType == typeof(int))
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, (int)num2, null);
							}
							else
							{
								this.m_propertyInfo[num].SetValue(this.m_tweenedObject, num2, null);
							}
						}
						else if (this.m_fieldInfo[num].FieldType == typeof(int))
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, (int)num2);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, num2);
						}
					}
					else
					{
						this.UpdateStructValues(num);
						if (!flag3)
						{
							if (this.m_structPropertyInfo[num].PropertyType == typeof(int))
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], (int)num2, null);
							}
							else
							{
								this.m_structPropertyInfo[num].SetValue(this.m_structObjs[num], num2, null);
							}
						}
						else if (this.m_structFieldInfo[num].FieldType == typeof(int))
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], (int)num2);
						}
						else
						{
							this.m_structFieldInfo[num].SetValue(this.m_structObjs[num], num2);
						}
						if (!flag)
						{
							this.m_propertyInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num], null);
						}
						else
						{
							this.m_fieldInfo[num].SetValue(this.m_tweenedObject, this.m_structObjs[num]);
						}
					}
					num++;
				}
			}
		}
		if (!this.m_endTweenWithoutEndHandlers)
		{
			this.InvokeEndHandlers();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060044C2 RID: 17602 RVA: 0x000F46B2 File Offset: 0x000F28B2
	private void OnDisable()
	{
		DisablePooledObjectManager.DisablePooledObject(this, false);
	}

	// Token: 0x060044C3 RID: 17603 RVA: 0x000F46BC File Offset: 0x000F28BC
	public void ResetValues()
	{
		this.m_tweenedObject = null;
		Array.Clear(this.m_propertyInfo, 0, this.m_propertyInfo.Length);
		Array.Clear(this.m_fieldInfo, 0, this.m_fieldInfo.Length);
		if (this.m_propertiesList != null)
		{
			Array.Clear(this.m_propertiesList, 0, this.m_propertiesList.Length);
		}
		this.m_propertiesList = null;
		if (this.m_initialValues != null)
		{
			Array.Clear(this.m_initialValues, 0, this.m_initialValues.Length);
		}
		this.m_initialValues = null;
		this.m_tweenStartTime = 0f;
		this.m_tweenEndTime = 0f;
		this.m_tweenTo = false;
		this.m_ease = new EaseDelegate(Ease.None);
		this.m_paused = false;
		this.ID = null;
		this.m_tweenStarted = false;
		this.TweenCoroutine = null;
		this.CoroutineStartTime = 0f;
		this.m_endTween = false;
		this.m_endTweenImmediately = false;
		this.m_endTweenWithoutEndHandlers = false;
		this.m_onTweenCompleteRelay.RemoveAll(true, true);
		if (this.OnTweenComplete_UnityEvent != null)
		{
			this.OnTweenComplete_UnityEvent.RemoveAllListeners();
		}
		this.m_methodObject = null;
		this.m_methodInfo = null;
		if (this.m_methodArgs != null)
		{
			Array.Clear(this.m_methodArgs, 0, this.m_methodArgs.Length);
		}
	}

	// Token: 0x060044C5 RID: 17605 RVA: 0x000F4867 File Offset: 0x000F2A67
	GameObject IGenericPoolObj.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04003A8E RID: 14990
	private const int MAX_PROPERTY_COUNT = 10;

	// Token: 0x04003A8F RID: 14991
	private const string DELAY_STRING = "delay";

	// Token: 0x04003A90 RID: 14992
	private const string SUBTRACT_STRING = "subtract";

	// Token: 0x04003A91 RID: 14993
	private PropertyInfo[] m_propertyInfo = new PropertyInfo[10];

	// Token: 0x04003A92 RID: 14994
	private FieldInfo[] m_fieldInfo = new FieldInfo[10];

	// Token: 0x04003A93 RID: 14995
	private object[] m_structObjs = new object[10];

	// Token: 0x04003A94 RID: 14996
	private PropertyInfo[] m_structPropertyInfo = new PropertyInfo[10];

	// Token: 0x04003A95 RID: 14997
	private FieldInfo[] m_structFieldInfo = new FieldInfo[10];

	// Token: 0x04003A96 RID: 14998
	private string[] m_structName = new string[10];

	// Token: 0x04003A97 RID: 14999
	private object m_tweenedObject;

	// Token: 0x04003A98 RID: 15000
	private float m_tweenStartTime;

	// Token: 0x04003A99 RID: 15001
	private float m_tweenEndTime;

	// Token: 0x04003A9A RID: 15002
	private float[] m_initialValues;

	// Token: 0x04003A9B RID: 15003
	private bool m_tweenTo;

	// Token: 0x04003A9C RID: 15004
	private EaseDelegate m_ease;

	// Token: 0x04003A9D RID: 15005
	private bool m_useUnscaledTime;

	// Token: 0x04003A9E RID: 15006
	private bool m_paused;

	// Token: 0x04003A9F RID: 15007
	private bool m_updatesForegroundPostProcessing;

	// Token: 0x04003AA0 RID: 15008
	private object[] m_propertiesList;

	// Token: 0x04003AA1 RID: 15009
	private bool m_tweenStarted;

	// Token: 0x04003AA2 RID: 15010
	private bool m_endTween;

	// Token: 0x04003AA3 RID: 15011
	private bool m_endTweenImmediately;

	// Token: 0x04003AA4 RID: 15012
	private bool m_endTweenWithoutEndHandlers;

	// Token: 0x04003AA5 RID: 15013
	private object m_methodObject;

	// Token: 0x04003AA6 RID: 15014
	private object[] m_methodArgs;

	// Token: 0x04003AA7 RID: 15015
	private MethodInfo m_methodInfo;

	// Token: 0x04003AA8 RID: 15016
	private const string m_nullString = "null";

	// Token: 0x04003AAD RID: 15021
	private Relay m_onTweenCompleteRelay = new Relay();

	// Token: 0x04003AAE RID: 15022
	public UnityEvent OnTweenComplete_UnityEvent;
}
