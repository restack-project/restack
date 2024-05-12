using Blace.Editing;
using Microsoft.AspNetCore.Identity;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using System.Xml.Linq;

namespace ReStack.Web.Pages.Stacks.Models
{
    public class StackFile : EditorFile
    {
        public IStackClient Client { get; }
        public int StackId { get; set; }

        // todo fix name
        public StackFile(IStackClient client, int stackId) : base("run.sh")
        {
            Client = client;
            StackId = stackId;
        }

        protected override async Task<string> LoadContent()
        {
            var text = await Client.DownloadFile(StackId);
            return text;
        }

        protected override async Task<bool> SaveContent()
        {
            await Client.UploadFile(StackId, Content);
            return true;
        }
    }
}
