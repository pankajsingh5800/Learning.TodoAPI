using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;
using Todo.Domain.DomainEntities;
using Todo.Domain.RepositoryInterface;

namespace Todo.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<bool> CreateUserAsync(CreateUserDto userDto)
        {
            var userDomain = mapper.Map<UserDomain>(userDto);

            userDomain.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDomain.PasswordHash);

            await userRepository.AddAsync(userDomain);

            var response = await userRepository.CommitAsync();

            return response > 0;
        }
    }
}
