﻿using chirpApi.Services.Model.DTOs;
using chirpApi.Services.Model.Filters;
using chirpApi.Services.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chirpApi.Services.Services.Interfaces
{
    public interface IChirpsService
    {
        Task<List<ChirpViewModel>> GetChirpsByFilter(ChirpFilter filter);
        Task<int?> CreateChirp(ChirpCreateDTO chirpCreateModel);
        Task<List<ChirpViewModel>> GetAllChirps();
        Task<ChirpViewModel?> GetChirpById(int id);
        Task<bool?> UpdateChirp(int id, ChirpUpdateDTO chirp);
        Task<int?> DeleteChirp(int id);
    }
}
