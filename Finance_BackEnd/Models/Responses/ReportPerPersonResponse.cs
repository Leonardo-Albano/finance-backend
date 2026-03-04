namespace Finance_BackEnd.Models.Responses
{
    public record ReportPerPersonResponse(
        int PersonId,
        string PersonName,
        decimal TotalIncome,
        decimal TotalExpense,
        decimal Balance
    );
}
