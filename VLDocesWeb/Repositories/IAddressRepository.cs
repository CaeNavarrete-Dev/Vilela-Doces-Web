using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IAddressRepository {
    void Create(Address address);
    List<Address> ReadAll(int usuarioId);
    // Address Read(int id);
    // void Update(Address address);
    // void Delete(int id);
}