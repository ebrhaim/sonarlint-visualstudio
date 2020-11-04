﻿/*
 * SonarLint for Visual Studio
 * Copyright (C) 2016-2020 SonarSource SA
 * mailto:info AT sonarsource DOT com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using SonarLint.VisualStudio.Core;
using SonarLint.VisualStudio.Core.Binding;
using SonarLint.VisualStudio.Core.InfoBar;
using SonarLint.VisualStudio.Integration;
using SonarLint.VisualStudio.IssueVisualization.Security.HotspotsList;

namespace SonarLint.VisualStudio.IssueVisualization.Security.OpenInIDE.Api
{
    internal interface IOpenInIDEStateValidator : IDisposable
    {
        /// <summary>
        /// Checks whether the IDE is in the correct status to handle an "Open in IDE" request
        /// for the specified server and project.
        /// </summary>
        /// <param name="serverUri">The URL of the target SonarQube/SonarCloud server (required)</param>
        /// <param name="projectKey">The key of the target project (required)</param>
        /// <param name="organizationKey">The key of the target organization (null for SonarQube servers, required for SonarCloud)</param>
        /// <returns>True if the IDE is in connected mode with a project bound to the specified server/project/organization, otherwise false</returns>
        /// <remarks>
        /// For an Open in IDE request to succeed:
        /// * a solution must be open
        /// * the solution must be in connected mode, pointing to the correct server/project/organization.
        ///
        /// The validator is responsible for handling any UX notifications if the IDE is not in an appropriate state.
        /// </remarks>
        bool CanHandleOpenInIDERequest(Uri serverUri, string projectKey, string organizationKey);
    }

    [Export(typeof(IOpenInIDEStateValidator))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class OpenInIdeStateValidator : IOpenInIDEStateValidator
    {
        private readonly IInfoBarManager infoBarManager;
        private readonly IConfigurationProvider configurationProvider;
        private readonly IOutputWindowService outputWindowService;
        private readonly ILogger logger;
        private IInfoBar currentInfoBar;

        [ImportingConstructor]
        public OpenInIdeStateValidator(IInfoBarManager infoBarManager, 
            IConfigurationProvider configurationProvider, 
            IOutputWindowService outputWindowService,
            ILogger logger)
        {
            this.infoBarManager = infoBarManager;
            this.configurationProvider = configurationProvider;
            this.outputWindowService = outputWindowService;
            this.logger = logger;
        }

        public bool CanHandleOpenInIDERequest(Uri serverUri, string projectKey, string organizationKey)
        {
            RemoveExistingInfoBar();

            var failureMessage = GetFailureMessage(serverUri, projectKey, organizationKey);

            if (string.IsNullOrEmpty(failureMessage))
            {
                return true;
            }

            AddInfoBar();
            logger.WriteLine(failureMessage);

            return false;
        }

        private string GetFailureMessage(Uri serverUri, string projectKey, string organizationKey)
        {
            var configuration = configurationProvider.GetConfiguration();
            var isRequestForSonarCloud = !string.IsNullOrEmpty(organizationKey);

            var instructions = isRequestForSonarCloud
                ? string.Format(OpenInIDEResources.RequestValidator_Instructions_SonarCloud, serverUri, organizationKey, projectKey)
                : string.Format(OpenInIDEResources.RequestValidator_Instructions_SonarQube, serverUri, projectKey);

            if (configuration.Mode == SonarLintMode.Standalone)
            {
                return string.Format(OpenInIDEResources.RequestValidator_InvalidState_NotInConnectedMode, instructions);
            }

            if (IsCorrectServer() && IsCorrectOrganization() && IsCorrectSonarProject())
            {
                return null;
            }

            var currentConfiguration = isRequestForSonarCloud
                ? string.Format(OpenInIDEResources.RequestValidator_CurrentState_SonarCloud,
                    configuration.Project.ServerUri,
                    configuration.Project.Organization?.Key,
                    configuration.Project.ProjectKey)
                : string.Format(OpenInIDEResources.RequestValidator_CurrentState_SonarQube,
                    configuration.Project.ServerUri,
                    configuration.Project.ProjectKey);

            return string.Format(OpenInIDEResources.RequestValidator_InvalidState_WrongConnection, instructions, currentConfiguration);

            bool IsCorrectServer()
            {
                return serverUri.Equals(configuration.Project.ServerUri);
            }

            bool IsCorrectOrganization()
            {
                return string.IsNullOrEmpty(organizationKey) ||
                       organizationKey.Equals(configuration.Project.Organization?.Key, StringComparison.OrdinalIgnoreCase);
            }

            bool IsCorrectSonarProject()
            {
                return projectKey.Equals(configuration.Project.ProjectKey, StringComparison.OrdinalIgnoreCase);
            }
        }

        private void AddInfoBar()
        {
            currentInfoBar = infoBarManager.AttachInfoBarWithButton(new Guid(HotspotsToolWindow.ToolWindowId), OpenInIDEResources.RequestValidator_InfoBarMessage, "Show Output Window", default);
            Debug.Assert(currentInfoBar != null, "currentInfoBar != null");
            
            currentInfoBar.ButtonClick += ShowOutputWindow;
            currentInfoBar.Closed += CurrentInfoBar_Closed;
        }

        private void ShowOutputWindow(object sender, EventArgs e)
        {
            outputWindowService.Show();
        }

        private void RemoveExistingInfoBar()
        {
            if (currentInfoBar != null)
            {
                currentInfoBar.Closed -= CurrentInfoBar_Closed;
                infoBarManager.DetachInfoBar(currentInfoBar);
                currentInfoBar = null;
            }
        }

        private void CurrentInfoBar_Closed(object sender, EventArgs e)
        {
            RemoveExistingInfoBar();
        }

        public void Dispose()
        {
            RemoveExistingInfoBar();
        }
    }
}