using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ts.Application.AppInterfaces;
using Ts.Common.Constant.AppConstants;
namespace Ts.SrcApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [HttpPost("document/{folderPath}/{isPublic}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UploadDocument(IFormFile file, string folderPath, bool isPublic, [FromForm] string oldFileName)
        {
            var fileStatus = await fileService.UploadDocumentAsync(file, folderPath, isPublic, oldFileName).ConfigureAwait(false);

            if (fileStatus.ErrorMessage.Count > 0)
                return UnprocessableEntity(fileStatus.ErrorMessage);
            return Ok(fileStatus.Data);
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [HttpPost("image/{folderPath}/{isPublic}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UploadImage(IFormFile file, string folderPath, bool isPublic, [FromForm] string oldFileName)
        {
            var fileStatus = await fileService.UploadImageAsync(file, folderPath, isPublic, oldFileName).ConfigureAwait(false);

            if (fileStatus.ErrorMessage.Count > 0)
                return UnprocessableEntity(fileStatus.ErrorMessage);
            return Ok(fileStatus.Data);
        }

        [Authorize(Roles = RoleConstant.Administrator)]
        [HttpPut("{folderPath}/{isPublic}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DeleteFile(string folderPath, bool isPublic, IEnumerable<string> fileNames)
        {
            var fileStatus = await fileService.DeleteFileAsync(folderPath, fileNames, isPublic).ConfigureAwait(false);
            if (fileStatus.ErrorMessage.Count > 0)
                return UnprocessableEntity(fileStatus.ErrorMessage);
            return NoContent();
        }

        [Authorize(Roles = $"{RoleConstant.Administrator},{RoleConstant.ShopInUser}")]
        [HttpPost("{folderPath}/download")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult DownloadFile(string folderPath, [FromBody] string encryptedFileName)
        {
            var fileStatus = fileService.DownloadFile(folderPath, encryptedFileName);
            if (fileStatus.ErrorMessage.Count > 0)
                return UnprocessableEntity(fileStatus.ErrorMessage);

            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
            return File(fileStatus.Data, fileStatus.ContentType, fileStatus.FileName);
        }
    }
}
