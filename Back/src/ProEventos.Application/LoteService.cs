using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application
{
    public class LoteService : ILoteService
    {
        private readonly IGenericPersist _genericPersist;
        private readonly ILotePersist _lotePersist;
        private readonly IMapper _mapper;
        
        public LoteService(IGenericPersist genericPersist, ILotePersist lotePersist, IMapper mapper)
        {
            _mapper = mapper;
            _lotePersist = lotePersist;
            _genericPersist = genericPersist;           
        }
        public async Task AddLote(int eventoId, LoteDto model)
        {
            try
            {
                var lote = _mapper.Map<Lote>(model);
                lote.EventoId = eventoId;
                 _genericPersist.Add<Lote>(lote);
                 await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> SaveLotes(int eventoId, LoteDto[] models)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return null;

                foreach (var model in models)
                {
                   if(model.Id == 0)
                   {
                       await AddLote(eventoId, model);
                   } else
                   {
                       var lote = lotes.FirstOrDefault(lote => lote.Id == model.Id);
                        model.EventoId = eventoId;
                        _mapper.Map(model, lote);
                        _genericPersist.Update<Lote>(lote);
                        await _genericPersist.SaveChangesAsync();
                        var retorno = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                        return _mapper.Map<LoteDto[]>(retorno);
                   }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteLote(int eventoId, int loteId)
        {
            try
            {
                 var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                 if(lote == null) throw new Exception("Lote n??o encontrado.");

                 _genericPersist.Delete<Lote>(lote);
                 return await _genericPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto[]> GetLotesByEventoIdAsync(int eventoId)
        {
            try
            {
                var lotes = await _lotePersist.GetLotesByEventoIdAsync(eventoId);
                if(lotes == null) return null;
                
                var resultado = _mapper.Map<LoteDto[]>(lotes);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LoteDto> GetLoteByIdsAsync(int eventoId, int loteId)
        {
            try
            {
                 var lote = await _lotePersist.GetLoteByIdsAsync(eventoId, loteId);
                 if(lote == null) return null;

                var resultado = _mapper.Map<LoteDto>(lote);
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}