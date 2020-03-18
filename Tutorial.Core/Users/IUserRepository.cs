using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial.Data.Documents;

namespace Tutorial.Core.Users
{
    public interface IUserRepository
    {
        Task<Guid> AddAsync(UserDocument document);

        Guid Add(UserDocument document);

        Task<UserDocument> GetAsync(Guid userId);

        UserDocument Get(Guid userId);

        Task<IList<UserDocument>> GetAllAsync();

        IList<UserDocument> GetAll();

        Task UpsertAsync(UserDocument document);
    }
}
