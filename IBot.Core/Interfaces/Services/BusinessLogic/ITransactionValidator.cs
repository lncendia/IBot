﻿using IBot.Core.Interfaces.DTOs;

namespace IBot.Core.Interfaces.Services.BusinessLogic;

public interface ITransactionValidator
{
    public bool Validate(TransactionDto transaction);
}