/*
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

using System.Diagnostics.Tracing;

namespace SonarLint.VisualStudio.Core.ETW
{
    [EventSource(Name = "sonarlint")]
    public sealed class Events : EventSource
    {
        public static readonly Events Instance = new Events();

        public static class Keywords
        {
            // Must be powers of 2 so they can be combined bitwise
            public const EventKeywords General = (EventKeywords)1;
            public const EventKeywords CFamily = (EventKeywords)2;
            public const EventKeywords Binding = (EventKeywords)4;
        }

        #region General 0-999

        private const int ProcessRunnerStartId = 1001;
        private const int ProcessRunnerEndId = 1001;

        [Event(ProcessRunnerStartId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void ProcessRunnerStart(int processId)
        {
            if (IsEnabled())
            {
                WriteEvent(ProcessRunnerStartId, processId);
            }
        }

        [Event(ProcessRunnerEndId, Level = EventLevel.Informational, Keywords = Keywords.General)]
        public void ProcessRunnerEnd(int processId, long elapsedMilliseconds)
        {
            if (IsEnabled())
            {
                WriteEvent(ProcessRunnerEndId, processId, elapsedMilliseconds);
            }
        }

        #endregion

        #region CFamily: 1000-1999

        private const int PchProcessingStartId = 1001;
        private const int PchProcessingEndId = 1002;
        private const int PchProcessingTriggeredId = 1003;

        private const int CFamilyGetBindingConfigStartId = 1004;
        private const int CFmailyGetBindingConfigEndId = 1005;

        [Event(PchProcessingStartId, Level = EventLevel.Informational, Keywords = Keywords.CFamily)]
        public void PchProcessingStart() => Write(PchProcessingStartId);

        [Event(PchProcessingEndId, Level = EventLevel.Informational, Keywords = Keywords.CFamily)]
        public void PchProcessingEnd() => Write(PchProcessingEndId);

        [Event(PchProcessingTriggeredId, Level = EventLevel.Informational, Keywords = Keywords.CFamily)]
        public void PchProcessingTriggered() => Write(PchProcessingTriggeredId);


        [Event(CFamilyGetBindingConfigStartId, Level = EventLevel.Informational, Keywords = Keywords.CFamily | Keywords.Binding)]
        public void CFamilyGetBindingConfigStart() => Write(CFamilyGetBindingConfigStartId);

        [Event(CFmailyGetBindingConfigEndId, Level = EventLevel.Informational, Keywords = Keywords.CFamily | Keywords.Binding)]
        public void CFamilyGetBindingConfigEnd() => Write(CFmailyGetBindingConfigEndId);

        #endregion


        #region Binding: 2000-2999

        private const int GetBindingConfigStartId = 2001;
        private const int GetBindingConfigEndId = 2002;

        private const int UnboundProjectsFinderStartId = 2003;
        private const int UnboundProjectsFinderEndId = 2004;

        private const int ProjectBinderStartId = 2005;
        private const int ProjectBinderEndId = 2006;

        [Event(GetBindingConfigStartId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void GetBindingConfigStart() => Write(UnboundProjectsFinderStartId);

        [Event(GetBindingConfigEndId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void GetBindingConfigEnd() => Write(UnboundProjectsFinderEndId);

        [Event(UnboundProjectsFinderStartId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void UnboundProjectFinderStart() => Write(UnboundProjectsFinderStartId);

        [Event(UnboundProjectsFinderEndId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void UnboundProjectFinderEnd() => Write(UnboundProjectsFinderEndId);

        [Event(ProjectBinderStartId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void ProjectBinderStart(string projectName)
        {
            if (IsEnabled())
            {
                WriteEvent(ProjectBinderStartId, projectName);
            }
        }

        [Event(ProjectBinderEndId, Level = EventLevel.Informational, Keywords = Keywords.Binding)]
        public void ProjectBinderEnd() => Write(ProjectBinderEndId);

        #endregion

        private void Write(int id)
        {
            if (IsEnabled())
            {
                WriteEvent(id);
            }
        }
    }
}
