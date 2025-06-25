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
            try
            {
                var token = await _coreService.AddItem(request);
                return Ok(ApiResponse<bool>.CreatedSuccess());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }

        [HttpPost("update-item")]
        public async Task<IActionResult> UpdateItem(ItemRequest request)
        {
            try
            {
                var token = await _coreService.UpdateItem(request);
                return Ok(ApiResponse<bool>.CreatedSuccess());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }
        [HttpPost("finalize-temp-item")]
        public async Task<IActionResult> FinalizeTempItem(int id, int categoryId)
        {
            try
            {
                var result = await _coreService.FinalizeTempItem(id, categoryId);
                return Ok(ApiResponse<bool>.CreatedSuccess());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailedMessage("Server error: " + ex.Message));
            }
        }
        #endregion
    }
}
