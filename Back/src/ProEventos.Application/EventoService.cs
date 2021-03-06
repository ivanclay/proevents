using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGenericPersist _genericPersist;
        private readonly IEventoPersist _eventoPersist;
        private readonly IMapper _mapper;
        
        public EventoService(IGenericPersist genericPersist, IEventoPersist eventoPersist, IMapper mapper)
        {
            _mapper = mapper;
            _eventoPersist = eventoPersist;
            _genericPersist = genericPersist;           
        }
        public async Task<EventoDto> AddEvento(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                 _genericPersist.Add<Evento>(evento);
                 if(await _genericPersist.SaveChangesAsync())
                 {
                     var retorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                     return _mapper.Map<EventoDto>(retorno);
                 }
                 return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;
                _mapper.Map(model, evento);

                _genericPersist.Update<Evento>(evento);

                if(await _genericPersist.SaveChangesAsync())
                {
                    var retorno = await _eventoPersist.GetEventoByIdAsync(evento.Id, false);
                    return _mapper.Map<EventoDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteEventos(int eventoId)
        {
            try
            {
                 var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                 if(evento == null) throw new Exception("Evento n??o encontrado.");

                 _genericPersist.Delete<Evento>(evento);
                 return await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if(eventos == null) return null;
                
                var resultado = _mapper.Map<EventoDto[]>(eventos);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema,includePalestrantes);
                if(eventos == null) return null;
                
                var resultado = _mapper.Map<EventoDto[]>(eventos);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,includePalestrantes);
                 if(evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}