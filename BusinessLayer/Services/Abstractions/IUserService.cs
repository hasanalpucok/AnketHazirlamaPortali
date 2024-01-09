using System;
using EntityLayer.Concrete;

namespace BusinessLayer.Services.Abstractions
{
	public interface IUserService
	{
        Task<List<AppUser>> GetAllUsersAsync();
    }
}

