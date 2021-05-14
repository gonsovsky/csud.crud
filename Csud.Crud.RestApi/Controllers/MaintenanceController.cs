﻿using System;
using System.IO;
using Csud.Crud.Models;
using Csud.Crud.Models.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Csud.Crud.RestApi.Controllers
{
    public partial class MaintenanceController: EntityController<AppImport>
    {
        [HttpPut]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Produces("application/json")]
        public IActionResult Put(IFormFile formFile, string applicant="", string email="", string comment="")
        {
            try
            {
                var filePath = Path.Combine(CsudService.Config.Import.Folder, formFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyToAsync(stream).Wait();
                }
                var entity = new AppImport()
                {
                    Comment = comment,
                    Applicant = applicant,
                    Email = email,
                    FileName = formFile.FileName
                };
                entity.Step = Const.Import.Uploaded;
                entity.Submitted = DateTime.Now;
                Svc.Add(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)] public override IActionResult Post(AppImport entity) => throw new NotImplementedException();
        [HttpPost("somewhere")]
        [ApiExplorerSettings(IgnoreApi = true)] public override IActionResult Put(AppImport entity) => throw new NotImplementedException();
        [ApiExplorerSettings(IgnoreApi = true)] public override IActionResult Copy(int key) => throw new NotImplementedException();
        [ApiExplorerSettings(IgnoreApi = true)] public override IActionResult Get(int key) => throw new NotImplementedException();
    }
}
