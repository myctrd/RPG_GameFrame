using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;

public class Map : MonoBehaviour {

    public int ID;
    public int col;  //列数
    public int line;  //行数

    public int defaultLine;
    public int defaultCol;  //默认初始坐标
    

    public void SaveMapInfo()
    {
        XmlDocument xml = CreateXML();
        AddNodeToXML(xml, "mapCol", col.ToString());
        AddNodeToXML(xml, "mapLine", line.ToString());
        AddNodeToXML(xml, "defaultLine", defaultLine.ToString());
        AddNodeToXML(xml, "defaultCol", defaultCol.ToString());
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            AddNodeToXML(xml, i.ToString(), transform.GetChild(i).GetComponent<MapTile>().ToString());
        }
        UpdateNodeToXML();

        SaveXML(xml);
        AssetDatabase.Refresh();

    }
    XmlDocument CreateXML()
    {
        XmlDocument xml = new XmlDocument();
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        xml.AppendChild(xml.CreateElement("Root"));
        return xml;
    }

    void AddNodeToXML(XmlDocument xml, string titleValue, string infoValue)
    {
        XmlNode root = xml.SelectSingleNode("Root");
        XmlElement element = xml.CreateElement("Node");
        element.SetAttribute("Type", "string");
        
        XmlElement titleElelment = xml.CreateElement("Title");
        titleElelment.InnerText = titleValue;

        XmlElement infoElement = xml.CreateElement("Info");
        infoElement.InnerText = infoValue;

        element.AppendChild(titleElelment);
        element.AppendChild(infoElement);
        root.AppendChild(element);
    }

    void UpdateNodeToXML()
    {
        string filepath = Application.dataPath + @"/Resources/Xmls/MapData/map_" + ID.ToString() + ".XML";
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
                            xelement.InnerText = "地图信息";
                        }
                    }
                    break;
                }
            }
            xmldoc.Save(filepath);
        }
    }


    void SaveXML(XmlDocument xml)
    {
        xml.Save(Application.dataPath + "/Resources/Xmls/MapData/map_" + ID.ToString() + ".XML");
    }

}