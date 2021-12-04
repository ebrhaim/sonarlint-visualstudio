﻿/*
 * SonarLint for Visual Studio
 * Copyright (C) 2016-2021 SonarSource SA
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EnvDTE;
using SonarLint.VisualStudio.Core.Binding;
using SonarLint.VisualStudio.Integration.Binding;
using SonarLint.VisualStudio.Integration.ETW;
using SonarLint.VisualStudio.Integration.Helpers;
using SonarLint.VisualStudio.Integration.NewConnectedMode;

namespace SonarLint.VisualStudio.Integration
{
    internal class UnboundProjectFinder : IUnboundProjectFinder
    {
        private readonly IProjectBinderFactory projectBinderFactory;
        private readonly IConfigurationProviderService configProvider;
        private readonly IProjectSystemHelper projectSystem;
        private readonly ILogger logger;

        public UnboundProjectFinder(IServiceProvider serviceProvider, ILogger logger)
            : this(serviceProvider, logger, new ProjectBinderFactory(serviceProvider, logger))
        {
        }

        internal UnboundProjectFinder(IServiceProvider serviceProvider, ILogger logger, IProjectBinderFactory projectBinderFactory)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.projectBinderFactory = projectBinderFactory ?? throw new ArgumentNullException(nameof(projectBinderFactory));

            configProvider = serviceProvider.GetService<IConfigurationProviderService>();
            projectSystem = serviceProvider.GetService<IProjectSystemHelper>();
        }

        public IEnumerable<Project> GetUnboundProjects()
        {
            CodeMarkers.Instance.UnboundProjectFinderStart();

            logger.LogDebug($"[Binding check] Checking for unbound projects...");

            // Only applicable in connected mode (legacy or new)
            var bindingConfig = configProvider.GetConfiguration();
            logger.LogDebug($"[Binding check] Binding mode: {bindingConfig.Mode}");

            var unbound = bindingConfig.Mode.IsInAConnectedMode() ? GetUnboundProjects(bindingConfig) : Array.Empty<Project>();
            logger.LogDebug($"[Binding check] Number of unbound projects: {unbound.Length}");

            CodeMarkers.Instance.UnboundProjectFinderEnd();
            return unbound;
        }

        private Project[] GetUnboundProjects(BindingConfiguration binding)
        {
            Debug.Assert(binding.Mode.IsInAConnectedMode());
            Debug.Assert(binding.Project != null);

            var filteredSolutionProjects = projectSystem.GetFilteredSolutionProjects().ToArray();
            logger.LogDebug($"[Binding check] Number of bindable projects: {filteredSolutionProjects.Length}");

            var result = filteredSolutionProjects
                .Where(project =>
                {
                    var configProjectBinder = projectBinderFactory.Get(project);

                    var projectName = project.Name;
                    ETW.CodeMarkers.Instance.CheckProjectBindingStart(projectName);

                    logger.LogDebug($"[Binding check] Checking binding for project '{projectName}'. Binder type: {configProjectBinder.GetType().Name}");
                    var required = configProjectBinder.IsBindingRequired(binding, project);
                    logger.LogDebug($"[Binding check] Is binding required: {required} (project: {projectName})");

                    ETW.CodeMarkers.Instance.CheckProjectBindingEnd();

                    return required;
                })
                .ToArray();

            return result;
        }
    }
}
