using System;
using System.IO;
using System.Xml;

// Token: 0x02000CD5 RID: 3285
public static class RNGControllerDataExporter
{
	// Token: 0x06005DA2 RID: 23970 RVA: 0x0015EC08 File Offset: 0x0015CE08
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

	// Token: 0x06005DA3 RID: 23971 RVA: 0x00033858 File Offset: 0x00031A58
	private static void WriteElement(XmlWriter xmlWriter, string elementName, int value)
	{
		RNGControllerDataExporter.WriteElement(xmlWriter, elementName, value.ToString());
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x00033868 File Offset: 0x00031A68
	private static void WriteElement(XmlWriter xmlWriter, string elementName, float value)
	{
		RNGControllerDataExporter.WriteElement(xmlWriter, elementName, value.ToString());
	}

	// Token: 0x06005DA5 RID: 23973 RVA: 0x00033878 File Offset: 0x00031A78
	private static void WriteElement(XmlWriter xmlWriter, string elementName, string value)
	{
		xmlWriter.WriteStartElement(elementName);
		xmlWriter.WriteString(value);
		xmlWriter.WriteEndElement();
	}

	// Token: 0x04004D0B RID: 19723
	private const string PATH = "Assets/Logs/RNG/";

	// Token: 0x04004D0C RID: 19724
	private const string POSTFIX = "_RNGDump";

	// Token: 0x04004D0D RID: 19725
	private const string ROOT_NAME = "callerData";

	// Token: 0x04004D0E RID: 19726
	private const string ID_NAME = "id";

	// Token: 0x04004D0F RID: 19727
	private const string SEED_NAME = "seed";

	// Token: 0x04004D10 RID: 19728
	private const string RECORD_NAME = "data";

	// Token: 0x04004D11 RID: 19729
	private const string ORDER_NUMBER_ELEMENT_NAME = "frameNumber";

	// Token: 0x04004D12 RID: 19730
	private const string NUMBER_ELEMENT_NAME = "number";

	// Token: 0x04004D13 RID: 19731
	private const string MIN_ELEMENT_NAME = "min";

	// Token: 0x04004D14 RID: 19732
	private const string MAX_ELEMENT_NAME = "max";

	// Token: 0x04004D15 RID: 19733
	private const string DESCRIPTION_ELEMENT_NAME = "description";
}
