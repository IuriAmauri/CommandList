using System;
using System.Collections.Generic;
using System.Linq;
using CommandList.Models;

namespace CommandList.Data
{
    public class SqlCommandRepository : ICommandRepository
    {
        private readonly CommandContext _dbContext;
        public SqlCommandRepository(CommandContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _dbContext.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _dbContext.Commands.FirstOrDefault(f => f.Id == id);
        }

        public void CreateCommand(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _dbContext.Commands.Add(command);
        }

        public void UpdateCommand(Command command)
        {
            //Nothing
        }

        public void DeleteCommand(Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _dbContext.Commands.Remove(command);
            _dbContext.SaveChanges();
        }
    }
}