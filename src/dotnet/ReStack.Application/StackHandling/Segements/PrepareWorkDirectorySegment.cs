using Microsoft.Extensions.Options;
using ReStack.Common.Helpers;
using ReStack.Common.Interfaces;
using ReStack.Common.Interfaces.Repositories;
using ReStack.Domain.Entities;
using ReStack.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReStack.Application.StackHandling.Segements
{
    public class PrepareWorkDirectorySegment : BaseSegment
    {
        private readonly ApiSettings _settings;

        public PrepareWorkDirectorySegment(
            INotificationPublisher publisher
            , ILogRepository logRepository
            , IOptions<ApiSettings> options
        ) : base(publisher, logRepository)
        {
            _settings = options.Value;
        }

        public override Task<PipelineContext> Run(PipelineContext context, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(context.WorkDirectory);
            
            CopyComponents(context);
            CopyKey(context);

            return Task.FromResult(context);
        }

        private void CopyKey(PipelineContext context)
        {
            var componentDirectory = Path.Combine(context.WorkDirectory, "keys");
            Directory.CreateDirectory(componentDirectory);

            var keyToPaste = Path.Combine(context.WorkDirectory, "keys", "id_rsa");
            var keyToCopy = Path.Combine(_settings.KeysStorage, "id_rsa");
            File.Copy(keyToCopy, keyToPaste);

            if (!OperatingSystem.IsWindows())
                File.SetUnixFileMode(keyToPaste, UnixFileMode.UserRead | UnixFileMode.UserWrite);
        }

        private void CopyComponents(PipelineContext context)
        {
            var stackToPaste = Path.Combine(context.WorkDirectory, context.Stack.GetFileName());
            File.Copy(context.Stack.GetLocation(_settings), stackToPaste);

            foreach (var component in context.Stack.Components.Select(x => x.Component))
            {
                var componentDirectory = Path.Combine(context.WorkDirectory, component.ComponentLibrary.Slug, "components");
                Directory.CreateDirectory(componentDirectory);

                var componentFolderToPaste = Path.Combine(componentDirectory, component.FolderName);
                Directory.CreateDirectory(componentFolderToPaste);
                DirectoryHelper.CopyFilesRecursively(component.GetLocation(_settings), componentFolderToPaste, except: "metadata.json");
            }
        }
    }
}
