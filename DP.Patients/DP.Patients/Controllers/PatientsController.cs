using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DP.Patients.Controllers.Model;
using DP.Patients.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DP.Patients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DpDataContext _context;
        private readonly ServiceBusSender _sender;

        public PatientsController(DpDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            return Ok(_context.Patients.ToList());
        }

        [HttpPost]
        [Authorize]
        public async  Task<IActionResult> PostAdd(Patient p)
        {
            _context.Patients.Add(p);
            _context.SaveChanges();

            await _sender.SendMesasge(new MessagePayload() { EventName = "NewUserRegistered", UserEmail = p.Email });

            return Created("/api/Created", p);
        }

        [HttpPut]
        public IActionResult InvalidAction()
        {
            throw new InvalidOperationException("Testowy wyjatek");
        }
    }


}
