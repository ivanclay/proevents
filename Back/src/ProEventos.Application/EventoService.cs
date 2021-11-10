using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IGenericPersist _genericPersist;
        private readonly IEventoPersist _eventoPersist;
        
        public EventoService(IGenericPersist genericPersist, IEventoPersist eventoPersist)
        {
            _eventoPersist = eventoPersist;
            _genericPersist = genericPersist;           
        }
        public async Task<Evento> AddEvento(Evento model)
        {
            try
            {
                 _genericPersist.Add(model);
                 if(await _genericPersist.SaveChangesAsync())
                 {
                     return await _eventoPersist.GetEventoByIdAsync(model.Id, false);
                 }
                 return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento> UpdateEvento(int eventoId, Evento model)
        {
            try
            {
                 var evento = await _eventoPersist.GetEventoByIdAsync(eventoId, false);
                 if(evento == null) return null;

                 model.Id = evento.Id;

                 _genericPersist.Update(model);
                 if(await _genericPersist.SaveChangesAsync())
                 {
                     return await _eventoPersist.GetEventoByIdAsync(model.Id, false);
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
                 if(evento == null) throw new Exception("Evento não encontrado.");

                 _genericPersist.Delete<Evento>(evento);
                 return await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            try
            {
                 var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                 if(eventos == null) return null;
                 return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            try
            {
                 var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema,includePalestrantes);
                 if(eventos == null) return null;
                 return eventos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventoPersist.GetEventoByIdAsync(eventoId,includePalestrantes);
                 if(evento == null) return null;
                 return evento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}