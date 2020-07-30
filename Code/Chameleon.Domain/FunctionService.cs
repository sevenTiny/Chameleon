﻿using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chameleon.Domain
{
    public interface IFunctionService : ICommonServiceBase<Function>
    {
        public string[] GetFunctionArrayFromString(string functionString);
    }

    public class FunctionService : CommonServiceBase<Function>, IFunctionService
    {
        IFunctionRepository _FunctionRepository;
        public FunctionService(IFunctionRepository FunctionRepository) : base(FunctionRepository)
        {
            _FunctionRepository = FunctionRepository;
        }

        public string[] GetFunctionArrayFromString(string functionString)
        {
            if (string.IsNullOrEmpty(functionString))
                return new string[0];

            return functionString.Split(',');
        }
    }
}
