﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
using VUTIS2.DAL.Entities;
using VUTIS2.DAL.Mappers;
using VUTIS2.DAL.Repositories;

namespace VUTIS2.DAL.UnitOfWork;

public sealed class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new()
        => new Repository<TEntity>(_dbContext, new TEntityMapper());

    public async Task CommitAsync() => await _dbContext.SaveChangesAsync().ConfigureAwait(false);

    public async ValueTask DisposeAsync() => await _dbContext.DisposeAsync().ConfigureAwait(false);
}
