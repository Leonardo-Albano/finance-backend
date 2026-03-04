namespace Finance_BackEnd.Models.Responses
{
    public record TransactionResponse(
        int Id,
        string Description,
        decimal Value,
        DateTime Date,
        string Type,
        string PersonName,
        string CategoryDescription
    );
}
