using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CatalyticReforming.ViewModels.DAL_VM;

using DAL;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace CatalyticReforming.Utils.Services;

public class GenericRepository
{
    private readonly Func<AppDbContext> _contextCreator;

    public GenericRepository(Func<AppDbContext> contextCreator)
    {
        _contextCreator = contextCreator;
    }

    public async Task<List<VMTYPE>> GetAll<VMTYPE, DALTYPE>(Func<DALTYPE, bool> predicate) where DALTYPE : EntityBase
                                                                                           where VMTYPE : IDALVM
    {
        await using var context = _contextCreator();

        return context.Set<DALTYPE>()
                      .Where(predicate)
                      .Adapt<List<VMTYPE>>();
    }

    public async Task<List<VMTYPE>> GetAll<VMTYPE, DALTYPE>() where DALTYPE : EntityBase
                                                              where VMTYPE : IDALVM
    {
        await using var context = _contextCreator();

        return context.Set<DALTYPE>()
                      .Adapt<List<VMTYPE>>();
    }

    public async Task<DALTYPE> Create<VMTYPE, DALTYPE>(VMTYPE dto) where DALTYPE : EntityBase
                                                                   where VMTYPE : IDALVM
    {
        await using var context = _contextCreator();

        var entity = dto.BuildAdapter()
                        .EntityFromContext(context)
                        .AdaptToType<DALTYPE>();

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task Update<VMTYPE, DALTYPE>(VMTYPE dto) where DALTYPE : EntityBase
                                                          where VMTYPE : IDALVM
    {
        await using var context = _contextCreator();

        var entity = await context.Set<DALTYPE>()
                                  .SingleOrDefaultAsync(x => x.Id == dto.Id);

        dto.BuildAdapter()
           .EntityFromContext(context)
           .AdaptTo(entity);

        //dto.Adapt(entity);

        await context.SaveChangesAsync();
    }

    public async Task Delete<VMTYPE, DALTYPE>(VMTYPE dto) where DALTYPE : EntityBase
                                                          where VMTYPE : IDALVM
    {
        await using var context = _contextCreator();

        var entity = await context.Set<DALTYPE>()
                                  .SingleOrDefaultAsync(x => x.Id == dto.Id);

        context.Remove(entity);
        await context.SaveChangesAsync();
    }
}


