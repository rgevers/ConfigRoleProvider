# ConfigRoleProvider
.Net Role provider that utilizes web.config entries to manage ad-group to role mapping.

Sample Configuration Entry

```xml
	<roleManager defaultProvider="ConfigRoleProvider"
		enabled="true"
		cacheRolesInCookie="true"
		cookieName=".ASPXROLES"
		cookieTimeout="30"
		cookiePath="/"
		cookieRequireSSL="false"
		cookieSlidingExpiration="true"
		cookieProtection="All" >
			<providers>
				<clear />
				<add  name="ConfigRoleProvider"  type="ConfigurationRoleProvider"/>
			</providers>
		</roleManager>
	</system.web>

	<!-- Define Role Structure -->
	<roleConfigurationSection name="DevelopmentRoles">
		<roles>
			<role name="Administrator">
				<groups>
					<group name="Main\Rob"/>
				</groups>
			</role>
			<role name="Members">
				<groups>
					<group name="Main\Rob"/>
					<group name="Main\SomeBodyElse"/>
				</groups>
			</role>
			<role name="OtherGuys">
				<groups>
					<group name="Main\SomeBodyElse"/>
					<group name="Main\AnotherOtherPerson"/>
				</groups>
			</role>
			<role name="NoGuys">
				<groups>
				</groups>
			</role>
		</roles>
	</roleConfigurationSection>
```

