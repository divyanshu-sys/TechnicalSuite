using Microsoft.Extensions.Configuration;
namespace Ts.DotCom.Service.PostHelpers
{
    public static class DescriptionHelper
    {
        public static Dictionary<string, string> ReplacementsDevEnv(IConfiguration config)
        {
            return new Dictionary<string, string>()
            {
                {"#[Post-Image-Url]", config.GetValue<string>("SrcApiDotCom:PostImageUrl") },
            };
        }

        public static Dictionary<string, string> ReplacementsProdEnv(IConfiguration config)
        {
            return new Dictionary<string, string>()
            {
                {
                    "#[in-article-ad-1]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                             crossorigin=""anonymous""></script>
                                        <!-- Responsive dotcom Ad-1 -->
                                        <ins class=""adsbygoogle""
                                             style=""display:block""
                                             data-ad-client=""ca-pub-5902017416803134""
                                             data-ad-slot=""7431723100""
                                             data-ad-format=""auto""
                                             data-full-width-responsive=""true""></ins>
                                        <script>
                                             (adsbygoogle = window.adsbygoogle || []).push({});
                                        </script>"
                },
                {
                    "#[in-article-ad-2]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                             crossorigin=""anonymous""></script>
                                        <!-- Responsive dotcom Ad-2 -->
                                        <ins class=""adsbygoogle""
                                             style=""display:block""
                                             data-ad-client=""ca-pub-5902017416803134""
                                             data-ad-slot=""3342783408""
                                             data-ad-format=""auto""
                                             data-full-width-responsive=""true""></ins>
                                        <script>
                                             (adsbygoogle = window.adsbygoogle || []).push({});
                                        </script>"
                },
                {
                    "#[in-article-ad-3]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                             crossorigin=""anonymous""></script>
                                        <!-- Responsive dotcom Ad-3 -->
                                        <ins class=""adsbygoogle""
                                             style=""display:block""
                                             data-ad-client=""ca-pub-5902017416803134""
                                             data-ad-slot=""4565868760""
                                             data-ad-format=""auto""
                                             data-full-width-responsive=""true""></ins>
                                        <script>
                                             (adsbygoogle = window.adsbygoogle || []).push({});
                                        </script>"
                },
                {
                    "#[in-article-ad-4]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                             crossorigin=""anonymous""></script>
                                        <!-- Responsive dotcom Ad-4 -->
                                        <ins class=""adsbygoogle""
                                             style=""display:block""
                                             data-ad-client=""ca-pub-5902017416803134""
                                             data-ad-slot=""7681653516""
                                             data-ad-format=""auto""
                                             data-full-width-responsive=""true""></ins>
                                        <script>
                                             (adsbygoogle = window.adsbygoogle || []).push({});
                                        </script>"
                },
                {"#[Post-Image-Url]", config.GetValue<string>("SrcApiDotCom:PostImageUrl") },
            };
        }
    }
}
