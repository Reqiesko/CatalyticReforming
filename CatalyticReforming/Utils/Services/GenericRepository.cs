using System;
using System.Linq;
using System.Threading.Tasks;

using CatalyticReforming.ViewModels.DAL_VM;

using DAL;

using Mapster;

using Microsoft.EntityFrameworkCore;


namespace CatalyticReforming.Services;

public class GenericRepository
{
    private readonly Func<AppDbContext> _contextCreator;

    public GenericRepository(Func<AppDbContext> contextCreator)
    {
        _contextCreator = contextCreator;
    }

    public async Task<DALTYPE> Create<VMTYPE,DALTYPE>(VMTYPE dto) where DALTYPE:  EntityBase 
                                                                  where VMTYPE: IDALVM
    {
        await using var context = _contextCreator();
        var entity = dto.Adapt<DALTYPE>();
        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task Update<VMTYPE,DALTYPE>(VMTYPE dto) where DALTYPE: EntityBase
                                                         where VMTYPE: IDALVM
    {
        await using var context = _contextCreator();

        var entity = await context.Set<DALTYPE>()
                                  .SingleOrDefaultAsync(x => x.Id == dto.Id);

        // dto.BuildAdapter()
        //    .EntityFromContext(context)
        //    .AdaptTo(entity);
        dto.Adapt(entity);

        await context.SaveChangesAsync();
    }

    public async Task Delete<VMTYPE,DALTYPE>(VMTYPE dto) where DALTYPE: EntityBase
                                                         where VMTYPE: IDALVM
    {
        await using var context = _contextCreator();

        var entity = await context.Set<DALTYPE>()
                            .SingleOrDefaultAsync(x => x.Id == dto.Id);

        context.Remove(entity);
        await context.SaveChangesAsync();
    }
}
