using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;

namespace Framework
{
	public class RoleConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get
			{
				return (string)this["name"];
			}
			set
			{
				this["name"] = value;
			}
		}

		[ConfigurationProperty("roles", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(RoleCollection), AddItemName = "role", ClearItemsName = "clear", RemoveItemName = "remove")]
		public RoleCollection Roles
		{
			get { return (RoleCollection)this["roles"]; }
			set { this["roles"] = value; }
		}
	}

	public class RoleCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new RoleElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((RoleElement)element).Name;
		}
	}

	public class RoleElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}

		[ConfigurationProperty("groups", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(AdGroupElement), AddItemName = "group", ClearItemsName = "clear", RemoveItemName = "remove")]
		public AdGroupCollection Groups
		{
			get{ return (AdGroupCollection)this["groups"]; }
			set { this["groups"] = value; }
		}
	}

	public class AdGroupCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new AdGroupElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AdGroupElement)element).Name;
		}
	}

	public class AdGroupElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get { return (string)this["name"]; }
			set { this["name"] = value; }
		}
	}

}
