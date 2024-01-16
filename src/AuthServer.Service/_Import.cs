﻿global using AuthServer.Contracts;
global using AuthServer.Contracts.Example;
global using AuthServer.Service.Application.Example.Commands;
global using AuthServer.Service.Application.Example.Queries;
global using AuthServer.Service.DataAccess;
global using Mapster;
global using Masa.BuildingBlocks.Ddd.Domain.Entities;
global using Masa.BuildingBlocks.Dispatcher.Events;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Commands;
global using Masa.BuildingBlocks.ReadWriteSplitting.Cqrs.Queries;
global using Masa.Contrib.Dispatcher.Events;
global using Masa.Utils.Models;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.EntityFrameworkCore.Storage;
global using Microsoft.OpenApi.Models;