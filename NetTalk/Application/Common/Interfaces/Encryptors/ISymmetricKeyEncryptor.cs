using Domain.Entities;

namespace Application.Interfaces;

public interface ISymmetricKeyEncryptor
{
    public abstract SymmetricKey GenerateKey();
}