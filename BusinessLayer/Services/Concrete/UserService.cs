using System;
using BusinessLayer.Extensions;
using BusinessLayer.Services.Abstractions;
using DataAccessLayer.UnitOfWorks;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
           return await unitOfWork.GetRepository<AppUser>().GetAllAsync();
        }
    }
}

