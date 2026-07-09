namespace Ts.Application.Client.AppCustomAttributes
{
    public class ReCaptchaResponse
    {
        public TokenProperty TokenProperties { get; set; }
        public RiskAnalysis RiskAnalysis { get; set; }
    }

    public class TokenProperty
    {
        public bool Valid { get; set; }
    }

    public class RiskAnalysis
    {
        public float Score { get; set; }
    }
}
