/*
The Lang Class adds easy to use multiple language support to any Unity project by parsing an XML file
containing numerous strings translated into any languages of your choice.  Refer to UMLS_Help.html and lang.xml
for more information.
 
Created by Adam T. Ryder
C# version by O. Glorieux
 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

using UnityEngine;
//using RotaryHeart.Lib.SerializableDictionary;

/// <summary>
/// This is the main class used to implement language control.
/// </summary>
public class Lang
{
    private Dictionary<string, string> Strings;

    /*
    Initialize Lang class
    path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
    language = language to use example:  "English"
    web = boolean indicating if resource is local or on-line example:  true if on-line, false if local
 
    NOTE:
    If XML resource is on-line rather than local do not supply the path to the path variable as stated above
    instead use the WWW class to download the resource and then supply the resource.text to this initializer
 
    Web Example:
    var wwwXML : WWW = new WWW("http://www.exampleURL.com/lang.xml");
    yield wwwXML;
 
    var LangClass : Lang = new Lang(wwwXML.text, currentLang, true)
    */
    public Lang(string language)
    {
        SetLanguage(language);
    }

    /*
    Use the setLanguage function to swap languages after the Lang class has been initialized.
    This function is called automatically when the Lang class is initialized.
    path = path to XML resource example:  Path.Combine(Application.dataPath, "lang.xml")
    language = language to use example:  "English"
 
    NOTE:
    If the XML resource is stored on the web rather than on the local system use the
    setLanguageWeb function
    */
    public void SetLanguage(string language)
    {
        // This path is directing to the StreamingAssets/Languages folder, since I wanted the xml file to remain acessible even after the game buid, and every language there have it's own xml file for the ease of editing them.

        string path = Path.Combine(Application.dataPath, "Languages", language + ".xml");
        XmlDocument xml = new XmlDocument();
        xml.Load(path);
        var languagesWrap = xml.LastChild;
        XmlNode languagenodes = languagesWrap.ChildNodes[0];
        var nodes = languagesWrap.SelectSingleNode(language);
        if (nodes != null)
        {
            Strings = new Dictionary<string, string>();

            int nodescount = languagesWrap.SelectSingleNode(language).SelectNodes("string").Count;

            for (var i = 0; i < nodescount; i++)
            {
                string entryLabel = nodes.SelectNodes("string")[i].Attributes.GetNamedItem("name").Value.ToLower();
                string entryContent = nodes.SelectNodes("string")[i].InnerText;
                if (!Strings.ContainsKey(entryLabel))
                    Strings.Add(entryLabel, entryContent);
                else
                    Debug.LogWarning("Detected conflicting entries " + entryLabel + ", entry " + (i + 1) + " weren't added.");
            }
        }
        else
            Debug.LogError("The specified language does not exist: " + language + ", in:" + path);
    }
    public string GetEntry(string name)
    {
        if (!Strings.ContainsKey(name.ToLower()))
        {
            Debug.LogWarning("The specified string does not exist: " + name);

            //if not found, return standard string, so you could instantly notice that something's wrong.
            return Strings["default"];
        }

        return Strings[name.ToLower()];
    }

}
