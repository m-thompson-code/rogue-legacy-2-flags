using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class CDGHelper
{
	// Token: 0x060041B5 RID: 16821 RVA: 0x000E9B88 File Offset: 0x000E7D88
	public static T FindStaticInstance<T>(bool createNewIfNull) where T : MonoBehaviour
	{
		T[] array = UnityEngine.Object.FindObjectsOfType<T>();
		if (array.Length == 0 && createNewIfNull)
		{
			return new GameObject(typeof(T).ToString()).AddComponent<T>();
		}
		if (array.Length == 1)
		{
			return array[0].GetComponent<T>();
		}
		if (array.Length > 1)
		{
			throw new Exception(string.Format("{0}: Found multiple instances of given Type", Time.frameCount));
		}
		return default(T);
	}

	// Token: 0x060041B6 RID: 16822 RVA: 0x000E9C00 File Offset: 0x000E7E00
	public static T FindStaticInstance<T>(string instanceName, bool createNewIfNull) where T : Component
	{
		GameObject gameObject = GameObject.Find(instanceName);
		T t = default(T);
		if (gameObject != null)
		{
			t = gameObject.GetComponent<T>();
			if (t == null)
			{
				throw new MissingComponentException(instanceName);
			}
		}
		if (gameObject == null && createNewIfNull)
		{
			t = new GameObject
			{
				name = instanceName
			}.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x000E9C64 File Offset: 0x000E7E64
	public static T FindStaticInstance<T>(string instanceName, string prefabPath, bool createNewIfNull) where T : Component
	{
		GameObject gameObject = GameObject.Find(instanceName);
		T t = default(T);
		if (gameObject != null)
		{
			t = gameObject.GetComponent<T>();
			if (t == null)
			{
				throw new MissingComponentException(instanceName);
			}
		}
		if (gameObject == null && createNewIfNull)
		{
			t = UnityEngine.Object.Instantiate<T>(CDGResources.Load<T>(prefabPath, "", true));
			t.gameObject.name = instanceName;
			if (t == null)
			{
				throw new Exception("Cannot create new static instance.");
			}
		}
		return t;
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x000E9CEE File Offset: 0x000E7EEE
	public static int RandomPlusMinus()
	{
		if (UnityEngine.Random.Range(0, 2) < 1)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x000E9D00 File Offset: 0x000E7F00
	public static Vector2 AngleToVector(float degAngle)
	{
		float num = degAngle;
		degAngle *= 0.017453292f;
		Vector2 result = new Vector2(Mathf.Cos(degAngle), Mathf.Sin(degAngle));
		if (num == 90f || num == -90f || num == 270f || num == -270f)
		{
			result.x = 0f;
		}
		if (num == 0f || num == 360f || num == 180f || num == -180f)
		{
			result.y = 0f;
		}
		return result;
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x000E9D86 File Offset: 0x000E7F86
	public static float VectorToAngle(Vector2 pt)
	{
		return CDGHelper.AngleBetweenPts(Vector2.zero, pt);
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x000E9D94 File Offset: 0x000E7F94
	public static float AngleBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		float x = pt2.x - pt1.x;
		return CDGHelper.WrapAngleDegrees(Mathf.Atan2(pt2.y - pt1.y, x) * 57.29578f, false);
	}

	// Token: 0x060041BC RID: 16828 RVA: 0x000E9DCE File Offset: 0x000E7FCE
	public static Vector2 VectorBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		return new Vector2(pt2.x - pt1.x, pt2.y - pt1.y);
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x000E9DF0 File Offset: 0x000E7FF0
	public static float DistanceBetweenPts(Vector2 pt1, Vector2 pt2)
	{
		float num = pt2.x - pt1.x;
		float num2 = pt2.y - pt1.y;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x060041BE RID: 16830 RVA: 0x000E9E24 File Offset: 0x000E8024
	public static float ConvertToGameUnits(float pixelFloat)
	{
		return pixelFloat / 60f;
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x000E9E2D File Offset: 0x000E802D
	public static Vector2 ConvertToGameUnits(Vector2 pixelVector)
	{
		return new Vector2(pixelVector.x / 60f, pixelVector.y / 60f);
	}

	// Token: 0x060041C0 RID: 16832 RVA: 0x000E9E4C File Offset: 0x000E804C
	public static float ConvertToPixelUnits(float gameFloat)
	{
		return gameFloat * 60f;
	}

	// Token: 0x060041C1 RID: 16833 RVA: 0x000E9E55 File Offset: 0x000E8055
	public static Vector2 ConvertToPixelUnits(Vector2 gameVector)
	{
		return new Vector2(gameVector.x * 60f, gameVector.y * 60f);
	}

	// Token: 0x060041C2 RID: 16834 RVA: 0x000E9E74 File Offset: 0x000E8074
	public static Vector3 ToVector3(Vector2 vector)
	{
		return new Vector3(vector.x, vector.y, 0f);
	}

	// Token: 0x060041C3 RID: 16835 RVA: 0x000E9E8C File Offset: 0x000E808C
	public static Vector3 ToVector3(float value)
	{
		return new Vector3(value, value, value);
	}

	// Token: 0x060041C4 RID: 16836 RVA: 0x000E9E98 File Offset: 0x000E8098
	public static Mesh BuildMeshFromPolygonCollider2D(PolygonCollider2D poly2D)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		int pathCount = poly2D.pathCount;
		for (int i = 0; i < pathCount; i++)
		{
			Vector2[] path = poly2D.GetPath(i);
			for (int j = 0; j < path.Length; j++)
			{
				list.Add(new Vector3(path[j].x, path[j].y, 0f));
			}
			int[] array = new Triangulator(path).Triangulate();
			for (int k = 0; k < array.Length; k++)
			{
				array[k] += i * pathCount;
			}
			list2.AddRange(array);
		}
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x000E9F7C File Offset: 0x000E817C
	public static Mesh BuildMeshFromPointList(List<Vector2> pointsList)
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		Vector2[] array = pointsList.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			list.Add(new Vector3(array[i].x, array[i].y, 0f));
		}
		int[] collection = new Triangulator(array).Triangulate();
		list2.AddRange(collection);
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return mesh;
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x000EA018 File Offset: 0x000E8218
	public static Vector2 RotatedPoint(Vector2 thePoint, Vector2 theOrigin, float theRotation)
	{
		theRotation *= 0.017453292f;
		double num = (double)Mathf.Cos(theRotation);
		double num2 = (double)Mathf.Sin(theRotation);
		return new Vector2
		{
			x = (float)((double)theOrigin.x + (double)(thePoint.x - theOrigin.x) * num - (double)(thePoint.y - theOrigin.y) * num2),
			y = (float)((double)theOrigin.y + (double)(thePoint.y - theOrigin.y) * num + (double)(thePoint.x - theOrigin.x) * num2)
		};
	}

	// Token: 0x060041C7 RID: 16839 RVA: 0x000EA0A9 File Offset: 0x000E82A9
	public static Vector2 ChangeVector2Length(Vector2 thePoint, Vector2 theOrigin, float theLength)
	{
		thePoint -= theOrigin;
		thePoint = thePoint.normalized * theLength;
		return thePoint + theOrigin;
	}

	// Token: 0x060041C8 RID: 16840 RVA: 0x000EA0CC File Offset: 0x000E82CC
	public static float GetLockedAngle(float currentAngle, int numberOfDirections)
	{
		currentAngle = CDGHelper.WrapAngleDegrees(currentAngle, false);
		float num = 360f / (float)numberOfDirections;
		float num2 = num / 2f;
		float num3 = 0f;
		int i = 0;
		while (i <= numberOfDirections)
		{
			float num4 = num3 + num2;
			float num5 = num3 - num2;
			if (currentAngle <= num4 && currentAngle >= num5)
			{
				if (num3 >= 360f)
				{
					return 0f;
				}
				return num3;
			}
			else
			{
				num3 += num;
				i++;
			}
		}
		return currentAngle;
	}

	// Token: 0x060041C9 RID: 16841 RVA: 0x000EA12E File Offset: 0x000E832E
	public static float WrapAngleDegrees(float degrees, bool convertToHalfRadius)
	{
		return CDGHelper.ToDegrees(CDGHelper.WrapAngleRadians(CDGHelper.ToRadians(degrees), convertToHalfRadius));
	}

	// Token: 0x060041CA RID: 16842 RVA: 0x000EA144 File Offset: 0x000E8344
	public static float WrapAngleRadians(float radians, bool convertToHalfRadius)
	{
		float num = 6.2831855f;
		int num2 = (int)(radians / num);
		radians -= num * (float)num2;
		if (convertToHalfRadius)
		{
			if (radians > 3.1415927f)
			{
				return radians - num;
			}
			if (radians < -3.1415927f)
			{
				return radians + num;
			}
			return radians;
		}
		else
		{
			if (radians < 0f)
			{
				return radians + num;
			}
			return radians;
		}
	}

	// Token: 0x060041CB RID: 16843 RVA: 0x000EA190 File Offset: 0x000E8390
	public static float TurnToFaceRadians(Vector2 sourcePosition, Vector2 targetPosition, float sourceTurnSpeedInRads, float sourceOrientationInRads, float elapsedTime, bool convertToHalfRadius)
	{
		float x = targetPosition.x - sourcePosition.x;
		float num = CDGHelper.WrapAngleRadians(Mathf.Atan2(targetPosition.y - sourcePosition.y, x), false);
		float num2 = CDGHelper.WrapAngleRadians(sourceOrientationInRads, false);
		float num3 = CDGHelper.WrapAngleRadians(sourceOrientationInRads, true);
		float num4 = num - num2;
		bool flag = num4 > 3.1415927f || num4 < -3.1415927f;
		float num5 = sourceTurnSpeedInRads * elapsedTime;
		if (flag)
		{
			float num6 = CDGHelper.WrapAngleRadians(num4, true);
			num6 = Mathf.Clamp(num6, -num5, num5);
			return CDGHelper.WrapAngleRadians(num3 + num6, convertToHalfRadius);
		}
		num4 = Mathf.Clamp(num4, -num5, num5);
		return CDGHelper.WrapAngleRadians(num2 + num4, convertToHalfRadius);
	}

	// Token: 0x060041CC RID: 16844 RVA: 0x000EA22C File Offset: 0x000E842C
	public static float TurnToFaceDegrees(Vector2 sourcePosition, Vector2 targetPosition, float sourceTurnSpeedInDegrees, float sourceOrientationInDegrees, float elapsedTime, bool convertToHalfRadius)
	{
		return CDGHelper.ToDegrees(CDGHelper.TurnToFaceRadians(sourcePosition, targetPosition, CDGHelper.ToRadians(sourceTurnSpeedInDegrees), CDGHelper.ToRadians(sourceOrientationInDegrees), elapsedTime, convertToHalfRadius));
	}

	// Token: 0x060041CD RID: 16845 RVA: 0x000EA24C File Offset: 0x000E844C
	public static int GetRandomOdds(List<float> oddsArray)
	{
		int count = oddsArray.Count;
		float num = 0f;
		for (int i = 0; i < count; i++)
		{
			num += oddsArray[i];
		}
		float num2 = UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		int num4 = 0;
		foreach (float num5 in oddsArray)
		{
			if (num5 == 0f)
			{
				num4++;
			}
			else
			{
				num3 += num5;
				if (num2 <= num3)
				{
					return num4;
				}
				num4++;
			}
		}
		return -1;
	}

	// Token: 0x060041CE RID: 16846 RVA: 0x000EA2FC File Offset: 0x000E84FC
	public static int GetRandomOdds(float[] oddsArray)
	{
		int num = oddsArray.Length;
		float num2 = 0f;
		for (int i = 0; i < num; i++)
		{
			num2 += oddsArray[i];
		}
		float num3 = UnityEngine.Random.Range(0f, num2);
		float num4 = 0f;
		int num5 = 0;
		foreach (float num6 in oddsArray)
		{
			if (num6 == 0f)
			{
				num5++;
			}
			else
			{
				num4 += num6;
				if (num3 <= num4)
				{
					return num5;
				}
				num5++;
			}
		}
		return -1;
	}

	// Token: 0x060041CF RID: 16847 RVA: 0x000EA380 File Offset: 0x000E8580
	public static int GetRandomOdds(List<int> oddsArray)
	{
		int count = oddsArray.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			num += oddsArray[i];
		}
		int num2 = UnityEngine.Random.Range(0, num + 1);
		int num3 = 0;
		int num4 = 0;
		foreach (int num5 in oddsArray)
		{
			if (num5 == 0)
			{
				num4++;
			}
			else
			{
				num3 += num5;
				if (num2 <= num3)
				{
					return num4;
				}
				num4++;
			}
		}
		return -1;
	}

	// Token: 0x060041D0 RID: 16848 RVA: 0x000EA424 File Offset: 0x000E8624
	public static int GetRandomOdds(int[] oddsArray)
	{
		int num = oddsArray.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			num2 += oddsArray[i];
		}
		int num3 = UnityEngine.Random.Range(0, num2 + 1);
		int num4 = 0;
		int num5 = 0;
		foreach (int num6 in oddsArray)
		{
			if (num6 == 0)
			{
				num5++;
			}
			else
			{
				num4 += num6;
				if (num3 <= num4)
				{
					return num5;
				}
				num5++;
			}
		}
		return -1;
	}

	// Token: 0x060041D1 RID: 16849 RVA: 0x000EA49C File Offset: 0x000E869C
	public static void Shuffle<T>(List<T> array)
	{
		for (int i = array.Count; i > 1; i--)
		{
			int index = UnityEngine.Random.Range(0, i);
			T value = array[index];
			array[index] = array[i - 1];
			array[i - 1] = value;
		}
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x000EA4E4 File Offset: 0x000E86E4
	public static void Shuffle<T>(T[] array)
	{
		for (int i = array.Length; i > 1; i--)
		{
			int num = UnityEngine.Random.Range(0, i);
			T t = array[num];
			array[num] = array[i - 1];
			array[i - 1] = t;
		}
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x000EA52C File Offset: 0x000E872C
	public static T CopyComponent<T>(T original, GameObject destination, bool forceAdd = false) where T : Component
	{
		Collider2D collider2D = original as Collider2D;
		bool flag = false;
		if (collider2D != null && collider2D.isTrigger)
		{
			flag = true;
		}
		Type type = original.GetType();
		Component component = (!forceAdd) ? destination.GetComponent(type) : null;
		if (component == null)
		{
			component = (destination.AddComponent(type) as T);
		}
		foreach (FieldInfo fieldInfo in type.GetFields())
		{
			if (!fieldInfo.IsStatic)
			{
				fieldInfo.SetValue(component, fieldInfo.GetValue(original));
			}
		}
		foreach (PropertyInfo propertyInfo in type.GetProperties())
		{
			if (propertyInfo.CanWrite && propertyInfo.CanRead && (!flag || !(propertyInfo.Name == "density")) && (!(type == typeof(CircleCollider2D)) || !(propertyInfo.Name == "usedByComposite")) && (!(type == typeof(CapsuleCollider2D)) || !(propertyInfo.Name == "usedByComposite")) && propertyInfo.CanWrite && !(propertyInfo.Name == "name"))
			{
				propertyInfo.SetValue(component, propertyInfo.GetValue(original, null), null);
			}
		}
		return component as T;
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x000EA6B4 File Offset: 0x000E88B4
	public static bool ComponentsEqual(Component component1, Component component2)
	{
		Type type = component1.GetType();
		foreach (FieldInfo fieldInfo in type.GetFields())
		{
			if (!(fieldInfo.Name == "name") && !(fieldInfo.Name == "transform") && !fieldInfo.IsStatic)
			{
				object value = fieldInfo.GetValue(component1);
				if (!(value.GetType() == typeof(UnityEngine.Object)))
				{
					object value2 = fieldInfo.GetValue(component2);
					if (value == fieldInfo.GetValue(component2))
					{
						Debug.Log(string.Concat(new string[]
						{
							"Diff on ",
							fieldInfo.Name,
							". Val 1: ",
							value.ToString(),
							". Val 2: ",
							value2.ToString()
						}));
						return false;
					}
				}
			}
		}
		foreach (PropertyInfo propertyInfo in type.GetProperties())
		{
			if (!(propertyInfo.Name == "name") && !(propertyInfo.Name == "transform"))
			{
				object value3 = propertyInfo.GetValue(component1, null);
				if (!(value3.GetType() == typeof(UnityEngine.Object)))
				{
					object value4 = propertyInfo.GetValue(component2, null);
					if (!value3.Equals(value4))
					{
						Debug.Log(string.Concat(new string[]
						{
							"Diff on ",
							propertyInfo.Name,
							". Val 1: ",
							value3.ToString(),
							". Val 2: ",
							value4.ToString()
						}));
						return false;
					}
				}
			}
		}
		return true;
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x000EA866 File Offset: 0x000E8A66
	public static float ToDegrees(float rads)
	{
		return rads * 57.29578f;
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x000EA86F File Offset: 0x000E8A6F
	public static float ToRadians(float degrees)
	{
		return degrees * 0.017453292f;
	}

	// Token: 0x060041D7 RID: 16855 RVA: 0x000EA878 File Offset: 0x000E8A78
	public static Color NormalizeRGB(float r, float g, float b, float a)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x000EA89B File Offset: 0x000E8A9B
	public static Color NormalizeRGB(Vector4 color)
	{
		return CDGHelper.NormalizeRGB(color.x, color.y, color.z, color.w);
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x000EA8BA File Offset: 0x000E8ABA
	public static Color NormalizeRGB(Color color)
	{
		return CDGHelper.NormalizeRGB(color.r, color.g, color.b, color.a);
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x000EA8DC File Offset: 0x000E8ADC
	public static BoxCollider2D ConvertPolyToBoxCollider(PolygonCollider2D polyCollider, bool destroyPolyCollider)
	{
		GameObject gameObject = polyCollider.gameObject;
		Vector3 localEulerAngles = gameObject.transform.localEulerAngles;
		gameObject.transform.localEulerAngles = Vector3.zero;
		if (!CDGHelper.CanPolyColliderBeBoxCollider(polyCollider))
		{
			gameObject.transform.localEulerAngles = localEulerAngles;
			return null;
		}
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.size = polyCollider.bounds.size;
		boxCollider2D.offset = polyCollider.bounds.center - polyCollider.gameObject.transform.position;
		boxCollider2D.isTrigger = polyCollider.isTrigger;
		boxCollider2D.usedByComposite = polyCollider.usedByComposite;
		boxCollider2D.usedByEffector = polyCollider.usedByEffector;
		if ((!Application.isEditor || Application.isPlaying) && destroyPolyCollider)
		{
			UnityEngine.Object.DestroyImmediate(polyCollider, true);
		}
		gameObject.transform.localEulerAngles = localEulerAngles;
		return boxCollider2D;
	}

	// Token: 0x060041DB RID: 16859 RVA: 0x000EA9B8 File Offset: 0x000E8BB8
	public static bool CanPolyColliderBeBoxCollider(PolygonCollider2D polyCollider)
	{
		if (polyCollider.GetTotalPointCount() != 4)
		{
			return false;
		}
		Vector2 vector = new Vector2(float.MinValue, float.MinValue);
		int num = 0;
		Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
		int num2 = 0;
		foreach (Vector2 vector3 in polyCollider.points)
		{
			if (vector.x != vector3.x && vector.y != vector3.x)
			{
				if (num == 0)
				{
					vector.x = vector3.x;
				}
				else if (num == 1)
				{
					vector.y = vector3.x;
				}
				num++;
			}
			if (vector2.x != vector3.y && vector2.y != vector3.y)
			{
				if (num2 == 0)
				{
					vector2.x = vector3.y;
				}
				else if (num2 == 1)
				{
					vector2.y = vector3.y;
				}
				num2++;
			}
		}
		return num == 2 && num2 == 2;
	}

	// Token: 0x060041DC RID: 16860 RVA: 0x000EAABC File Offset: 0x000E8CBC
	public static string ToRoman(int number)
	{
		if (number < 0 || number > 3999)
		{
			Debug.Log("Cannot return roman numberals for: " + number.ToString() + ". Value must be between 1 and 3999");
			return string.Empty;
		}
		if (number < 1)
		{
			return string.Empty;
		}
		if (number >= 1000)
		{
			return "M" + CDGHelper.ToRoman(number - 1000);
		}
		if (number >= 900)
		{
			return "CM" + CDGHelper.ToRoman(number - 900);
		}
		if (number >= 500)
		{
			return "D" + CDGHelper.ToRoman(number - 500);
		}
		if (number >= 400)
		{
			return "CD" + CDGHelper.ToRoman(number - 400);
		}
		if (number >= 100)
		{
			return "C" + CDGHelper.ToRoman(number - 100);
		}
		if (number >= 90)
		{
			return "XC" + CDGHelper.ToRoman(number - 90);
		}
		if (number >= 50)
		{
			return "L" + CDGHelper.ToRoman(number - 50);
		}
		if (number >= 40)
		{
			return "XL" + CDGHelper.ToRoman(number - 40);
		}
		if (number >= 10)
		{
			return "X" + CDGHelper.ToRoman(number - 10);
		}
		if (number >= 9)
		{
			return "IX" + CDGHelper.ToRoman(number - 9);
		}
		if (number >= 5)
		{
			return "V" + CDGHelper.ToRoman(number - 5);
		}
		if (number >= 4)
		{
			return "IV" + CDGHelper.ToRoman(number - 4);
		}
		if (number >= 1)
		{
			return "I" + CDGHelper.ToRoman(number - 1);
		}
		return string.Empty;
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x000EAC5C File Offset: 0x000E8E5C
	public static bool IsPercent(float value)
	{
		return Mathf.Floor(value) != value;
	}

	// Token: 0x060041DE RID: 16862 RVA: 0x000EAC6A File Offset: 0x000E8E6A
	public static int ToPercent(float value)
	{
		return Mathf.RoundToInt(value * 100f);
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x000EAC78 File Offset: 0x000E8E78
	public static bool DoCollisionTypesCollide_V2(CollisionType collidesWithType, GameObject other)
	{
		TagType tagType = TagType.Untagged;
		bool flag = false;
		foreach (KeyValuePair<string, TagType> keyValuePair in CDGHelper.m_tagToEnumTable)
		{
			if (other.CompareTag(keyValuePair.Key))
			{
				flag = true;
				tagType = keyValuePair.Value;
				break;
			}
		}
		if (!flag)
		{
			string tag = other.tag;
			tagType = TagType_RL.ToEnum(tag);
			CDGHelper.m_tagToEnumTable.Add(tag, tagType);
		}
		return (CollisionType_RL.GetEquivalentCollisionType(tagType) & collidesWithType) > CollisionType.None;
	}

	// Token: 0x0400393B RID: 14651
	private static Dictionary<string, TagType> m_tagToEnumTable = new Dictionary<string, TagType>();
}
