using System.Collections.Generic;
using Commander.Models;
using System.Linq;
using System;

namespace Commander.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext Context;
        public SqlCommanderRepo(CommanderContext context)
        {
            Context = context;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() >= 0;
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return Context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return Context.Commands.FirstOrDefault(p => p.Id == id);
        }

        public void CreateCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Context.Commands.Add(command);
        }

        public void UpdateCommand(Command command)
        {
            //Nothing
        }

        public void DeleteCommand(Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Context.Commands.Remove(command);
        }
    }
}