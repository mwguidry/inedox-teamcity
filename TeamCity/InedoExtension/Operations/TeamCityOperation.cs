﻿using System.ComponentModel;
using Inedo.Extensions.TeamCity.Credentials;
using Inedo.Documentation;
using Inedo.Extensibility;
using Inedo.Extensibility.Credentials;
using Inedo.Extensibility.Operations;

namespace Inedo.Extensions.TeamCity.Operations
{
    public abstract class TeamCityOperation : ExecuteOperation, IHasCredentials<TeamCityCredentials>, ITeamCityConnectionInfo
    {
        public abstract string CredentialName { get; set; }

        [Category("Connection/Identity")]
        [ScriptAlias("Server")]
        [DisplayName("TeamCity server URL")]
        [MappedCredential(nameof(TeamCityCredentials.ServerUrl))]
        public string ServerUrl { get; set; }

        [Category("Connection/Identity")]
        [ScriptAlias("UserName")]
        [DisplayName("User name")]
        [PlaceholderText("Use guest authentication")]
        [MappedCredential(nameof(TeamCityCredentials.UserName))]
        public string UserName { get; set; }

        [Category("Connection/Identity")]
        [ScriptAlias("Password")]
        [DisplayName("Password")]
        [MappedCredential(nameof(TeamCityCredentials.Password))]
        public string Password { get; set; }
    }
}
