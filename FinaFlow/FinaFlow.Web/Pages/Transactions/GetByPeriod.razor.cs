using FinaFlow.Core.Handlers;
using FinaFlow.Core.Models;
using FinaFlow.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinaFlow.Web.Pages.Transactions;

public partial class GetTransactionsByPeriodPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Transaction> Transactions { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IDialogService Dialog { get; set; } = null!;

    [Inject]
    public ITransactionHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransactionsByPeriodRequest();
            var result = await Handler.GetByPeriodAsync(request);
            if (result.IsSuccess)
                Transactions = result.Data ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await Dialog.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir a transação {title} será removida. Deseja continuar?",
            yesText: "Excluir",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        try
        {
            var request = new DeleteTransactionRequest
            {
                Id = id
            };
            await Handler.DeleteAsync(request);
            Transactions.RemoveAll(x => x.Id == id);
            Snackbar.Add($"Transação {title} removida", Severity.Info);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
    }
}