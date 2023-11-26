using MediatR;
using Notes.Application.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        ICurrentUserService _currenUserService;

        public LoggingBehavior(ICurrentUserService currentUserService) => this._currenUserService = currentUserService;

        //public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        //{
        //    //var requestName=typeof(TRequest).Name;
        //    //var userId = _currenUserService.UserId;

        //    //Log.Information("Notes Request: {Name} {@UserId} {@Request}", requestName,userId, request);

        //    //var response = await next();

        //    //return response;

        //}

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currenUserService.UserId;

            Log.Information("Notes Request: {Name} {@UserId} {@Request}", requestName, userId, request);

            var response = await next();

            return response;
        }
    }
    }
