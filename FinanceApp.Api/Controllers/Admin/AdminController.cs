using FinanceApp.Api.IService;
using FinanceApp.Api.Model;
using FinanceApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Api.Controllers.Admin
{
    public class AdminController : AdminBaseClass
    {
        private readonly ICoreService _coreService;
        public AdminController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        #region Item
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItem(ItemRequest request)
        {
            await _coreService.AddItem(request);
            return Ok(ApiResponse<bool>.CreatedSuccess());
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItem(ItemRequest request)
        {
            await _coreService.UpdateItem(request);
            return Ok(ApiResponse<bool>.CreatedSuccess());
        }
        [HttpPost("finalize-temp-item")]
        public async Task<IActionResult> FinalizeTempItem(int id, int categoryId)
        {
            await _coreService.FinalizeTempItem(id, categoryId);
            return Ok(ApiResponse<bool>.CreatedSuccess());
        }
        #endregion
    }
}
