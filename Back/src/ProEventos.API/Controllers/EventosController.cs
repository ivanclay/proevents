using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Context;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _service;
        
        public EventosController(IEventoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                 var eventos = await _service.GetAllEventosAsync(true);
                 if(eventos == null) return NotFound("Nenhum evento encontrado.");
                 return Ok(eventos);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                 var evento = await _service.GetEventoByIdAsync(id, true);
                 if(evento == null) return NotFound("Nenhum evento encontrado.");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

         [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
            try
            {
                 var evento = await _service.GetAllEventosByTemaAsync(tema, true);
                 if(evento == null) return NotFound("Nenhum evento encontrado.");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento model)
        {
           try
            {
                 var evento = await _service.AddEvento(model);
                 if(evento == null) return BadRequest("Erro ao tentar adicionar Evento.");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar incluir o evento. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Evento model)
        {
            try
            {
                 var evento = await _service.UpdateEvento(id, model);
                 if(evento == null) return BadRequest("Erro ao tentar adicionar Evento.");
                 return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar o eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DELETE(int id)
        {
            try
            {
                return await _service.DeleteEventos(id) ?
                    Ok("Evento excluído com sucesso!") :
                    BadRequest("Evento não excluído!");

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir o evento. Erro: {ex.Message}");
            }
        }
    }
}
