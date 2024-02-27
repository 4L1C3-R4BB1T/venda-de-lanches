namespace VendaLanches.Models;

public class ErrorViewModel
{
    #pragma warning disable CS8632
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
