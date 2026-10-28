namespace Ts.DotCom.Client.Resilience
{
    public sealed class DotComResilienceSetting
    {
        public RestResilienceOption Rest { get; set; }
    }

    public sealed class RestResilienceOption
    {
        public CircuitBreakerPolicyOption WebApi { get; set; }
    }

    public sealed class CircuitBreakerPolicyOption
    {
        public double FailureRatio { get; set; }
        public int MinimumThroughput { get; set; }
        public int SamplingDurationSeconds { get; set; }
        public int BreakDurationSeconds { get; set; }
    }
}
