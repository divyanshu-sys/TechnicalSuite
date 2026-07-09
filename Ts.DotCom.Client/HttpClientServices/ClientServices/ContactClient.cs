using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ts.Client.HttpClientServices;
using Ts.DotCom.Client.HttpClientServices.ClientInterfaces;
using Ts.DotCom.Client.ViewModels;
using Ts.Dto;
namespace Ts.DotCom.Client.HttpClientServices.ClientServices
{
    public class ContactClient : IContactClient
    {
        private const string ApiVersion = "1";
        private const string ApiUrl = $"api/v{ApiVersion}";
        private readonly IHttpPublicService httpPublicService;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly ILogger<ContactClient> logger;
        private readonly string BaseUrl;

        public ContactClient(IHttpPublicService httpPublicService, IConfiguration configuration,
            IMapper mapper, ILogger<ContactClient> logger)
        {
            this.httpPublicService = httpPublicService;
            this.configuration = configuration;
            this.mapper = mapper;
            this.logger = logger;
            BaseUrl = configuration["Api:BaseUri"];
        }

        public async Task<ResponseMessageDto<bool>> ContactAsync(ContactFormVm model)
        {
            var modelDto = mapper.Map<ContactFormDto>(model);
            modelDto.Subject = $"Message from contact form - {configuration["SiteTitle"]}";
            var response = await httpPublicService.PostAsync<bool>($"{BaseUrl}/{ApiUrl}/publicapp", modelDto, true).ConfigureAwait(false);
            if (response.ErrorMessage.Count > 0)
                logger.LogError("Error sending contact form message: {ErrorMessage}", string.Join(',', response.ErrorMessage));
            return response;
        }
    }
}
