using System;
using System.IO;
using System.Xml;

// Token: 0x0200080B RID: 2059
public static class RNGControllerDataExporter
{
	// Token: 0x06004417 RID: 17431 RVA: 0x000F0FC4 File Offset: 0x000EF1C4
	public static void ExportControllerToFile(RNGController controller)
	{
		if (controller.CallerData != null && controller.CallerData.Count > 0)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Indent = true,
				IndentChars = "\t",
				NewLineOnAttributes = true
			};
			string text = string.Format("{0}{1}{2}", "Assets/Logs/RNG/", controller.ID.ToString(), "_RNGDump");
			if (File.Exists(text + ".xml"))
			{
				int num = 2;
				string text2 = string.Format("{0} ({1})", text, num);
				while (File.Exists(text2 + ".xml"))
				{
					num++;
					text2 = string.Format("{0} ({1})", text, num);
				}
				text = text2;
			}
			XmlWriter xmlWriter = XmlWriter.Create(text + ".xml", settings);
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("callerData");
			xmlWriter.WriteAttributeString("id", controller.ID.ToString());
			xmlWriter.WriteAttributeString("seed", controller.Seed.ToString());
			foreach (RNGData rngdata in controller.CallerData)
			{
				xmlWriter.WriteStartElement("data");
				xmlWriter.WriteAttributeString("frameNumber", rngdata.Order.ToString());
				RNGControllerDataExporter.WriteElement(xmlWriter, "min", rngdata.Min);
				RNGControllerDataExporter.WriteElement(xmlWriter, "max", rngdata.Max);
				RNGControllerDataExporter.WriteElement(xmlWriter, "number", rngdata.Number);
				RNGControllerDataExporter.WriteElement(xmlWriter, "description", rngdata.CallerDescription);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
		}
	}

	// Token: 0x06004418 RID: 17432 RVA: 0x000F11B0 File Offset: 0x000EF3B0
	private static void WriteElement(XmlWriter xmlWriter, string elementName, int value)
	{
		RNGControllerDataExporter.WriteElement(xmlWriter, elementName, value.ToString());
	}

	// Token: 0x06004419 RID: 17433 RVA: 0x000F11C0 File Offset: 0x000EF3C0
	private static void WriteElement(XmlWriter xmlWriter, string elementName, float value)
	{
		RNGControllerDataExporter.WriteElement(xmlWriter, elementName, value.ToString());
	}

	// Token: 0x0600441A RID: 17434 RVA: 0x000F11D0 File Offset: 0x000EF3D0
	private static void WriteElement(XmlWriter xmlWriter, string elementName, string value)
	{
		xmlWriter.WriteStartElement(elementName);
		xmlWriter.WriteString(value);
		xmlWriter.WriteEndElement();
	}

	// Token: 0x04003A33 RID: 14899
	private const string PATH = "Assets/Logs/RNG/";

	// Token: 0x04003A34 RID: 14900
	private const string POSTFIX = "_RNGDump";

	// Token: 0x04003A35 RID: 14901
	private const string ROOT_NAME = "callerData";

	// Token: 0x04003A36 RID: 14902
	private const string ID_NAME = "id";

	// Token: 0x04003A37 RID: 14903
	private const string SEED_NAME = "seed";

	// Token: 0x04003A38 RID: 14904
	private const string RECORD_NAME = "data";

	// Token: 0x04003A39 RID: 14905
	private const string ORDER_NUMBER_ELEMENT_NAME = "frameNumber";

	// Token: 0x04003A3A RID: 14906
	private const string NUMBER_ELEMENT_NAME = "number";

	// Token: 0x04003A3B RID: 14907
	private const string MIN_ELEMENT_NAME = "min";

	// Token: 0x04003A3C RID: 14908
	private const string MAX_ELEMENT_NAME = "max";

	// Token: 0x04003A3D RID: 14909
	private const string DESCRIPTION_ELEMENT_NAME = "description";
}
