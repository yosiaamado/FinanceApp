using FinanceApp.Api.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using FinanceApp.Shared;

namespace FinanceApp.Api.Controllers.User
{
    public class SyncController : UserBaseClass
    {
        private ISynchronizeService _syncService;
        public SyncController(ISynchronizeService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost("sync-transaction")]
        public async Task SyncTransaction([FromBody] List<SyncRequest> requests)
        {
            HttpContext.Response.ContentType = "application/json";

            foreach (var trx in requests)
            {
                var result = await _syncService.ProcessSingleSync(trx);

                var json = JsonSerializer.Serialize(result) + "\n";
                var bytes = Encoding.UTF8.GetBytes(json);
                await HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                await HttpContext.Response.Body.FlushAsync();
            }
        }
    }
}
