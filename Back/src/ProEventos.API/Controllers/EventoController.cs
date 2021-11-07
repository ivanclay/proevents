using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {
            
        }

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _eventos.Where(evento => evento.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return "Exemplo de POST";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
             return $"Exemplo de POST com id = {id}";
        }

        [HttpDelete("id")]
        public string DELETE(int id)
        {
            return $"Exemplo de DELETE com id = {id}";
        }

        public IEnumerable<Evento> _eventos = new Evento[]{
                new Evento(){
                    EventoId = 1,
                    Tema = "Angular 12",
                    Local = "Salvador",
                    Lote = "1 lote",
                    QtdPessoas = 250,
                    DataEvento = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                    ImagemURL = "foto.png"
                },
                new Evento(){
                        EventoId = 2,
                        Tema = "Angular 12",
                        Local = "Salvador",
                        Lote = "1 lote",
                        QtdPessoas = 250,
                        DataEvento = DateTime.Now.AddDays(4).ToString("dd/MM/yyyy"),
                        ImagemURL = "foto2.png"
                },
                new Evento(){
                        EventoId = 3,
                        Tema = "Angular 12",
                        Local = "Salvador",
                        Lote = "1 lote",
                        QtdPessoas = 250,
                        DataEvento = DateTime.Now.AddDays(8).ToString("dd/MM/yyyy"),
                        ImagemURL = "foto3.png"
                }
            };
    }
}
