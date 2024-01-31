using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.ExcelWebApp.Hubs;
using RabbitMQ.ExcelWebApp.Models;

namespace RabbitMQ.ExcelWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHubContext<MyHub> _hub;
        public FileController(AppDbContext appDbContext, IHubContext<MyHub> hub)
        {
            _appDbContext = appDbContext;
            _hub = hub;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();

            var userFile = await _appDbContext.UserFiles.FirstAsync(x => x.Id == fileId);

            var filePath = userFile.FileName + Path.GetExtension(file.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", filePath);

            using FileStream stream = new(path, FileMode.Create);

            await file.CopyToAsync(stream);

            userFile.CreatedDate = DateTime.Now;
            userFile.FilePath = filePath;

            userFile.FileStatus = FileStatus.Completed;
            await _appDbContext.SaveChangesAsync();

            await _hub.Clients.User(userFile.UserId).SendAsync("DosyaTamamlandi");
            return Ok();    

        }
    }
}
