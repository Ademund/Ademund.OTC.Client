using RestEase;
using System;

namespace Ademund.OTC.Client
{
    public interface IOTCApiBase : IDisposable
    {
        [Header("X-Project-Id")]
        string XProjectId { get; set; }

        [Path("project_id")]
        string ProjectId { get; set; }
    }
}
