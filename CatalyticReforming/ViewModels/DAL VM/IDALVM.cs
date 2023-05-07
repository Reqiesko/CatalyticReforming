using System;


namespace CatalyticReforming.ViewModels.DAL_VM;

public interface IDALVM : IEquatable<IDALVM>
{
    public int Id { get; set; }

    bool IEquatable<IDALVM>.Equals(IDALVM? other)
    {
        return Id == other.Id;
    }
}


