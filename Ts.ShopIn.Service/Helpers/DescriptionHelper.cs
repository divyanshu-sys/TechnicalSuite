using Microsoft.Extensions.Configuration;
namespace Ts.ShopIn.Service.Helpers
{
    public static class DescriptionHelper
    {
        public static Dictionary<string, string> ReplacementsDevEnv(IConfiguration config)
        {
            return new Dictionary<string, string>()
            {
                {"#[Blog-Image-Url]", config.GetValue<string>("SrcApiShopIn:BlogImageUrl") },
                {"#[ProductDetail-Image-Url]", config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl") }
            };
        }

        public static Dictionary<string, string> ReplacementsProdEnv(IConfiguration config)
        {
            return new Dictionary<string, string>()
            {
                {
                    "#[in-article-ad-1]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                                 crossorigin=""anonymous""></script>
                                            <!-- Responsive shopin Ad-1 -->
                                            <ins class=""adsbygoogle""
                                                 style=""display:block""
                                                 data-ad-client=""ca-pub-5902017416803134""
                                                 data-ad-slot=""2065194329""
                                                 data-ad-format=""auto""
                                                 data-full-width-responsive=""true""></ins>
                                            <script>
                                                 (adsbygoogle = window.adsbygoogle || []).push({});
                                            </script>"
                },
                {
                    "#[in-article-ad-2]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                                 crossorigin=""anonymous""></script>
                                            <!-- Responsive shopin Ad-2 -->
                                            <ins class=""adsbygoogle""
                                                 style=""display:block""
                                                 data-ad-client=""ca-pub-5902017416803134""
                                                 data-ad-slot=""4926317495""
                                                 data-ad-format=""auto""
                                                 data-full-width-responsive=""true""></ins>
                                            <script>
                                                 (adsbygoogle = window.adsbygoogle || []).push({});
                                            </script>"
                },
                {
                    "#[in-article-ad-3]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                                 crossorigin=""anonymous""></script>
                                            <!-- Responsive shopin Ad-2 -->
                                            <ins class=""adsbygoogle""
                                                 style=""display:block""
                                                 data-ad-client=""ca-pub-5902017416803134""
                                                 data-ad-slot=""4926317495""
                                                 data-ad-format=""auto""
                                                 data-full-width-responsive=""true""></ins>
                                            <script>
                                                 (adsbygoogle = window.adsbygoogle || []).push({});
                                            </script>"
                },
                {
                    "#[in-article-ad-4]", @"<script async src=""https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js?client=ca-pub-5902017416803134""
                                                 crossorigin=""anonymous""></script>
                                            <!-- Responsive shopin Ad-4 -->
                                            <ins class=""adsbygoogle""
                                                 style=""display:block""
                                                 data-ad-client=""ca-pub-5902017416803134""
                                                 data-ad-slot=""2066943723""
                                                 data-ad-format=""auto""
                                                 data-full-width-responsive=""true""></ins>
                                            <script>
                                                 (adsbygoogle = window.adsbygoogle || []).push({});
                                            </script>"
                },
                {"#[Blog-Image-Url]", config.GetValue<string>("SrcApiShopIn:BlogImageUrl") },
                {"#[ProductDetail-Image-Url]", config.GetValue<string>("SrcApiShopIn:ProductDetailImageUrl") }
            };
        }
    }
}
