using RestEase;

namespace Ademund.OTC.Client
{
    public interface IOTCApiBase
    {
        [Header("X-Project-Id")] string ProjectId { get; set; }
    }
}
