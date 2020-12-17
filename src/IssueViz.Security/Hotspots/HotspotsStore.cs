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
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using SonarLint.VisualStudio.IssueVisualization.Models;
using SonarLint.VisualStudio.IssueVisualization.Security.Hotspots.Models;

namespace SonarLint.VisualStudio.IssueVisualization.Security.Hotspots
{
    internal interface IHotspotsStore : IDisposable
    {
        ReadOnlyObservableCollection<IAnalysisIssueVisualization> GetAll();

        /// <summary>
        /// Adds a given visualization to the list if it does not already exist.
        /// Returns the given visualization, or the existing visualization with the same hotspot key.
        /// </summary>
        IAnalysisIssueVisualization GetOrAdd(IAnalysisIssueVisualization hotspotViz);

        void Remove(IAnalysisIssueVisualization hotspotViz);
    }

    [Export(typeof(IHotspotsStore))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal sealed class HotspotsStore : IHotspotsStore
    {
        private readonly ObservableCollection<IAnalysisIssueVisualization> hotspots;
        private readonly ReadOnlyObservableCollection<IAnalysisIssueVisualization> readOnlyWrapper;
        private readonly IDisposable unregisterCallback;

        [ImportingConstructor]
        public HotspotsStore(IIssueStoreObserver issueStoreObserver)
        {
            hotspots = new ObservableCollection<IAnalysisIssueVisualization>();
            readOnlyWrapper = new ReadOnlyObservableCollection<IAnalysisIssueVisualization>(hotspots);
            unregisterCallback = issueStoreObserver.Register(readOnlyWrapper);
        }

        public IAnalysisIssueVisualization GetOrAdd(IAnalysisIssueVisualization hotspotViz)
        {
            var existingHotspot = FindExisting(hotspotViz);

            if (existingHotspot != null)
            {
                return existingHotspot;
            }

            hotspots.Add(hotspotViz);

            return hotspotViz;
        }

        public void Remove(IAnalysisIssueVisualization hotspotViz)
        {
            hotspots.Remove(hotspotViz);
        }

        private IAnalysisIssueVisualization FindExisting(IAnalysisIssueVisualization hotspotViz)
        {
            var key = ((IHotspot)hotspotViz.Issue).HotspotKey;

            return hotspots.FirstOrDefault(x => ((IHotspot)x.Issue).HotspotKey == key);
        }

        public ReadOnlyObservableCollection<IAnalysisIssueVisualization> GetAll() => readOnlyWrapper;

        public void Dispose()
        {
            unregisterCallback.Dispose();
        }
    }
}