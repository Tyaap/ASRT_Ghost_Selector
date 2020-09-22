using System;
using System.Configuration;
using System.Drawing;
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
        [ConfigurationProperty("Nametag")]
        public NametagElement Nametag
        {
            get
            {
                if (base["Nametag"] is NametagElement tmp)
                    return tmp;
                else
                    return new NametagElement();
            }
        }

        [ConfigurationProperty("PBGhost")]
        public PBGhostElement PBGhost
        {
            get
            {
                if (base["PBGhost"] is PBGhostElement tmp)
                    return tmp;
                else
                    return new PBGhostElement();
            }
        }

        [ConfigurationProperty("OnlineGhost")]
        public OnlineGhostElement OnlineGhost
        {
            get
            {
                if (base["OnlineGhost"] is OnlineGhostElement tmp)
                    return tmp;
                else
                    return new OnlineGhostElement();
            }
        }
    }

    public class NametagElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Opacity", DefaultValue = "1")]
        public float Opacity
        {
            get
            {
                try
                {
                    return (float)this["Opacity"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Opacity");
                    this["Opacity"] = 1;
                    return 1;
                }
            }
            set => this["Opacity"] = value;
        }
    }

    public class PBGhostElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Hide", DefaultValue = "false")]
        public bool Hide
        {
            get
            {
                try
                {
                    return (bool)this["Hide"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Hide");
                    this["Hide"] = false;
                    return false;
                }
            }
            set => this["Hide"] = value;
        }

        [ConfigurationProperty("Opacity", DefaultValue = "1")]
        public float Opacity
        {
            get
            {
                try
                {
                    return (float)this["Opacity"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Opacity");
                    this["Opacity"] = 1;
                    return 1;
                }
            }
            set => this["Opacity"] = value;
        }

        [ConfigurationProperty("UseCustomColour", DefaultValue = "false")]
        public bool UseCustomColour
        {
            get
            {
                try
                {
                    return (bool)this["UseCustomColour"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("UseCustomColour");
                    this["UseCustomColour"] = false;
                    return false;
                }
            }
            set => this["UseCustomColour"] = value;
        }

        [ConfigurationProperty("Colour", DefaultValue = "magenta")]
        public Color Colour
        {
            get
            {
                try
                {
                    return (Color)this["Colour"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Colour");
                    this["Colour"] = Color.Magenta;
                    return Color.Magenta;
                }
            }
            set => this["Colour"] = value;
        }
    }

    public class OnlineGhostElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Opacity", DefaultValue = "1")]
        public float Opacity
        {
            get
            {
                try
                {
                    return (float)this["Opacity"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Opacity");
                    this["Opacity"] = 1;
                    return 1;
                }
            }
            set => this["Opacity"] = value;
        }

        [ConfigurationProperty("UseCustomColour", DefaultValue = "false")]
        public bool UseCustomColour
        {
            get
            {
                try
                {
                    return (bool)this["UseCustomColour"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("UseCustomColour");
                    this["UseCustomColour"] = false;
                    return false;
                }
            }
            set => this["UseCustomColour"] = value;
        }

        [ConfigurationProperty("Colour", DefaultValue = "yellow")]
        public Color Colour
        {
            get
            {
                try
                {
                    return (Color)this["Colour"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Colour");
                    this["Colour"] = Color.Yellow;
                    return Color.Yellow;
                }
            }
            set => this["Colour"] = value;
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