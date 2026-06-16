namespace Application.Revenue.DTOs;

public record RevenueDto(
    decimal CurrentRevenue,
    decimal PredictedRevenue,
    string Currency
    );