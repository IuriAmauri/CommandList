using System.Collections.Generic;
using AutoMapper;
using CommandList.Data;
using CommandList.DTOs;
using CommandList.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(
            ICommandRepository commandRepository,
            IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var result = _commandRepository.GetAllCommands();

            if (result != null)
                return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(result));
            
            return NotFound();
        }

        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var result = _commandRepository.GetCommandById(id);

            if (result != null)
                return Ok(_mapper.Map<CommandReadDto>(result));

            return NotFound();
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _commandRepository.CreateCommand(commandModel);
            _commandRepository.SaveChanges();
            
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var command = _commandRepository.GetCommandById(id);

            if (command == null)
                return NotFound();

            _mapper.Map(commandUpdateDto, command);
            
            _commandRepository.UpdateCommand(command);
            _commandRepository.SaveChanges();
            
            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PatchCommand(int id, JsonPatchDocument<CommandUpdateDto> patchDocument)
        {
            var command = _commandRepository.GetCommandById(id);

            if (command == null)
                return NotFound();

            var commandToPatch = _mapper.Map<CommandUpdateDto>(command);
            patchDocument.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
                return ValidationProblem(ModelState);
            
            _mapper.Map(commandToPatch, command);

            _commandRepository.UpdateCommand(command);
            _commandRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var command = _commandRepository.GetCommandById(id);

            if (command == null)
                return NotFound();

            _commandRepository.DeleteCommand(command);
            
            return NoContent();
        }
    }
}