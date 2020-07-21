using AutoMapper;
using CommandList.DTOs;
using CommandList.Models;

namespace CommandList.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Command, CommandReadDto>().ReverseMap();
            CreateMap<Command, CommandCreateDto>().ReverseMap();
            CreateMap<Command, CommandUpdateDto>().ReverseMap();
        }
    }
}