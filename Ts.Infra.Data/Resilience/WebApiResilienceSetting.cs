namespace Ts.Infra.Data.Resilience
{
    public sealed class WebApiResilienceSetting
    {
        public RestResilienceOption Rest { get; set; }
    }

    public sealed class RestResilienceOption
    {
        public CircuitBreakerPolicyOption File { get; set; }
    }

    public sealed class CircuitBreakerPolicyOption
    {
        public int SamplingDurationSeconds { get; set; }
        public double FailureRatio { get; set; }
        public int MinimumThroughput { get; set; }
        public int BreakDurationSeconds { get; set; }
    }
}
