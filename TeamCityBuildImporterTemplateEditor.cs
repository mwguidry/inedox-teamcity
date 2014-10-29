﻿using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.BuildImporters;
using Inedo.BuildMaster.Web.Controls.Extensions;
using Inedo.BuildMaster.Web.Controls.Extensions.BuildImporters;
using Inedo.Web.Controls;
using Inedo.Web.Controls.SimpleHtml;

namespace Inedo.BuildMasterExtensions.TeamCity
{
    internal sealed class TeamCityBuildImporterTemplateEditor : BuildImporterTemplateEditorBase
    {
        private ValidatingTextBox txtArtifactName;
        private CheckBox chkArtifactNameLocked;
        private SelectBuildConfigurationPicker ddlBuildConfigurationId;
        private ComboSelect ddlBuildNumber;

        public TeamCityBuildImporterTemplateEditor()
        {
            this.ValidateBeforeSave += TeamCityBuildImporterTemplateEditor_ValidateBeforeSave;
        }

        private void TeamCityBuildImporterTemplateEditor_ValidateBeforeSave(object sender, ValidationEventArgs<BuildImporterTemplateBase> e)
        {
            var template = (TeamCityBuildImporterTemplate)e.Extension;
            if (string.IsNullOrWhiteSpace(template.BuildConfigurationId))
            {
                e.Message = "A build configuration is required";
                e.ValidLevel = ValidationLevel.Error;
            }
        }

        public override void BindToForm(BuildImporterTemplateBase extension)
        {
            var template = (TeamCityBuildImporterTemplate)extension;

            this.txtArtifactName.Text = template.ArtifactName;
            this.ddlBuildConfigurationId.SelectedValue = template.BuildConfigurationId;
            this.chkArtifactNameLocked.Checked = !template.ArtifactNameLocked;
            this.ddlBuildNumber.SelectedValue = template.BuildNumber;
        }

        public override BuildImporterTemplateBase CreateFromForm()
        {
            return new TeamCityBuildImporterTemplate
            {
                ArtifactName = this.txtArtifactName.Text,
                ArtifactNameLocked = !this.chkArtifactNameLocked.Checked,
                BuildConfigurationId = this.ddlBuildConfigurationId.SelectedValue,
                BuildConfigurationDisplayName = this.ddlBuildConfigurationId.SelectedItem != null ? this.ddlBuildConfigurationId.SelectedItem.Text : string.Empty,
                BuildNumber = this.ddlBuildNumber.SelectedValue
            };
        }

        protected override void CreateChildControls()
        {
            this.txtArtifactName = new ValidatingTextBox { Required = true };
            this.chkArtifactNameLocked = new CheckBox { Text = "Allow selection at build time" };

            this.ddlBuildConfigurationId = new SelectBuildConfigurationPicker() { ID = "ddlBuildConfigurationId" };
            this.ddlBuildConfigurationId.Init +=
                (s, e) =>
                {
                    this.ddlBuildConfigurationId.FillItems(TeamCityConfigurer.GetConfigurer());
                };

            this.ddlBuildNumber = new ComboSelect();
            this.ddlBuildNumber.Items.Add(new ListItem("Select at build import time", ""));
            this.ddlBuildNumber.Items.Add(new ListItem("Always use last successful build", "lastSuccessful"));
            this.ddlBuildNumber.Items.Add(new ListItem("Always use last finished build", "lastFinished"));
            this.ddlBuildNumber.Items.Add(new ListItem("Always use last pinned build", "lastPinned"));

            this.Controls.Add(
                new SlimFormField("Build configuration:", this.ddlBuildConfigurationId),
                new SlimFormField("TeamCity build number:", this.ddlBuildNumber),
                new SlimFormField("Artifact name:", new Div(this.txtArtifactName), new Div(this.chkArtifactNameLocked))
            );
        }
    }
}
