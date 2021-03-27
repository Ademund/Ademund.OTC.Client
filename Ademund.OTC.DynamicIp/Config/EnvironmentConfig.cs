namespace Ademund.OTC.DynamicIp.Config
{
    public record EnvironmentConfig
    {
        public string Name { get; init; }
        public string AccessKey { get; init; }
        public string SecretKey { get; init; }
        public string ProjectId { get; init; }
        public string SecurityGroupId { get; init; }
        public string Region { get; init; }
    }
}
