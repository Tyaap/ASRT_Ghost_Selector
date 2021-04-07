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
            string message = "Found an unknown value for the following option while reading the config file:\n" + option;
            Message(message);
        }

        public static void UnknownElementMessage(string option)
        {
            string message = "Found an unknown element while reading the config file:\n" + option;
            Message(message);
        }

        public static void UnknownAttributeMessage(string option)
        {
            string message = "Found an unknown attribute while reading the config file:\n" + option;
            Message(message);
        }

        public static void MissingPropertymessage(string option)
        {
            string message = "Could not find a required property while reading the config file:\n" + option;
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
        [ConfigurationProperty("GhostSelectors")]
        public GhostSelectorsElement GhostSelectors
        {
            get
            {
                if (base["GhostSelectors"] is GhostSelectorsElement tmp)
                    return tmp;
                else
                    return new GhostSelectorsElement();
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

        [ConfigurationProperty("GhostSaver")]
        public GhostSaverElement GhostSaver
        {
            get
            {
                if (base["GhostSaver"] is GhostSaverElement tmp)
                    return tmp;
                else
                    return new GhostSaverElement();
            }
        }
    }

    public class GhostSelectorsElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Choice", DefaultValue = "Default")]
        public GhostSelector Choice
        {
            get
            {
                try
                {
                    return (GhostSelector)this["Choice"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Choice");
                    this["Choice"] = true;
                    return GhostSelector.Default;
                }
            }
            set => this["Choice"] = value;
        }

        [ConfigurationProperty("LeaderboardRank")]
        public LeaderboardRankElement LeaderboardRank
        {
            get
            {
                if (base["LeaderboardRank"] is LeaderboardRankElement tmp)
                    return tmp;
                else
                    return new LeaderboardRankElement();
            }
        }

        [ConfigurationProperty("FastestPlayer")]
        [ConfigurationCollection(typeof(PlayerElement), AddItemName = "Player", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public GenericConfigurationElementCollection<PlayerElement> FastestPlayer
        {
            get
            {
                if (base["FastestPlayer"] is GenericConfigurationElementCollection<PlayerElement> tmp)
                    return tmp;
                else
                    return new GenericConfigurationElementCollection<PlayerElement>();
            }
        }

        [ConfigurationProperty("FromFile")]
        public FromFileElement FromFile
        {
            get
            {
                if (base["FromFile"] is FromFileElement tmp)
                    return tmp;
                else
                    return new FromFileElement();
            }
        }
    }

    public class LeaderboardRankElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Rank", DefaultValue = "0")]
        public uint Rank
        {
            get
            {
                try
                {
                    return (uint)this["Rank"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Rank");
                    this["Rank"] = 0;
                    return 0;
                }
            }
            set => this["Rank"] = value;
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

    public class FromFileElement : ConfigurationElementEx
    {
        [ConfigurationProperty("NameTag", DefaultValue = "Ghost")]
        public string NameTag
        {
            get
            {
                try
                {
                    return (string)this["NameTag"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("NameTag");
                    this["NameTag"] = "Rival";
                    return "Rival";
                }
            }
            set => this["NameTag"] = value;
        }

        [ConfigurationProperty("File", DefaultValue = "")]
        public string File
        {
            get
            {
                try
                {
                    return (string)this["File"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("File");
                    this["File"] = "";
                    return "";
                }
            }
            set => this["File"] = value;
        }
    }

    public class GraphicsElement : ConfigurationElementEx
    {
        [ConfigurationProperty("NameTag")]
        public NameTagElement Nametag
        {
            get
            {
                if (base["NameTag"] is NameTagElement tmp)
                    return tmp;
                else
                    return new NameTagElement();
            }
        }

        [ConfigurationProperty("PBGhost")]
        public GhostAppearanceElement PBGhost
        {
            get
            {
                if (base["PBGhost"] is GhostAppearanceElement tmp)
                    return tmp;
                else
                    return new GhostAppearanceElement();
            }
        }

        [ConfigurationProperty("RivalGhost")]
        public GhostAppearanceElement RivalGhost
        {
            get
            {
                if (base["RivalGhost"] is GhostAppearanceElement tmp)
                    return tmp;
                else
                    return new GhostAppearanceElement();
            }
        }
    }

    public class NameTagElement : ConfigurationElementEx
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

    public class GhostAppearanceElement : ConfigurationElementEx
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

        [ConfigurationProperty("ChangeColour", DefaultValue = "false")]
        public bool ChangeColour
        {
            get
            {
                try
                {
                    return (bool)this["ChangeColour"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("ChangeColour");
                    this["ChangeColour"] = false;
                    return false;
                }
            }
            set => this["ChangeColour"] = value;
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

    public class GhostSaverElement : ConfigurationElementEx
    {
        [ConfigurationProperty("Enabled", DefaultValue = "false")]
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
                    ErrorMessage.UnknownValueMessage("Enabed");
                    this["Enabled"] = false;
                    return false;
                }
            }
            set => this["Enabled"] = value;
        }

        [ConfigurationProperty("Folder", DefaultValue = "")]
        public string Folder
        {
            get
            {
                try
                {
                    return (string)this["Folder"];
                }
                catch
                {
                    ErrorMessage.UnknownValueMessage("Folder");
                    this["Folder"] = "";
                    return "";
                }
            }
            set => this["Folder"] = value;
        }
    }
}