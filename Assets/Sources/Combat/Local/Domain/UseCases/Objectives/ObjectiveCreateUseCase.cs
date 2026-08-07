using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public interface IObjectiveCreateHandler
    {
        
    }

    public interface IObjectiveCreateOutput
    {

    }

    public sealed class ObjectiveCreateUseCase
    {
        private readonly IObjectiveCreateHandler _handler;

        public void Execute()
        {

        }
    }
}
