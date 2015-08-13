using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Vurdalakov.Configuration
{
    public class ConfigurationFile
    {
        private string fileName;
        private XmlDocument document;

        public ConfigurationFile()
        {
            this.fileName = Path.ChangeExtension(Application.ExecutablePath, ".cfg");

            Load();
        }

        public ConfigurationFile(String applicationName, String fileName)
        {
            this.fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationName);
            this.fileName = Path.Combine(this.fileName, fileName);
            this.fileName += ".cfg";
            
            Load();
        }

        public ConfigurationFile(String fileName)
        {
            this.fileName = fileName;

            Load();
        }

        public bool Load()
        {
            document = new XmlDocument();

            try
            {
                document.Load(fileName);
            }
            catch
            {
                document.LoadXml("<!DOCTYPE options[\n<!ELEMENT options ANY>\n<!ELEMENT option ANY>\n" +
                "<!ATTLIST option name ID #REQUIRED>\n<!ATTLIST option value IDREF #REQUIRED>\n]>\n" +
                "<options>\n</options>");

                Save();
            }

            return true;
        }
        
        public void Save()
        {
            try
            {
                String directoryName = Path.GetDirectoryName(fileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                document.Save(fileName);
            }
            catch
            {
            }
        }

        public XmlElement GetElementById(string name)
        {
            Load();

            return document.GetElementById(name);
        }

        public bool SettingExists(string name)
        {
            XmlElement option = GetElementById(name);
            return option != null;
        }

        public string GetSetting(string name)
        {
            XmlElement option = GetElementById(name);
            return null == option ? null : option.GetAttribute("value");
        }

        public string GetSetting(string name, string defaultValue)
        {
            XmlElement option = GetElementById(name);
            return null == option ? defaultValue : option.GetAttribute("value");
        }

        public bool GetSetting(string name, bool defaultValue)
        {
            XmlElement option = GetElementById(name);
            return null == option ? defaultValue : option.GetAttribute("value").Equals("1");
        }

        public int GetSetting(string name, int defaultValue)
        {
            try
            {
                XmlElement option = GetElementById(name);
                return null == option ? defaultValue : Convert.ToInt32(option.GetAttribute("value"));
            }
            catch
            {
                return defaultValue;
            }
        }

        public decimal GetSetting(string name, decimal defaultValue)
        {
            try
            {
                XmlElement option = GetElementById(name);
                return null == option ? defaultValue : Convert.ToDecimal(option.GetAttribute("value"));
            }
            catch
            {
                return defaultValue;
            }
        }

        public float GetSetting(string name, float defaultValue)
        {
            String value = GetSetting(name);

            float result;
            return (value != null) && float.TryParse(value, out result) ? result : defaultValue;
        }

        public Font GetSetting(string name, String defaultFontName, float defaultFontSize)
        {
            try
            {
                String fontName = GetSetting(name + "-name", defaultFontName);
                float fontSize = GetSetting(name + "-size", defaultFontSize);

                return new Font(fontName, fontSize);
            }
            catch
            {
                return new Font(defaultFontName, defaultFontSize);
            }
        }

        public Font GetSetting(string name, Font defaultValue)
        {
            return GetSetting(name, defaultValue.Name, defaultValue.SizeInPoints);
        }

        public string[] GetSettingArray(string name, string defaultValue)
        {
            string S = GetSetting(name, defaultValue);
            return null == S ? null : S.Split(new Char[] { ',' });
        }

        public int GetSettingArray(string name, ComboBox combobox, string defaultValue)
        {
            string[] array = GetSettingArray(name, defaultValue);
            if (array != null)
                combobox.Items.AddRange(array);
            if (combobox.Items.Count > 0)
                combobox.SelectedIndex = 0;
            
            return combobox.Items.Count;
        }

        public void SetSetting(string name, string value)
        {
            XmlElement option = GetElementById(name);
            if (null == option)
            {
                option = (XmlElement)document.CreateNode(XmlNodeType.Element, "option", "");
                option.SetAttribute("name", name);
                document.DocumentElement.AppendChild(option);
            }

            option.SetAttribute("value", value);
            Save();
        }

        public void SetSetting(string name, bool value)
        {
            SetSetting(name, value ? "1" : "0");
        }

        public void SetSetting(string name, int value)
        {
            SetSetting(name, value.ToString());
        }

        public void SetSetting(string name, decimal value)
        {
            SetSetting(name, value.ToString());
        }

        public void SetSetting(string name, float value)
        {
            SetSetting(name, value.ToString());
        }

        public void SetSetting(string name, Font value)
        {
            SetSetting(name + "-name", value.Name);
            SetSetting(name + "-size", value.SizeInPoints.ToString());
        }

        public void SetSettingArray(string name, string[] value)
        {
            string S = string.Join(",", value);
            SetSetting(name, S);
        }

        public void SetSettingArray(string name, ComboBox combobox)
        {
            string[] array = new string[combobox.Items.Count];
            for (int i = 0; i < combobox.Items.Count; i++)
                array[i] = combobox.Items[i].ToString();
            SetSettingArray(name, array);
        }

        public void RemoveSetting(string name)
        {
            XmlElement option = GetElementById(name);
            if (option != null)
                document.DocumentElement.RemoveChild(option);
            Save();
        }
    }
}
