using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LotesController : ControllerBase
    {
        private readonly ILoteService _service;
        
        public LotesController(ILoteService service)
        {
            _service = service;
        }

        [HttpGet("{eventoId}")]
        public async Task<IActionResult> Get(int eventoId)
        {
            try
            {
                 var lotes = await _service.GetLotesByEventoIdAsync(eventoId);
                 if(lotes == null) return NoContent();

                 return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar lotes. Erro: {ex.Message}");
            }
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                 var lotes = await _service.SaveLotes(eventoId, models);
                 if(lotes == null) return BadRequest("Erro ao tentar adicionar Evento.");
                 return Ok(lotes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar salvar lotes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{eventoId}/{loteId}")]
        public async Task<IActionResult> Delete(int eventoId, int loteId)
        {
            try
            {
                var lote = await _service.GetLoteByIdsAsync(eventoId, loteId);
                if(lote == null) return NoContent();

                return await _service.DeleteLote(lote.EventoId, lote.Id)
                    ? Ok(new {message = "Lote excluído com sucesso!"}) 
                    : throw new Exception("Ocorreu um erro não especificado ao tentar excluir o lote.");

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError, $"Erro ao tentar excluir o lote. Erro: {ex.Message}");
            }
        }
    }
}
