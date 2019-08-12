using System.Configuration;

namespace GhostSelector
{
    public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection where T : ConfigurationElement, new()
    {
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


    public class ProgramConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("PositionSelector", IsRequired = true)]
        public PositionSelectorElement PositionSelector
        {
            get { return (PositionSelectorElement)base["PositionSelector"]; }
            set { base["PositionSelector"] = value; }
        }

        [ConfigurationProperty("FastestPlayerSelector", IsRequired = true)]
        public FastestPlayerSelectorElement FastestPlayerSelector
        {
            get { return (FastestPlayerSelectorElement)base["FastestPlayerSelector"]; }
        }

        [ConfigurationProperty("Graphics", IsRequired = true)]
        public GraphicsElement Graphics
        {
            get { return (GraphicsElement)base["Graphics"]; }
        }
    }

    public class GraphicsElement : ConfigurationElement
    {

        [ConfigurationProperty("HideNameTags", IsRequired = true, DefaultValue = false)]
        public bool HideNameTags
        {
            get { return (bool)this["HideNameTags"]; }
            set { this["HideNameTags"] = value; }
        }

        [ConfigurationProperty("HideGhostCars", IsRequired = true, DefaultValue = false)]
        public bool HideGhostCars
        {
            get { return (bool)this["HideGhostCars"]; }
            set { this["HideGhostCars"] = value; }
        }

        [ConfigurationProperty("HidePBGhost", IsRequired = true, DefaultValue = false)]
        public bool HidePBGhost
        {
            get { return (bool)this["HidePBGhost"]; }
            set { this["HidePBGhost"] = value; }
        }
    }

    public class FastestPlayerSelectorElement : ConfigurationElement
    {
        [ConfigurationProperty("Enabled", IsRequired = true, DefaultValue = false)]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        [ConfigurationProperty("Players", IsRequired = true)]
        [ConfigurationCollection(typeof(PlayerElement), AddItemName = "Entry", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public GenericConfigurationElementCollection<PlayerElement> Players
        {
            get { return (GenericConfigurationElementCollection<PlayerElement>)base["Players"]; }
        }

    }
    public class PlayerElement : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("SteamId", DefaultValue = (long)0)]
        public long SteamId
        {
            get { return (long)this["SteamId"]; }
            set { this["SteamId"] = value; }
        }

        [ConfigurationProperty("Enabled")]
        public bool Enabled
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }
    }

    public class PositionSelectorElement : ConfigurationElement
    {
        [ConfigurationProperty("SelectedPosition", IsRequired = true, DefaultValue = (uint)0)]
        public uint SelectedPosition
        {
            get { return (uint)this["SelectedPosition"]; }
            set { this["SelectedPosition"] = value; }
        }
    }
}