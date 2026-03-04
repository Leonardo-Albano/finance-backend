namespace Finance_BackEnd.Models.DTOs
{
    public record TransactionDto(
        string Description,
        decimal Value,
        DateTime Date,
        TransactionType Type,
        int PersonId,
        int CategoryId
    );
}
