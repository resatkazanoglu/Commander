using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo Repository;
        private IMapper Mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = Repository.GetAllCommands();
            return Ok(Mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = Repository.GetCommandById(id);

            if (commandItem == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto command)
        {
            var commandModel = Mapper.Map<Command>(command);

            Repository.CreateCommand(commandModel);
            Repository.SaveChanges();

            var commandReadDto = Mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto command)
        {
            var existing = Repository.GetCommandById(id);

            if (existing == null)
            {
                return NotFound();
            }

            Mapper.Map(command, existing);
            Repository.UpdateCommand(existing);
            Repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDocument)
        {
            var existing = Repository.GetCommandById(id);

            if (existing == null)
            {
                return NotFound();
            }

            var commandToPatch = Mapper.Map<CommandUpdateDto>(existing);
            patchDocument.ApplyTo(commandToPatch, ModelState);

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            Mapper.Map(commandToPatch, existing);
            Repository.UpdateCommand(existing);
            Repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var existing = Repository.GetCommandById(id);

            if (existing == null)
            {
                return NotFound();
            }

            Repository.DeleteCommand(existing);
            Repository.SaveChanges();

            return NoContent();
        }
    }
}