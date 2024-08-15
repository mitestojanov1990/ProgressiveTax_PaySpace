namespace PaySpace.Calculator.Web.Models;

public sealed record ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}