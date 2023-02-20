using DocumentGenerationApi.DAL.DbContexts;
using DocumentGenerationApi.DAL.Entity;
using DocumentGenerationApi.DAL.Repositories.Interfaces;
using DocumentGenerationApi.Models.RequestViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocumentGenerationApi.DAL.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;  
        public UserRepository(ApplicationDbContext service) 
        {
            _context = service;
        }

        public async Task<List<Document>> GetAllDocumentsAsync()
        {
            var list = await _context.Documents.ToListAsync();
            await _context.SaveChangesAsync();
            return list;
        }
        public async Task<Document> GetContent(string docCode)
        {
            var items = await _context.Documents.ToListAsync();

            var template = items.Where(t => t.DocumentCode.Equals(docCode)).FirstOrDefault();

            return template;
        }

        public async Task AddAsync(User user)
        {
            _context.Add<User>(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByPolicyNumber(UserRequestModel userRequestModel)
        {
            var list = await _context.Users.ToListAsync();
            var item = list.Where(t => t.PolicyNumber.Equals(userRequestModel.PolicyNumber) && 
                                       t.ProductCode.Equals(userRequestModel.ProductCode)).FirstOrDefault();
            await _context.SaveChangesAsync();

            return item;    
        }

        public async Task SaveDocInDBAsync(SaveDocument saveDocument)
        {
            _context.Add<SaveDocument>(saveDocument);
            await _context.SaveChangesAsync();
        }

        public async Task MakeIsDeleteTrue(string ObjectCode)
        {
            var doc = await _context.SavedDocuments.Where(t => t.ObjectCode.Equals(ObjectCode)).FirstOrDefaultAsync();

            if (doc != null)
            {
                doc.isDeleted = true;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DocExistOrNot(string ObjectCode)
        {
            var item = await _context.SavedDocuments.
                Where(t => t.ObjectCode.Equals(ObjectCode)).FirstOrDefaultAsync();

            if (item != null) 
            {
                return true; 
            }
            else
            {
                return false; 
            }
        }
    }
}
