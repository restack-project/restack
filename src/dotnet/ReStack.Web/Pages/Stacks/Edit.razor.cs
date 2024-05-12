using Blace.Editing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ReStack.Common.Exceptions;
using ReStack.Common.Interfaces.Clients;
using ReStack.Common.Models;
using ReStack.Domain.ValueObjects;
using ReStack.Web.Extensions;
using ReStack.Web.Modals;
using ReStack.Web.Pages.Stacks.Models;

namespace ReStack.Web.Pages.Stacks;

public partial class Edit
{
    private StackFile _file;
    private bool _fileLoaded;
    private ValidationMessageStore _messageStore;

    [Inject] public IStackClient StackClient { get; set; }
    [Inject] public IComponentLibraryClient ComponentLibraryClient { get; set; }

    [Parameter] public string QueryStackId { get; set; }
    [SupplyParameterFromQuery(Name = "showEditor")][Parameter] public string QueryShowEditor { get; set; }

    public StackModel Stack { get; set; }
    public bool ShowEditor { get; set; } = true;
    public List<ComponentLibraryModel> ComponentLibraries { get; private set; }
    public Blace.Components.Editor<StackFile> Editor { get; set; }
    public EditContext EditContext { get; set; }
    public List<string> Validations { get; private set; } = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            await SetLoading(true);

            BreadcrumbLinks = new() { { "Stacks", "/stacks" } };

            if (bool.TryParse(QueryShowEditor, out var showEditor))
            {
                ShowEditor = showEditor;
            }

            if (!string.IsNullOrWhiteSpace(QueryStackId))
            {
                if (int.TryParse(QueryStackId, out var stackId))
                {
                    Stack = await StackClient.Get(stackId);

                    BreadcrumbLinks.Add(Stack.Name, $"stacks/{Stack.Id}/");
                    BreadcrumbLinks.Add("Edit", $"stacks/{Stack.Id}/edit");
                }
                else
                {
                    LoadError = true;
                }
            }
            else
            {
                Stack = new()
                {
                    Type = ProgrammingLanguage.Shell.ToString(),
                    FailOnStdError = true
                };

                ShowEditor = false;

                BreadcrumbLinks.Add("Add", $"stacks/add");
            }

            ComponentLibraries = await ComponentLibraryClient.GetAll();
            EditContext = new(Stack);
            _messageStore = new(EditContext);

            // TODO should be done another way
            foreach (var component in Stack.Components)
            {
                var library = ComponentLibraries.FirstOrDefault(x => x.Id == component.ComponentLibraryId);
                library.InUse = true;
            }
        }
        catch (Exception ex)
        {
            await ShowError(ex);
        }
        finally
        {
            await SetLoading(false);
        }

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_fileLoaded && Editor is not null && Stack.Id != 0)
        {
            _fileLoaded = true;

            try
            {
                await SetLoading(true);

                _file = new StackFile(StackClient, Stack.Id);

                await Editor.Open(_file, new() { Theme = Theme.Clouds_Midnight, Syntax = Syntax.Sh });
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                await SetLoading(false);
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task<bool> Save(bool hideFinalLoader = true)
    {
        try
        {
            var validated = Validate();

            if (!validated)
                return false;

            await SetLoading(true);

            // TODO should be done another way
            Stack.Components = ComponentLibraries.Where(x => x.InUse).SelectMany(x => x.Components).ToList();

            if (Stack.Id == 0)
            {
                Stack = await StackClient.Add(Stack);

                BreadcrumbLinks.Remove("Add");
                BreadcrumbLinks.Add(Stack.Name, $"stacks/{Stack.Id}/");
                BreadcrumbLinks.Add("Edit", $"stacks/{Stack.Id}/edit");

                await JS.UpdateUrl($"{NavigationManager.BaseUri}stacks/{Stack.Id}/edit");

                ShowEditor = true;
            }
            else
            {
                Stack = await StackClient.Update(Stack);
                await Editor.Save();
            }

            return true;
        }
        catch (ModelValidationException ex)
        {
            Validations = ex.Validations.SelectMany(x => x.Value).ToList();

            return false;
        }
        catch (Exception ex)
        {
            await ShowError(ex);

            return false;
        }
        finally
        {
            if (hideFinalLoader)
            {
                await SetLoading(false);
            }
        }
    }

    private async Task SaveAndExecute()
    {
        if (await Save(hideFinalLoader: false))
        {
            try
            {
                await SetLoading(true);

                var job = await StackClient.Execute(Stack.Id);

                NavigationManager.NavigateToJobDetail(Stack.Id, job.Id, forceLoad: true);
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                await SetLoading(false);
            }
        }
    }

    private async Task Delete()
    {
        var answer = await Modal.Question($"Delete '{Stack.Name}'?", $"This will delete all jobs & files related to the stack. This cannot be undone.");

        if (answer == QuestionResult.Yes)
        {
            try
            {
                await SetLoading(true);

                await StackClient.Delete(Stack.Id);

                NavigationManager.NavigateToHome();
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                await SetLoading(false);
            }
        }
    }

    private async Task ToggleEditor()
    {
        ShowEditor = !ShowEditor;

        await JS.UpdateUrl($"{NavigationManager.BaseUri}stacks/{Stack.Id}/edit?showEditor={ShowEditor}");

        await StateHasChangedAsync();
    }

    private bool Validate()
    {
        Validations.Clear();

        _messageStore.Clear();

        if (string.IsNullOrWhiteSpace(Stack.Name))
            _messageStore.Add(() => Stack.Name, "Name cannot be empty");

        if (string.IsNullOrWhiteSpace(Stack.Type))
            _messageStore.Add(() => Stack.Type, "Type cannot be empty");

        return EditContext.Validate();
    }
}
