using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Framework
{
	class ConfigurationRoleProvider : RoleProvider
	{

		private WindowsTokenRoleProvider _windowsProvider;
		private IDictionary<string, IList<string>> _roleMapping;

		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
		{
			base.Initialize(name, config);
			_windowsProvider = new WindowsTokenRoleProvider();
			_roleMapping = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
			BuildRoleMapping();
		}

		private void BuildRoleMapping()
		{
			RoleConfigurationSection configurationSection = ConfigurationManager.GetSection("roleConfigurationSection") as RoleConfigurationSection;
			if (configurationSection == null)
			{
				throw new ProviderException();
			}

			RoleCollection roles = configurationSection.Roles;

			foreach (RoleElement role in roles)
			{
				if (!_roleMapping.ContainsKey(role.Name))
				{
					_roleMapping.Add(role.Name, new List<string>());
				}
				foreach (AdGroupElement group in role.Groups)
				{
					if (!_roleMapping[role.Name].Contains(group.Name))
					{
						_roleMapping[role.Name].Add(group.Name);
					}
				}
			}
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(username, string.Empty) || StringComparer.OrdinalIgnoreCase.Equals(roleName, string.Empty))
			{
				throw new ArgumentException();
			}

			if (username == null || roleName == null)
			{
				throw new ArgumentNullException();
			}

			if (!_roleMapping.ContainsKey(roleName))
			{
				throw new ProviderException();
			}
			//Call should throw provider exception if user does not exist at all handling this case for this provider
			return _roleMapping[roleName].Any(groupName => _windowsProvider.IsUserInRole(username, groupName));
		}

		public override string[] GetRolesForUser(string username)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(username, string.Empty))
			{
				throw new ArgumentException();
			}

			if (username == null)
			{
				throw new ArgumentNullException();
			}

			return _roleMapping.Keys.Where(roleName => IsUserInRole(username, roleName)).ToArray();
		}

		public override bool RoleExists(string roleName)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(roleName, string.Empty))
			{
				throw new ArgumentException();
			}

			if (roleName == null)
			{
				throw new ArgumentNullException();
			}

			return _roleMapping.ContainsKey(roleName);
		}

		public override string[] GetUsersInRole(string roleName)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(roleName, string.Empty) || roleName.Contains(','))
			{
				throw new ArgumentException();
			}

			if (roleName == null)
			{
				throw new ArgumentNullException();
			}

			if (!_roleMapping.ContainsKey(roleName))
			{
				throw new ProviderException();
			}

			IEnumerable<string> users = new List<string>();
			if (_roleMapping.ContainsKey(roleName))
			{
				users = _roleMapping[roleName].Aggregate(users, (current, groupName) => current.Union(_windowsProvider.GetUsersInRole(groupName)));
			}
			return users.ToArray();
		}

		public override string[] GetAllRoles()
		{
			return _roleMapping.Keys.ToArray();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(usernameToMatch, string.Empty) || StringComparer.OrdinalIgnoreCase.Equals(roleName, string.Empty))
			{
				throw new ArgumentException();
			}

			if (usernameToMatch == null || roleName == null)
			{
				throw new ArgumentNullException();
			}

			if (!_roleMapping.ContainsKey(roleName))
			{
				throw new ProviderException();
			}

			IEnumerable<string> users = new List<string>();
			users = _roleMapping[roleName].Aggregate(users, (current, groupName) => current.Union(_windowsProvider.FindUsersInRole(groupName, usernameToMatch)));
			return users.ToArray();
		}

		public override void CreateRole(string roleName)
		{
			throw new InvalidOperationException("This operation is not supported by the role provider implementation.");
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new InvalidOperationException("This operation is not supported by the role provider implementation.");
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new InvalidOperationException("This operation is not supported by the role provider implementation.");
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new InvalidOperationException("This operation is not supported by the role provider implementation.");
		}

		//Web.Config is application specific
		public override string ApplicationName { get; set; }
	}
}
