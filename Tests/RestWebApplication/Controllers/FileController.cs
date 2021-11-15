/*
 * MIT License
 *
 * Copyright (c) Evgeny Nazarchuk.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestWebApplication.Models;
using RestWebApplication.Services;
using System.IO;
using System.Threading.Tasks;

namespace RestWebApplication.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FileController : ControllerBase
{
    private readonly DataAccess _data;

    public FileController(DataAccess data)
    {
        this._data = data;
    }

    [HttpGet]
    public async Task<IActionResult> GetFileList()
    {
        var files = await _data.FileStorage.AsNoTracking().ToListAsync();
        return Ok(files);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest();
        }

        using var fileStream = file.OpenReadStream();
        using var binaryFileStream = new BinaryReader(fileStream);
        byte[] fileData = binaryFileStream.ReadBytes((int)file.Length);
        var dbFile = new FileStorage
        {
            Name = file.FileName,
            Content = fileData,
            Type = file.ContentType
        };

        await _data.FileStorage.AddAsync(dbFile);
        await _data.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFiles([FromForm] IFormFileCollection files)
    {
        if (files == null)
        {
            return BadRequest();
        }

        foreach (var file in files)
        {
            using var fileStream = file.OpenReadStream();
            using var binaryStream = new BinaryReader(fileStream);
            byte[] fileData = binaryStream.ReadBytes((int)file.Length);

            var dbFile = new FileStorage
            {
                Name = file.FileName,
                Content = fileData,
                Type = file.ContentType
            };

            await _data.FileStorage.AddAsync(dbFile);
        }

        await _data.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var file = await _data.FileStorage.SingleOrDefaultAsync(x => x.Id == id);

        if (file == null)
        {
            return NotFound();
        }

        return File(file.Content, file.Type, file.Name);
    }
}
