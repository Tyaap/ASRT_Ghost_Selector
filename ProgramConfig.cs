using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;

namespace GhostSelector
{
    public static class ErrorMessage
    {
        public static void Message(string message)
        {
            DialogResult result = MessageBox.Show(
                message + "\n\n" +
                "Do you wish to continue, with this being ignored?",
                "Error reading configuration.",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                Environment.Exit(0);
            }
        }

        public static void UnknownValueMessage(string option)
        {
            string message = "Found an unknown value for the following option while reading the config file: " + option;
            Message(message);
        }

        public static void UnknownElementMessage(string option)
        {
            string message = "Found an unknown element while reading the config file: " + option;
            Message(message);
        }

        public static void UnknownAttributeMessage(string option)
        {
            string message = "Found an unknown attribute while reading the config file: " + option;
            Message(message);
        }

        public static void MissingPropertymessage(string option)
        {
            string message = "Could not find a required property while reading the config file: " + option;
            Message(message);
        }
    }

    public class ConfigurationSectionEx : ConfigurationSection
    {
        protected override bool OnDeserializeUnrecognizedAttribute(string attribute, string value)
        {
            ErrorMessage.UnknownAttributeMessage(attribute + "=\"" + value + "\"");

            return true;
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            ErrorMessage.UnknownElementMessage(elementName);

            return true;
        }

        protected override object OnRequiredPropertyNotFound(string name)
        {
            ErrorMessage.MissingPropertymessage(name);

            return true;
        }
    }

    public class ConfigurationElementEx : ConfigurationElement
    {
        protected override bool OnDeserializeUnrecognizedAttribute(string attribute, string value)
        {
            ErrorMessage.UnknownAttributeMessage(attribute + " = \"" + value + "\"");

            return true;
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            bool result = base.OnDeserializeUnrecognizedElement(elementName, reader);


            ErrorMessage.UnknownElementMessage(elementName);


            return true;
        }

        protected override object OnRequiredPropertyNotFound(string name)
        {
            ErrorMessage.MissingPropertymessage(name);

            return true;
        }
    }

    public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElementEx, new()
    {
        protected override bool OnDeserializeUnrecognizedAttribute(string attribute, string value)
        {
            ErrorMessage.UnknownAttributeMessage(attribute + " = \"" + value + "\"");

            return true;
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            bool result = base.OnDeserializeUnrecognizedElement(elementName, reader);
            if (!result)
            {
                ErrorMessage.UnknownElementMessage(elementName);
            }
            return true;
        }

        protected override object OnRequiredPropertyNotFound(string name)
        {
            ErrorMessage.MissingPropertymessage(name);

            return true;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((T)element);
        }

        public void Add(T element)
        {
            BaseAdd(element);
        }

        public void AddAt(int index, T element)
        {
            BaseAdd(index, element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(T element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(T element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public T this[int index]
        {
            get { return (T)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);

                BaseAdd(index, value);
            }
        }
    }

    public class ProgramConfigSection : ConfigurationSectionEx
    {
        [ConfigurationProperty("PositionSelector")]
        public PositionSelectorElement PositionSelector
        {
            get
            {
                if (base["PositionSelector"] is PositionSelectorElement tmp)
                    return tmp;
                else
                    return new PositionSelectorElement();
            }
        }

        [ConfigurationProperty("FastestPlayerSelector")]
        public FastestPlayerSelectorElement FastestPlayerSelector
        {
            get
            {
                if (base["FastestPlayerSelector"] is FastestPlayerSelectorElement tmp)
                    return tmp;
                else
                    return new FastestPlayerSelectorElement();
            }
        }

        [ConfigurationProperty("Graphics")]
        public GraphicsElement Graphics
        {
            get
            {
                if (base["Graphics"] is GraphicsElement tmp)
                    return tmp;
                else
                    return new GraphicsElement();
            }
        }
    }

    public class GraphicsElement : ConfigurationElementEx
    {
        [ConfigurationProperty("HideGhostCars", DefaultValue = "False")]
        public bool HideGhostCars
        {
            get
            {
                try
                {
                    return (bool)this["HideGhostCars"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("HideGhostCars");
                    this["HideGhostCars"] = false;
                    return false;
                }
            }
            set => this["HideGhostCars"] = value;
        }

        [ConfigurationProperty("HidePBGhost", DefaultValue = "False")]
        public bool HidePBGhost
        {
            get
            {
                try
                {
                    return (bool)this["HidePBGhost"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("HidePBGhost");
                    this["HidePBGhost"] = false;
                    return false;
                }
            }
            set => this["HidePBGhost"] = value;
        }

        [ConfigurationProperty("NameTagOpacity", DefaultValue = "1")]
        public float NameTagOpacity
        {
            get
            {
                try
                {
                    return (float)this["NameTagOpacity"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("NameTagOpacity");
                    this["NameTagOpacity"] = 1;
                    return 1;
                }
            }
            set => this["NameTagOpacity"] = value;
        }
    }

    public class FastestPlayerSelectorElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Enabled", DefaultValue = "True")]
        public bool Enabled
        {
            get
            {
                try
                {
                    return (bool)this["Enabled"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Enabled");
                    this["Enabled"] = false;
                    return false;
                }
            }
            set => this["Enabled"] = value;
        }

        [ConfigurationProperty("Players")]
        [ConfigurationCollection(typeof(PlayerElement), AddItemName = "Entry", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public GenericConfigurationElementCollection<PlayerElement> Players
        {
            get
            {
                if (base["Players"] is GenericConfigurationElementCollection<PlayerElement> tmp)
                    return tmp;
                else
                    return new GenericConfigurationElementCollection<PlayerElement>();
            }
        }

    }
    public class PlayerElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Name", DefaultValue = "")]
        public string Name
        {
            get
            {
                if (this["Name"] is string tmp)
                    return tmp;
                else
                    return "";
            }
            set => this["Name"] = value;
        }

        [ConfigurationProperty("SteamId", DefaultValue = "0")]
        public ulong SteamId
        {
            get
            {
                try
                {
                    return (ulong)this["SteamId"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("SteamId");
                    this["SteamId"] = 0;
                    return 0;
                }
            }
            set => this["SteamId"] = value;
        }

        [ConfigurationProperty("Enabled", DefaultValue = "True")]
        public bool Enabled
        {
            get
            {
                try
                {
                    return (bool)this["Enabled"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Enabled");
                    this["Enabled"] = true;
                    return true;
                }
            }
            set => this["Enabled"] = value;
        }
    }

    public class PositionSelectorElement : ConfigurationElementEx
    {
        [ConfigurationProperty("SelectedPosition", DefaultValue = "0")]
        public uint SelectedPosition
        {
            get
            {
                try
                {
                    return (uint)this["SelectedPosition"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("SelectedPosition");
                    this["SelectedPosition"] = 0;
                    return 0;
                }
            }
            set => this["SelectedPosition"] = value;
        }
    }
}