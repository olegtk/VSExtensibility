﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace VSProjectQueryAPISample;

using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using Microsoft.VisualStudio.ProjectSystem.Query;

/// <summary>
/// A sample command for reloading a project.
/// </summary>
[VisualStudioContribution]
public class ReloadProjectCommand : Command
{
    /// <inheritdoc />
    public override CommandConfiguration CommandConfiguration => new("%VSProjectQueryAPISample.ReloadProjectCommand.DisplayName%")
    {
        Placements = new[] { CommandPlacement.KnownPlacements.ToolsMenu },
        Icon = new(ImageMoniker.KnownValues.Extension, IconSettings.IconAndText),
    };

    /// <inheritdoc />
    public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
    {
        WorkspacesExtensibility querySpace = this.Extensibility.Workspaces();

        const string solutionName = "ConsoleApp32";
        const string projectPath = "ConsoleApp1\\\\ConsoleApp1.csproj";

        await querySpace.UpdateSolutionAsync(
            solution => solution.Where(solution => solution.BaseName == solutionName),
            solution => solution.ReloadProject(projectPath),
            cancellationToken);

        await this.Extensibility.Shell().ShowPromptAsync($"Reloaded a project called {projectPath} in solution {solutionName}.", PromptOptions.OK, cancellationToken);
    }
}
