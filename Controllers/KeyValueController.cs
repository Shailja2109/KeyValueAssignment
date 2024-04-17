using KeyValue.Data;
using KeyValue.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KeyValue.Controllers
{
    [Route("api/")]
    [ApiController]
    public class KeyValueController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public KeyValueController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("keys/{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var keyValue = await _dbContext.KeyValues.FindAsync(key);

            if (keyValue == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Key does not exist.",
                });
            }

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Key-Value retrieved successfully.",
                Data = keyValue.Value
            });
        }

        [HttpPost("keys")]
        public async Task<IActionResult> AddKeyValue(KeyValueData keyValue)
        {
            if (_dbContext.KeyValues.Any(k => k.Key == keyValue.Key))
            {
                return Conflict(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "The key is already exists.",
                });
            }

            await _dbContext.KeyValues.AddAsync(keyValue);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<KeyValueData>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Key-Value added successfully.",
                Data = keyValue
            });
        }

        [HttpPatch("keys/{key}/{value}")]
        public async Task<IActionResult> UpdateValue(string key, string value)
        {
            var keyValues = await _dbContext.KeyValues.FindAsync(key);

            if (keyValues == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Key does not exist.",
                });
            }

            keyValues.Value = value;
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Value updated successfully.",
                Data = keyValues.Value
            });
        }

        [HttpDelete("keys/{key}")]
        public async Task<IActionResult> DeleteByKey(string key)
        {
            var keyValue = await _dbContext.KeyValues.FindAsync(key);

            if (keyValue == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Key does not exist.",
                });
            }

            _dbContext.KeyValues.Remove(keyValue);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Key deleted successfully",
                Data = keyValue.Key
            });
        }
    }
}
