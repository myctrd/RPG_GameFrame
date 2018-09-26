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
    XmlDocument xml_notesData;

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
        XmlNode root = xml_itemData.SelectSingleNode("Root");
        foreach (XmlElement node in root)
        {
            if(int.Parse(node.ChildNodes[1].InnerText) > 0)
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("id", node.ChildNodes[0].InnerText);
                p.Add("count", node.ChildNodes[1].InnerText);
                EventManager.Broadcast("DataManager.AddItem", p);
            }
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
        XmlNode root = xml_equipData.SelectSingleNode("Root");
        foreach (XmlElement node in root)
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("id", node.ChildNodes[0].InnerText);
            EventManager.Broadcast("DataManager.AddEquip", p);
        }
    }

    public void LoadNotesData()
    {
        if (File.Exists(Application.dataPath + dataPath + "NotesData.XML"))
        {
            xml_notesData = new XmlDocument();
            xml_notesData.Load(Application.dataPath + dataPath + "NotesData.XML");
        }
        else
        {
            xml_notesData = CreateXML();
            UpdateNodeToXML("NotesData");
            SaveXML(xml_notesData, "NotesData");
        }
        XmlNode root = xml_notesData.SelectSingleNode("Root");
        foreach (XmlElement node in root)
        {
            Dictionary<string, object> p = new Dictionary<string, object>();
            p.Add("id", node.ChildNodes[0].InnerText);
            p.Add("type", node.ChildNodes[1].InnerText);
            EventManager.Broadcast("DataManager.AddNotes", p);
        }
    }

    public void AddEquip(string id, string count)
    {
        AddNodeToXML(xml_equipData, "ID", id, "COUNT", count);
        SaveXML(xml_equipData, "EquipData");
    }

    public void UpdateItem(string id, string count)
    {
        AddNodeToXML(xml_itemData, "ID", id, "COUNT", count);
        SaveXML(xml_itemData, "ItemData");
    }

    public void AddNotes(string id, string type)
    {
        AddNodeToXML(xml_notesData, "ID", id, "TYPE", type);
        SaveXML(xml_notesData, "NotesData");
        Dictionary<string, object> p = new Dictionary<string, object>();
        p.Add("id", id);
        p.Add("type", type);
        EventManager.Broadcast("DataManager.AddNotes", p);
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

        foreach (XmlElement node in root)
        {
            if(node.ChildNodes[0].InnerText == value_1)
            {
                node.ChildNodes[1].InnerText = value_2;
                return;
            }
        }

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
