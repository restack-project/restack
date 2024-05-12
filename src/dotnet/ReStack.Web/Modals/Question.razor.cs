using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace ReStack.Web.Modals;

public partial class Question
{
    [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

    [Parameter] public string Title { get; set; }
    [Parameter] public string Body { get; set; }
    [Parameter] public EventCallback<QuestionResult> Answer { get; set; }

    protected Task AnswerYes() => InvokeAnswer(QuestionResult.Yes);
    protected Task AnswerNo() => InvokeAnswer(QuestionResult.No);
    protected Task AnswerCancel() => InvokeAnswer(QuestionResult.Cancel);

    private async Task InvokeAnswer(QuestionResult answer)
    {
        await Answer.InvokeAsync(answer);
        await BlazoredModal.CloseAsync(ModalResult.Ok(answer));
    }
}

public enum QuestionResult
{
    Yes, No, Cancel
}