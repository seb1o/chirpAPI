using chirpApi.Services.Model.DTOs;
using chirpApi.Services.Model.Filters;
using chirpApi.Services.Model.ViewModel;
using chirpApi.Services.Services.Interfaces;
using chirpAPI.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services
{
    public class SebioChirpsService : IChirpsService
    {
        private readonly ChirpContext _context;
        public SebioChirpsService(ChirpContext context)
        {
            _context = context;
        }
        public  async  Task<List<ChirpViewModel>> GetChirpsByFilter(ChirpFilter filter)
        {
            IQueryable<Chirp> query = _context.Chirps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Text))
            {
                query = query.Where(x => x.Text == filter.Text);
            }
            if (!string.IsNullOrWhiteSpace(filter.ExtUrl))
            {
                query = query.Where(x => x.ExtUrl == filter.ExtUrl);
            }
            var result = await query.Select(x => new ChirpViewModel
            {
                Id = x.Id,
                Text = x.Text,
                ExtUrl = x.ExtUrl,
                Lat = x.Lat,
                Lng = x.Lng,
                CreationTime = x.CreationTime
            }).ToListAsync(); 
           
                return result;
            
        }
        public async Task<int?> CreateChirp(ChirpCreateDTO chirp)
        {
            if(string.IsNullOrWhiteSpace(chirp.Text))
            {
                return null; 
            }

            var newChirp = new Chirp
            {
                Text = chirp.Text,
                ExtUrl = chirp.ExtUrl,
                Lat = chirp.Lat,
                Lng = chirp.Lng,
               
            };
            _context.Chirps.Add(newChirp);
            await _context.SaveChangesAsync();
            return newChirp.Id;
        }

        public async Task<List<ChirpViewModel>> GetAllChirps()
        {
            return await _context.Chirps
                //.OrderByDescending(c => c.CreationTime) // Ordina per data decrescente (modificabile)
                .Select(c => new ChirpViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    ExtUrl = c.ExtUrl,
                    CreationTime = c.CreationTime,
                    Lat = c.Lat,
                    Lng = c.Lng
                })
                .ToListAsync();
        }


        public async Task<ChirpViewModel?> GetChirpById(int id)
        {
            return await _context.Chirps
                .Where(c => c.Id == id)
                .Select(c => new ChirpViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    ExtUrl = c.ExtUrl,
                    CreationTime = c.CreationTime,
                    Lat = c.Lat,
                    Lng = c.Lng
                })
                .FirstOrDefaultAsync();
        }



        public async Task<bool?> UpdateChirp(int id, ChirpUpdateDTO chirp)
        {
            var existingChirp = await _context.Chirps.FindAsync(id);

            if (existingChirp == null)
            {
                return false;
            }

            if(!string.IsNullOrWhiteSpace(chirp.ExtUrl))
                existingChirp.ExtUrl = chirp.ExtUrl;
            if (!string.IsNullOrWhiteSpace(chirp.Text))
            
                existingChirp.Text = chirp.Text;
            if (chirp.Lat.HasValue)
                existingChirp.Lat = chirp.Lat.Value;
             if (chirp.Lng.HasValue)
                existingChirp.Lng = chirp.Lng.Value;


             _context.Entry(chirp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;


        }

        public async Task<int?> DeleteChirp(int id)
        {
            Chirp? chirp = await  _context.Chirps.Include(x=>x.Comments)
                                                        .Where(x=>x.Id == id)
                                                        .SingleOrDefaultAsync();
            if (chirp == null)
            {
                return null;
            }
            if(chirp.Comments != null && chirp.Comments.Count > 0)
            {
                // If there are comments, we can choose to delete them or not.
                // For now, let's just return null to indicate that we can't delete the chirp.
                return -1;
            }

            _context.Chirps.Remove(chirp);
             await _context.SaveChangesAsync();
            return id;
        }

       
    }
}
