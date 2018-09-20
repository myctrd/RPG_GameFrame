using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;

public class DataManager : MonoBehaviour {

    public static DataManager m_instance;

    void Awake()
    {
        if(m_instance == null)
        {
            m_instance = this;
        }
    }

    public const string dataPath = "/Resources/Xmls/";

    XmlDocument xml_itemData;
    XmlDocument xml_equipData;

    Dictionary<string, string> m_itemDic = new Dictionary<string, string>();
    Dictionary<string, string> m_equipDic = new Dictionary<string, string>();

    public void LoadItemData()
    {
        if (File.Exists(Application.dataPath + dataPath + "ItemData.XML"))
        {
            xml_itemData = new XmlDocument();
            xml_itemData.Load(Application.dataPath + dataPath + "ItemData.XML");
        }
        else
        {
            xml_itemData = CreateXML();
            UpdateNodeToXML("ItemData");
            SaveXML(xml_itemData, "ItemData");
        }
        m_itemDic.Clear();
        XmlNode root = xml_itemData.SelectSingleNode("Root");
        foreach (XmlElement node in root)
        {
            m_itemDic[node.ChildNodes[0].InnerText] = node.ChildNodes[1].InnerText;
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("id", node.ChildNodes[0].InnerText);
            p.Add("count", node.ChildNodes[1].InnerText);
            EventManager.Broadcast("DataManager.AddItem", p);
        }
    }

    public void LoadEquipData()
    {
        if (File.Exists(Application.dataPath + dataPath + "EquipData.XML"))
        {
            xml_equipData = new XmlDocument();
            xml_equipData.Load(Application.dataPath + dataPath + "EquipData.XML");
        }
        else
        {
            xml_equipData = CreateXML();
            UpdateNodeToXML("EquipData");
            SaveXML(xml_equipData, "EquipData");
        }
        m_equipDic.Clear();
        XmlNode root = xml_equipData.SelectSingleNode("Root");
        foreach (XmlElement node in root)
        {
            m_equipDic[node.ChildNodes[0].InnerText] = node.ChildNodes[1].InnerText;
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("id", node.ChildNodes[0].InnerText);
            EventManager.Broadcast("DataManager.AddEquip", p);
        }
    }

    public void AddEquip(string id, string count)
    {
        AddNodeToXML(xml_equipData, "ID", id, "COUNT", count);
        SaveXML(xml_equipData, "EquipData");
    }

    public XmlDocument CreateXML()
    {
        XmlDocument xml = new XmlDocument();
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        xml.AppendChild(xml.CreateElement("Root"));
        return xml;
    }

    void AddNodeToXML(XmlDocument xml, string title_1, string value_1, string title_2, string value_2)
    {
        XmlNode root = xml.SelectSingleNode("Root");
        XmlElement element = xml.CreateElement("Node");
        element.SetAttribute("Type", "string");

        XmlElement titleElelment = xml.CreateElement(title_1);
        titleElelment.InnerText = value_1;

        XmlElement infoElement = xml.CreateElement(title_2);
        infoElement.InnerText = value_2;

        element.AppendChild(titleElelment);
        element.AppendChild(infoElement);
        root.AppendChild(element);
    }

    void UpdateNodeToXML(string fileName)
    {
        string filepath = Application.dataPath + dataPath + fileName + ".XML";
        if (File.Exists(filepath))
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filepath);
            XmlNodeList nodeList = xmldoc.SelectSingleNode("Root").ChildNodes;
            foreach (XmlElement xe in nodeList)
            {
                if (xe.GetAttribute("Type") == "string")
                {
                    xe.SetAttribute("type", "text");
                    foreach (XmlElement xelement in xe.ChildNodes)
                    {
                        if (xelement.Name == "TitleNode")
                        {
                            xelement.InnerText = fileName;
                        }
                    }
                    break;
                }
            }
            xmldoc.Save(filepath);
        }
    }


    void SaveXML(XmlDocument xml, string fileName)
    {
        xml.Save(Application.dataPath + dataPath + fileName + ".XML");
    }
}
